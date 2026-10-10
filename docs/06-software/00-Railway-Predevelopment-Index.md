# Railway Platform — Paquete de predesarrollo

**Estado:** base de trabajo antes de crear historias de desarrollo  
**Proyecto:** sistema ferroviario para restaurante, escala G, JMRI y DCC-EX/CSB1.

## Propósito

Estos documentos convierten la conversación y la arquitectura inicial en decisiones, alcance y criterios verificables. Su objetivo es que las futuras historias tengan un límite claro y criterios de aceptación medibles.

## Documentos

| Orden | Documento | Para qué sirve |
|---:|---|---|
| 0 | `Railway-JMRI-Architecture.md` | Arquitectura general de servicios, modelo y despliegue. |
| 1 | `01-Railway-MVP-Scope.md` | Qué demuestra el primer MVP y qué queda fuera. |
| 2 | `02-Railway-Operational-Use-Cases.md` | Comportamientos que deben convertirse en historias. |
| 3 | `03-Railway-Hybrid-Integration.md` | Integración JMRI/DCC-EX, batería, ESP32, sensores y E-STOP. |
| 4 | `04-Railway-Validation-Test-Strategy.md` | Qué se prueba virtualmente y qué exige hardware físico. |
| ADR | `ADR-001` a `ADR-007` | Decisiones ya tomadas para no reinterpretarlas durante desarrollo. |

## Cómo pasar a historias

Una vez revisados estos documentos, las historias se crean por épica:

1. Modelo y versión de layout.
2. Compilación y despliegue a JMRI.
3. Cliente runtime JMRI/WebSocket.
4. UI de control y observabilidad.
5. Simulación y pruebas.
6. Integración física de sensores y desvíos.
7. Control híbrido y seguridad.

No se debe crear una historia que contradiga un ADR. Si una decisión cambia, se crea un ADR nuevo que reemplaza o invalida el anterior.

## Pendientes que bloquean solo etapas posteriores

- Elegir el protocolo del controlador ESP32 de accesorios/trenes a batería.
- Definir la implementación física de feedback del desvío.
- Elegir persistencia inicial (MongoDB o PostgreSQL).
- Definir el mecanismo seguro de despliegue de scripts/configuración en JMRI.
- Diseñar el botón E-STOP físico y su cableado.

Estos pendientes no bloquean comenzar UI, modelo de dominio, simulación ni el cliente WebSocket.
