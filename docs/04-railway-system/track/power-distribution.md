# Power Distribution

**Status:** Draft  
**Last updated:** 2026-08-09  
**Owner:** TBD

## Purpose

Comparar arquitectura sin decisión prematura.

| Dimension | Track power | Battery + wireless |
|---|---|---|
| Energy | Continua si contacto bueno | Limitada por batería |
| Maintenance | Rail/wheel/joints | Charge, cells, connectors |
| Contamination | Directly affects pickup | Less traction-power sensitivity |
| Voltage drop | Feeders/zones/boosters | Local onboard |
| Short circuit | Whole zone impact unless isolated | Onboard fault containment needed |
| Vehicle mass | Lower onboard energy mass | Battery adds mass |
| Scaling | Wiring, districts, current budget | Fleet charging and RF management |
| Control | Analog/DCC possible | Wireless controller required |

**Candidate:** track power for MVP-00 because the PIKO set supplies 22 V/36 VA and a controller. This is not the restaurant decision.

## Experiments

Measure drop/current at 1/10/20 m equivalent, dirty rail cycles, short recovery, simultaneous trains, cleaning time, charge turnaround, RF loss and fail-safe. Design E-stop and protection with qualified electrical review.

## Sources

- [PIKO set 37100](https://www.piko-shop.de/en/artikel/g-db-br80-ore-starter-set-9345.html) — PIKO, consultado 2026-08-09.
- [PIKO G scale](https://www.piko-shop.de/en/warengruppe/g-scale-4.html) — PIKO, consultado 2026-08-09.
