# Product Vision

**Status:** Draft  
**Last updated:** 2026-08-09  
**Owner:** TBD

## Purpose

Definir el producto inicial y su experiencia verificable.

Para familias, turistas y grupos que buscan una comida memorable, railway-restaurant es un restaurante con entrega ferroviaria parcial que combina gastronomía, hospitalidad y un ritual confiable. A diferencia de una maqueta decorativa, el sistema forma parte de la operación y tiene fallback humano.

## Objectives

Entrega correcta y segura; experiencia comprensible; comida en condición aceptable; recuperación rápida; operación mantenible; evidencia de repetición y margen.

## Principles

Human-in-the-loop, fail-safe, modular, observable, limpio, accesible y construido por etapas. La plataforma para terceros es visión futura no validada.

~~~mermaid
flowchart LR
  Kitchen --> Dispatch --> Train --> Table --> Return --> Cleaning --> Dispatch
~~~

See: [scope](product-scope.md), [MVP](mvp-definition.md), [vision](../00-discovery/vision.md).
