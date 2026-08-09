# Evidence-Based Roadmap

**Status:** Draft  
**Last updated:** 2026-08-09  
**Owner:** TBD

## Purpose

Secuenciar gates, no prometer fechas arbitrarias.

~~~mermaid
flowchart TD
  D[Discovery] --> R[Reference Train MVP-00]
  R --> W[Wagon MVP-01]
  W --> T[Traction MVP-02]
  T --> C[Custom Train MVP-03]
  C --> A[Automated Turnout MVP-04]
  A --> P[20 m Prototype Candidate]
  P --> S[Restaurant Pilot MVP-05]
~~~

| Gate | Exit evidence |
|---|---|
| Discovery | Segment/pricing signal + ranked hazards/assumptions |
| Reference | Baseline V/I/speed/temp and maintenance |
| Wagon | Interface and cleanability |
| Traction | Load/curve/grade/endurance data |
| Custom | No regression and maintainability |
| Automation | Safe routing and recovery |
| 20 m candidate | Layout-derived need; endurance |
| Pilot | Capacity, safety, customer, cost and repeat signal |

La longitud de 20 m es **Candidate**, no requisito. Cancelar o rediseñar si un gate invalida economía o seguridad.
