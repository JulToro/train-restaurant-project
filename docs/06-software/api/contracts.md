# Contrato MQTT entre ESP32 y .NET — issue #54

Fecha: **5 de noviembre de 2026**. [Issue #54](https://github.com/JulToro/train-restaurant-project/issues/54).

## Alcance y estado

Este documento define el contrato para eventos de sensores, consulta de estado, disponibilidad y órdenes de desvío. Es la especificación para implementar y validar; no afirma que el firmware o .NET ya lo soporten. La [guía de Mosquitto](../mosquitto-local-macos.md) registra la instalación y las pruebas del broker.

Decisiones acordadas: MQTT con JSON, sin campo `version` ni versión en los topics, identidades fijas de sensores y seguimiento de ubicación/ruta/sentido en .NET. Los tiempos, límites y reglas siguientes concretan la propuesta inicial y deben validarse durante la implementación.

El contrato no expone GPIO, direcciones I²C, canales PWM, ángulos del servo ni identificadores propietarios de JMRI. Una configuración local traduce los identificadores lógicos al hardware; un adaptador .NET traduce los mensajes al dominio.

## Transporte y reglas comunes

- MQTT 3.1.1, mensajes JSON UTF-8, máximo 4096 bytes por mensaje.
- Publicación y suscripción QoS 1 en todos los canales. Puede haber duplicados; un PUBACK MQTT no acredita ejecución física.
- `retain=false` en todos los mensajes. No se utiliza el último mensaje retenido para inferir detecciones ni ejecutar movimientos.
- Sesiones limpias (`cleanSession=true`) en ESP32 y .NET; en MQTT 5, usar el equivalente sin persistencia de sesión. Restaurar suscripciones después de cada conexión.
- No reenviar colas de aplicación de una conexión anterior. El ESP32 continúa leyendo sensores desconectado, pero no promete almacenar/reproducir sus pasos. Cada reconexión exige sincronizar el estado actual.
- Un único cliente controlador .NET emite órdenes. Las herramientas de diagnóstico pueden suscribirse, pero no deben competir por el mismo client ID ni emitir órdenes de operación.
- El dispositivo se identifica por el topic. No repetir `deviceId` dentro del payload. Los IDs son sensibles a mayúsculas, de 1 a 64 caracteres de `A-Z`, `a-z`, `0-9`, `_` y `-`; no contienen `/`, `+` ni `#`.
- `bootId` es un identificador nuevo y no reutilizado por cada arranque (por ejemplo UUID aleatorio). Se conserva al reconectar MQTT sin reiniciar.
- Los campos indicados en los ejemplos son obligatorios, salvo donde se indique otra cosa. Rechazar tipos/valores inválidos; ignorar campos adicionales para permitir extensiones compatibles.
- Sin campo de versión: conservar significado y tipos existentes. Una modificación incompatible requiere coordinar firmware y .NET; no se introduce silenciosamente.
- Usuario/contraseña son configuración de despliegue, no campos de estos mensajes. No guardar secretos en código, documentación ni registros. La prueba actual en LAN usa 1883 sin TLS; no equivale a un despliegue cifrado.

## Topics

`{deviceId}` es, por ejemplo, `railway-esp32-01`.

| Topic | Publica → recibe | Propósito |
| --- | --- | --- |
| `railway/devices/{deviceId}/sensors` | ESP32 → .NET | Transiciones de todos los sensores de ese dispositivo. |
| `railway/devices/{deviceId}/snapshot/request` | .NET → ESP32 | Solicitar una lectura completa para sincronizar. |
| `railway/devices/{deviceId}/snapshot` | ESP32 → .NET | Estado completo actual, sin contar pasos. |
| `railway/devices/{deviceId}/health` | ESP32 → .NET | Latido y estado del controlador. |
| `railway/devices/{deviceId}/commands` | .NET → ESP32 | Órdenes de desvío. |
| `railway/devices/{deviceId}/results` | ESP32 → .NET | Aceptación, rechazo y resultado de cada orden. |

.NET puede suscribirse a `railway/devices/+/sensors` y a los canales correspondientes de snapshot, health y results. Debe consumir todos los sensores del dispositivo para comprobar su secuencia compartida.

## Eventos de sensor

Topic de ejemplo: `railway/devices/railway-esp32-01/sensors`.

```json
{
  "bootId": "arranque-a81f",
  "sequence": 42,
  "componentId": "S1",
  "state": "active"
}
```

| Campo | Significado |
| --- | --- |
| `bootId` | Ejecución del dispositivo que produjo el evento. |
| `sequence` | Entero de 1 a 4294967295; contador global de eventos de sensor por dispositivo/arranque. |
| `componentId` | Identidad física lógica estable: `S1`, `S2`, etc. No cambia con el sentido del tren. |
| `state` | `active`: imán detectado; `inactive`: no detectado; `unknown`: lectura no disponible o no confiable. |

El contador comienza en cero internamente; el primer evento usa 1. Aumenta en cada transición confirmada, incluso si no hay conexión; un reenvío del mismo evento conserva el número. Snapshot, health y results no lo incrementan. Antes de agotarlo, realizar un reinicio controlado del dispositivo y resincronizar con un nuevo `bootId`; nunca volver a cero con el mismo `bootId`.

El ESP32 publica ambos flancos después del antirrebote y una transición a `unknown` cuando pierde una lectura válida. Mantener un imán sobre el sensor no genera activaciones periódicas. Al arrancar, el estado es `unknown` hasta estabilizar la lectura; recuperar `unknown → active` no demuestra un nuevo paso.

.NET identifica duplicados por `(deviceId, bootId, sequence)`. Para el arranque vigente, ignora números ya procesados. Un salto hacia adelante invalida el seguimiento antes de aplicar el evento y dispara una consulta de snapshot. Eventos de otro arranque no se aplican directamente: primero confirmar el arranque mediante una sincronización correlacionada. Mensajes contradictorios con el mismo identificador son un error de protocolo.

Con una base confiable, solo una transición confirmada `inactive → active` cuenta como candidato a paso. `active → inactive` rearma el detector. Ni duplicados, ni snapshots, ni `unknown → active` cuentan como paso.

### Dos marcadores y sentido del tren

Los marcadores delantero y trasero no se distinguen en este mensaje. El reed detecta un imán, no identifica el tren, su cabeza, su cola ni su dirección. Esa interpretación depende del estado del recorrido en .NET.

```text
                    tramo B1
──────── S1 ───────────────────── S2 ────────
                   S1 → S2
```

| Paso esperado | Interpretación con recorrido confiable |
| --- | --- |
| Cabeza activa S1 | B1 pasa a ocupado. |
| Cola activa S1 | El tren terminó de cruzar este límite de entrada. |
| Cabeza activa S2 | Comienza a salir de B1; B1 continúa ocupado. |
| Cola activa S2 | B1 puede pasar a libre si toda la secuencia sigue siendo confiable. |

En sentido contrario, S2 cumple el papel de entrada y S1 el de salida, conservando sus IDs. El segundo paso por S1 no implica que todo el tren esté dentro de B1 si su cabeza ya cruzó S2: en tramos cortos, esos pasos pueden intercalarse. El modelo mantiene los cruces de cada límite y la longitud/configuración del recorrido; no exige que las cuatro activaciones siempre tengan el orden de la tabla.

La interpretación requiere ubicación inicial confirmada, ruta y sentido conocidos, exactamente dos marcadores y un único tren autorizado para ese recorrido. Al cambiar de sentido se conservan los cruces ya observados y se recalcula el movimiento esperado; no se reinician contadores a ciegas. Inicialización del tren y órdenes de marcha pertenecen a la aplicación .NET y no son mensajes de este controlador de accesorios.

El antirrebote local actual es de 30 ms; validar ese valor y la separación entre marcadores con la velocidad real. Un segundo paso físico por vibración o retroceso tiene otro `sequence`; no lo elimina la deduplicación MQTT. Dos imanes idénticos pueden producir situaciones ambiguas: si no se puede justificar la salida completa, mantener el tramo ocupado o desconocido y solicitar reconciliación, sin liberarlo por un simple conteo par/impar.

## Snapshot y sincronización

Solicitud, después de confirmar las suscripciones de .NET:

```json
{
  "requestId": "consulta-9b72"
}
```

Respuesta en `/snapshot`:

```json
{
  "requestId": "consulta-9b72",
  "bootId": "arranque-a81f",
  "lastSequence": 42,
  "sensors": [
    { "componentId": "S1", "state": "inactive" },
    { "componentId": "S2", "state": "active" }
  ]
}
```

`requestId` es un ID nuevo por consulta. El ESP32 también envía un snapshot espontáneo al conectar, con `requestId: null`. `lastSequence` admite cero y representa el último número asignado a un evento en el momento de capturar el estado. `sensors` incluye cada sensor configurado exactamente una vez, aun si está `unknown`.

El ESP32 captura estados y contador de manera coherente y serializa publicaciones: primero los eventos hasta N, después el snapshot N y luego los eventos N+1 en adelante. Una pérdida de la cola de publicación obliga a resincronizar; no se oculta asignando nuevos números a eventos anteriores.

Al iniciar/reconectar .NET, o detectar un hueco/reinicio:

1. Marcar el seguimiento como no confiable y bloquear liberaciones automáticas y nuevas órdenes hasta sincronizar.
2. Suscribirse con QoS 1 y esperar la confirmación de suscripción. Solicitar snapshot con un nuevo `requestId`.
3. Esperar hasta 3 segundos; si no responde, repetir cada 5 segundos con nuevo ID y mantener el dispositivo no sincronizado. Aplicar como base únicamente la respuesta de la consulta vigente. Un snapshot espontáneo sirve para detectar reconexión, pero no sustituye esta consulta.
4. Consumir los mensajes del dispositivo mediante una sola cola ordenada. Mientras espera la base, no contar transiciones. Con el snapshot N establecer los estados y el contador; solo eventos posteriores N+1, N+2, etc. pueden interpretarse como transiciones. Conservar en una cola acotada los eventos recibidos después de la solicitud; descartar los cubiertos por N y resincronizar si faltan números, hay otro arranque o se desborda esa cola.
5. Recuperar el estado de lectura no recupera el historial del tren: un snapshot no indica qué marcadores cruzaron durante la desconexión. La ocupación afectada requiere reconciliación explícita antes de volver a liberarla automáticamente.

Un `requestId` anterior o su duplicado no reinicia el seguimiento en curso. Los resultados de órdenes de un arranque anterior tampoco pueden confirmar el estado del arranque actual.

## Latido y desconexión

Topic `/health`, al conectar y cada 5 segundos:

```json
{
  "bootId": "arranque-a81f",
  "state": "ready",
  "uptimeMs": 125000,
  "lastSequence": 42,
  "clockSynced": true
}
```

`uptimeMs` es un entero monotónico de 64 bits en milisegundos desde el arranque; extender el reloj local si su contador nativo desborda. Cada nuevo latido tiene un valor mayor; una retransmisión conserva el valor. `state` admite `ready` o `degraded`. `ready` indica que el controlador funciona sin fallos detectados; no acredita ocupación, posición física ni sincronización del consumidor. `degraded` señala fallo de lectura/control; publicar los sensores afectados como `unknown`. `clockSynced` informa si el reloj UTC es apto para comprobar caducidad de órdenes. Un reloj no sincronizado no impide transmitir sensores.

.NET usa un reloj monotónico local para medir 15 segundos sin health válido y declara la conexión no confiable. Un cierre MQTT detectado invalida inmediatamente la conexión, sin esperar el plazo. Solo un health del arranque sincronizado con `uptimeMs` mayor que el último aceptado renueva el plazo; los duplicados o mensajes anteriores no lo renuevan. Un arranque distinto solicita sincronización. El plazo no es una garantía de tiempo real.

`lastSequence` permite detectar un evento final perdido aunque no llegue otro evento de sensor. Procesar health en la misma cola; si indica un contador mayor que el aplicado y no está en curso una sincronización, solicitar snapshot e invalidar el seguimiento. Un health no establece por sí mismo una nueva base ni cambia la ocupación.

El ESP32 desconectado continúa leyendo sensores, no inicia nuevos movimientos remotos y no reproduce órdenes antiguas al reconectar. Si pierde conexión con un movimiento ya aceptado, completa solamente la acción local acotada si puede hacerlo sin fallos, sin cambiar a otra posición por la desconexión. .NET deja su resultado como no confirmado. No se presume que perder Wi-Fi detenga las locomotoras.

## Órdenes de desvío

Topic `/commands`:

```json
{
  "commandId": "cmd-123",
  "targetBootId": "arranque-a81f",
  "componentId": "D1",
  "action": "setPosition",
  "position": "thrown",
  "expiresAt": "2026-11-05T15:00:05Z"
}
```

| Campo | Regla |
| --- | --- |
| `commandId` | ID único por orden lógica; los reintentos conservan el ID y payload completos. |
| `targetBootId` | Debe coincidir con el arranque sincronizado actual. |
| `componentId` | Identificador lógico del desvío configurado. |
| `action` | Único valor inicial: `setPosition`. |
| `position` | `normal` o `thrown`. El adaptador traduce `normal` a `Closed` del dominio actual y `thrown` a `Thrown`. |
| `expiresAt` | Instante UTC RFC 3339 terminado en `Z`, con precisión de segundos o milisegundos. Es límite para empezar a ejecutar. |

.NET genera una caducidad de 5 segundos desde el envío inicial. Ambos relojes UTC deben estar sincronizados con incertidumbre combinada máxima de 1 segundo (como máximo 500 ms por extremo); rechazar nuevas órdenes si no se puede acreditar esa condición. El ESP32 exige margen superior a 1 segundo antes de caducar y rechaza fechas más de 6 segundos en el futuro. Revalidar antes del movimiento si la orden esperó; no prolongar `expiresAt` al reintentar. La sincronización de relojes es un requisito de esta implementación, no una función de MQTT.

El ESP32 valida payload, arranque, componente, acción, posición, reloj y vigencia antes de aceptar. Rechaza componentes no soportados: el firmware actual aún no controla el servo. Solo permite un movimiento a la vez; otra orden recibe `busy`. No mantiene una cola de movimientos futuros.

Mantener una caché de deduplicación por `commandId` durante todo el arranque, con retención mínima de 10 minutos. Capacidad inicial: 128 entradas; si no hay espacio, rechazar nuevas órdenes con `busy` sin expulsar entradas vigentes ni olvidar acciones en curso. Una entrada terminal puede eliminarse después de ese plazo: su orden ya habrá caducado.

Si el ID ya existe con el mismo contenido, devolver el estado guardado y no mover otra vez, aunque el plazo original haya pasado. Si cambia el contenido para el mismo ID, rechazar `command_conflict`. Tras reiniciar, `targetBootId` bloquea órdenes del arranque previo. Esto evita repetir un movimiento por retransmisión; no afirma entrega/ejecución exactamente una vez ante cualquier fallo físico.

## Confirmaciones y errores

Topic `/results`, con ejemplo de resultado terminal:

```json
{
  "bootId": "arranque-a81f",
  "commandId": "cmd-123",
  "componentId": "D1",
  "status": "executed",
  "position": "thrown",
  "verified": false,
  "error": null
}
```

| `status` | Significado |
| --- | --- |
| `accepted` | Orden validada y registrada, aún sin resultado de ejecución. |
| `executed` | Terminó la secuencia de control local sin fallo detectado. |
| `rejected` | No se inició el movimiento; `error` explica por qué. |
| `failed` | La ejecución comenzó y falló; posición física desconocida. |

`position` refleja el objetivo, no una medición. Para una posición inválida usar `null`. `verified` es siempre `false` en esta etapa sin realimentación física; un valor `true` requiere un mecanismo de confirmación implementado y validado. `error` es `null` para accepted/executed y un objeto `code`/`message` para rejected/failed.

```json
{
  "bootId": "arranque-a81f",
  "commandId": "cmd-124",
  "componentId": "D1",
  "status": "rejected",
  "position": "normal",
  "verified": false,
  "error": {
    "code": "clock_unsynchronized",
    "message": "No se puede comprobar la vigencia de la orden."
  }
}
```

Códigos iniciales: `invalid_message`, `unknown_component`, `unsupported_action`, `invalid_position`, `wrong_boot`, `expired`, `clock_unsynchronized`, `busy`, `command_conflict`, `retained_command`, `hardware_error` y `execution_timeout`. No reflejar el payload completo ni secretos en `message`. Rechazar una orden marcada como retenida; la regla principal sigue siendo que ningún emisor publique órdenes con retain.

Una orden identificable inválida obtiene `rejected`. Si no se pueden extraer IDs válidos de JSON malformado, descartar y registrar diagnóstico acotado; no fabricar una correlación. Los errores de sensor usan `state: unknown` y health degraded, no un resultado de comando.

.NET espera accepted o resultado terminal durante 1 segundo. Puede repetir la misma orden una sola vez si sigue vigente, conectado y sincronizado. Si recibió accepted, no genera un ID nuevo por falta de resultado. El ESP32 tiene 3 segundos como máximo de ejecución local desde aceptación, tras los cuales informa `execution_timeout`; .NET espera hasta 5 segundos desde el envío inicial por el resultado terminal. Si vence el plazo, se pierde la conexión o cambia el arranque, mantiene el resultado como no confirmado y no supone posición alcanzada ni reintenta automáticamente el movimiento con otro ID. Un resultado tardío se registra, pero no resuelve automáticamente una ocupación o sincronización que ya era incierta.

## Servicio receptor .NET

Una conexión MQTT de larga duración alojada en un `BackgroundService` recibe notificaciones al llegar mensajes. No se necesita un job programado que consulte sensores. Sí se necesitan temporizadores internos para health, plazos de órdenes y reconexión.

Al arrancar: conectar, suscribirse, solicitar snapshot y reconciliar el estado operativo. Reintentar conexión con esperas de 1, 2, 4, 8 y hasta 30 segundos, con pequeña variación aleatoria; restablecer suscripciones y sincronizar después de cada recuperación. Respetar la cancelación y cerrar la conexión al detener el servicio.

Validar topic/JSON antes de modificar el dominio, comprobar dispositivos/componentes configurados y procesar secuencialmente por dispositivo con cola acotada. Un desbordamiento invalida el seguimiento y solicita sincronización; nunca descartar silenciosamente un evento y mantener el tramo como confiable. Mantener los DTO y el transporte en un adaptador; el dominio conserva sus propios estados y no depende de MQTT ni de JMRI.

## Validación pendiente y criterios del issue

| Criterio de aceptación del #54 | Definición en este documento |
| --- | --- |
| Formato de eventos de sensor | Topic sensors, campos, estados, antirrebote y secuencias. |
| Comandos y confirmaciones de desvío | Topics commands/results, estados, errores y plazos. |
| Identificador del componente y estado | `componentId`, `state`, `position` y `status`; dispositivo en topic. |
| Pérdida de conexión | Health, snapshot correlacionado, descarte de historial y reconciliación. |
| Independencia de JMRI e internos del ESP32 | IDs lógicos y adaptadores locales. |

Escenarios para comprobar al implementar (todavía no ejecutados):

- Activación, desactivación, rebotes y marcador mantenido: una sola activación por paso estable.
- Duplicados MQTT: no contar otro marcador ni repetir un movimiento.
- Hueco, evento final perdido, reinicio, pérdida de Wi-Fi o broker/.NET: invalidar seguimiento y sincronizar sin fabricar cruces.
- .NET arranca después del ESP32: obtener snapshot por consulta, sin depender de mensajes retenidos.
- Dos marcadores, ambos sentidos, tramo corto con eventos intercalados y devolución parcial: no liberar un tramo con secuencia ambigua.
- Estado inicial activo, unknown y recuperación del sensor: no contar pasos inventados.
- Comando correcto, expirado, retenido, de otro arranque, sin reloj, desconocido, duplicado, en conflicto y ejecución fallida: resultados correlacionados y movimiento como máximo una vez por orden reconocida.
- Timeout y desconexión durante el movimiento: resultado no confirmado, sin reordenar automáticamente.

El contrato documentado cubre la definición solicitada. La instalación de Mosquitto y su prueba autenticada no validan todavía estos mensajes ni el seguimiento físico del tren.

## Referencias

- [MQTT: QoS, mensajes retenidos y sesiones — Mosquitto](https://mosquitto.org/man/mqtt-7.html).
- [MQTT 3.1.1 — especificación OASIS](https://docs.oasis-open.org/mqtt/mqtt/v3.1.1/os/mqtt-v3.1.1-os.pdf).
- [Integración híbrida del proyecto](../03-Railway-Hybrid-Integration.md).
- [Modelo de dominio](../domain-model.md).
