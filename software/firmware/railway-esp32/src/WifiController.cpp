#include "WifiController.h"
#include "AppConfig.h"
#include "CredentialStore.h"
#include <WiFi.h>
#include <cstring>

namespace {
// Estas variables solo se usan dentro de este .cpp; otros archivos llaman a las funciones públicas.
WifiCredentials credentials = {};
bool connecting = false;
bool wasConnected = false;
bool saveAfterConnect = false;
bool scanning = false;
unsigned long connectionStarted = 0;
String networksJson = "[]";
const char* scanState = "idle";
const char* connectionState = "DISCONNECTED";
bool configurationSaved = false;

// Protege comillas, barras y caracteres de control al convertir un SSID a JSON.
// El nombre de una red es un dato externo; no debe romper la respuesta al navegador.
String quoteJson(const String& value) {
    String result = "\"";
    for (size_t i = 0; i < value.length(); i++) {
        const unsigned char character = static_cast<unsigned char>(value[i]);
        if (character == '"' || character == '\\') {
            result += '\\';
            result += static_cast<char>(character);
        } else if (character < 32) {
            char escaped[7];
            snprintf(escaped, sizeof(escaped), "\\u%04x", character);
            result += escaped;
        } else {
            result += static_cast<char>(character);
        }
    }
    return result + '"';
}
}

namespace WifiController {
// Devuelve true si hay un nombre y una contraseña con las longitudes admitidas.
static bool hasCredentials() {
    return strlen(credentials.ssid) > 0 && strlen(credentials.password) >= 8;
}

static void stopWifi() {
    connecting = false;
    wasConnected = false;
    saveAfterConnect = false;
    configurationSaved = false;
    connectionState = "DISCONNECTED";
    WiFi.disconnect();
}

// Un parámetro es un dato que recibe una función. Aquí decide si debemos guardar
// las credenciales después de comprobar que permiten obtener una conexión.
void connect(bool saveOnSuccess) {
    if (!hasCredentials()) {
        Serial.println("[WIFI] Configura primero wifi ssid y wifi password.");
        return; // Sale de esta función sin ejecutar el resto.
    }

    stopWifi();
    WiFi.begin(credentials.ssid, credentials.password);
    connectionStarted = millis();
    connecting = true;
    connectionState = "CONNECTING";
    saveAfterConnect = saveOnSuccess;
    Serial.println("[WIFI] Connecting... (maximo 15 segundos)");
}

static void updateWifi() {
    // == compara dos valores; = (uno solo) asigna un valor.
    const bool connected = WiFi.status() == WL_CONNECTED;
    if (connecting && connected) { // && significa que ambas condiciones deben cumplirse.
        connecting = false;
        wasConnected = true;
        connectionState = "CONNECTED";
        Serial.print("[WIFI] Connected | IP: ");
        Serial.println(WiFi.localIP());

        if (saveAfterConnect) {
            // Delegamos el guardado al módulo que conoce cómo usar Preferences.
            if (CredentialStore::save(credentials)) {
                configurationSaved = true;
                Serial.println("[WIFI] Configuracion guardada para el proximo arranque.");
            } else {
                connectionState = "SAVE_FAILED";
                Serial.println("[ERROR] Conectado, pero no se pudo guardar la configuracion.");
            }
            saveAfterConnect = false;
        } else {
            configurationSaved = true; // Esta conexión usa las credenciales leídas de NVS.
        }
    } else if (connecting && millis() - connectionStarted >= AppConfig::WIFI_TIMEOUT_MS) {
        stopWifi();
        connectionState = "FAILED";
        Serial.println("[WIFI] Tiempo agotado. Revisa red y clave; usa wifi connect para reintentar.");
    } else if (wasConnected && !connected) {
        wasConnected = false;
        configurationSaved = false;
        connectionState = "DISCONNECTED";
        Serial.println("[WIFI] Conexion perdida. Usa wifi connect para reintentar.");
    }
}

void startScan() {
    if (scanning || connecting) { // || significa que basta con que una condición se cumpla.
        Serial.println("[WIFI] Espera a que termine la busqueda o el intento de conexion.");
        return;
    }

    // Elimina los resultados de la búsqueda anterior para liberar memoria.
    WiFi.scanDelete();
    // true activa la búsqueda asíncrona: empieza ahora y recogemos el resultado
    // en loop(), sin detener los latidos ni la lectura de comandos.
    const int result = WiFi.scanNetworks(true);
    if (result == WIFI_SCAN_FAILED) {
        scanState = "error";
        Serial.println("[WIFI] No se pudo iniciar la busqueda. Reintenta wifi scan.");
        return;
    }
    scanning = true;
    scanState = "scanning";
    networksJson = "[]";
    Serial.println("[WIFI] Buscando redes de 2,4 GHz...");
}

static void updateWifiScan() {
    if (!scanning) {
        return;
    }
    // scanComplete() devuelve un número de redes, o un valor especial negativo
    // para indicar que la búsqueda sigue en curso o que ha fallado.
    const int networks = WiFi.scanComplete();
    if (networks == WIFI_SCAN_RUNNING) {
        return; // Lo volveremos a consultar en la siguiente vuelta de loop().
    }
    scanning = false;
    scanState = networks < 0 ? "error" : "done";
    networksJson = "[";
    if (networks < 0) {
        Serial.println("[WIFI] La busqueda fallo. Reintenta wifi scan.");
    } else if (networks == 0) {
        Serial.println("[WIFI] No se encontraron redes visibles de 2,4 GHz.");
    } else {
        Serial.println("[WIFI] Nombre de red (SSID) | Senal | Seguridad");
        for (int i = 0; i < networks; i++) {
            // Limitamos a las primeras 40 entradas para acotar el uso de RAM del portal.
            if (i < 40) {
                if (i > 0) {
                    networksJson += ',';
                }
                networksJson += "{\"ssid\":" + quoteJson(WiFi.SSID(i));
                networksJson += ",\"rssi\":" + String(WiFi.RSSI(i));
                networksJson += ",\"open\":";
                networksJson += WiFi.encryptionType(i) == WIFI_AUTH_OPEN ? "true}" : "false}";
            }
            Serial.print("  ");
            Serial.print(WiFi.SSID(i)); // SSID es el nombre que identifica una red.
            Serial.print(" | ");
            Serial.print(WiFi.RSSI(i)); // Intensidad en dBm: -40 es más fuerte que -80.
            Serial.print(" dBm | ");
            Serial.println(WiFi.encryptionType(i) == WIFI_AUTH_OPEN ? "abierta" : "protegida");
        }
        Serial.println("[WIFI] Copia el nombre en wifi ssid <nombre>; luego configura la clave.");
    }
    networksJson += ']';
    WiFi.scanDelete(); // Ya mostramos los datos; liberamos la memoria del resultado.
}

bool isScanning() {
    return scanning;
}

bool setSsid(const String& value) {
    // Recibimos solo el nombre; main.cpp ya quitó el prefijo "wifi ssid ".
    // No usamos trim(): los espacios pueden ser parte del nombre o contraseña.
    if (value.length() == 0 || value.length() > 32) {
        Serial.println("[ERROR] El nombre de red debe tener de 1 a 32 bytes.");
        return false;
    }
    stopWifi();
    credentials = {}; // Cambiar la red también borra la contraseña pendiente.
    value.toCharArray(credentials.ssid, sizeof(credentials.ssid));
    Serial.println("[USB] SSID preparado. Ahora introduce wifi password <contrasena>.");
    return true;
}

bool isPasswordValid(const String& value) {
    bool valid = value.length() >= 8 && value.length() <= 63;
    // for recorre el texto. i++ incrementa i en uno después de cada vuelta.
    for (size_t i = 0; i < value.length(); i++) {
        if (value[i] < 32 || value[i] > 126) {
            valid = false; // Solo caracteres ASCII imprimibles en esta versión.
        }
    }
    return valid;
}

bool setPassword(const String& value) {
    if (strlen(credentials.ssid) == 0) {
        Serial.println("[ERROR] Introduce primero wifi ssid <nombre de red>.");
        return false;
    }
    if (!isPasswordValid(value)) {
        Serial.println("[ERROR] Usa una clave de 8 a 63 caracteres ASCII imprimibles.");
        return false;
    }
    stopWifi();
    value.toCharArray(credentials.password, sizeof(credentials.password));
    Serial.println("[USB] Contrasena preparada (oculta). Ejecuta wifi connect.");
    return true;
}

void printStatus() {
    // ?: elige el primer texto si la condición es verdadera y el segundo si no.
    Serial.print("[WIFI] Estado: ");
    Serial.println(connecting ? "CONNECTING" :
                   (WiFi.status() == WL_CONNECTED ? "CONNECTED" : "DISCONNECTED"));
    Serial.print("[WIFI] Configuracion en memoria RAM: ");
    Serial.println(hasCredentials() ? "lista" : "incompleta");
    if (WiFi.status() == WL_CONNECTED) {
        Serial.print("[WIFI] IP: ");
        Serial.println(WiFi.localIP());
    }
}

void forget() {
    stopWifi();
    credentials = {};
    if (CredentialStore::clear()) {
        Serial.println("[WIFI] Configuracion borrada de RAM y memoria persistente.");
    } else {
        Serial.println("[ERROR] RAM borrada; no se pudo borrar la memoria persistente.");
    }
}

void begin() {
    WiFi.persistent(false); // Solo CredentialStore guarda las credenciales.
    WiFi.setHostname(AppConfig::DEVICE_ID);
    WiFi.mode(WIFI_STA);
    WiFi.setAutoReconnect(false);
    if (!CredentialStore::begin()) {
        Serial.println("[ERROR] No se pudo abrir la memoria de configuracion.");
        return;
    }
    if (CredentialStore::load(credentials)) {
        Serial.println("[WIFI] Configuracion guardada encontrada.");
        connect(false);
    } else {
        credentials = {};
        Serial.println("[WIFI] Sin configuracion valida. Configura la red por USB.");
    }
}

void update() {
    updateWifiScan();
    updateWifi();
}

bool isConnecting() { return connecting; }
bool isConnected() { return WiFi.status() == WL_CONNECTED; }
const String& getNetworksJson() { return networksJson; }
const char* getScanState() { return scanState; }
const char* getConnectionState() { return connectionState; }
bool wasConfigurationSaved() { return configurationSaved; }
} // namespace WifiController
