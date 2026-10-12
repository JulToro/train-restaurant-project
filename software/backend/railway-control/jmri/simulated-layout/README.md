# Trazado simulado de JMRI

Esta carpeta contiene la configuración ejecutable de JMRI para el primer corte vertical de automatización ferroviaria.

## Contenido

```text
simulated-layout.xml  Configuración exportada desde PanelPro.
README.md             Instrucciones breves para abrir y validar el trazado.
```

## Propósito

Permitir que cualquier persona abra el mismo trazado simulado y pruebe la integración entre JMRI y la aplicación .NET sin conectar un CSB1, rieles, sensores ni desvíos físicos.

## Objetos configurados

| Tipo | Identificador funcional | Identificador de JMRI |
|---|---|---|
| Sensor | `IS1` | `I2S1` |
| Desvío | `T1` | Prefijo interno + dirección `1` |
| Bloque | `B1` | `I2B1` |
| Bloque | `B2` | `I2B2` |
| Bloque | `B3` | `I2B3` |

## Abrir el trazado

1. Instalar y abrir **PanelPro** de JMRI.
2. Crear o seleccionar un perfil sin hardware físico:
   - System manufacturer: `None`
   - System connection: `No Connection`
3. Usar el menú **File** para cargar `simulated-layout.xml`.
4. Abrir las tablas de `Sensors`, `Turnouts` y `Blocks`.

## Comprobación rápida

1. Cambiar el sensor `IS1` a `Active`.
2. Confirmar que el bloque `B1` pasa a `Occupied`.
3. Cambiar `IS1` a `Inactive`.
4. Confirmar que `B1` vuelve a `Unoccupied`.
5. Cambiar el desvío `T1` entre `Closed` y `Thrown`.

## Documentación completa

La configuración detallada, los valores introducidos y las notas sobre identificadores están en:

[`docs/06-software/jmri-simulated-layout-setup.md`](../../../../docs/06-software/jmri-simulated-layout-setup.md)

## Notas

- Este trazado es de simulación; no debe conectarse al CSB1.
- El prefijo interno de JMRI puede variar entre perfiles o equipos. Los nombres funcionales (`IS1`, `T1`, `B1`, `B2`, `B3`) son los que debe usar el dominio de la aplicación.
