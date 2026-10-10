# Casos de uso operativos — MVP ferroviario

## UC-01 — Crear una versión de layout

**Actor:** administrador técnico.  
**Resultado:** existe una versión en borrador con blocks, sensores, turnouts y conexiones válidas.

**Flujo principal:**

1. Crea o abre un layout.
2. Declara `B1`, `B2`, `B3`, `T1` y los sensores asociados.
3. Define las conexiones y posiciones requeridas de `T1`.
4. El sistema valida IDs, referencias y conflictos.
5. Guarda como `Draft`.

## UC-02 — Validar y desplegar un layout

**Actor:** administrador técnico.  
**Resultado:** una versión queda `Deployed` o `Failed` con evidencia.

**Flujo principal:**

1. Solicita validar una versión `Draft`.
2. El sistema valida modelo y genera configuración JMRI.
3. Solicita el despliegue explícitamente.
4. El adaptador aplica la configuración en JMRI.
5. El sistema registra script/configuración, respuesta y estado final.

## UC-03 — Supervisar sensores y ocupación

**Actor:** operador.  
**Resultado:** ve qué sensores están activos y qué blocks se consideran ocupados.

**Reglas:**

- El evento debe incluir hora, origen y estado anterior/nuevo.
- Un estado simulado debe distinguirse visualmente de uno físico.
- Una desconexión con JMRI debe mostrarse como estado desconocido, no como vía libre.

## UC-04 — Operar un tren manualmente

**Actor:** operador.  
**Resultado:** puede iniciar, detener, cambiar dirección y fijar velocidad del tren autorizado.

**Reglas:**

- La orden y la confirmación/estado conocido se registran por separado.
- Velocidad inicial y máxima son configurables.
- No debe haber más de un controlador lógico activo para el mismo tren.

## UC-05 — Cambiar un desvío

**Actor:** operador o flujo de ruta.  
**Resultado:** `T1` recibe orden `CLOSED` o `THROWN` y se muestra su estado.

**Reglas:**

- Si hay feedback físico, diferenciar orden enviada de posición confirmada.
- No cambiar un desvío que esté bloqueado por una regla de seguridad futura.
- Mientras no haya hardware, permitir modo simulado explícito.

## UC-06 — Ejecutar una ruta demostrable

**Actor:** operador.  
**Resultado:** el sistema prepara el desvío, permite mover el tren por una ruta seleccionada y detiene ante éxito o timeout.

**Flujo principal:**

1. Selecciona origen y destino lógico.
2. El sistema verifica que la ruta exista.
3. Reserva los blocks de la ruta.
4. Establece el turnout requerido.
5. Autoriza el movimiento.
6. Espera los sensores esperados.
7. Detiene el tren y libera las reservas al completar o fallar.

## UC-07 — Parada de emergencia

**Actor:** operador o sistema de seguridad.  
**Resultado:** se emite una orden de parada a todos los controladores de trenes activos y se deja evidencia del evento.

El botón físico definitivo no forma parte de este MVP, pero la aplicación debe tener el caso de uso y una interfaz que permita incorporarlo sin cambiar la lógica.
