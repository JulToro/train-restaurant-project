# Mosquitto local en macOS — instalación y seguimiento

Fecha del registro: **5 de noviembre de 2026**.

Relacionado con el [issue #54: contrato de comunicación entre ESP32 y .NET](https://github.com/JulToro/train-restaurant-project/issues/54).

## Objetivo y estado

Preparar un broker MQTT local para las futuras pruebas entre ESP32 y .NET. La instalación inicial se realizó directamente en macOS con Homebrew. El traslado a Docker queda para una etapa futura.

Instalación y prueba local verificadas el **5 de noviembre de 2026**: Homebrew informa `mosquitto 2.1.2_3`, el servicio está activo y se comprobó publicación y recepción en `127.0.0.1:1883`. Posteriormente se habilitó el listener de red con autenticación y se comprobó el rechazo de conexiones sin credenciales. El usuario confirmó publicación y recepción autenticada en el Mac. El acceso desde otro dispositivo, ESP32 y .NET sigue pendiente. El Mac tiene arquitectura ARM64 y Homebrew en `/opt/homebrew/bin/brew`.

## 1. Instalar

En Terminal:

```bash
brew install mosquitto
```

La fórmula incluye el broker y los clientes `mosquitto_pub` y `mosquitto_sub`. Registrar la versión instalada:

```bash
brew list --versions mosquitto
```

## 2. Ejecutar el broker en primer plano

En una primera terminal:

```bash
"$(brew --prefix)/opt/mosquitto/sbin/mosquitto" -v
```

Mantenerla abierta para consultar los registros. `Ctrl+C` detiene el broker.

Sin pasar un archivo de configuración, Mosquitto usa el puerto MQTT **1883** y permite la prueba sin credenciales únicamente desde el propio computador. Este comando no carga automáticamente el archivo de configuración que utiliza el servicio de Homebrew.

## 3. Suscribirse a un topic

En una segunda terminal:

```bash
mosquitto_sub -h 127.0.0.1 -p 1883 -t 'railway/prueba' -v
```

El cliente espera mensajes del topic `railway/prueba`. La opción `-v` muestra el topic junto al contenido. Dejarlo escuchando antes de publicar; esta prueba no utiliza mensajes retenidos.

## 4. Publicar y comprobar

En una tercera terminal:

```bash
mosquitto_pub -h 127.0.0.1 -p 1883 \
  -t 'railway/prueba' \
  -m 'Hola desde mi Mac'
```

Resultado esperado en la segunda terminal:

```text
railway/prueba Hola desde mi Mac
```

Este resultado comprueba publicación y recepción dentro del Mac. No comprueba todavía comunicación con el ESP32, integración con .NET ni ejecución de comandos de desvío. `railway/prueba` es un topic de diagnóstico, no el contrato definitivo del issue #54.

## 5. Servicio en segundo plano (opcional)

Detener primero el broker de la primera terminal con `Ctrl+C`, para liberar el puerto 1883. Luego:

```bash
brew services start mosquitto
brew services list
```

El servicio se inicia ahora y al iniciar sesión. Utiliza el archivo:

```text
$(brew --prefix)/etc/mosquitto/mosquitto.conf
```

Para obtener la ruta expandida en la terminal:

```bash
ls "$(brew --prefix)/etc/mosquitto/mosquitto.conf"
```

Si el archivo no existe (como ocurrió con la instalación observada de `2.1.2_3`), crearlo antes de iniciar el servicio con esta configuración para pruebas exclusivamente locales:

```text
listener 1883 127.0.0.1
allow_anonymous true
```

Guardar esas líneas en `$(brew --prefix)/etc/mosquitto/mosquitto.conf` sin sobrescribir una configuración existente. Validar con:

```bash
"$(brew --prefix)/opt/mosquitto/sbin/mosquitto" \
  -c "$(brew --prefix)/etc/mosquitto/mosquitto.conf" --test-config
```

`--test-config` está disponible en la versión instalada 2.1.2. La prueba sin credenciales queda restringida a la interfaz local IPv4; por eso los ejemplos usan `127.0.0.1`.

Repetir los pasos 3 y 4 para validar el servicio, pues su configuración puede diferir de la ejecución sin archivo. Para detenerlo o reiniciarlo después de cambios:

```bash
brew services stop mosquitto
brew services restart mosquitto
```

Ejecutar `restart` solo cuando se quiera volver a dejar el servicio activo.

## Problemas básicos

| Síntoma | Comprobación |
| --- | --- |
| `command not found` para un cliente | Revisar que terminó la instalación y que Homebrew está en el PATH de esa terminal. |
| Conexión rechazada | Confirmar que el broker sigue ejecutándose y que el cliente usa el puerto 1883. |
| Puerto ocupado / `Address already in use` | Revisar `brew services list` y `lsof -nP -iTCP:1883 -sTCP:LISTEN`; usar una sola instancia. |
| El suscriptor no muestra el mensaje | Abrirlo antes de publicar y comprobar que ambos comandos usan exactamente `railway/prueba`. |

## Próxima etapa: acceso desde ESP32

Se habilitó el acceso de red con esta configuración en `/opt/homebrew/etc/mosquitto/mosquitto.conf`:

```conf
listener 1883 0.0.0.0
allow_anonymous false
password_file /opt/homebrew/etc/mosquitto/passwd
```

El usuario creó el archivo de contraseñas usando `mosquitto_passwd`. La contraseña y el contenido del archivo no se guardan en el repositorio. Para la primera creación se indicó:

```bash
mosquitto_passwd -c /opt/homebrew/etc/mosquitto/passwd railway-esp32
```

`-c` reemplaza el archivo; para actualizar o agregar usuarios posteriormente, omitir esa opción.

Se validó la configuración y se reinició el servicio. `brew services list` mostró `started` y `lsof` mostró escucha IPv4 en `*:1883`. Una publicación local sin credenciales fue rechazada con `Connection Refused: not authorised`, código de salida 5. Esto verifica el rechazo anónimo. Después, el usuario confirmó una conexión autenticada local con el usuario `railway-esp32` y la recepción de `railway/prueba Prueba autenticada`. Esta evidencia no confirma todavía acceso desde la LAN.

Los ejemplos sin credenciales de los pasos 3 y 4 documentan la prueba inicial: después de este cambio serán rechazados. Las siguientes pruebas deben proporcionar usuario y contraseña. MQTT en 1883 no está cifrado; esta etapa se limita a la red de confianza, sin publicar el puerto en Internet.

En el ESP32 se utilizará la IP local del Mac. `localhost` en el ESP32 apunta al propio ESP32. El Mac deberá estar encendido, sin suspensión y accesible para la placa durante las pruebas.

## Registro de validación

Actualizar esta tabla con resultados observados; conservar las salidas de comandos sin credenciales.

| Comprobación | Estado al 5 de noviembre de 2026 | Evidencia / siguiente validación |
| --- | --- | --- |
| Homebrew disponible | Comprobado previamente | `/opt/homebrew/bin/brew`; arquitectura `arm64` |
| Instalación de Mosquitto | Verificada | `brew list --versions mosquitto`: `mosquitto 2.1.2_3` |
| Broker en primer plano | Pendiente | Registro de arranque en puerto 1883 |
| Publicación y recepción local | Verificada | Mensaje recibido: `railway/prueba Hola desde mi Mac`, conexión a `127.0.0.1:1883` |
| Servicio en segundo plano | Verificado | `brew services list`: `started`; `lsof`: escucha en `127.0.0.1:1883`; publicación/recepción correcta |
| Configuración de autenticación y listener de red | Verificada | Configuración válida, servicio `started`, escucha IPv4 `*:1883`, acceso anónimo rechazado |
| Publicación y recepción autenticada local | Confirmada por el usuario | Suscriptor en `127.0.0.1` con usuario `railway-esp32`: `railway/prueba Prueba autenticada` |
| Acceso desde la LAN | Pendiente | Prueba autenticada desde otro dispositivo |
| Comunicación ESP32 y .NET | Pendiente, siguiente etapa | Eventos y confirmaciones conforme al contrato |

La instalación local por sí sola no completa el issue #54: faltan los formatos de mensajes y las reglas de comunicación acordados.


## Incidente de arranque y corrección — 5 de noviembre de 2026

El usuario ejecutó `brew services start mosquitto` y recibió una confirmación de arranque. Al repetirlo apareció `Bootstrap failed: 5: Input/output error`.

La inspección posterior mostró `mosquitto error 3`, `launchctl` con estado `not running` y código de salida 3, y ningún proceso escuchando en 1883. El LaunchAgent apuntaba a `/opt/homebrew/etc/mosquitto/mosquitto.conf`, pero solo existía `mosquitto.conf.example`. El código de bootstrap por sí solo no identificaba la causa.

Se creó el archivo faltante con el listener local indicado arriba, se validó (`Configuration file is OK.`) y se ejecutó `brew services restart mosquitto`, sin `sudo`. Después se verificaron el servicio `started`, la escucha en `127.0.0.1:1883` y el envío/recepción del mensaje de prueba. El arranque en una próxima sesión de macOS todavía no se ha probado.

## Referencias

- [Fórmula oficial de Homebrew: instalación, binarios y servicio](https://formulae.brew.sh/formula/mosquitto).
- [Descarga oficial de Mosquitto para macOS](https://mosquitto.org/download/).
- [Comportamiento de acceso local desde Mosquitto 2.0](https://mosquitto.org/documentation/migrating-to-2-0/).
- [Configuración y autenticación](https://mosquitto.org/documentation/authentication-methods/).
