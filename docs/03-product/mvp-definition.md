# MVP Definition

**Status:** Draft  
**Last updated:** 2026-08-09  
**Owner:** TBD

## Purpose

Ordenar aprendizaje barato antes de integración.

| ID | Objective / hypothesis | Components | Acceptance criteria (candidate) | Measurements | Dependencies |
|---|---|---|---|---|---|
| MVP-00 | Caracterizar tren comercial | Set G, óvalo, instrumentos | Baseline repetible sin carga | V/I/speed/temp/falls | Purchase |
| MVP-01 | Probar vagón de comida propio | Chasis/ruedas/coupler/tray | Acopla, limpia, circula | Mass/envelope/derailments | MVP-00 |
| MVP-02 | Validar carga; HYP-006/007 | Pesos 1/3/5 kg, curvas/grade | Completa protocolo; 3/5 kg no son pass predefinido | Drawbar, slip, V/I/temp | MVP-01 |
| MVP-03 | Separar identidad de transmisión | Motor block comercial + body/chassis | Acceso de mantenimiento y desempeño no peor al baseline | MTTR, thermal, traction | MVP-02 |
| MVP-04 | Ruteo básico | 1 turnout, sensor, controller, E-stop | Ruta/parada/retorno y estado seguro ante fallo | success rate/latency/recovery | MVP-03 |
| MVP-05 | Piloto integrado pequeño | Despacho, mesas, menú, staff | Ciclo completo supervisado y criterios safety aprobados | Time/order, failures, spills, feedback, cost | All + safety review |

Cada gate exige resultados y decisión documentada; “moverse” no basta. Ver [traction tests](../04-railway-system/locomotive/traction-tests.md).
