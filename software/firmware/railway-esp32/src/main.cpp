// main.cpp coordina el programa. La conexión y la memoria viven en otros archivos.
#include <Arduino.h>
#include "AppConfig.h"
#include "WifiController.h"
#include "SetupPortal.h"
#include "OccupancySensor.h"
#include "I2cBus.h"

// Estado propio de la consola USB y del latido.
unsigned long lastHeartbeat = 0;
String commandBuffer;
bool discardCommand = false;

void printHelp() {
    Serial.println("[USB] Comandos disponibles (terminar con Enter):");
    Serial.println("  wifi scan  (listar redes cercanas de 2,4 GHz)");
    Serial.println("  wifi ssid <nombre de red>");
    Serial.println("  wifi password <contrasena de 8 a 63 caracteres ASCII>");
    Serial.println("  wifi connect  |  wifi status  |  wifi forget  |  help");
    Serial.println("  wifi setup  |  wifi setup stop  (abrir/cerrar el portal web)");
    Serial.println("  sensor status  (consultar los reeds en MCP23017 GPB0 y GPB1)");
    Serial.println("  i2c scan  (comprobar direcciones del MCP23017 y PCA9685)");
    Serial.println("[USB] Sin comillas. Configura SSID y luego password; connect prueba y guarda.");
}

// const String& permite consultar el texto recibido sin copiarlo ni modificarlo.
void handleCommand(const String& command) {
    // Evita cambiar la conexión mientras la radio está ocupada buscando redes.
    // La consulta de estado y la ayuda siguen disponibles.
    if (WifiController::isScanning() && command.length() > 0 && command != "help" &&
        command != "wifi status" && command != "wifi scan" && command != "sensor status" &&
        command != "i2c scan") {
        Serial.println("[WIFI] Espera a que termine la busqueda antes de cambiar la configuracion.");
        return;
    }
    if (command == "help") {
        printHelp();
    } else if (command == "sensor status") {
        OccupancySensor::printStatus();
    } else if (command == "i2c scan") {
        I2cBus::startScan();
    } else if (command == "wifi scan") {
        WifiController::startScan();
    } else if (command.startsWith("wifi ssid ")) {
        // substring(10) quita "wifi ssid " y entrega solo el nombre al controlador.
        WifiController::setSsid(command.substring(10));
    } else if (command.startsWith("wifi password ")) {
        WifiController::setPassword(command.substring(14));
    } else if (command == "wifi connect") {
        WifiController::connect(true);
    } else if (command == "wifi status") {
        WifiController::printStatus();
    } else if (command == "wifi setup") {
        SetupPortal::start();
    } else if (command == "wifi setup stop") {
        SetupPortal::stop();
    } else if (command == "wifi forget") {
        WifiController::forget();
    } else if (command.length() > 0) {
        // No repetimos el texto recibido: podría contener una contraseña.
        Serial.println("[ERROR] Comando desconocido. Escribe help.");
    }
}

void readSerialCommands() {
    // available() indica cuántos bytes hay por leer. El límite evita que una entrada
    // continua ocupe toda la ejecución y retrase el latido o el control del Wi-Fi.
    for (size_t count = 0; count < 64 && Serial.available() > 0; count++) {
        const char character = static_cast<char>(Serial.read()); // Convierte a carácter.
        if (character == '\n' || character == '\r') { // Fin de línea: LF o CR.
            if (!discardCommand) { // ! significa "no".
                handleCommand(commandBuffer);
            }
            commandBuffer = "";
            discardCommand = false;
        } else if (character == '\b' || character == 127) { // Tecla de borrar.
            if (commandBuffer.length() > 0) {
                commandBuffer.remove(commandBuffer.length() - 1);
            }
        } else if (!discardCommand) {
            if (commandBuffer.length() >= AppConfig::MAX_COMMAND_LENGTH) {
                commandBuffer = "";
                discardCommand = true; // Descarta la línea completa si es muy larga.
                Serial.println("[ERROR] Comando demasiado largo. Vuelve a escribirlo.");
            } else {
                commandBuffer += character; // += añade un carácter al texto acumulado.
            }
        }
    }
}

// Una función agrupa instrucciones. () contiene sus parámetros; aquí no hay ninguno.
// void significa que la función no devuelve un resultado a quien la llama.
// Las llaves { y } delimitan el cuerpo de la función.
// Arduino llama a setup() una sola vez al encender o reiniciar el ESP32.
void setup() {
    // Serial es el objeto que usamos para comunicarnos con el monitor serie.
    // El punto . permite llamar a una de sus funciones, en este caso begin().
    // El valor entre paréntesis indica la velocidad: el monitor debe usar la misma.
    Serial.begin(AppConfig::SERIAL_BAUD_RATE);

    // Pausa el programa 1000 milisegundos para dar un margen al iniciar el puerto.
    delay(1000);

    // println() escribe y después pasa a la siguiente línea.
    Serial.println(); // Sin texto, solo agrega un salto de línea.
    Serial.println("==========================="); // Separador visual.
    Serial.println(" Railway ESP32 Controller"); // Nombre del programa.
    Serial.println("===========================");
    Serial.println("[BOOT] Firmware started"); // Mensaje de arranque.

    // print() escribe sin saltar de línea: el identificador aparecerá a continuación.
    Serial.print("[INFO] Device: ");
    Serial.println(AppConfig::DEVICE_ID); // Escribe el valor de la constante y termina la línea.

    Serial.print("[INFO] Version: ");
    Serial.println(AppConfig::FIRMWARE_VERSION); // Muestra la versión del firmware.

    // Es un mensaje definido por nosotros: indica que llegamos a este punto.
    Serial.println("[INFO] Status: RUNNING");

    printHelp();
    I2cBus::begin();
    OccupancySensor::begin();
    WifiController::begin();
    SetupPortal::begin();

    // millis() devuelve los milisegundos transcurridos desde el inicio del programa.
    // Guardamos el momento actual para esperar 5 segundos antes del primer latido.
    lastHeartbeat = millis();
}

// Después de setup(), Arduino llama a loop() una y otra vez mientras está encendido.
void loop() {
    OccupancySensor::update();
    I2cBus::update();
    // Leer comandos y comprobar el Wi-Fi en cada vuelta permite seguir atendiendo
    // la consola y enviando latidos incluso mientras intentamos conectarnos.
    readSerialCommands();
    WifiController::update();
    SetupPortal::update();

    // const impide modificar esta variable después de inicializarla.
    // now es una variable local: se crea en cada llamada y solo existe en esta función.
    const unsigned long now = millis();

    // if ejecuta el bloque entre llaves solo si se cumple la condición entre paréntesis.
    // La resta calcula cuánto tiempo ha pasado desde el último mensaje.
    // >= significa "mayor o igual que": aquí preguntamos si ya pasaron 5000 ms.
    // Con unsigned long, esta resta también funciona cuando millis() vuelve a cero.
    if (now - lastHeartbeat >= AppConfig::HEARTBEAT_INTERVAL_MS) {
        // Actualiza el momento del último latido para empezar a contar el siguiente.
        lastHeartbeat = now;

        // Heartbeat significa "latido": permite observar que el bucle sigue ejecutándose.
        // Este mensaje no comprueba la red, los sensores ni los servos.
        Serial.print("[HEARTBEAT] Uptime: ");

        // / divide: convertimos milisegundos a segundos.
        // UL indica que 1000 es unsigned long. La división entera descarta decimales.
        Serial.print(now / 1000UL);
        Serial.println(" seconds"); // Completa el mensaje y pasa a otra línea.
    }

    // Pausa 1 ms y permite que el sistema atienda otras tareas.
    // La espera de 5 segundos se controla con millis(), sin una pausa larga en loop().
    delay(1);
}
