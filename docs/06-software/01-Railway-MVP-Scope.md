# Alcance del MVP — Control ferroviario inicial

## 1. Objetivo

Demostrar que una aplicación propia puede configurar y supervisar un circuito ferroviario sencillo usando JMRI, y posteriormente intercambiar eventos simulados por hardware físico sin rediseñar el sistema.

## 2. Circuito de referencia

El MVP parte del circuito disponible actualmente:

- Un óvalo de vía escala G.
- Un desvío (`T1`) que deriva a una ruta alternativa o apartadero.
- Una locomotora de prueba, inicialmente Blue Goose.
- Sensores que al comienzo pueden estar simulados y luego serán reed/Hall físicos.

Modelo lógico inicial sugerido:

```text
             ┌────────────── B2 ──────────────┐
             │                                │
  Cocina/B1 ─ T1                               │
             │                                │
             └────────────── B3 ──────────────┘
```

`B1`, `B2` y `B3` son blocks lógicos. La geometría física puede ajustarse cuando se monte el desvío y sus sensores.

## 3. Criterio de éxito del MVP

El MVP se considera exitoso cuando el operador puede:

1. Ver el layout activo y el estado de cada block, sensor y turnout.
2. Ordenar inicio, parada, dirección y velocidad de la locomotora compatible con el controlador disponible.
3. Cambiar `T1` entre `CLOSED` y `THROWN` desde la aplicación.
4. Registrar y mostrar cambios de sensor en tiempo real.
5. Usar los mismos flujos con eventos simulados en JMRI y con al menos un sensor físico posteriormente.
6. Detener el movimiento ante una orden explícita de parada o un timeout de ruta configurado.

## 4. Alcance incluido

- Crear, guardar y validar una versión de layout sencilla.
- Generar configuración JMRI para blocks, sensores, turnouts, paths y requisitos de turnout.
- Conexión .NET ↔ JMRI para estados y órdenes runtime.
- Panel mínimo: estado de conexión, potencia, turnout, sensores, block ocupado y control básico de un tren.
- Simulación de eventos de sensor/ocupación para pruebas de software.
- Bitácora de órdenes y eventos principales.

## 5. Fuera de alcance

- Varios trenes autónomos simultáneos.
- Pedidos reales, facturación, cocina o asignación comercial de mesas.
- RFID, desacople automático y reconocimiento de bandejas.
- Optimización automática de rutas.
- Múltiples boosters/distritos eléctricos.
- Un editor visual completo estilo JMRI Layout Editor.
- Certificación o arquitectura final de seguridad para operación comercial.

## 6. Hitos

| Hito | Resultado |
|---|---|
| M1 — Software simulado | UI, layout y eventos simulados funcionando contra JMRI. |
| M2 — Tren controlado | Control de velocidad/parada de una locomotora por el mecanismo disponible. |
| M3 — Desvío físico | `T1` mueve su servo y reporta estado. |
| M4 — Sensor físico | Un reed/Hall actualiza un block desde el paso del tren. |
| M5 — Ruta demostrable | Recorrido controlado por el óvalo y desvío, con parada por condición o timeout. |
