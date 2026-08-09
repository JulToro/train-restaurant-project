# Turnouts

**Status:** Draft  
**Last updated:** 2026-08-09  
**Owner:** TBD

## Purpose

Definir evolución de ruteo y riesgos.

Un desvío dirige ruedas entre rutas; geometría, frog, flanges, alimentación y confirmación de posición influyen en confiabilidad.

| Stage | Actuation/control | Evidence |
|---|---|---|
| MVP-00/02 | Sin desvío o manual bloqueado | Baseline tracción |
| MVP-04A | Actuador eléctrico comercial + comando local | PIKO 35271 sirve para sus turnouts |
| MVP-04B | Servo alternativo + feedback | Banco, no público |
| Future | Decoder DCC / route control | Solo si complejidad lo justifica |

Sensores deben confirmar posición y ocupación; el comando enviado no es confirmación. Interlocking candidato impide mover un turnout bajo tren o autorizar rutas conflictivas. Estado desconocido => stop/fallback.

## Sources

- [PIKO electric turnout drive 35271](https://www.piko-shop.de/en/artikel/electric-point-switch-4450.html) — PIKO, consultado 2026-08-09.
- [PIKO G track](https://www.piko-shop.de/en/warengruppe/tracks-7/l-18/o-artikelnr_asc.html) — PIKO, consultado 2026-08-09.
