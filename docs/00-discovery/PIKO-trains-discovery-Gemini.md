# Investigación Técnica de Locomotoras PIKO (Escala G)
## Clasificación Arquitectónica, Mecánica y Eléctrica para Pruebas de Tracción y Automatización

**Estado:** Completado / Versión de Referencia Técnica  
**Fecha de Actualización:** 6 de Septiembre de 2026  
**Investigador:** Experto Técnico en Ferromodelismo Escala G y Sistemas DCC  

---

## 1. Introducción y Objetivos

El propósito de esta investigación es analizar en profundidad la gama de locomotoras de la marca **PIKO G** (escala 1:22.5, ancho de vía de 45 mm) para identificar las plataformas electromecánicas más viables para pruebas de tracción, automatización y operación continua dentro del sistema de transporte automatizado de nuestro restaurante ferroviario.

Al igual que en el estudio anterior de la marca LGB, **esta clasificación se basa estrictamente en la arquitectura interna y la electromecánica de las locomotoras**, prescindiendo de valoraciones estéticas o históricas. Analizaremos factores críticos como el tipo y cantidad de motores, el diseño de la transmisión, el aislamiento de los bloques de motor, los tipos de interfaz digital (incluyendo las nuevas generaciones PIKO SmartDecoder XP 5.1 G y las interfaces desmontables por techo) y su comportamiento de consumo eléctrico. Esto nos permitirá comparar de manera objetiva ambas marcas para tomar una decisión informada sobre la flota final del restaurante.

---

## 2. Definiciones Técnicas y Diferencias Críticas

Para interpretar adecuadamente los resultados de esta investigación, es necesario comprender la evolución técnica de PIKO G y las características de sus motorizaciones y sistemas electrónicos.

### A. Modularidad del Chasis y Acceso por Techo (Removable Roof)
A diferencia de LGB, donde el desmontaje de la carrocería suele requerir remover múltiples tornillos inferiores y lidiar con cableados de luces frágiles, PIKO introdujo en sus modelos modernos de gran tamaño (como la locomotora eléctrica **BR 103** y la diésel pesada **BR 132 "Ludmilla"**) un sistema de **acceso directo por el techo**.
*   **Funcionamiento:** Retirando únicamente un par de tornillos en las secciones superiores del techo, este se levanta de manera independiente, exponiendo la placa de circuito principal y las publicaciones de montaje del decoder de manera inmediata.
*   **Impacto Técnico:** Reduce el tiempo de instalación de decoders y mantenimiento a menos de 15 minutos, minimizando drásticamente la manipulación de piezas estéticas delicadas y el riesgo de pellizcar cables durante el reensamblaje.

### B. Conectividad y Aislamiento de los Motores PIKO G
Todos los bloques motores de PIKO G son modernos y vienen con **aislamiento galvánico de fábrica**.
*   **Bloques de 4 Pines:** Cada bloque motor tiene un conector de 4 pines en su parte superior. Los dos pines exteriores están conectados directamente a las escobillas de las ruedas y los patines de latón para captar la corriente de la vía (*Track Power*). Los dos pines interiores se conectan directamente a las terminales del motor eléctrico (*Motor Power*).
*   **Facilidad de Digitalización:** Al estar completamente aislados por diseño, no existe riesgo de cortocircuito por "retornos comunes" (como ocurre en los bloques antiguos de 3 pines de LGB). La conversión de analógico a DCC es siempre directa y reversible.

### C. Análisis de Corriente y Potencia Eléctrica en Motores PIKO G
PIKO G utiliza principalmente motores de corriente continua de 5 polos de alta precisión (muchos fabricados por Bühler o clones de alta calidad bajo especificaciones idénticas de Bühler).

*   **Corriente sin Carga (Light Engine):** 0.3 A a 0.5 A por bloque motor (a 24V DC nominales).
*   **Corriente Operacional Normal (Pulling Cars):** 0.8 A a 1.0 A por bloque motor bajo carga en trazado plano.
*   **Corriente de Patinado de Ruedas (Wheel-Slip):** 1.2 A a 1.5 A por motor.
*   **Corriente de Bloqueo (Stall Current - Rotor Fijo):** **1.3 A** nominal según catálogo técnico del motor a 24V. No obstante, bajo condiciones de vía del mundo real con voltajes de 18V a 22V, un bloqueo total (engranaje trabado, bielas desalineadas) puede registrar picos de **1.5 A a 2.5 A** en un solo motor, y hasta **3.0 A a 4.0 A** combinados en locomotoras bimotores.
*   **Potencia Eléctrica de Bloqueo Calculada ($P = V \times I$):**
    *   *Monomotor (Nominal):* $24\text{V} \times 1.3\text{A} = 31.2\text{ W}$ de potencia consumida en bloqueo.
    *   *Monomotor (Pico en vía):* $22\text{V} \times 2.5\text{A} = 55\text{ W}$.
    *   *Bimotor (Nominal):* $24\text{V} \times 2.6\text{A} = 62.4\text{ W}$ de potencia consumida en bloqueo.
    *   *Bimotor (Pico en vía):* $22\text{V} \times 4.0\text{A} = 88\text{ W}$.
*   **Potencia Eléctrica Normal Calculada en Tracción Media:**
    *   *Monomotor:* $24\text{V} \times 0.8\text{A} = 19.2\text{ W}$.
    *   *Bimotor:* $24\text{V} \times 1.6\text{A} = 38.4\text{ W}$.

### D. Evolución de los Decoders PIKO: De Massoth a SmartDecoder XP 5.1 G
PIKO ha tenido tres etapas muy claras en su ecosistema digital para la escala G:
1.  **Etapa de Cooperación con Massoth (Legacy G):** Los primeros decoders de PIKO (ej. referencias **#36120, #36121, #36122**) eran fabricados por Massoth bajo especificación OEM. Se caracterizaban por ser robustos, programarse de manera idéntica a la gama eMOTION y contar con un puerto físico **SUSI** para conectar módulos de sonido separados (ej. **#36190, #36220**).
2.  **Generación SmartDecoder 4.1 G (Transición):** Introducida para unificar su gama con decoders multiprotocolo de alta corriente (hasta 3.0A continuos), equipados con interfaces SUSI y mejoras en el control de carga.
3.  **Generación SmartDecoder XP 5.1 G (Actual):** Representa el estándar tecnológico actual de PIKO. Son decoders de hasta **5.0A de corriente continua** con sonido digital de 16 bits (8 canales independientes), soporte RailComPlus y control de energía inteligente incorporado. Se conectan mediante placas adaptadoras de interfaz dedicadas (ej. **PIKO #36511** para diésel/eléctricas y **#36512** para vapor BR 80).
    *   *Incompatibilidad de NCE/American DCC (Problema de los 3 segundos):* Al operar decoders XP 5.1 G en sistemas DCC americanos como NCE o Digitrax, se presenta una anomalía de firmware donde la locomotora apaga luces y sonido automáticamente tras 3 segundos de inactividad física. **La solución oficial es programar la CV 250 a un valor de 1 (o CV 2550 a 1 dependiendo del lote de firmware) para deshabilitar este temporizador de inactividad.**

---

## 3. Clasificación de Familias Técnicas PIKO G

Establecemos la siguiente codificación arquitectónica para las plataformas de PIKO:

*   **PM-1M-DR:** Un motor de 5 polos. Bloque Sandwich aislado de fábrica con conexiones de 4 pines en placa. Sin sonido de fábrica, DCC-Ready mediante regleta de bornes o interfaz de tornillo simple.
*   **PM-1M-S:** Un motor de 5 polos. Bloque aislado. Sonido digital instalado de fábrica (MTS/DCC SmartDecoder).
*   **PM-1M-SPEC:** Un motor de 5 polos en chasis rígido con transmisión por bielas y engranajes combinados. Compartimento digital y altavoz ubicados en el Tender (con cable umbilical de conexión). DCC-Ready o con sonido.
*   **PM-2M-DR:** Dos motores de 5 polos en bogies independientes aislados. DCC-Ready mediante placa central analógica con puentes desmontables o DIP switches. Sin sonido de fábrica.
*   **PM-2M-PnP:** Dos motores de 5 polos. Acceso rápido al compartimento digital mediante compuerta de techo desmontable (*Removable Roof*). Interfaz DCC-Ready de instalación plug-and-play directa.
*   **PM-2MS-D:** Dos motores de 5 polos. Equipadas de fábrica con decoders SmartDecoder XP 5.1 G con sonido digital de 16 bits y RailComPlus.

---

## 4. Tabla Principal de Familias Técnicas (Consolidada)

| Familia técnica | Marca | Configuración interna | Referencias que utilizan la configuración | Motores | Bloques motores | Ejes motrices | Transmisión | Decoder de fábrica | Protocolos | Corriente decoder | Consumo conocido | Potencia conocida o calculada | Sonido | Humo | Servos | Capacitor | Decoders alternativos | Dificultad de conversión | Compatibilidad EX-CSB1 | Repuestos compartidos | Nivel de confianza | Fuentes |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **PM-1M-DR** | PIKO | Monomotor compacto, bloque Sandwich aislado, DCC-Ready. | 37100, 38500, 38502, 37520, 37521, 37522, 37530, 37560 | 1 motor de 5 polos (Bühler o similar) | 1 | 2 (BR80/GE25) o 3 (V36/V60) | Engranajes helicoidales cerrados, tracción directa o bielas | Ninguno (placa analógica de puente) | Analógico DC | N/A | 0.3A-0.5A normal. Est. bloqueo: 1.5A | Bloqueo calc: 36W. Tracción calc: 19.2W | No (37120 tiene sonido analógico) | No (Preparado para 5V Seuthe en BR 80) | No | No | PIKO 36122, Massoth eMOTION L, Zimo MS950, ESU LokSound 5 L | **Baja** (Acceso simple, bornes de tornillo o conectores slip-on) | Excelente y directa | Motores de 5 polos, escobillas de carbón, patines de latón, piñones | **Alto** (Confirmado por manuales de despiece y guías de servicio oficial PIKO) | [1], [3], [5], [6], [19] |
| **PM-1M-S** | PIKO | Monomotor con sonido digital instalado de fábrica. | 37125, 37525, 37562 | 1 motor de 5 polos | 1 | 2 o 3 | Helicoidal cerrado con bielas de acoplamiento | PIKO SmartDecoder 4.1 o XP 5.1 G | DCC, Motorola, mfx | 3.0A-4.0A Motor | 0.4A-0.6A normal. Est. bloqueo: 1.5A | Bloqueo calc: 36W. Tracción calc: 14.4W | Sí (16 bits de fábrica) | Sí (Controlado por decoder en vapor) | No | No (algunos con capacitor de fábrica) | ESU LokSound 5 XL, Massoth XLS | **Muy Baja** (Listo para correr) | Directa. Requiere dirección DCC (fábrica suele ser 3) | Motor, piñones, engranajes, altavoz, carbones | **Alto** (Fichas técnicas y catálogos de repuestos oficiales) | [4], [10], [13] |
| **PM-1M-SPEC**| PIKO | Monomotor a vapor con electrónica alojada en el Tender. | 37220, 37221, 38210, 38220, 37240 | 1 motor de 5 polos (montado en caldera/bogie) | 1 | 3 (Mogul/BR24) | Piñón sinfín en bloque motor, bielas de acoplamiento Walschaerts | Ninguno (Tender con PCB analógica de puente) | Analógico DC | N/A | 0.4A-0.7A normal. Est. bloqueo: 1.8A | Bloqueo calc: 43.2W. Tracción calc: 16.8W | No | Sí (Seuthe 5V en caldera) | No | No | PIKO 36122 + 36222 SUSI, Massoth XLS (Tender), ESU LokSound 5 L | **Media** (Requiere abrir tender, pasar cables a la locomotora por umbilical) | Excelente. Monitorear cable umbilical para evitar cortos | Motor, bielas Walschaerts, engranajes, zapatas | **Alto** (Especificaciones de manual de BR 24 y Mogul en PDF) | [5], [6], [8], [10] |
| **PM-2M-DR** | PIKO | Bimotor bogies independientes analógicos, DCC-Ready. | 37410, 37411, 37500, 37501, 37510 | 2 motores de 5 polos | 2 | 4 | Dos bogies motorizados independientes con tracción total | Ninguno (Placa analógica con jumper de puente) | Analógico DC | N/A | 0.6A-1.2A normal. Est. bloqueo: 3.0A | Bloqueo calc: 72W. Tracción calc: 38.4W | No | No | No | No | PIKO 36511 (Carrier) + 36535, Massoth eMOTION XL, ESU LokSound 5 XL | **Media-Baja** (Apertura de carrocería, montaje de placa puente y decoder) | Excelente. Cuidado con corriente de bloqueo de dos motores | Bogies motorizados de repuesto, ruedas con llantas, patines | **Alto** (Documentado ampliamente en foros de conversión técnica) | [1], [2], [3], [5], [10] |
| **PM-2M-PnP** | PIKO | Bimotor con acceso rápido por techo para instalación directa. | 37300, 37540, 37511 | 2 motores de 5 polos | 2 | 4 (BR103) o 6 (BR132) | Bogies de 2 o 3 ejes con piñones de nylon | Ninguno (Placa analógica con regleta accesible por techo) | Analógico DC | N/A | 0.6A-1.3A normal. Est. bloqueo: 3.0A | Bloqueo calc: 72W. Tracción calc: 31.2W | No | No | No | No | PIKO 36526 / 36529 XP 5.1 Sound, ESU LokSound 5 XL, Zimo MS990 | **Muy Baja** (Solderless, apertura de compuerta de techo y atornillar bornes) | Directa y muy limpia | Bogies, motores, piñones de repuesto | **Alto** (Confirmado por manuales de despiece de BR 103 y BR 132) | [1], [2], [4], [12] |
| **PM-2MS-D** | PIKO | Bimotor pesado con sonido y RailCom de fábrica. | 37512, 37542, 37302 | 2 motores de 5 polos | 2 | 4 o 6 | Bogies articulados pesados | PIKO SmartDecoder XP 5.1 G (OEM) | DCC, mfx, Motorola, DC | 5.0A Motor | 0.8A-1.5A normal. Est. bloqueo: 3.0A | Bloqueo calc: 72W. Tracción calc: 36W | Sí (16 bits de fábrica) | No (Pre-equipado en diésel BR 132) | No | Sí (Buffer de energía integrado) | No requiere (Reemplazo con ESU LokSound 5 XL) | **Muy Baja** (No requiere modificación alguna) | Compatible. **Atención a la configuración CV 250 para sistemas NCE** | Motores, ejes, engranajes, altavoz, patines | **Alto** (Manuales de usuario oficiales de lanzamientos post-2022) | [4], [5], [10], [11] |

---

## 5. Tabla Secundaria por Referencia (Detallada para Compras)

La siguiente tabla desglosa de manera individual las **24 referencias analizadas** para facilitar su rastreo y adquisición en mercados secundarios o distribuidores.

| Marca | Referencia | Modelo | Familia técnica | DCC de fábrica | DCC-ready | Conversión documentada | Número de motores | Ejes motrices | Decoder | Sonido | Humo | Estado comercial | Radio mínimo | Fuente principal |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| PIKO | **37100** | DB BR 80 (Vapor) | **PM-1M-DR** | No | Sí | Sí (Bornes de tornillo) | 1 | 2 | Ninguno | No | No (Prep 5V) | Descontinuada | 600 mm | piko-shop.de [3] |
| PIKO | **37120** | DB BR 80 (Ore Set) | **PM-1M-DR** | No | Sí | Sí (Bypass de sonido analógico) | 1 | 2 | Ninguno | Analógico | Sí (5V) | Descontinuada | 600 mm | piko-shop.de [5] |
| PIKO | **37125** | DB BR 80 (Sonido) | **PM-1M-S** | Sí | Sí | N/A (Uso directo) | 1 | 2 | SmartDecoder 4.1 G | Sí | Sí (5V) | Descontinuada | 600 mm | Catálogo Oficial PIKO |
| PIKO | **37410** | DB Taurus Rh 1116 | **PM-2M-DR** | No | Sí | Sí (Fácil, placa en chasis) | 2 | 4 | Ninguno | No | No | Descontinuada | 600 mm | swl4.com [1] |
| PIKO | **37411** | DB Taurus (New Run) | **PM-2M-DR** | No | Sí | Sí (Fácil, puente de fábrica) | 2 | 4 | Ninguno | No | No | Descontinuada | 600 mm | piko-shop.de [28] |
| PIKO | **37520** | DB BR 260 / V 60 | **PM-1M-DR** | No | Sí | Sí (LEDs requieren cuidado 5V) | 1 | 3 | Ninguno | No | No | Descontinuada | 600 mm | onlytrains.com [1] |
| PIKO | **37521** | DB BR 260 (Azul/Beige) | **PM-1M-DR** | No | Sí | Sí (Igual a 37520) | 1 | 3 | Ninguno | No | No | Descontinuada | 600 mm | onlytrains.com [1] |
| PIKO | **37522** | DB BR 260 (Red Livery) | **PM-1M-DR** | No | Sí | Sí (Igual a 37520) | 1 | 3 | Ninguno | No | No | Descontinuada | 600 mm | onlytrains.com [1] |
| PIKO | **37500** | DB BR 218 AG Red | **PM-2M-DR** | No | Sí | Sí (Requiere 36511 Carrier) | 2 | 4 | Ninguno | No | No | Descontinuada | 600 mm | youtube.com [5] |
| PIKO | **37501** | DB BR 218 Blue/Beige | **PM-2M-DR** | No | Sí | Sí (Requiere 36511 Carrier) | 2 | 4 | Ninguno | No | No | Descontinuada | 600 mm | swl4.com [6] |
| PIKO | **37511** | DB BR 218 Cottbus | **PM-2M-PnP** | No | Sí | Sí (Plug-and-play directo) | 2 | 4 | Ninguno | No | No | Descontinuada | 600 mm | ironplanethobbies.com [7] |
| PIKO | **37512** | DB BR 218 Latz (Sound) | **PM-2MS-D** | Sí | Sí | N/A (Uso directo) | 2 | 4 | SmartDecoder XP 5.1 G | Sí | No | Actual | 600 mm | piko-shop.de [4] |
| PIKO | **37220** | DB BR 24 (Steam) | **PM-1M-SPEC** | No | Sí | Sí (Electrónica en Tender) | 1 | 3 | Ninguno | No | No (Prep 5V) | Descontinuada | 600 mm | manuals.plus [5] |
| PIKO | **37221** | DB BR 24 (New Run) | **PM-1M-SPEC** | No | Sí | Sí (Tender con Carrier) | 1 | 3 | Ninguno | No | No (Prep 5V) | Descontinuada | 600 mm | world-of-trains.ch [6] |
| PIKO | **37300** | DB BR 103 (Electric) | **PM-2M-PnP** | No | Sí | Sí (Acceso por techo, bornes) | 2 | 4 | Ninguno | No | No | Descontinuada | 600 mm | elriwa.de [2] |
| PIKO | **37302** | DB BR 103 (Sound) | **PM-2MS-D** | Sí | Sí | N/A (Uso directo) | 2 | 4 | SmartDecoder XP 5.1 G | Sí | No | Actual | 600 mm | piko-shop.de [4] |
| PIKO | **37540** | DR BR 132 Ludmilla | **PM-2M-PnP** | No | Sí | Sí (Acceso por techo, bornes) | 2 | 6 | Ninguno | No | No (Prep) | Descontinuada | 600 mm | elriwa.de [2] |
| PIKO | **37542** | DR BR 132 (Sound) | **PM-2MS-D** | Sí | Sí | N/A (Uso directo) | 2 | 6 | SmartDecoder XP 5.1 G | Sí | No (Prep) | Actual | 600 mm | piko-shop.de [10] |
| PIKO | **38500** | GE 25-Ton Switcher D&RGW| **PM-1M-DR** | No | Sí | Sí (Desmontar techo cabina) | 1 | 2 | Ninguno | No | No | Descontinuada | 600 mm | modelrailroadnews.com [2] |
| PIKO | **38502** | GE 25-Ton "Blue Goose" | **PM-1M-DR** | No | Sí | Sí (Igual a 38500) | 1 | 2 | Ninguno | No | No | Descontinuada | 600 mm | modelrailroadnews.com [2] |
| PIKO | **38210** | Mogul 2-6-0 Steam | **PM-1M-SPEC** | No | Sí | Sí (Electrónica en Tender) | 1 | 3 | Ninguno | No | Sí (5V) | Descontinuada | 600 mm | facebook.com [11] |
| PIKO | **38220** | Camelback 2-6-0 Steam | **PM-1M-SPEC** | No | Sí | Sí (Igual a 38210) | 1 | 3 | Ninguno | No | Sí (5V) | Descontinuada | 600 mm | Manuales de Servicio PIKO |
| PIKO | **37510** | DR V100 / BR 110 | **PM-2M-DR** | No | Sí | Sí (Desmontaje total) | 2 | 4 | Ninguno | No | No | Descontinuada | 600 mm | gscalecentral.net [1] |
| PIKO | **37530** | DB V36 (Diesel) | **PM-1M-DR** | No | Sí | Sí (Desmontar cabina/capó) | 1 | 3 | Ninguno | No | No | Descontinuada | 600 mm | piko.de [21] |

---

## 6. Compatibilidad de Decoders de Otras Marcas

El diseño de las placas electrónicas modernas de PIKO y el total aislamiento de sus bloques de motor permiten una flexibilidad excelente para instalar decoders de fabricantes especializados en escala G:

### A. Fabricantes Alternativos Recomendados

#### 1. Massoth (Gama eMOTION XL / XLS / XLS-M)
*   **Compatibilidad:** Máxima. Al haber sido fabricantes OEM de las primeras generaciones de PIKO G, la compatibilidad con su electrónica es nativa.
*   **Modelos Recomendados:**
    *   **Massoth XLS-M (#8220530 para V60 / #8220038 para BR 218):** Son decoders precargados con sonido específico de cada locomotora PIKO y con conectores específicos para la escala G[10][13].
    *   **Massoth eMOTION XL (3.0A continuos / 4.0A pico):** Excelente opción para las locomotoras bimotores analógicas (como Taurus o BR 218 de la primera generación) donde se realiza una instalación limpia sin regleta propietaria[10].
*   **Consideraciones de Iluminación:** Las salidas de función de Massoth son perfectamente compatibles con los LEDs de PIKO, siempre que se utilicen los cables originales de PIKO que ya integran resistencias inline, o se programen las salidas de Massoth para limitar el voltaje[10].

#### 2. ESU (Gama LokSound 5 L / 5 XL)
*   **Compatibilidad:** Sobresaliente. Es la opción preferida por modelistas que buscan máxima fidelidad sonora (16 bits) y la función de calibración automática del motor (*Auto-Tune*)[8].
*   **Modelos Recomendados:**
    *   **LokSound 5 XL (5.0A continuos):** Es obligatorio usar la versión XL para todas las locomotoras bimotores pesadas de PIKO (como la BR 132 "Ludmilla" o la BR 103)[8]. Puede manejar cómodamente la corriente agregada de arranque de ambos motores y tiene un excelente control térmico[8].
    *   **LokSound 5 L (3.0A continuos):** Ideal para locomotoras monomotor ligeras como el GE 25-Ton o la V36, donde el espacio interior es más limitado pero se requiere sonido digital de alta gama[8].

#### 3. ZIMO (Gama MS950 / MS990 / MX699)
*   **Compatibilidad:** Excelente (Altas prestaciones). Zimo destaca por su control de motor a baja velocidad y su capacidad para gestionar capacitores de almacenamiento masivo sin placas externas de control[8].
*   **Modelos Recomendados:**
    *   **Zimo MS990 (6.0A continuos):** Ideal para las locomotoras de vapor pesadas (como la BR 24 o Mogul) donde se desea sincronizar los chuffs de vapor mediante sensores magnéticos en las ruedas (*Wheel Pulse*)[10].
*   **Soporte de Salidas:** Ofrece la flexibilidad de configurar salidas a bajo voltaje constante para las unidades de humo de 5V de Seuthe instaladas en las locomotoras de vapor de PIKO.

---

### B. Matriz de Reversibilidad y Complejidad de Conversión

*   **¿Es reversible la conversión?** Sí, en un 100% en todas las familias de PIKO G. Al no requerir cortes de láminas internas (ya que los motores vienen totalmente aislados con conectores independientes de 4 pines), es tan simple como desconectar el decodificador e insertar la placa puente o jumper analógico original[11][12].
*   **Nivel de Dificultad de Conversión:**
    *   *Muy Bajo (Plug-and-Play de Techo):* Familias **PM-2M-PnP** (ej. BR 132 / BR 103). Se retiran tornillos del techo, se remueve el jumper analógico de la regleta y se monta el decoder atornillando los cables en regletas de bornes[2]. No requiere soldadura ni remoción de la carrocería completa[2].
    *   *Bajo (Cabina Desmontable):* Modelos compactos monomotor como el GE 25-Ton (38500). Se retira la tapa del techo de la cabina y se reemplaza la placa analógica por el decodificador #36122 directamente[4].
    *   *Medio (Desmontaje Total de Carrocería):* Familias **PM-2M-DR** y **PM-1M-SPEC** (ej. Taurus, BR 218 antiguos, locomotoras de vapor). Requiere retirar entre 6 y 10 tornillos inferiores, desmontar las carrocerías, instalar la placa carrier #36511 y cablear los motores, luces y altavoz hacia los terminales de tornillo del decodificador[1][5][10].

---

## 7. Compatibilidad con Centrales DCC NMRA y DCC-EX EX-CSB1

Nuestra central de pruebas basada en Arduino, la **DCC-EX EX-CSB1**, ofrece un driver de alta potencia con una salida constante de **5.0 A** y conectividad integrada. El comportamiento de las locomotoras PIKO G con este sistema presenta las siguientes consideraciones técnicas:

### A. Compatibilidad Directa y Protocolos
*   **Protocolo DCC Estándar:** Todos los decodificadores oficiales de PIKO (SmartDecoder 4.1 y XP 5.1 G) y de terceras marcas (Massoth, ESU, Zimo) instalados en los modelos analizados cumplen estrictamente con las normas NMRA DCC. Responden directamente a comandos de velocidad, luces y mapeo de hasta 28 funciones enviadas por la central DCC-EX.
*   **Margen de Corriente de la Central (EX-CSB1):**
    *   Un motor de 5 polos de PIKO en condiciones normales de limpieza consume entre **0.4A y 0.5A** en trazados planos sin carga[4].
    *   Con carga moderada (vagones bandeja del restaurante), el consumo sube a **0.8A–1.0A** por motor[6].
    *   *Monomotor (V36, BR 80, GE 25-Ton):* El EX-CSB1 puede mantener en funcionamiento simultáneo hasta **5 locomotoras monomotor con carga máxima** ($5 \times 1.0\text{A} = 5.0\text{A}$ de margen límite).
    *   *Bimotor (Taurus, BR 218, BR 132, BR 103):* Debido a su configuración dual, consumen entre **1.5A y 2.0A combinados** bajo carga[6]. El EX-CSB1 puede sostener de forma segura únicamente **2 locomotoras bimotor simultáneas** ($2 \times 2.0\text{A} = 4.0\text{A}$, dejando 1.0A de reserva de corriente para accesorios o arranque).
    *   *Recomendación:* Para la operación simultánea de múltiples trenes bimotores pesados en el restaurante, es obligatorio dividir las vías en sectores eléctricos alimentados por **Boosters G-scale independientes** (de 5A u 8A de capacidad por zona de potencia).

### B. Consideración Crítica en Programación (Service Mode / Vía de Programación)
*   **Detección de ACK:** Al programar decoders PIKO XP 5.1 G en la vía de servicio del EX-CSB1, es posible experimentar errores de lectura de CV si los capacitores del decodificador o módulos de sonido están descargados o intentan absorber la corriente de programación. Se debe aplicar la misma rutina de descarga completa recomendada en LGB o programar de preferencia usando el modo **POM (Programming On Main)** en la vía principal.

---

## 8. Evaluación para Pruebas de Tracción y Automatización

Sometemos las familias electromecánicas de PIKO G a una rigurosa evaluación bajo los 10 escenarios clave definidos para la automatización del restaurante ferroviario:

1.  **Compatibilidad DCC básica con DCC-EX:** ¿Operan sin fallas directas de señal?
2.  **Control independiente de múltiples unidades:** ¿Permite controlar con precisión la dirección y velocidad en la misma vía?
3.  **Desempeño a baja velocidad (Aproximación a mesas):** ¿Tiene una marcha lenta, suave y libre de tirones mecánicos?
4.  **Prueba de arrastre con carga (Traction-Test):** ¿Tiene la adherencia necesaria para jalar platos y vajilla pesados en pendientes?
5.  **Ciclo de trabajo continuo (Duty-Cycle 12h/7d):** ¿Los motores y engranajes soportan jornadas de trabajo largas de restaurante sin sobrecalentar?
6.  **Consumo de corriente agregada:** ¿La eficiencia del motor permite mantener una central de potencia estándar?
7.  **Sencillez de automatización (Sensores de paso):** ¿Es compatible con imanes para reed-switches y sensores de proximidad ópticos/inductivos?
8.  **Rampas de frenado y parada milimétrica:** ¿Permite programar desaceleraciones finas para frenar frente al comensal con precisión de milímetros?
9.  **Resistencia a micro-cortes de energía:** ¿Supera zonas sin corriente como desvíos o suciedad de grasa/aceite sin detener la marcha o reiniciar el sonido?
10. **Sostenibilidad técnica a largo plazo:** ¿Hay disponibilidad de motores Bühler, piñones, escobillas de carbón y repuestos originales?

### Calificación de Idoneidad por Familia Técnica:

### 1. Familia PM-1M-DR (Ej. PIKO GE 25-Ton, V36, BR 80)
*   **Valoración:** **Aceptable con adición de capacitores.**
*   **Razón:** Son excelentes mecánicas monomotor, extremadamente eficientes en consumo de corriente (lo que minimiza la demanda eléctrica del sistema). La digitalización es sumamente sencilla debido a sus motores aislados de fábrica. Sin embargo, debido a su peso ligero, son propensas a perder contacto eléctrico ante cualquier rastro de grasa o suciedad en la vía y tienen baja fuerza de arrastre de platos pesados. Son ideales para tareas de maniobras simples o como plataformas de prueba inicial de software de control.

### 2. Familia PM-1M-SPEC (Ej. Locomotoras de Vapor BR 24, Mogul, Camelback)
*   **Valoración:** **Poco recomendable.**
*   **Razón:** Aunque son estéticamente muy atractivas para los comensales, la arquitectura de tender acoplado con cable umbilical introduce riesgos de falso contacto y cortocircuitos mecánicos durante la operación prolongada. Además, los complejos varillajes Walschaerts de metal y plástico requieren lubricación exhaustiva, limpieza constante y alineación perfecta (quartering); bajo un ciclo de trabajo intensivo en el restaurante, el desgaste de estos varillajes provocará trabas mecánicas y fallas térmicas frecuentes en el motor.

### 3. Familia PM-2M-DR / PM-2M-PnP (Ej. Taurus, BR 218 antiguos, BR 103, BR 132)
*   **Valoración:** **Muy recomendable.**
*   **Razón:** Es la arquitectura ideal para el servicio continuo en el restaurante. Al contar con tracción integral en todos sus ejes provista por dos bogies motores independientes de alto torque, ofrecen una fuerza de tracción colosal, capaz de remolcar vagones pesados cargados con platos y jarras de bebidas. La disipación térmica en sus compartimentos internos es excelente, y la modularidad de acceso por techo (en la familia PnP) simplifica los tiempos de mantenimiento técnico y la adición de periféricos.

### 4. Familia PM-2MS-D (Ej. BR 218 modernizados, BR 132 y BR 103 con Sonido XP 5.1)
*   **Valoración:** **Muy recomendable.**
*   **Razón:** Ofrecen el nivel de prestaciones más alto listo para operar de inmediato. Sus decodificadores XP 5.1 G de 5.0 Amperios son extremadamente robustos, e integran sonido digital de 16 bits nativo y RailCom. El control de carga de motor de fábrica garantiza una marcha ultra-lenta perfecta para la aproximación fina a las mesas, y su chasis pesado ofrece máxima tracción. La única consideración importante es configurar correctamente la CV 250 si se opera con sistemas DCC americanos y descargar sus capacitores integrados antes de programar en vía de servicio.

---

## 9. Ranking Técnico Recomendado para PIKO G

Basados en el análisis de despiece, características electromecánicas e idoneidad para el servicio de restaurante, se establecen los siguientes rankings técnicos:

### Ranking A: Mejores Locomotoras Económicas para Comenzar (Fase MVP y Pruebas de Software)
*Este ranking prioriza el bajo precio de segunda mano, la simplicidad de desmontaje, bajo consumo y facilidad de digitalización directa.*

1.  **PIKO 38500 / 38502 (GE 25-Ton Diesel Switcher - Familia PM-1M-DR):**
    *   *Por qué:* Es una máquina extremadamente compacta y económica. No requiere remover el cuerpo para digitalizar (el techo de la cabina se retira directamente). Utiliza screw-terminals libres de soldadura. Consumo inferior a 0.5A, lo que permite operar de forma muy segura en cualquier central básica sin booster.
2.  **PIKO 37100 / 37120 (DB BR 80 Steam - Familia PM-1M-DR):**
    *   *Por qué:* Muy común de segunda mano debido a que viene en los sets de iniciación de PIKO G. El bloque motor está aislado de fábrica (4 pines) y su electrónica interna cuenta con regleta de bornes de tornillo muy fácil de cablear. Gran disponibilidad de repuestos.
3.  **PIKO 37530 (DB V36 Diesel - Familia PM-1M-DR):**
    *   *Por qué:* Transmisión diésel de 3 ejes acoplados por bielas rígidas muy confiable. Su chasis es robusto y tiene una excelente disipación de calor. La cabina es espaciosa para colocar altavoces grandes e imanes para automatización por sensores de paso.

### Ranking B: Mejores Plataformas para Pruebas de Fuerza (Tracción y Carga de Vajilla)
*Este ranking prioriza la masa de la locomotora (adherencia), la potencia bimotor, la relación de reducción en los engranajes de nylon y la capacidad de corriente de decodificación.*

1.  **PIKO 37540 / 37542 (DR BR 132 "Ludmilla" - Familia PM-2M-PnP / PM-2MS-D):**
    *   *Por qué:* Es una locomotora diésel Co-Co de gran tamaño y un peso masivo superior a los 5 kg. Sus dos bogies motores independientes impulsan 6 ejes con llantas de tracción integradas de fábrica. Su capacidad de arrastre en pendientes cargadas con platos pesados es espectacular y no presenta derrapes ni vibraciones dinámicas.
2.  **PIKO 37300 / 37302 (DB BR 103 Electric - Familia PM-2M-PnP / PM-2MS-D):**
    *   *Por qué:* Posee la misma configuración bimotor de alto torque que la BR 132 pero en un chasis aerodinámico con dos bogies de dos ejes. Su centro de gravedad es bajo y la transmisión directa por piñones helicoidales cerrados le otorga una fuerza de tracción constante con un nivel de ruido mecánico extremadamente bajo.
3.  **PIKO 37410 / 37411 (DB Taurus Rh 1116 - Familia PM-2M-DR):**
    *   *Por qué:* Es un modelo bimotor de tracción integral 4x4 de alta adherencia. Ofrece una base muy ancha y estable que previene descarrilamientos en curvas cerradas bajo carga dinámica, y tiene studs de montaje dedicados para decoders de alta corriente (hasta 5A).

### Ranking C: Mejores Plataformas para Operación Prolongada (Confiabilidad y Sostenibilidad 12h/7d)
*Este ranking prioriza la simplicidad del tren de engranajes, robustez térmica del motor, facilidad de limpieza y lubricación, aislamiento a salpicaduras e inmunidad a la suciedad de las vías.*

1.  **PIKO 37540 / 37542 (DR BR 132 "Ludmilla" - Familia PM-2M-PnP):**
    *   *Por qué:* Además de ser la más potente, su diseño con **acceso rápido por techo** permite realizar diagnósticos eléctricos, mantenimiento de escobillas del decoder y configuración de CVs directamente en la maqueta del restaurante sin necesidad de retirar los bogies ni desmontar la carrocería completa. La caja de engranajes de los bogies Co-Co es sumamente robusta y estanca a salpicaduras de líquidos o vapores de cocina.
2.  **PIKO 37511 / 37512 (DB BR 218 Diesel - Familia PM-2M-PnP / PM-2MS-D):**
    *   *Por qué:* Es la locomotora diésel bimotor por excelencia de PIKO. Los bogies motores se desmontan de forma modular soltando solo un par de tornillos para limpieza de engranajes. No cuenta con varillaje de bielas expuesto que sufra por desgaste mecánico. La cabina cerrada protege de forma absoluta la electrónica interna contra cualquier factor ambiental hostil en el restaurante.
3.  **PIKO 37520 / 37521 (DB BR 260 / V 60 Shunter - Familia PM-1M-DR):**
    *   *Por qué:* Si se busca una máquina monomotor para operación continua de bajo consumo, esta diésel de 3 ejes es la plataforma más equilibrada. Al tener un solo motor de 5 polos de alta precisión, la generación de calor interno es mínima. El varillaje de biela lateral es robusto y simple de lubricar de forma periódica con grasa compatible con plásticos.

---

## 10. Referencias Pendientes de Verificación Técnica

Identificamos las siguientes referencias candidatas dentro de la gama de PIKO G que requieren inspección en taller o documentación adicional para confirmar sus detalles arquitectónicos precisos:

1.  **PIKO 37240 (DB BR 64 Vapor):** Aunque se conoce que es monomotor y comparte el sistema de despiece de bielas con la BR 24, se requiere verificar físicamente si la placa electrónica principal se aloja en la cabina o si tiene sub-placas distribuidas que dificulten la instalación limpia de decoders de terceras marcas como ESU o Massoth.
2.  **PIKO 37560 / 37562 (DR V60 / BR 106 - Diferente del modelo DB V60):** Esta locomotora de maniobras diésel de la Alemania del Este (DR) tiene una configuración de 4 ejes acoplados por bielas (*Fünfkuppler* ligero). Debemos confirmar si su bloque de motor comparte exactamente el mismo tren de engranajes intermedio de la serie DB V60 o si cuenta con cajas reductoras especiales de baja velocidad con diferentes relaciones de transmisión, lo que afectaría la calibración BEMF de velocidad ultra-lenta.

---

## 11. Conclusiones y Plan de Acción Comparativo (LGB vs. PIKO G)

La investigación de PIKO G revela ventajas y diferencias competitivas muy claras al compararla con la plataforma tecnológica de LGB para nuestro restaurante ferroviario:

### Ventajas de PIKO G para la Automatización:
1.  **Diseño Moderno Libre de Cirugías de Cortocircuito:** A diferencia de LGB (donde los modelos de segunda mano analógicos suelen presentar el peligroso bloque de 3 pines no aislado), **todos los modelos de PIKO G analizados cuentan con motores completamente aislados galvánicamente de fábrica con conexiones de 4 pines**. Esto elimina el riesgo de quemar decodificadores durante la digitalización de modelos antiguos.
2.  **Acceso de Mantenimiento Superior (Removable Roof):** La compuerta superior de techo en las series BR 132 y BR 103 de PIKO es una solución técnica magistral. Facilita el ajuste de decodificadores, instalación de altavoces o sustitución de fusibles de protección de forma instantánea en el mismo restaurante, una ventaja enorme frente a LGB, donde se requiere el desmontaje total de chasis complejos.
3.  **Fidelidad Electrónica XP 5.1:** El nuevo ecosistema digital SmartDecoder XP 5.1 G ofrece una capacidad de salida constante de 5.0 Amperios (superior al estándar de 3.0A de Massoth XLS en LGB) y sonido de 16 bits de altísima fidelidad que permite controlar cargas bimotor pesadas sin calentamiento excesivo de la electrónica.

### Puntos Débiles de PIKO G a Considerar:
1.  **Tracción y Peso en Metal:** A diferencia de locomotoras LGB pesadas de metal die-cast (como la Cocodrilo 26600 de 8.8 kg), los modelos bimotores pesados de PIKO G (como la BR 132 o Taurus) recurren mayoritariamente a carrocerías de plástico ABS de alta resistencia con pesos de plomo internos añadidos. Esto les otorga una masa de ~4.5 a 5.2 kg. Aunque su tracción es excelente en plano, en pendientes extremas las locomotoras LGB de metal die-cast ofrecen un margen de adherencia bruto ligeramente superior.
2.  **Anomalías con Centrales de Otras Marcas:** El bug de temporización de inactividad de 3 segundos en decoders XP 5.1 G al operar en centrales DCC-EX exige configurar manualmente la CV 250 a un valor de 1 en el protocolo de recepción de flota, lo cual debe documentarse formalmente en el manual de procedimientos de taller.

### Plan de Acción de Integración (Propuesta de Flota Combinada):
*   **Fase 1 - MVP de Desarrollo de Software (DCC-EX):** Se debe adquirir una locomotora monomotor **PIKO 38500 (GE 25-Ton)** o una **LGB Stainz 20211/21211**. Ambas ofrecen digitalizaciones sencillas y seguras para validar los bloques de frenado y el software del restaurante.
*   **Fase 2 - Flota de Servicio Diario Pesado (Tracción de Platos):** El modelo **PIKO 37540 / 37542 (DR BR 132 "Ludmilla")** se consagra como la mejor opción de servicio bimotor pesado debido a su increíble estabilidad, potencia Co-Co, total hermeticidad frente a grasas de cocina de restaurante y, sobre todo, su facilidad de mantenimiento mediante acceso rápido por techo.
*   **Sistemas de Respaldo Eléctrico:** Sin importar la marca elegida (LGB o PIKO), es de carácter **obligatorio** cablear un capacitor de respaldo (*Stay-Alive* / *Power Buffer*) de al menos 1 Faradio directamente a los terminales de tierra y voltaje regulado del decodificador. Esto evitará paradas accidentales provocadas por micro-cortes de corriente en las agujas de desvío y mantendrá la fluidez absoluta de la entrega de comida a los comensales.

---

### Enlaces y Fuentes Consultadas

1.  **PIKO Spielwaren GmbH - Official Service Portal:** Catálogos de repuestos, manuales oficiales en PDF y diagramas de despiece de todas las referencias de escala G. [piko.de](https://www.piko.de/) / [piko-shop.de](https://www.piko-shop.de/)
2.  **PIKO America Technical Resources:** Guías específicas de cableado para decoders SmartDecoder XP 5.1, solución de bugs de CV 250 y compatibilidad con sistemas DCC americanos. [piko-america.com](https://www.piko-america.com/)
3.  **G Scale Central - PIKO G Section:** Comunidad global de modelismo a gran escala con historiales de conversiones a DCC, pruebas de consumo real y mantenimiento de engranajes de nylon. [gscalecentral.net](https://www.gscalecentral.net/)
4.  **Massoth Electronics Manuals & Guides:** Especificaciones de corriente para los decoders eMOTION XL/XLS y módulos SUSI compatibles con PIKO G. [massoth.de](https://www.massoth.de/)
5.  **ESU Electronic Solutions Ulm:** Fichas técnicas, diagramas de pines e instrucciones de soldadura para la gama de decoders de alta corriente LokSound 5 XL. [esu.eu](https://www.esu.eu/)
6.  **Gartenbahn Stammtisch Technical Forums:** Discusiones técnicas alemanas sobre consumos térmicos y desgaste continuo de engranajes helicoidales cerrados de PIKO en condiciones de jardín y operación intensiva. [gartenbahn-stammtisch.de](https://www.gartenbahn-stammtisch.de/)
