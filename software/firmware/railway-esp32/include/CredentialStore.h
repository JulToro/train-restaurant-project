#pragma once

#include <Arduino.h>

// Los tamaños incluyen una posición para '\0', el carácter que termina el texto.
// Conservamos el mismo formato para leer la configuración que ya tiene tu placa.
struct WifiCredentials {
    char ssid[33];
    char password[64];
};

namespace CredentialStore {
// Estas declaraciones anuncian funciones cuyo código está en CredentialStore.cpp.
// bool devuelve true si la operación tuvo éxito y false si falló.
bool begin();
// & permite rellenar la estructura del código que nos llama, sin hacer una copia.
bool load(WifiCredentials& credentials);
// const & permite leer los datos sin copiarlos y sin modificarlos.
bool save(const WifiCredentials& credentials);
bool clear();
} // namespace CredentialStore
