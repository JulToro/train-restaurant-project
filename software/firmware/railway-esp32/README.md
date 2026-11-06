# Firmware base del ESP32

## Comunicación MQTT: preparación local

La [guía de Mosquitto local en macOS](../../../docs/06-software/mosquitto-local-macos.md) documenta la instalación con Homebrew, la prueba de publicación y recepción, el servicio opcional y el seguimiento de validación para el issue #54. La instalación y la prueba local quedaron verificadas el 5 de noviembre de 2026; el acceso desde ESP32 y la migración futura a Docker se tratarán después.

El [contrato MQTT ESP32 ↔ .NET](../../../docs/06-software/api/contracts.md) define los mensajes del issue #54 y su interpretación. Es una especificación pendiente de implementación; los sensores conservarán IDs fijos y .NET interpretará entrada/salida según el recorrido.

## Dos reeds en MCP23017 (version actual: 0.5.1)

La versión 0.5.1 ajusta el puerto A de 0.5.0 al puerto B, según el cableado PB0/PB1 reportado por el usuario. La lectura pasa de GPIO27 a **dos entradas del MCP23017 en 0x27**. Con USB y fuente externa desconectados, conecta:

| Sensor | Un terminal | Otro terminal |
| --- | --- | --- |
| `entrada` | MCP23017 **GPB0** (también puede figurar como PB0 o B0 del banco de GPIO) | GND común |
| `salida` | MCP23017 **GPB1** (PB1 o B1 del banco de GPIO) | GND común |

Los reeds no tienen polaridad ni necesitan positivo. No confundas GPB0/GPB1 con **A0/A1/A2 de selección de dirección**: esos puentes conservan la dirección 0x27 del montaje. Identifica las etiquetas de tu placa antes de cablear. Ambos pines de sensores son entradas digitales; los nombres `entrada` y `salida` indican su función futura en la vía.

SDA/SCL siguen en GPIO21/GPIO22, compartidos con PCA9685; lógica a 3,3 V y GND común. RESET del MCP permanece alto. El firmware configura el expansor mediante `Wire`, sin otra biblioteca: BANK=0, GPB0/GPB1 con pull-up, GPA7/GPB7 como salidas bajas sin uso y los demás GPIO como entradas sin pull-up. Este módulo administra todo el MCP; deja las otras conexiones de GPIO sin usar por ahora.

Después de **Build** y **Upload**, abre **Monitor** a 115200, pulsa EN/RESET y comprueba versión 0.5.1. Sin imanes se espera:

```text
[SENSOR] entrada (GPB0): INACTIVO (iman no detectado)
[SENSOR] salida (GPB1): INACTIVO (iman no detectado)
```

Acerca un imán a cada reed por separado, mantenlo y retíralo: solo su nombre debe cambiar a `ACTIVO` y volver a `INACTIVO`. Prueba ambos activos a la vez y reinicia con el imán presente. `sensor status` consulta los dos estados; `i2c scan` sigue comprobando las direcciones. Repite con el portal y durante `wifi scan`, observando que continúan los latidos.

Cada sensor tiene antirrebote independiente de 30 ms y se consulta el puerto cada 5 ms como mínimo, según la disponibilidad del bucle. Una presencia estable genera un solo mensaje; al arrancar, ambos estados empiezan en `PENDIENTE`. Un fallo de comunicación, lectura incompleta o pérdida de configuración del MCP invalida ambos estados como `DESCONOCIDO`. Se intenta reconfigurar cada segundo; al recuperarse se estabilizan otra vez durante 30 ms. Para validar recuperación física, apaga todo, desconecta el MCP y arranca: debe mostrar `DESCONOCIDO`; apaga, reconecta y repite. No manipules el cableado energizado. El muestreo no garantiza detectar pulsos breves y un cable de reed cortado se interpreta como contacto abierto: no hay supervisión de ese cable.

Validación de desarrollo de 0.5.1: `pio run` terminó en `SUCCESS`. Una prueba temporal C++ con reloj e I²C simulados pasó lecturas independientes y simultáneas, rebotes, ausencia de duplicados, dispositivo ausente, lectura incompleta, recuperación, reinicio del MCP y desbordamiento de tiempo. Todavía no se calcula ocupación del bloque ni se controla el servo ni se envían eventos a .NET/JMRI.

### Prueba física reportada el 2 de noviembre de 2026

El usuario confirmó que corrigió un problema de conexión y compartió transiciones repetidas de ambos reeds con 0.5.1. La salida muestra GPB0 (`entrada`) y GPB1 (`salida`) alternando entre activo e inactivo; los latidos continúan, incluidos los de 186 y 191 segundos:

```text
[HEARTBEAT] Uptime: 186 seconds
[SENSOR] salida (GPB1): ACTIVO (iman detectado)
[SENSOR] salida (GPB1): INACTIVO (iman no detectado)
[SENSOR] entrada (GPB0): ACTIVO (iman detectado)
[SENSOR] entrada (GPB0): INACTIVO (iman no detectado)
[HEARTBEAT] Uptime: 191 seconds
```

El registro de arranque compartido previamente confirma versión 0.5.1, configuración de ambos pull-ups y respuestas I²C en 0x27 y 0x40 sin errores de bus. Esto documenta la lectura física básica de ambos sensores y su publicación por consola. No mide los rebotes eléctricos ni verifica el umbral de 30 ms en hardware.

En la comprobación posterior, el usuario confirmó que el comportamiento era correcto y compartió una activación y desactivación de GPB1 durante `wifi scan`, con latidos antes y durante la búsqueda y resultados de redes al finalizar:

```text
[HEARTBEAT] Uptime: 356 seconds
[WIFI] Buscando redes de 2,4 GHz...
[SENSOR] salida (GPB1): ACTIVO (iman detectado)
[SENSOR] salida (GPB1): INACTIVO (iman no detectado)
[HEARTBEAT] Uptime: 361 seconds
[WIFI] Nombre de red (SSID) | Senal | Seguridad
```

La captura confirma lectura de GPB1 durante la búsqueda Wi-Fi sin detener los latidos. No permite medir cuánto tiempo se mantuvo el imán ni verifica GPB0 durante esa búsqueda. Activación simultánea, arranque con imán y recuperación del expansor siguen pendientes de confirmación física; la ausencia de duplicados con un imán mantenido no está cronometrada en los registros.

Las secciones siguientes conservan instrucciones y evidencia de versiones anteriores; la conexión actual de los sensores es la descrita arriba.

## Comprobar MCP23017 y PCA9685 (0.4.2)

El firmware inicia I²C en GPIO21/SDA y GPIO22/SCL a 100 kHz. Ambos modulos comparten SDA, SCL y GND; su lógica usa 3,3 V. La alimentación V+ de 5 V del servo sigue separada. El firmware espera el MCP23017 en **0x27**, de acuerdo con la respuesta observada en el montaje. En ese chip, A0/A1/A2 altos seleccionan 0x27; todos a GND seleccionan 0x20. RESET debe mantenerse alto. Verifica los pull-ups de SDA/SCL a 3,3 V antes de energizar.

Después de **Upload**, abre **Monitor** a 115200 y reinicia. La versión 0.4.2 busca automáticamente direcciones 0x08..0x77, consultando una por vuelta con un timeout de 25 ms por transacción. Puedes repetir con `i2c scan`, también durante `wifi scan`. Se espera:

```text
[I2C] MCP23017 esperado en 0x27: RESPONDE
[I2C] PCA9685 esperado en 0x40: RESPONDE
```

Una respuesta confirma presencia en esa dirección, no identifica el chip ni verifica sensores o actuadores. Pueden aparecer otras direcciones (por ejemplo, la dirección colectiva de la PCA9685). Si aparece `SIN RESPUESTA`, revisa alimentación, SDA/SCL, dirección y RESET con el montaje apagado. Los errores de bus se distinguen de direcciones sin respuesta.

Esta versión comprueba comunicación; **todavía no configura ni lee las entradas del MCP23017**. `sensor status` sigue consultando el reed en GPIO27. Si ya lo moviste al expansor, ese comando no representa su estado. El usuario reportó con 0.4.1 respuestas en 0x27 y 0x40, dos direcciones y cero errores de bus; esto confirma comunicación en esas direcciones, no identifica automáticamente los modelos. La versión 0.4.2 ajusta la dirección esperada del MCP23017 a ese montaje. El escáner no mueve el servo.

La versión 0.4.0 integra la lectura del reed del issue #53 con el montaje documentado en el issue #51: GPIO27, contacto hacia GND y antirrebote de 30 ms.

## Probar el sensor reed (issue #53)

Con el ESP32 apagado, conserva o comprueba el reed entre **GPIO27 y GND**. No requiere alimentación propia. Se usa `INPUT_PULLUP`: contacto cerrado = `LOW` = activo. El Hall HW-477 queda fuera de esta integración. El registro anterior está en [el cableado del issue #51](../../../docs/05-electronics/issues/51/issue-51-cableado-seguro.md).

`include/OccupancySensor.h` y `src/OccupancySensor.cpp` implementan la lectura; `AppConfig.h` define el pin, el nivel activo y los 30 ms. `main.cpp` inicializa el módulo y lo atiende en cada vuelta. Un cambio debe permanecer estable durante 30 ms antes de publicarse, incluido el estado inicial. La lectura usa `millis()` y no añade esperas. Una activación más corta puede descartarse; la prueba con pasadas rápidas determinará si hace falta ajustar el tiempo. Las operaciones de Wi-Fi/HTTP pueden retrasar el muestreo; esta lectura por sondeo no garantiza capturar pulsos breves.

1. Ejecuta **Build**, después **Upload** y abre **Monitor** a 115200 baudios. Reinicia para ver la versión 0.4.0.
2. Sin imán, espera `[SENSOR] Estado: INACTIVO (iman no detectado)`.
3. Acerca el imán y mantenlo: debe aparecer una sola vez `[SENSOR] Estado: ACTIVO (iman detectado)`.
4. Retíralo: debe aparecer una sola transición a `INACTIVO`. Repite varias veces y prueba también pasadas rápidas.
5. Escribe `sensor status`: consulta el estado confirmado. Antes de estabilizarse la lectura inicial muestra `PENDIENTE`.
6. Reinicia con el imán cerca: debe informar `ACTIVO` después de estabilizarse.
7. Repite con `wifi scan` y con el portal abierto mediante `wifi setup`. Deben seguir los latidos y la lectura; `sensor status` también está disponible durante la búsqueda.

El sensor informa presencia del imán en ese punto; no determina por sí solo la ocupación de todo un tramo. Esta versión no acciona el servo ni envía eventos a .NET/JMRI. La validación física del firmware integrado está pendiente: conserva las salidas de Build/Upload y las transiciones observadas antes de cerrar el issue #53.

Validación de desarrollo: `pio run` terminó en `SUCCESS` para `esp32dev` con Arduino 3.3.12. Una prueba temporal en C++ con GPIO, reloj y consola simulados verificó el estado inicial, el umbral de 30 ms, rebotes en ambos sentidos, ausencia de eventos repetidos, arranque con imán y desbordamiento del reloj. Estas comprobaciones no sustituyen la prueba del sensor en la placa.

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
