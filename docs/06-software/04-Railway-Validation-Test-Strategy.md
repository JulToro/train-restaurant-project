# Estrategia de validación y pruebas — MVP ferroviario

## 1. Objetivo

Avanzar con software antes de que estén todos los componentes físicos, sin confundir una simulación lógica con una prueba de comportamiento físico.

## 2. Capas de prueba

| Capa | Qué valida | Mecanismo inicial |
|---|---|---|
| Dominio | Layout, rutas, reservas, timeouts y reglas. | Pruebas unitarias .NET. |
| Compilador | Modelo neutral → configuración JMRI/Jython correcta. | Golden files/snapshots. |
| Integración JMRI | Conexión, órdenes y eventos JSON/WebSocket. | JMRI local con objetos simulados. |
| UI | Estado, controles y manejo de desconexión. | Pruebas de componente/e2e. |
| Hardware | Servo, sensor, locomotora, feedback y parada. | Banco de pruebas físico. |

## 3. Alcance de JMRI simulado

JMRI permite representar objetos ferroviarios y simular cambios de sensor/turnout para probar la aplicación. No sustituye la física real: adherencia, aceleración, batería, ruido eléctrico, pérdida de contacto o movimiento mecánico del servo se validan físicamente.

## 4. Escenarios mínimos de aceptación

1. Un evento simulado de `S1 ACTIVE` ocupa `B1` en la UI.
2. Una orden `T1 THROWN` actualiza el estado y queda registrada.
3. Una desconexión WebSocket marca estado de JMRI como no confiable.
4. La ruta que exige `T1 CLOSED` no inicia si `T1` está en otra posición.
5. Un timeout de sensor detiene el tren lógico y genera alarma.
6. Con hardware: el reed/Hall físico cambia el estado en la UI.
7. Con hardware: el servo alcanza la posición deseada y, cuando exista, su feedback confirma el resultado.

## 5. Orden recomendado

1. Modelo + pruebas unitarias.
2. Compilador JMRI + snapshots.
3. WebSocket JMRI + simulación.
4. Panel de operación.
5. Locomotora controlada.
6. ESP32/PCA9685/servo.
7. Sensor físico.
8. Ruta demostrable de extremo a extremo.
