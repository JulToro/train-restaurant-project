#include "I2cBus.h"
#include "AppConfig.h"
#include <Wire.h>

namespace {
bool initialized = false;
bool scanning = false;
uint8_t nextAddress = 8;
unsigned int found = 0;
unsigned int errors = 0;
bool mcpResponded = false;
bool pcaResponded = false;
}

namespace I2cBus {
bool isReady() { return initialized; }
void startScan() {
    if (!initialized) {
        Serial.println("[I2C] Bus no disponible; revisa el arranque.");
        return;
    }
    if (scanning) {
        Serial.println("[I2C] Busqueda en curso.");
        return;
    }
    nextAddress = 8;
    found = 0;
    errors = 0;
    mcpResponded = false;
    pcaResponded = false;
    scanning = true;
    Serial.println("[I2C] Buscando dispositivos: SDA=21, SCL=22, 100 kHz.");
}

void begin() {
    initialized = Wire.begin(AppConfig::I2C_SDA_PIN, AppConfig::I2C_SCL_PIN,
                             AppConfig::I2C_FREQUENCY_HZ);
    if (!initialized) {
        Serial.println("[ERROR] No se pudo iniciar el bus I2C.");
        return;
    }
    Wire.setTimeOut(AppConfig::I2C_TIMEOUT_MS);
    startScan();
}

void update() {
    if (!scanning) {
        return;
    }
    Wire.beginTransmission(nextAddress);
    const uint8_t result = Wire.endTransmission();
    if (result == 0) {
        found++;
        Serial.printf("[I2C] Respuesta en 0x%02X\n", nextAddress);
        if (nextAddress == AppConfig::MCP23017_ADDRESS) mcpResponded = true;
        if (nextAddress == AppConfig::PCA9685_ADDRESS) pcaResponded = true;
    } else if (result != 2) { // 2 = direccion sin respuesta (NACK).
        errors++;
        Serial.printf("[I2C] Error %u al consultar 0x%02X\n", result, nextAddress);
    }
    nextAddress++;
    if (nextAddress == 120) { // Consulta 0x08..0x77; excluye rangos reservados.
        scanning = false;
        Serial.printf("[I2C] Fin: %u direcciones responden; %u errores de bus.\n", found, errors);
        Serial.printf("[I2C] MCP23017 esperado en 0x%02X: %s\n",
                      AppConfig::MCP23017_ADDRESS, mcpResponded ? "RESPONDE" : "SIN RESPUESTA");
        Serial.printf("[I2C] PCA9685 esperado en 0x%02X: %s\n",
                      AppConfig::PCA9685_ADDRESS, pcaResponded ? "RESPONDE" : "SIN RESPUESTA");
        Serial.println("[I2C] Una respuesta confirma direccion, no modelo ni cableado de sensores.");
    }
}
}
