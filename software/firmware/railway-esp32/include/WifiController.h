#pragma once

#include <Arduino.h>

namespace WifiController {
// Este archivo describe las operaciones disponibles, sin exponer sus variables internas.
void begin(); // Prepara Wi-Fi y lee la configuración guardada.
void update(); // Atiende búsqueda y conexión sin bloquear loop().
void startScan();
bool isScanning();
bool setSsid(const String& value);
bool setPassword(const String& value);
void connect(bool saveOnSuccess);
void printStatus();
void forget();
bool isConnecting();
bool isConnected();
bool isPasswordValid(const String& value);
// JSON es un formato de texto para intercambiar datos con el JavaScript de la página.
const String& getNetworksJson();
const char* getScanState();
const char* getConnectionState();
bool wasConfigurationSaved();
} // namespace WifiController
