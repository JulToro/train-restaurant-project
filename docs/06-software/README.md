# Software y automatización ferroviaria

Este directorio contiene la definición y evolución del software que controla, observa y simula la operación ferroviaria del restaurante.

## Objetivo actual

Construir una primera columna vertebral de automatización demostrable en un entorno simulado con JMRI antes de depender del hardware pendiente.

El primer resultado buscado es:

1. JMRI simula el trazado mínimo.
2. Una aplicación .NET se conecta a JMRI por WebSocket.
3. Un sensor simulado (`IS1`) genera un evento.
4. El sistema actualiza la ocupación del bloque (`B1`).
5. La aplicación puede ordenar el cambio de un desvío (`T1`).
6. Una consola o página mínima muestra los eventos y estados.

Esto no es todavía el producto completo ni una arquitectura de microservicios. Es un corte vertical para validar la integración y generar una base real sobre la cual iterar.

## Estado de la documentación

| Documento | Propósito |
|---|---|
| [00-Railway-Predevelopment-Index.md](00-Railway-Predevelopment-Index.md) | Índice de documentos requeridos antes del desarrollo. |
| [01-Railway-MVP-Scope.md](01-Railway-MVP-Scope.md) | Alcance y criterio de éxito del MVP. |
| [02-Railway-Operational-Use-Cases.md](02-Railway-Operational-Use-Cases.md) | Casos operativos principales. |
| [Railway-JMRI-Architecture.md](Railway-JMRI-Architecture.md) | Arquitectura propuesta e integración con JMRI. |
| [03-Railway-Hybrid-Integration.md](03-Railway-Hybrid-Integration.md) | Operación híbrida: trenes DCC y trenes a batería. |
| [04-Railway-Validation-Test-Strategy.md](04-Railway-Validation-Test-Strategy.md) | Estrategia de validación simulada y física. |
| [architecture.md](architecture.md) | Notas y propuesta arquitectónica previa; debe consolidarse progresivamente con los documentos anteriores. |
| [domain-model.md](domain-model.md) | Modelo de dominio ferroviario. |
| [routing.md](routing.md) | Rutas y reservas. |
| [switch-management.md](switch-management.md) | Gestión de desvíos. |
| [train-control.md](train-control.md) | Control de trenes. |
| [integrations.md](integrations.md) | Integraciones externas. |

## Decisiones arquitectónicas

Las decisiones permanentes se registran en [`../13-decisions`](../13-decisions/).

Las decisiones existentes `ADR-001` a `ADR-003` cubren el sistema ferroviario físico. Las decisiones de software se numerarán desde `ADR-004` para mantener una única secuencia de ADRs en todo el proyecto.

Decisiones de software previstas:

- `ADR-004`: separación entre Configuration Service y Traffic Service.
- `ADR-005`: dominio ferroviario independiente de JMRI.
- `ADR-006`: WebSocket de JMRI para eventos de tiempo real.
- `ADR-007`: configuración versionada y despliegue controlado a JMRI.
- `ADR-008`: abstracción para trenes DCC y a batería.
- `ADR-009`: estrategia de parada de emergencia.
- `ADR-010`: simulación con JMRI antes de integración física.

## Corte vertical actual

**Épico:** `Automation Foundation — Simulated Control Spine`

**Hipótesis:** JMRI puede ser el adaptador ferroviario del sistema y una aplicación .NET puede reaccionar a eventos de sensores y controlar desvíos con la latencia y estabilidad necesarias para el MVP.

**Alcance del trazado simulado:**

| Tipo | Identificadores iniciales |
|---|---|
| Bloques | `B1`, `B2`, `B3` |
| Sensor de entrada | `IS1` |
| Desvío | `T1` |
| Tren | `Train-01` simulado |

**Fuera de alcance por ahora:**

- Base de datos de configuración.
- Configuration Service completo.
- Traffic Service completo.
- Interfaz React.
- Control de locomotora real.
- Sensores, ESP32, PCA9685 y servos físicos.
- Lógica avanzada de rutas, reservas o prevención de colisiones.

## Backlog inmediato

Crear las siguientes historias en este orden:

1. **Crear solución .NET de automatización ferroviaria**
   - Resultado: estructura compilable con `Railway.Core`, `Railway.Jmri.Client`, aplicación de prueba y pruebas unitarias.

2. **Configurar el trazado mínimo simulado en JMRI**
   - Resultado: `B1`, `B2`, `B3`, `IS1` y `T1` operables en JMRI sin hardware físico.

3. **Conectar .NET al WebSocket de JMRI**
   - Resultado: la aplicación establece conexión, registra eventos y reporta reconexiones.

4. **Actualizar ocupación de bloque desde un evento de sensor**
   - Resultado: el evento de `IS1` cambia el estado de `B1` en el modelo de dominio.

5. **Cambiar la posición de un desvío desde .NET**
   - Resultado: la aplicación ordena `CLOSED` o `THROWN` a `T1` y confirma el estado recibido desde JMRI.

6. **Mostrar diagnóstico mínimo de la operación**
   - Resultado: consola o interfaz mínima con conexión, eventos, bloques y estado del desvío.

## Definition of Ready

Una historia puede entrar a desarrollo cuando tiene:

- Resultado observable o demostrable.
- Criterios de aceptación concretos.
- Riesgo o hipótesis que valida.
- Enlace al documento o ADR que la justifica.
- Tamaño aproximado de uno a tres días.

## Definition of Done

Una historia está terminada cuando:

- El resultado funciona en JMRI simulado.
- Tiene una demostración repetible.
- Incluye pruebas cuando corresponde.
- Actualiza el documento técnico o ADR si cambió una decisión.
- Registra evidencia: captura, log, video corto o reporte de prueba.

## Siguiente acción

1. Crear y confirmar este archivo.
2. Crear el épico `Automation Foundation — Simulated Control Spine`.
3. Crear únicamente la primera historia: **Crear solución .NET de automatización ferroviaria**.
4. Comenzar a implementarla.

La prioridad es obtener evidencia funcionando, no completar documentación adicional.