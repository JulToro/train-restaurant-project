#include "SetupPortal.h"
#include "AppConfig.h"
#include "WifiController.h"
#include "SetupPage.h"
#include <WebServer.h>
#include <DNSServer.h> // Responde a los nombres consultados por el celular con la IP del portal.
#include <WiFi.h>
#include <esp_system.h>
#include <cstring>

namespace {
const IPAddress portalAddress(192, 168, 4, 1);
// Escucha en la dirección de la red temporal, no en la IP del Wi-Fi doméstico.
WebServer server(portalAddress, 80);
DNSServer dnsServer;
bool active = false;
bool automaticStartPending = true;
bool waitingForConnection = false;
bool closing = false;
unsigned long closingStarted = 0;
String sessionToken;

bool isPortalHost() {
    // Todas las operaciones del formulario usan la misma dirección conocida.
    // Un nombre externo resuelto por nuestro DNS solo debe llevar a esa dirección.
    const String host = server.hostHeader();
    return host == "192.168.4.1" || host == "192.168.4.1:80";
}

void redirectToPortal() {
    // 302 indica al navegador que abra otra dirección. El destino es fijo:
    // nunca usamos como destino un dato recibido desde el navegador.
    server.sendHeader("Cache-Control", "no-store");
    server.sendHeader("Location", "http://192.168.4.1/", true);
    server.send(302, "text/plain", "Abre http://192.168.4.1/");
}

void sendJson(int statusCode, const String& body) {
    server.sendHeader("Cache-Control", "no-store");
    server.send(statusCode, "application/json; charset=utf-8", body);
}

// El navegador obtiene un token de sesión al consultar el estado y lo adjunta
// a cada cambio. No habilitamos CORS: otra página no puede leer ese token.
bool authorizeChange() {
    if (!isPortalHost() || server.header("X-Setup-Token") != sessionToken) {
        sendJson(403, "{\"error\":\"Recarga la pagina e intenta de nuevo.\"}");
        return false;
    }
    return true;
}

void handleStatus() {
    if (!isPortalHost()) {
        sendJson(403, "{\"error\":\"Abre http://192.168.4.1/\"}");
        return;
    }
    String body = "{\"state\":\"";
    body += WifiController::getConnectionState();
    body += "\",\"ip\":\"";
    body += WifiController::isConnected() ? WiFi.localIP().toString() : "";
    body += "\",\"scan\":\"";
    body += WifiController::getScanState();
    body += "\",\"token\":\"" + sessionToken + "\",\"closing\":";
    body += closing ? "true}" : "false}";
    sendJson(200, body);
}

void handleScan() {
    if (!authorizeChange()) {
        return;
    }
    if (WifiController::isConnecting() || WifiController::isScanning() || closing) {
        sendJson(409, "{\"error\":\"Espera a que termine la operacion actual.\"}");
        return;
    }
    WifiController::startScan();
    if (!WifiController::isScanning()) {
        sendJson(503, "{\"error\":\"No se pudo iniciar la busqueda.\"}");
        return;
    }
    sendJson(202, "{\"ok\":true}");
}

void handleConnect() {
    if (!authorizeChange()) {
        return;
    }
    if (WifiController::isConnecting() || WifiController::isScanning() || closing) {
        sendJson(409, "{\"error\":\"Espera a que termine la operacion actual.\"}");
        return;
    }
    // arg() lee los campos enviados por el formulario mediante POST.
    // Validamos ambos antes de modificar la configuración que está en RAM.
    const String ssid = server.arg("ssid");
    const String password = server.arg("password");
    if (ssid.length() == 0 || ssid.length() > 32 ||
        !WifiController::isPasswordValid(password) || strlen(ssid.c_str()) != ssid.length()) {
        sendJson(400, "{\"error\":\"Revisa el nombre y la clave: 8 a 63 caracteres ASCII.\"}");
        return;
    }
    WifiController::setSsid(ssid);
    WifiController::setPassword(password);
    WifiController::connect(true);
    waitingForConnection = true;
    sendJson(202, "{\"ok\":true}");
}
} // namespace

namespace SetupPortal {
void begin() {
    const char* headers[] = {"X-Setup-Token"};
    server.collectHeaders(headers, 1);
    // on() vincula una dirección del navegador con una función de C++.
    // []() { ... } es una función sin nombre (lambda).
    server.on("/", HTTP_GET, []() {
        if (!isPortalHost()) {
            redirectToPortal();
            return;
        }
        server.sendHeader("Cache-Control", "no-store");
        server.sendHeader("X-Content-Type-Options", "nosniff");
        server.sendHeader("X-Frame-Options", "DENY");
        // send_P entrega el HTML desde flash, sin copiar toda la página a RAM.
        server.send_P(200, "text/html; charset=utf-8", SETUP_PAGE);
    });
    server.on("/api/status", HTTP_GET, handleStatus);
    server.on("/api/scan", HTTP_POST, handleScan);
    server.on("/api/networks", HTTP_GET, []() {
        if (!isPortalHost()) {
            sendJson(403, "{\"error\":\"Abre http://192.168.4.1/\"}");
            return;
        }
        sendJson(200, WifiController::getNetworksJson());
    });
    server.on("/api/connect", HTTP_POST, handleConnect);
    server.onNotFound([]() {
        // Android, iOS y otros sistemas consultan distintas rutas para comprobar
        // Internet. Redirigir sus peticiones HTTP permite detectar el portal.
        // Conservamos los errores JSON para rutas de nuestra API mal escritas.
        if (server.method() == HTTP_GET && !server.uri().startsWith("/api/")) {
            redirectToPortal();
            return;
        }
        // No reflejamos argumentos de la petición: podrían contener contraseñas.
        sendJson(404, "{\"error\":\"Direccion no disponible.\"}");
    });
}

void start() {
    if (WifiController::isScanning() || WifiController::isConnecting()) {
        Serial.println("[SETUP] Espera a que termine la operacion Wi-Fi actual.");
        return;
    }
    if (!active) {
        // AP_STA mantiene la red temporal (AP) y permite conectar como cliente (STA).
        if (!WiFi.mode(WIFI_AP_STA) ||
            !WiFi.softAPConfig(portalAddress, portalAddress, IPAddress(255, 255, 255, 0)) ||
            !WiFi.softAP(AppConfig::SETUP_NETWORK_NAME, AppConfig::SETUP_NETWORK_PASSWORD)) {
            WiFi.softAPdisconnect(false);
            WiFi.mode(WIFI_STA);
            Serial.println("[SETUP] No se pudo iniciar la red temporal.");
            return;
        }
        char token[33];
        snprintf(token, sizeof(token), "%08lx%08lx%08lx%08lx",
                 static_cast<unsigned long>(esp_random()), static_cast<unsigned long>(esp_random()),
                 static_cast<unsigned long>(esp_random()), static_cast<unsigned long>(esp_random()));
        sessionToken = token;
        waitingForConnection = false;
        closing = false;
        active = true;
        // "*" significa cualquier nombre: las consultas DNS reciben 192.168.4.1.
        // El DNS solo permanece activo mientras está abierto el portal.
        dnsServer.setTTL(0); // Evita conservar estas respuestas al volver al Wi-Fi doméstico.
        if (!dnsServer.start(53, "*", portalAddress)) {
            Serial.println("[SETUP] DNS no disponible. Abre http://192.168.4.1 manualmente.");
        }
        server.begin();
    }
    automaticStartPending = false;
    Serial.print("[SETUP] Red temporal: ");
    Serial.println(AppConfig::SETUP_NETWORK_NAME);
    // Es la clave del portal, no la contraseña de la red doméstica.
    Serial.print("[SETUP] Clave de acceso: ");
    Serial.println(AppConfig::SETUP_NETWORK_PASSWORD);
    Serial.println("[SETUP] Desde esa red abre http://192.168.4.1");
    Serial.println("[SETUP] El celular puede ofrecer 'Iniciar sesion en la red' para abrir el formulario.");
}

void stop() {
    automaticStartPending = false;
    if (!active) {
        return;
    }
    server.stop();
    dnsServer.stop();
    WiFi.softAPdisconnect(false);
    WiFi.mode(WIFI_STA); // Mantiene la conexión del cliente con el router.
    active = false;
    closing = false;
    waitingForConnection = false;
    sessionToken = "";
    Serial.println("[SETUP] Portal y red temporal cerrados.");
}

void update() {
    // En el arranque esperamos el resultado de la conexión guardada. Si falla o
    // no hay configuración, abrimos el portal. No lo reabrimos en cada vuelta.
    if (automaticStartPending && !WifiController::isConnecting() && !WifiController::isScanning()) {
        automaticStartPending = false;
        if (!WifiController::isConnected()) {
            start();
        }
    }
    if (!active) {
        return;
    }
    if (waitingForConnection && WifiController::isConnected() &&
        WifiController::wasConfigurationSaved() && !closing) {
        closing = true;
        closingStarted = millis();
        Serial.println("[SETUP] Configuracion lista. El portal se cerrara en 15 segundos.");
    }
    if (closing && !WifiController::isConnected()) {
        closing = false; // Si se pierde la conexión, dejamos disponible el formulario.
    }
    // En Arduino-ESP32 3.3.12, DNSServer atiende por eventos de forma asíncrona;
    // processNextRequest() se conserva por compatibilidad con otras versiones.
    dnsServer.processNextRequest();
    // Atiende una petición pendiente. Esta llamada necesita repetirse desde loop().
    server.handleClient();
    if (closing && millis() - closingStarted >= AppConfig::PORTAL_CLOSE_DELAY_MS) {
        stop();
    }
}

bool isActive() { return active; }
} // namespace SetupPortal
