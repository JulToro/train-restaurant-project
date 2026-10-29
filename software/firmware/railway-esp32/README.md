# Firmware base del ESP32

Primera tarea del [issue #52](https://github.com/JulToro/train-restaurant-project/issues/52): proyecto PlatformIO con Arduino Framework, identificación por consola y señal periódica de ejecución.

## Configuración

- Placa: ESP32 Dev Module (`esp32dev`), usada previamente con Arduino IDE.
- Plataforma: Espressif 32 fijada en `6.12.0` para repetir la compilación.
- Arduino Framework: 2.0.17, incluido por esa plataforma. Es una versión distinta del paquete 3.3.12 registrado en las pruebas previas con Arduino IDE.
- Monitor serie: 115200 baudios.
- `platformio.ini`: placa, framework y parámetros de compilación.
- `src/main.cpp`: funciones `setup()` y `loop()`.
- `.gitignore`: excluye dependencias y archivos generados.

Referencias: [placa esp32dev](https://docs.platformio.org/en/latest/boards/espressif32/esp32dev.html), [plataforma 6.12.0](https://github.com/platformio/platform-espressif32/releases/tag/v6.12.0).

## Compilar y cargar desde VS Code

1. Instala la extensión oficial **PlatformIO IDE** de PlatformIO. Incluye PlatformIO Core; no requiere instalarlo por separado. [Guía oficial](https://docs.platformio.org/en/latest/integration/ide/vscode.html).
2. Abre esta carpeta (`software/firmware/railway-esp32`) en una ventana de VS Code. El proyecto ya está creado; no necesitas usar New Project.
3. Espera a que PlatformIO termine de inicializarse y descargar las herramientas. La primera compilación requiere acceso a Internet.
4. En PlatformIO > Project Tasks > esp32dev > General, ejecuta **Build**. Espera el resultado `SUCCESS`.
5. Conecta el ESP32 por USB y cierra el monitor serie de Arduino IDE si está abierto.
6. Ejecuta **Upload** y espera `SUCCESS`. Esta operación reemplaza el programa cargado previamente en la placa.
7. Ejecuta **Monitor**. Pulsa EN/RESET para volver a ver el mensaje de arranque.

Si hay varios dispositivos conectados, selecciona el puerto correspondiente. También puedes indicar `--upload-port` o `--port` en los comandos de carga o monitor, respectivamente.

Desde una terminal de PlatformIO, ubicada en esta carpeta, los comandos equivalentes son:

```sh
pio run
pio run --target upload
pio device monitor --baud 115200
```

Salida esperada:

```text
=== Railway ESP32 ===
Dispositivo: railway-esp32-01
Firmware: 0.1.0
Estado: iniciado
HEALTH OK | uptime_s=6
HEALTH OK | uptime_s=11
```

Los tiempos son aproximados. `HEALTH OK` indica que el bucle del firmware sigue ejecutándose; no comprueba sensores, actuadores ni conectividad.

## Evidencia y alcance

Guarda el resultado de Build, Upload y el arranque seguido de al menos dos mensajes periódicos. La compilación, carga y ejecución física deben verificarse antes de dar esta tarea por terminada.

Esta base todavía no implementa red ni comunicación con .NET. La configuración externa de red queda para la siguiente tarea del issue #52. El programa no acciona el PCA9685 ni realiza pruebas de servo.

Los archivos quedan dentro del repositorio para su revisión y posterior commit/push a GitHub.
