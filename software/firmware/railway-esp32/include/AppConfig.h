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
constexpr char FIRMWARE_VERSION[] = "0.1.0";
constexpr char SETUP_NETWORK_NAME[] = "Railway-Setup";
// Clave fija solo para entrar al portal de configuración, no para el Wi-Fi doméstico.
constexpr char SETUP_NETWORK_PASSWORD[] = "Railway123";
constexpr unsigned long PORTAL_CLOSE_DELAY_MS = 15000; // Margen para leer la confirmación.
} // namespace AppConfig
