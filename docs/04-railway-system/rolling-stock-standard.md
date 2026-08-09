# Rolling Stock Standard

**Status:** Draft / Candidate v0.1  
**Last updated:** 2026-08-09  
**Owner:** TBD

## Purpose

Definir interfaces comunes de locomotoras y vagones.

| Interface | v0.1 candidate |
|---|---|
| Track gauge | 45 mm |
| Rail/wheel compatibility | PIKO G Code 332 baseline; wheel dimensions TBD |
| Coupling | Commercial baseline; height, force and geometry TBD |
| Maximum vehicle envelope | TBD after curve/layout sweep |
| Maximum vehicle length | TBD after R1/R3/R5 tests |
| Maximum payload | 3 kg hypothesis; not approved |
| Overload | 5 kg controlled experiment only |
| Electrical interface | Track pickup candidate; onboard connector TBD |
| Identification | Unique asset ID + type + revision |
| Maintenance | Pre-shift inspection; interval based on cycles/failures TBD |
| Versioning | Semantic document revision + configuration ID |

## Conformance record

Dimensions, mass/CG, wheel and coupler gauges, curve/turnout test, electrical isolation, load rating evidence, cleaning status and deviations.

## Compatibility warning

Running on the same 45 mm track does not prove coupling, envelope, turnout or electrical compatibility.

## Change process

Proposal → impact review → prototype test → safety review where applicable → Candidate Decision/Decision. Current content remains candidate pending [MVP-02](../03-product/mvp-definition.md).
