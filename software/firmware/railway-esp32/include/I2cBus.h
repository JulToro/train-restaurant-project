#pragma once

namespace I2cBus {
void begin();
bool isReady();
void startScan();
void update(); // Consulta una direccion por vuelta para seguir atendiendo el programa.
}
