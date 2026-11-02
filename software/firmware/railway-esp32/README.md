# Firmware base del ESP32

Firmware del [issue #52](https://github.com/JulToro/train-restaurant-project/issues/52): proyecto PlatformIO con Arduino Framework, identificación por consola, señal periódica de ejecución y configuración Wi-Fi por USB o formulario web local.

## Configuración

- Placa: ESP32 Dev Module (`esp32dev`), usada previamente con Arduino IDE.
- Plataforma: `espressif32`, actualmente sin versión fija en `platformio.ini`.
- Entorno observado en la carga reportada por el usuario: Espressif 32 8.0.0 y Arduino Framework 3.3.12.
- Monitor serie: 115200 baudios.
- `platformio.ini`: placa, framework y parámetros de compilación.
- `src/main.cpp`: funciones `setup()` y `loop()`.
- `.gitignore`: excluye dependencias y archivos generados.

Referencia: [placa esp32dev](https://docs.platformio.org/en/latest/boards/espressif32/esp32dev.html).

## Organización del código

La versión 0.2.2 separa el firmware existente en módulos, manteniendo los comandos USB y el formato de credenciales guardadas por 0.2.1.

| Archivo | Responsabilidad |
| --- | --- |
| `include/AppConfig.h` | Constantes: versión, identificador, velocidad y tiempos. |
| `include/CredentialStore.h` | Estructura `WifiCredentials` y declaraciones para leer, guardar y borrar. |
| `src/CredentialStore.cpp` | Implementación del almacenamiento con Preferences/NVS. |
| `include/WifiController.h` | Declaraciones de las operaciones Wi-Fi disponibles. |
| `src/WifiController.cpp` | Conexión, búsqueda de redes, validación y estado. |
| `src/main.cpp` | Arranque, comandos USB y latidos. |
| `include/SetupPortal.h` y `src/SetupPortal.cpp` | Red temporal, servidor HTTP y peticiones del formulario. |
| `src/SetupPage.h` | HTML, CSS y JavaScript almacenados en flash, sin dependencias externas. |

Un `.h` anuncia funciones y tipos a otros archivos. Un `.cpp` contiene las instrucciones de esas funciones. Los `namespace` agrupan nombres: `WifiController::begin()` llama a la función `begin` de ese módulo. Estos módulos usan funciones; todavía no son clases propias.

La versión 0.3.0 implementa el portal en esos archivos. `SetupPortal` coordina HTTP y el punto de acceso, `WifiController` conserva la lógica de conexión y búsqueda, y `CredentialStore` mantiene las credenciales en NVS. El navegador consulta respuestas JSON mediante `fetch()`.

Después de cargar 0.3.2, comprueba el arranque con la red guardada, `wifi status`, `wifi scan`, `help`, los latidos y el portal. No es necesario borrar las credenciales existentes para abrirlo.

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
[BOOT] Firmware started
[INFO] Device: railway-esp32-01
[INFO] Version: 0.3.2
[INFO] Status: RUNNING
[HEARTBEAT] Uptime: 6 seconds
[HEARTBEAT] Uptime: 11 seconds
```

Los tiempos son aproximados. `HEARTBEAT` indica que el bucle del firmware sigue ejecutándose; no comprueba sensores, actuadores ni conectividad.

## Configurar Wi-Fi por USB

Usa una red doméstica de **2,4 GHz** con contraseña WPA/WPA2 de 8 a 63 caracteres ASCII imprimibles. Esta primera versión no contempla redes abiertas, claves hexadecimales de 64 caracteres, redes empresariales ni portales de inicio de sesión.

1. Carga esta versión del firmware con **Upload** y abre **Monitor** desde PlatformIO, para que use las opciones de `platformio.ini`.
2. Haz clic en la terminal del monitor y escribe cada comando seguido de **Enter**. El eco local está activado (`monitor_echo = yes`), por lo que se ve todo lo escrito, incluida la contraseña. Puedes desactivarlo con `monitor_echo = no` y volver a abrir el monitor. El firmware responde al recibir la línea completa.
3. Escribe `wifi scan` y pulsa Enter para ver las redes visibles de 2,4 GHz. La búsqueda se ejecuta sin detener los latidos. La lista muestra el nombre (SSID), la señal en dBm y si la red es abierta o protegida. Un valor de -40 dBm indica mayor intensidad que -80 dBm. La lista no proporciona contraseñas ni identifica cuál es tu red: elige la tuya por el nombre que usas en tu celular o que figura en la etiqueta del router (si no se ha cambiado).
4. Sustituye los valores de ejemplo por los tuyos, sin comillas:

```text
wifi ssid Nombre de mi red
wifi password MiClaveDeEjemplo123
wifi connect
```

El nombre puede contener espacios. Se conservan los espacios del valor, incluidos los iniciales y finales; deja solo un espacio de separación después de `ssid` o `password`. Configurar un SSID borra la contraseña pendiente, por lo que siempre debes introducir la contraseña después.

El ESP32 intentará conectarse durante un máximo de 15 segundos, mientras sigue enviando latidos y atendiendo comandos. Si obtiene conexión, muestra su IP y guarda juntos los datos en NVS mediante `Preferences`. Después de reiniciar los leerá e intentará conectarse de nuevo. Las respuestas del firmware no imprimen las contraseñas; el eco local del monitor sí puede mostrarlas.

```text
[WIFI] Connected | IP: 192.168.1.50
[WIFI] Configuracion guardada para el proximo arranque.
```

Si falla el intento, la nueva configuración no reemplaza la anterior guardada. Puedes corregir la contraseña y ejecutar otra vez `wifi connect`. Si se pierde la conexión después, el firmware informa del problema y espera `wifi connect`; esta versión no implementa reintentos automáticos continuos.

| Comando | Acción |
| --- | --- |
| `help` | Muestra los comandos disponibles. |
| `wifi scan` | Lista las redes visibles de 2,4 GHz con su señal y protección. |
| `wifi ssid <nombre>` | Prepara una nueva red en RAM y borra la clave pendiente. |
| `wifi password <clave>` | Prepara la contraseña en RAM sin imprimirla. |
| `wifi connect` | Intenta conectarse y guarda la configuración si lo consigue. |
| `wifi status` | Muestra el estado de conexión y la IP cuando existe. |
| `wifi forget` | Desconecta y borra la configuración propia guardada y pendiente. |
| `wifi setup` | Abre el portal web e imprime los datos de su red temporal. |
| `wifi setup stop` | Cierra el portal y la red temporal. |

Para cambiar de casa, repite los tres comandos iniciales con los nuevos datos. Para borrar la configuración, usa `wifi forget` y comprueba tras reiniciar que solicita configurar una red.

Durante una búsqueda se permiten `help` y `wifi status`; espera a que termine para cambiar la configuración o conectar. Si hay un intento de conexión en curso, espera su resultado antes de usar `wifi scan`. Una red protegida en la lista no garantiza compatibilidad con esta versión: usa una red doméstica WPA/WPA2 con contraseña. Las redes ocultas no se listan y las redes exclusivamente de 5 GHz quedan fuera del alcance de esta placa.

Las credenciales quedan en la memoria del dispositivo, fuera del código y de GitHub. Este guardado no añade cifrado de NVS. Desactiva el eco antes de capturar la terminal si vas a introducir una contraseña.

Referencias: [Preferences/NVS](https://docs.espressif.com/projects/arduino-esp32/en/latest/api/preferences.html), [Wi-Fi](https://docs.espressif.com/projects/arduino-esp32/en/latest/api/wifi.html), [monitor serie](https://docs.platformio.org/en/latest/core/userguide/device/cmd_monitor.html).

## Configurar Wi-Fi desde el celular

1. Carga el firmware 0.3.2 y espera que termine el intento de conexión inicial.
2. Escribe `wifi setup` por USB. No borra las credenciales existentes.
3. El monitor muestra la red **Railway-Setup**, su clave fija **Railway123** y la dirección `http://192.168.4.1`. Desde 0.3.1 la clave del portal se define en `AppConfig.h`, por elección del usuario. Las claves generadas con 0.3.0 ya no se consultan; no hace falta borrar NVS. La contraseña del router sigue configurándose por USB o formulario y guardándose fuera del código fuente.
4. Desde el celular, conéctate a Railway-Setup usando esa clave. Si el celular avisa que la red no tiene Internet, elige permanecer conectado.
5. Al conectarte, el celular puede abrir el formulario o mostrar una notificación como **Iniciar sesión en la red**. Si no aparece, abre **http://192.168.4.1** manualmente. La versión 0.3.2 incorpora DNS y redirecciones HTTP para favorecer la detección del portal cautivo; el sistema del celular decide cuándo y cómo mostrarlo.
6. Pulsa **Buscar redes**. Selecciona tu red o escribe su nombre. Se muestran hasta 40 puntos de acceso, con nombres repetidos agrupados en la página. Las redes abiertas se muestran deshabilitadas porque esta versión requiere contraseña.
7. Introduce la contraseña del Wi-Fi doméstico y pulsa **Conectar y guardar**.
8. Si logra conectar y guardar, muestra la IP. El portal se cierra después de 15 segundos; vuelve a conectar el celular a tu red doméstica.

Si la contraseña es incorrecta o se agota el tiempo, el formulario sigue disponible para corregirla. Si conecta pero falla el almacenamiento, informa del error y mantiene el portal abierto. Los datos nuevos reemplazan los guardados únicamente después de una conexión correcta y un guardado exitoso.

Al arrancar sin configuración válida o si falla el intento inicial, el ESP32 abre el portal automáticamente. Si consigue conectar con la configuración guardada, no crea la red temporal. Para volver a abrirla, usa `wifi setup`; si ya está abierta, ese comando vuelve a mostrar los datos de acceso. Una pérdida de conexión posterior al arranque no abre automáticamente el portal.

El modo `WIFI_AP_STA` permite mantener Railway-Setup mientras el ESP32 prueba la conexión al router. El servidor escucha solo en `192.168.4.1`. La conexión a Internet no se comparte con el celular. La página usa HTTP local sobre la red Wi-Fi protegida; no implementa HTTPS. El portal protege los cambios con un token de sesión, no habilita CORS y presenta los SSID como texto. Nunca devuelve ni imprime la contraseña del router en sus respuestas. La clave de acceso de Railway-Setup es la clave compartida Railway123, definida para simplificar las pruebas del prototipo.

La subred del portal es `192.168.4.0/24`; esta versión no resuelve conflictos si el router usa esa misma subred.

### Portal cautivo (0.3.2)

Mientras el portal está activo, `DNSServer` responde a las consultas de nombres con `192.168.4.1`, usando TTL cero. Las peticiones HTTP de detección del sistema, como `/generate_204`, `/hotspot-detect.html` y `/connecttest.txt`, se redirigen a `http://192.168.4.1/`. La raíz también redirige si se solicitó usando un nombre externo. Las rutas desconocidas de `/api/` conservan su error JSON y las operaciones de la API solo se aceptan con el host canónico. DNS y HTTP se detienen al cerrar el portal.

No redirigimos HTTPS ni garantizamos que se abra el navegador predeterminado. La apertura depende del dispositivo, su configuración y la detección almacenada para esa red. Para probar de nuevo, olvida Railway-Setup en el celular y vuelve a conectarte con Railway123. Mantén siempre la dirección HTTP manual como alternativa.

Referencia de implementación: [ejemplo CaptivePortal de Arduino-ESP32](https://github.com/espressif/arduino-esp32/blob/master/libraries/DNSServer/examples/CaptivePortal/CaptivePortal.ino).

### Peticiones de la página

| Petición | Función |
| --- | --- |
| `GET /` | Entrega la página desde flash. |
| `GET /api/status` | Devuelve estado, IP, estado de búsqueda y token de sesión. |
| `POST /api/scan` | Inicia la búsqueda asíncrona; requiere token. |
| `GET /api/networks` | Devuelve los resultados sin contraseñas. |
| `POST /api/connect` | Valida los datos e inicia la conexión; requiere token. |

### Validación del portal

La compilación y las comprobaciones simuladas no sustituyen las pruebas físicas. En la placa comprueba:

- Abrir el portal con `wifi setup`, olvidar y volver a conectar Railway-Setup en el celular y comprobar si se muestra el formulario o la notificación de inicio de sesión. Verificar también el acceso manual por IP, sin Internet.
- Buscar, seleccionar y configurar una red; confirmar conexión, guardado y cierre de Railway-Setup.
- Reiniciar y conectar con los datos nuevos sin abrir el portal.
- Introducir una contraseña incorrecta; comprobar que el formulario permite corregirla y que no se sobrescribe la configuración guardada.
- Arrancar sin credenciales (`wifi forget` y luego EN/RESET): el portal debe abrirse automáticamente.
- Arrancar con un router no disponible: tras el intento debe abrirse el portal.
- Cerrar y reabrir mediante `wifi setup stop` y `wifi setup`, manteniendo el funcionamiento de USB, búsqueda y latidos.

### Comprobaciones en la placa

- Ejecutar `wifi scan`: recibir una lista o un aviso de que no se encontraron redes; comprobar que siguen los latidos y que es posible repetir la búsqueda. Probar también durante un intento de conexión: debe pedir esperar.
- Configurar una red válida: obtener IP y confirmación de guardado, manteniendo los latidos.
- Reiniciar: conectar con la configuración guardada sin volver a introducir datos.
- Configurar una red o clave incorrecta: agotar el intento en 15 segundos y seguir atendiendo `help` y `wifi status`.
- Probar comandos desconocidos, una línea de más de 128 bytes y una contraseña corta: recibir errores sin que se repita el texto introducido; el siguiente comando válido debe funcionar.
- Cambiar a otra red: conectar y conservar la nueva configuración después del reinicio.
- Ejecutar `wifi forget` y reiniciar: comprobar que no vuelve a conectar con los datos borrados.
- Revisar que los mensajes y capturas no contengan contraseñas.

## Evidencia y alcance

Guarda el resultado de Build, Upload y el arranque seguido de al menos dos mensajes periódicos. La compilación, carga y ejecución física deben verificarse antes de dar esta tarea por terminada.

El usuario reportó una carga exitosa y latidos cada cinco segundos con la versión 0.1.0. Con 0.2.1 reportó búsqueda de redes, conexión, guardado y conexión posterior al reinicio. También hubo un intento de arranque que agotó los 15 segundos; su causa no se ha determinado. Con 0.2.2 confirmó arranque, conexión guardada, estado, búsqueda y latidos después de separar el código. La versión 0.3.0 añade el portal; 0.3.1 fija su clave en Railway123 y 0.3.2 añade detección de portal cautivo. Estas funciones requieren las pruebas físicas descritas arriba.

Durante el desarrollo de 0.3.0 se verificaron la compilación para ESP32 y la sintaxis de JavaScript. Con sustitutos de navegador y hardware se probaron el envío del formulario, validación de longitudes y contraseña, nombres con caracteres especiales, agrupación de SSID, rechazo de peticiones sin token, respuesta a errores y cierre diferido solo tras conexión y guardado. En 0.3.2 también se comprobó la compilación y, con sustitutos, la redirección de rutas de detección, rechazo de hosts externos en la API y ciclo de inicio/cierre del DNS. Estas comprobaciones no verifican la radio, el navegador real, la detección del sistema operativo ni el almacenamiento físico.

Todavía no implementa comunicación con .NET. El programa no acciona el PCA9685 ni realiza pruebas de servo.

Los archivos quedan dentro del repositorio para su revisión y posterior commit/push a GitHub.
