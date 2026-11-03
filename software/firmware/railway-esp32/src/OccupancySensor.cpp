#include "OccupancySensor.h"
#include "AppConfig.h"
#include "I2cBus.h"
#include <Wire.h>

namespace {
struct SensorState {
    bool candidate = false;
    bool stable = false;
    bool sampled = false;
    bool ready = false;
    unsigned long since = 0;
};
SensorState sensors[AppConfig::OCCUPANCY_SENSOR_COUNT];
bool connected = false;
unsigned long lastPoll = 0;
unsigned long lastAttempt = 0;
constexpr uint8_t INPUT_MASK = 0x03; // GPB0 y GPB1.

bool writeRegister(uint8_t reg, uint8_t value) {
    Wire.beginTransmission(AppConfig::MCP23017_ADDRESS);
    Wire.write(reg);
    Wire.write(value);
    return Wire.endTransmission() == 0;
}

bool readRegister(uint8_t reg, uint8_t& value) {
    Wire.beginTransmission(AppConfig::MCP23017_ADDRESS);
    Wire.write(reg);
    if (Wire.endTransmission(false) != 0) return false;
    if (Wire.requestFrom(AppConfig::MCP23017_ADDRESS, static_cast<uint8_t>(1)) != 1) return false;
    value = static_cast<uint8_t>(Wire.read());
    return true;
}

bool configure() {
    if (!I2cBus::isReady()) return false;
    // Normaliza BANK=0: en BANK=1, 0x05 es IOCON; en BANK=0 es GPINTENB.
    if (!writeRegister(0x05, 0x00) || !writeRegister(0x0A, 0x00)) return false;
    // El módulo administra todo el expansor. GPA7/GPB7 se dejan como salidas bajas
    // según la ficha actual; las otras entradas no usadas quedan sin pull-up.
    if (!writeRegister(0x14, 0x00) || !writeRegister(0x15, 0x00) ||
        !writeRegister(0x00, 0x7F) || !writeRegister(0x01, 0x7F) ||
        !writeRegister(0x03, 0x00) || !writeRegister(0x04, 0x00) ||
        !writeRegister(0x0C, 0x00) || !writeRegister(0x0D, INPUT_MASK)) return false;
    uint8_t direction = 0, pullups = 0;
    return readRegister(0x01, direction) && direction == 0x7F &&
           readRegister(0x0D, pullups) && pullups == INPUT_MASK;
}

void printSensor(size_t index) {
    const auto& sensor = sensors[index];
    const char* state = !connected ? "DESCONOCIDO (MCP23017 no disponible)" :
                        !sensor.ready ? "PENDIENTE (estabilizando lectura)" :
                        sensor.stable ? "ACTIVO (iman detectado)" : "INACTIVO (iman no detectado)";
    Serial.printf("[SENSOR] %s (GPB%u): %s\n", AppConfig::OCCUPANCY_SENSOR_NAMES[index],
                  AppConfig::OCCUPANCY_SENSOR_PINS[index], state);
}

void invalidate() {
    connected = false;
    for (auto& sensor : sensors) sensor = SensorState{};
    OccupancySensor::printStatus();
}
}

namespace OccupancySensor {
void begin() {
    for (auto& sensor : sensors) sensor = SensorState{};
    lastAttempt = millis();
    lastPoll = lastAttempt;
    connected = configure();
    if (connected) Serial.println("[SENSOR] MCP23017: dos entradas digitales con pull-up (GPB0 y GPB1).");
    printStatus();
}

void update() {
    const unsigned long now = millis();
    if (!connected) {
        if (now - lastAttempt < AppConfig::SENSOR_RETRY_INTERVAL_MS) return;
        lastAttempt = now;
        if (!configure()) return;
        connected = true;
        Serial.println("[SENSOR] Comunicacion recuperada; estabilizando ambos sensores.");
        printStatus();
    }
    if (now - lastPoll < AppConfig::SENSOR_POLL_INTERVAL_MS) return;
    lastPoll = now;
    uint8_t direction = 0, pullups = 0, port = 0;
    // Detecta también un reinicio del MCP que haya deshabilitado los pull-ups.
    if (!readRegister(0x01, direction) || direction != 0x7F ||
        !readRegister(0x0D, pullups) || pullups != INPUT_MASK || !readRegister(0x13, port)) {
        lastAttempt = millis();
        invalidate();
        return;
    }
    const unsigned long sampledAt = millis();
    for (size_t i = 0; i < AppConfig::OCCUPANCY_SENSOR_COUNT; i++) {
        auto& sensor = sensors[i];
        const bool active = ((port >> AppConfig::OCCUPANCY_SENSOR_PINS[i]) & 1) ==
                            AppConfig::OCCUPANCY_SENSOR_ACTIVE_LEVEL;
        if (!sensor.sampled || sensor.candidate != active) {
            sensor.sampled = true;
            sensor.candidate = active;
            sensor.since = sampledAt;
        }
        if ((!sensor.ready || sensor.stable != sensor.candidate) &&
            sampledAt - sensor.since >= AppConfig::SENSOR_DEBOUNCE_MS) {
            sensor.stable = sensor.candidate;
            sensor.ready = true;
            printSensor(i);
        }
    }
}

void printStatus() {
    for (size_t i = 0; i < AppConfig::OCCUPANCY_SENSOR_COUNT; i++) printSensor(i);
}
bool isReady(size_t index) {
    return index < AppConfig::OCCUPANCY_SENSOR_COUNT && connected && sensors[index].ready;
}
bool isActive(size_t index) { return isReady(index) && sensors[index].stable; }
}
