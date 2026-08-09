# Assumptions Register

**Status:** Draft  
**Last updated:** 2026-08-09  
**Owner:** TBD

## Purpose

Mantener una lista única, priorizable y falsable de hipótesis.

| ID | Assumption | Category | Importance | Confidence | Validation method | Status |
|---|---|---|---|---|---|---|
| HYP-001 | El tren aumenta intención de visita | Market | High | Low | Concept test con precio y depósito | Open |
| HYP-002 | La comida y servicio producen repetición tras la novedad | Customer | Critical | Low | Piloto y seguimiento 60 días | Open |
| HYP-003 | Familias y turistas son segmentos iniciales accesibles | Market | High | Low | 20 entrevistas/segmento + canales | Open |
| HYP-004 | La entrega parcial es preferible a llevar todo por tren | Restaurant | High | Medium | Service blueprint y simulación | Open |
| HYP-005 | El cliente descarga la bandeja sin ayuda ni demora | Operations | High | Low | Prueba observada | Open |
| HYP-006 | 3 kg es una carga normal útil y alcanzable | Railway | Critical | Low | EXP-TRAC-002 | Open |
| HYP-007 | 5 kg funciona como sobrecarga breve, no operación normal | Railway | High | Low | EXP-TRAC-003 con límites de paro | Open |
| HYP-008 | Vía de 45 mm y ecosistema G permiten prototipado híbrido | Railway | High | Medium | Compra, compatibilidad y medición | Open |
| HYP-009 | Radios mayores reducen derrames y sobrevuelo | Railway | High | Medium | Ensayo comparativo R1/R3/R5 | Open |
| HYP-010 | Track power es adecuado para MVP | Electronics | High | Low | Ensayo suciedad/caída de tensión | Open |
| HYP-011 | Detección simple basta para automatización básica | Software | Medium | Low | Banco con dos bloques | Open |
| HYP-012 | Mantenimiento incremental por pedido es económicamente tolerable | Economy | Critical | Low | Registrar horas, piezas y fallos | Open |
| HYP-013 | El menú puede diseñarse para baja salpicadura y descarga segura | Food | Critical | Low | Pruebas de bandejas y temperatura | Open |
| HYP-014 | El riesgo infantil puede controlarse físicamente y con operación | Safety | Critical | Low | HAZID profesional + piloto | Open |
| HYP-015 | Hay repuestos importables con lead time manejable | Supply | High | Low | Cotizar stock y tiempos en Colombia | Open |
| HYP-016 | La identidad ferroviaria local puede usarse sin infringir marcas | IP | High | Low | Búsqueda y asesoría jurídica | Open |

## Prioritization rule

Atacar primero hipótesis Critical/Low cuya falsedad invalida el concepto: HYP-002, HYP-006, HYP-012, HYP-013 y HYP-014.

## Next Actions

Asignar owner, fecha objetivo y resultado enlazado a cada experimento. Ver [questions](questions-to-validate.md) y [risks](../02-business-model/risks.md).
