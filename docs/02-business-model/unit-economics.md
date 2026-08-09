# Unit Economics

**Status:** Draft  
**Last updated:** 2026-08-09  
**Owner:** TBD

## Purpose

Definir un modelo rellenable antes de tener datos reales.

## Variables

| Symbol | Variable | Unit |
|---|---|---|
| AT | Average Ticket net of tax/discounts | COP/customer |
| FC | Food Cost % | % revenue |
| VC | Other variable cost/customer | COP |
| CM | Contribution Margin/customer | COP |
| C | Customers/day | customers |
| TR | Table Rotation | turns/table/day |
| OTH | Orders/train/hour | orders |
| MF | Railway maintenance fixed/day | COP |
| MO | Maintenance Cost/order | COP |
| F | Failed delivery rate | failures/orders |
| CAC | Acquisition cost/customer | COP |

## Formulas

`CM = AT × (1 − FC) − VC − MO − CAC`

`Daily contribution = C × CM − MF`

`Rail capacity/hour = active trains × OTH × (1 − F)`

`Required table turns = C / (tables × seats × average party fill)`

`Break-even customers/day = daily fixed costs / CM` when CM > 0.

## Scenario template

| Input | Low | Base | High | Source |
|---|---:|---:|---:|---|
| AT | TBD | TBD | TBD | Menu test |
| Customers/day | TBD | TBD | TBD | Pilot |
| FC | TBD | TBD | TBD | Recipe costing |
| MO | TBD | TBD | TBD | Maintenance log |
| OTH | TBD | TBD | TBD | Load simulation |
| F | TBD | TBD | TBD | Pilot telemetry |

## Decision rule

No aprobar local permanente hasta que un piloto entregue rango observable de CM, capacidad pico, fallos y mantenimiento. No mezclar CAPEX hundido con costo marginal, pero amortizarlo en el caso de inversión.
