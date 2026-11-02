#include "CredentialStore.h" // Comillas para los archivos de nuestro proyecto.
#include <Preferences.h> // Ángulos para archivos de las bibliotecas.
#include <cstring>

// Un namespace sin nombre limita estos datos a este archivo .cpp.
// Así main.cpp y WifiController.cpp no manipulan Preferences directamente.
namespace {
Preferences preferences;
bool storageReady = false;
}

namespace CredentialStore {
bool begin() {
    // No cambiamos el nombre del espacio ni de la clave usados en la versión anterior.
    storageReady = preferences.begin("railway-wifi", false);
    return storageReady;
}

bool load(WifiCredentials& credentials) {
    // Leemos primero en una variable temporal para no entregar datos incompletos.
    WifiCredentials stored = {};
    if (!storageReady || preferences.getBytesLength("credentials") != sizeof(stored) ||
        preferences.getBytes("credentials", &stored, sizeof(stored)) != sizeof(stored)) {
        return false;
    }
    // Antes de usar strlen(), comprobamos que ambos textos tengan terminador.
    if (memchr(stored.ssid, '\0', sizeof(stored.ssid)) == nullptr ||
        memchr(stored.password, '\0', sizeof(stored.password)) == nullptr ||
        strlen(stored.ssid) == 0 || strlen(stored.password) < 8) {
        return false;
    }
    credentials = stored;
    return true;
}

bool save(const WifiCredentials& credentials) {
    // Guardamos la estructura completa en una sola entrada de la memoria NVS.
    return storageReady && preferences.putBytes("credentials", &credentials,
                                                sizeof(credentials)) == sizeof(credentials);
}

bool clear() {
    return storageReady && preferences.clear();
}

} // namespace CredentialStore
