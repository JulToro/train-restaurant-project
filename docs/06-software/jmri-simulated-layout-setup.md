# Configuración del trazado simulado en JMRI

Esta guía deja documentada la configuración mínima de JMRI usada por el primer corte vertical de automatización. Su objetivo es disponer de sensores, desvíos y bloques simulados antes de conectar la aplicación .NET o el hardware ferroviario.

## Alcance

La configuración cubre:

- Un sensor virtual `IS1`.
- Un desvío virtual `T1`.
- Tres bloques lógicos: `B1`, `B2` y `B3`.
- La relación `IS1` → `B1`, para que el estado del sensor determine la ocupación del bloque.

No cubre aún locomotoras, rieles físicos, DCC-EX, CSB1, ESP32, servos ni PCA9685.

## Resultado esperado

```text
IS1 = Active   -> B1 = Occupied
IS1 = Inactive -> B1 = Unoccupied

T1 = Closed o Thrown
```

## 1. Crear el perfil de JMRI

1. Instalar JMRI y abrir **PanelPro**. No es necesario descargar ni compilar el repositorio de JMRI.
2. Durante el asistente de conexión seleccionar:

   | Campo | Valor |
   |---|---|
   | System manufacturer | `None` |
   | System connection | `No Connection` |

3. Conservar el prefijo y nombre internos asignados por JMRI. En esta configuración quedaron como `I2` e `Internal2`.
4. Crear o seleccionar el perfil `My JMRI Railroad`.

> El valor `I2` es propio de este perfil y puede variar en otro equipo. No representa hardware físico; es el prefijo de la conexión interna de JMRI.

## 2. Abrir las tablas de PanelPro

En PanelPro abrir la vista de tablas y usar el panel izquierdo para acceder a:

- `Sensors`
- `Turnouts`
- `Blocks`

## 3. Crear el sensor virtual IS1

En `Sensors`, usar **Add...** con estos datos:

| Campo | Valor |
|---|---|
| System Connection | `Internal2` |
| Hardware Address | `1` |
| User Name | `IS1` |

El resultado generado por JMRI es:

| Tipo de identificador | Valor |
|---|---|
| System Name | `I2S1` |
| User Name | `IS1` |

Validación manual:

1. En la tabla de sensores, seleccionar el estado actual.
2. Cambiarlo a `Active` y luego a `Inactive`.
3. Confirmar que ambos cambios se reflejan en la tabla.

## 4. Crear el desvío virtual T1

En `Turnouts`, usar **Add...** con estos datos:

| Campo | Valor |
|---|---|
| System Connection | `Internal2` |
| Hardware Address | `1` |
| User Name | `T1` |

Validación manual:

1. Cambiar el desvío a `Closed`.
2. Cambiar el desvío a `Thrown`.
3. Confirmar que JMRI actualiza el estado en ambos casos.

`Closed` y `Thrown` son posiciones de un desvío; no significan habilitado y deshabilitado.

## 5. Crear los bloques lógicos

En `Blocks`, crear los tres bloques siguientes:

| Valor escrito en System Name | User Name | System Name generado |
|---|---|---|
| `1` | `B1` | `I2B1` |
| `2` | `B2` | `I2B2` |
| `3` | `B3` | `I2B3` |

### Nota sobre el campo System Name

En este diálogo JMRI agrega automáticamente el prefijo del sistema y del tipo de objeto. Por eso se debe escribir solamente la dirección (`1`, `2` o `3`).

No escribir `IB1`: eso genera un identificador redundante como `I2BIB1`.

## 6. Asociar IS1 con B1

1. En la tabla `Blocks`, localizar la fila con User Name `B1`.
2. En la columna `Sensor`, abrir el selector.
3. Seleccionar `IS1`.
4. Dejar `B2` y `B3` sin sensor por ahora.

Comprobar el resultado alternando `IS1`:

| Estado de IS1 | Estado esperado de B1 |
|---|---|
| `Active` | `Occupied` |
| `Inactive` | `Unoccupied` |

## 7. Guardar la configuración

1. Dejar `IS1` en `Inactive` para guardar el trazado sin un bloque ocupado.
2. En el menú **File**, usar la opción de almacenar la configuración (por ejemplo, `Store Configuration To File...`; el texto puede variar según la versión).
3. Guardar el XML como `simulated-layout.xml`.

El archivo XML debe versionarse en el repositorio en:

```text
software/railway-control/jmri/simulated-layout/simulated-layout.xml
```

## Inventario resultante

| Objeto | Identificador funcional | Identificador de JMRI |
|---|---|---|
| Sensor | `IS1` | `I2S1` |
| Desvío | `T1` | Prefijo interno + dirección `1` |
| Bloque | `B1` | `I2B1` |
| Bloque | `B2` | `I2B2` |
| Bloque | `B3` | `I2B3` |

## Siguiente paso

Habilitar y comprobar el servidor JSON/WebSocket de JMRI. La aplicación .NET se conectará a este servidor, escuchará los cambios de `IS1` y `T1`, y usará esas actualizaciones para alimentar el modelo de dominio.

## Documentación relacionada

- [Arquitectura JMRI](Railway-JMRI-Architecture.md)
- [Modelo de dominio](domain-model.md)
- [Estrategia de validación](04-Railway-Validation-Test-Strategy.md)
