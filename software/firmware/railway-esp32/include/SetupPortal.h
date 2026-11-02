#pragma once

namespace SetupPortal {
void begin(); // Registra las direcciones web y prepara el arranque automático.
void update(); // Atiende el navegador; se llama en cada vuelta de loop().
void start(); // Crea la red temporal y abre el servidor.
void stop(); // Cierra el servidor y la red temporal.
bool isActive();
} // namespace SetupPortal
