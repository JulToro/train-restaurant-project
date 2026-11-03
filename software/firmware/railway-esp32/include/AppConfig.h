#pragma once // Evita incluir este archivo más de una vez en una misma compilación de .cpp.

#include <Arduino.h>

// namespace agrupa nombres para indicar a qué parte del programa pertenecen.
// Por ejemplo, AppConfig::SERIAL_BAUD_RATE accede a esta constante.
namespace AppConfig {
constexpr unsigned long SERIAL_BAUD_RATE = 115200;
constexpr unsigned long HEARTBEAT_INTERVAL_MS = 5000;
constexpr unsigned long WIFI_TIMEOUT_MS = 15000;
constexpr size_t MAX_COMMAND_LENGTH = 128;
constexpr char DEVICE_ID[] = "railway-esp32-01";
constexpr char FIRMWARE_VERSION[] = "0.5.1";
constexpr int I2C_SDA_PIN = 21;
constexpr int I2C_SCL_PIN = 22;
constexpr uint32_t I2C_FREQUENCY_HZ = 100000;
constexpr uint16_t I2C_TIMEOUT_MS = 25;
constexpr uint8_t MCP23017_ADDRESS = 0x27; // Direccion observada en el montaje del usuario.
constexpr uint8_t PCA9685_ADDRESS = 0x40;
constexpr size_t OCCUPANCY_SENSOR_COUNT = 2;
constexpr uint8_t OCCUPANCY_SENSOR_PINS[] = {0, 1}; // GPB0 y GPB1 (PB0/PB1 en la placa).
constexpr const char* OCCUPANCY_SENSOR_NAMES[] = {"entrada", "salida"};
constexpr uint8_t OCCUPANCY_SENSOR_ACTIVE_LEVEL = LOW;
constexpr unsigned long SENSOR_DEBOUNCE_MS = 30;
constexpr unsigned long SENSOR_POLL_INTERVAL_MS = 5;
constexpr unsigned long SENSOR_RETRY_INTERVAL_MS = 1000;
constexpr char SETUP_NETWORK_NAME[] = "Railway-Setup";
// Clave fija solo para entrar al portal de configuración, no para el Wi-Fi doméstico.
constexpr char SETUP_NETWORK_PASSWORD[] = "Railway123";
constexpr unsigned long PORTAL_CLOSE_DELAY_MS = 15000; // Margen para leer la confirmación.
} // namespace AppConfig
