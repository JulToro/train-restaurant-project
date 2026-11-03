#pragma once
#include <stddef.h>

namespace OccupancySensor {
void begin();
void update(); // Confirma estados estables sin detener el bucle.
bool isReady(size_t index); // Cada sensor confirma su estado por separado.
bool isActive(size_t index); // Consultar cuando isReady(index) sea true.
void printStatus();
} // namespace OccupancySensor
