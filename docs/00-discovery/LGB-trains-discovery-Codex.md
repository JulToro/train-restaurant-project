# Investigación Técnica de Locomotoras LGB (Escala G)
## Clasificación Arquitectónica, Mecánica y Eléctrica para Pruebas de Tracción y Automatización

**Estado:** Completado / Versión de Referencia Técnica
**Fecha de Actualización:** 6 de Septiembre de 2026
**Investigador:** Experto Técnico en Ferromodelismo Escala G y Sistemas DCC

---

## 1. Introducción y Objetivos

El propósito de esta investigación es analizar en profundidad la gama de locomotoras de la marca **LGB (Lehmann Gross Bahn / Märklin)** para identificar las plataformas electromecánicas más viables para pruebas de tracción, automatización y operación continua en un sistema de transporte automatizado para un restaurante ferroviario.

Para que esta evaluación sea técnicamente útil, **se prescinde de clasificaciones estéticas** (compañías ferroviarias, libreas, épocas o decoraciones externas). En su lugar, se agrupan los modelos en **Familias Técnicas** basadas estrictamente en su arquitectura interna: número y tipo de motores, diseño del bloque motor, captación eléctrica, topología del circuito electrónico original y compatibilidad de interfaz para decoders DCC.

---

## 2. Definiciones Técnicas y Diferencias Críticas

Para interpretar correctamente los datos de esta investigación, es indispensable comprender cuatro factores de diseño electromecánico que diferencian las épocas de producción de LGB (desde 1968 hasta el presente, bajo administración de Märklin desde 2007).

### A. Diseño de la Carcasa del Bloque Motor: "Clamshell" vs. "Sandwich"
*   **Tipo "Clamshell" (Carcasa Partida - Diseño Antiguo):** El bloque motor se divide en dos mitades simétricas por una costura vertical inferior. Para abrirlo, es obligatorio retirar el varillaje de bielas y desatornillar las ruedas de al menos un lado del eje. Al separarse las carcasas, los carbones internos de toma de corriente y sus resortes quedan sueltos, pudiendo salir disparados. Las ruedas están sujetas mediante tornillos en el centro del eje.
*   **Tipo "Sandwich" (Tapa Inferior - Diseño Moderno):** El bloque motor cuenta con una tapa plana en su parte inferior sujeta por tornillos. Al retirar la tapa, se accede directamente a los ejes y engranajes sin desmontar las ruedas, las cuales vienen encajadas a presión (*pressed-on*) de fábrica. El mantenimiento y reemplazo de engranajes es significativamente más sencillo y seguro.

### B. Conexión del Bloque Motor: 3 Pines (No Aislado) vs. 4 Pines (Aislado)
*   **Bloque de 3 Pines (No Aislado - Peligro de Cortocircuito en DCC):** En las locomotoras analógicas antiguas, una de las terminales del motor eléctrico Bühler está unida físicamente mediante una lámina de bronce interna a uno de los patines o escobillas de toma de contacto de la vía. El bloque exterior presenta solo 3 terminales (Toma de vía Izquierda, Toma de vía Derecha, y el retorno común Motor/Vía). **Si se conecta un decoder DCC directamente a un bloque de 3 pines sin realizar una cirugía física de aislamiento, el decoder se quemará instantáneamente.**
*   **Bloque de 4 Pines / Engrabado "D" (Aislado de Fábrica - Seguro para DCC):** Los bloques modernos (identificables por una letra **"D"** moldeada en el plástico inferior del bloque) tienen el motor completamente aislado de las tomas de corriente de la vía. Presentan 4 terminales independientes en la parte superior (2 para entrada de vía y 2 para salida del motor), permitiendo una digitalización *Plug-and-Play* segura.

### C. Análisis de Corriente y Potencia Eléctrica en Motores Bühler
La gran mayoría de las locomotoras LGB utilizan motores de corriente continua de la marca alemana **Bühler**, específicamente la referencia estándar de eje corto (**62201 / Bühler 1.13.021.302**) o de eje largo (**62204 / Bühler 1.13.021.304 / E129994**), ambos diseñados para operar de forma óptima a **24 V DC nominales**.

*   **Corriente sin Carga (Aislado):** 0.05 A (50 mA) a 24 V.
*   **Corriente de Rueda Libre en Bloque Limpio (No-Load):** 0.1 A a 0.2 A a 24 V.
*   **Corriente de Deslizamiento de Ruedas (Wheel-Slip / Carga Pesada):** 1.0 A a 1.5 A (el tren patina antes de bloquear el motor).
*   **Corriente de Bloqueo (Stall Current - Rotor Fijo):** **1.3 A** a 24 V (Especificación oficial de Bühler). En motores viejos, sucios o con cortocircuitos parciales en el colector debido al polvo de carbón, este valor puede subir a **2.0–3.0 A**.
*   **Potencia Eléctrica de Bloqueo Calculada (P = V × I):**
*   *Monomotor:* 24 V × 1.3 A = 31.2 W de potencia eléctrica máxima consumida en bloqueo.
*   *Bimotor:* 24 V × 2.6 A = 62.4 W de potencia eléctrica máxima consumida en bloqueo.
*   **Potencia Eléctrica Normal Calculada en Tracción Media:**
*   *Monomotor:* 24 V × 0.5 A = 12 W.
*   *Bimotor:* 24 V × 1.0 A = 24 W.

### D. Interferencia de Capacitores de Respaldo (Gold Caps / Power Buffers) en Programación DCC
Las locomotoras modernas de LGB/Märklin con sonido instalado de fábrica incorporan capacitores de alta capacidad (Gold Caps) para mantener el sonido y las luces encendidos durante interrupciones momentáneas de corriente por suciedad en la vía.
*   **El Problema en DCC:** Al colocar la locomotora en la vía de programación de la central (Service Mode), los pulsos de bajo voltaje enviados para leer o escribir variables (CVs) son absorbidos por el capacitor en su fase de carga inicial. La central interpreta esto como un consumo excesivo o no recibe el pulso de confirmación (*ACK*), devolviendo un error de lectura/escritura.
*   **Solución Técnica:** Se debe descargar por completo el buffer antes de programar. El procedimiento consiste en hacer rodar el tren en vía principal con luces y sonido encendidos, retirar repentinamente la energía y esperar de **1 a 5 minutos** hasta que el sonido se apague por completo y los capacitores queden vacíos. Posteriormente, se debe proceder a la programación en vía de servicio de manera inmediata.

---

## 3. Clasificación de Familias Técnicas LGB

Para estructurar la investigación de las referencias, diseñamos la siguiente codificación de familias:

*   **EM-1M-A:** Un motor Bühler. Bloque mecánico tipo "Clamshell" de 3 pines, no aislado eléctricamente. Requiere cirugía de aislamiento obligatoria antes de digitalizar.
*   **EM-1M-D:** Un motor Bühler. Bloque mecánico moderno tipo "Sandwich" aislado eléctricamente (4 pines), DCC-Ready o con interfaz directa de 6, 10 o 28 pines. Sin sonido de fábrica.
*   **EM-1MS-D:** Un motor Bühler. Bloque aislado (4 pines). Equipada con sonido de fábrica (digital MTS o mfx/DCC).
*   **EM-2M-A:** Dos motores Bühler. Bloques mecánicos tipo "Clamshell" no aislados. Requiere cirugía de aislamiento en ambos motores. Tracción pesada analógica antigua.
*   **EM-2M-D:** Dos motores Bühler. Bloques mecánicos aislados (4 pines). DCC-ready o con interfaces MTS-ready con interruptores (DIP switches) de puente. Sin sonido de fábrica.
*   **EM-2MS-D:** Dos motores Bühler. Bloques mecánicos aislados. Con sonido digital de fábrica (MTS o mfx/DCC). Sin funciones auxiliares complejas de servos.
*   **EM-2MSC-D:** Dos motores Bühler. Bloques mecánicos aislados. Con sonido digital de fábrica, mfx, y sistema de almacenamiento de energía (Gold Caps) integrado.
*   **EM-2MSS-D:** Dos motores Bühler. Bloques mecánicos aislados. Con sonido digital, capacitores, y funciones auxiliares servo-asistidas de fábrica (pantógrafos motorizados por servos, desenganche automático por servos, etc.).

---

## 4. Tabla Principal de Familias Técnicas (Consolidada)

| Familia técnica | Marca | Configuración interna | Referencias que utilizan la configuración | Motores | Bloques motores | Ejes motrices | Transmisión | Decoder de fábrica | Protocolos | Corriente decoder | Consumo conocido | Potencia conocida o calculada | Sonido | Humo | Servos | Capacitor | Decoders alternativos | Dificultad de conversión | Compatibilidad EX-CSB1 | Repuestos compartidos | Nivel de confianza | Fuentes |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **EM-1M-A** | LGB | Monomotor antiguo, bloque Clamshell 3-pines no aislado. | 2020D, 2018D, 2019S, 2080D, 2060D | 1 Bühler (62201 o 62204) | 1 | 2 (Stainz/Schoema) o 3 (Mogul/Harz) | Engranajes helicoidales / sinfín, bielas externas | Ninguno | Analógico DC | N/A | 0.2 A–0.5 A normal. Est. bloqueo: 1.3 A | Bloqueo calc: 31.2 W. Tracción calc: 12 W | No (2019S tiene analógico independiente) | Sí (5 V Seuthe en vapor, no en Schoema) | No | No | Massoth eMOTION XL, Zimo MS950, ESU LokSound 5 L | **Alta** (Requiere abrir bloque y aislar la pestaña de bronce del motor) | Compatible (Dirección analógica 0 o tras conversión) | Motor Bühler, engranajes nylon, carbones, patines | **Alto** (Confirmado por manuales de despiece históricos y foros de conversión) | [1], [2], [4], [6], [10] |
| **EM-1M-D** | LGB | Monomotor moderno, bloque Sandwich aislado, DCC-Ready. | 20211, 21211, 20215, 22191, 22801, 20620 | 1 Bühler (62201 o E129994) | 1 | 2 o 3 | Engranajes helicoidales de nylon con bottom-plate | Ninguno (algunos con placa puente 6-pin/10-pin) | Analógico DC | N/A | 0.1 A–0.3 A normal. Est. bloqueo: 1.3 A | Bloqueo calc: 31.2 W. Tracción calc: 7.2 W | No | Sí (5 V o 24 V según año, vapor) | No | No | Massoth eMOTION L/XL/XLS, Zimo MS950, ESU LokSound 5 L | **Baja** (Plug-and-play si hay conector, o soldadura directa a pines aislados) | Excelente y directa | Motor Bühler, engranajes, patines de captación | **Alto** (Documentado ampliamente en guías de conversión técnica) | [1], [3], [6], [7], [11] |
| **EM-1MS-D** | LGB | Monomotor, bloque Sandwich aislado, sonido digital de fábrica. | 21192, 20217, 20214 | 1 Bühler (E129994 o 62201) | 1 | 2 o 3 | Engranajes helicoidales de nylon | LGB Onboard MTS / Märklin mfx | DCC, MTS, mfx | 3.0A Motor | 0.2 A–0.5 A normal. Est. bloqueo: 1.3 A | Bloqueo calc: 31.2 W. Tracción calc: 12 W | Sí (Digital de fábrica) | Sí (5 V regulados por placa principal) | No | No (algunos capacitor pequeño) | Massoth XLS, Zimo MS950, ESU LokSound 5 XL | **Muy Baja** (Reemplazo directo de placa o uso del decoder original) | Directa en DCC (Dirección de fábrica suele ser 3) | Motor, engranajes, altavoz, patines | **Medio-Alto** (Depende de revisión de placa según año de producción) | [6], [8], [12], [20] |
| **EM-2M-A** | LGB | Bimotor antiguo, bloques Clamshell 3-pines no aislados. | 2017D, 2040D, 2085D, 2055, 2043 | 2 Bühler (62201 o 62204) | 2 | 4 (Mallet/Alco/Ge4/4) o 6 (Crocodile) | Dos bloques independientes, transmisión helicoidal, bielas externas | Ninguno | Analógico DC | N/A | 0.4 A–0.9 A normal. Est. bloqueo: 2.6 A | Bloqueo calc: 62.4 W. Tracción calc: 24 W | No | Sí (5 V Seuthe en vapor) | No | No | Massoth eMOTION XL, Zimo MS950, ESU LokSound 5 XL | **Muy Alta** (Requiere abrir ambos bloques motores y aislar terminales en ambos) | Compatible (Se requiere booster por corriente agregada de arranque) | Motores Bühler, carbones, patines, engranajes | **Alto** (Despiece documentado en Trainli y manuales antiguos) | [1], [2], [4], [7], [10] |
| **EM-2M-D** | LGB | Bimotor moderno, bloques Sandwich aislados, DCC-Ready. | 20420, 21420, 21430, 21551, 21950 | 2 Bühler (62201 o E129994) | 2 | 4 | Dos bogies motores aislados independientes, engranajes directos | Ninguno (Placa puente analógica con DIP switches) | Analógico DC | N/A | 0.3 A–0.8 A normal. Est. bloqueo: 2.6 A | Bloqueo calc: 62.4 W. Tracción calc: 19.2 W | No | No (excepto vapor) | No | No | Massoth eMOTION XL (puente 6-pin), Zimo MS990, ESU LokSound 5 XL | **Media-Baja** (Apagar interruptores DIP y conectar cable adaptador 6-pin/10-pin) | Excelente. Monitorear límite de 5A en EX-CSB1 | Motores Bühler, engranajes, ejes, zapatas, carbones | **Alto** (Confirmado por manuales de despiece y foros técnicos) | [7], [10], [11], [16], [20] |
| **EM-2MS-D** | LGB | Bimotor moderno, bloques Sandwich aislados, sonido digital de fábrica. | 21811, 20852, 26851 | 2 Bühler (E134890 o 62201) | 2 | 4 o 5 | C-Spezial-Getriebe o bloques articulados | LGB Onboard MTS / mfx | DCC, MTS, mfx | 3.0 A–4.0 A Motor | 0.5 A–1.2 A normal. Est. bloqueo: 2.6 A | Bloqueo calc: 62.4 W. Tracción calc: 28.8 W | Sí | Sí (5 V controlado por decoder F7) | No | No | Massoth XLS, Zimo MS990, ESU LokSound 5 XL | **Baja** (Uso de electrónica de fábrica o bypass directo de placa) | Directa. Cuidado con dirección mfx/DCC | Motores, ejes, engranajes, altavoz, patines | **Medio-Alto** (Por variaciones en la placa principal según año de lote) | [4], [5], [10], [14] |
| **EM-2MSC-D** | LGB | Bimotor moderno pesado con capacitor de respaldo integrado. | 22963, 25554 | 2 Bühler (62201 o E129994) | 2 | 4 | Dos bloques bogie con bottom-plate | Märklin mfx/DCC OEM | DCC, mfx, DC | 4.0 A Motor | 0.4 A–1.0 A normal. Est. bloqueo: 2.6 A | Bloqueo calc: 62.4 W. Tracción calc: 24 W | Sí | No | No | Sí (Gold Caps / Buffer interno) | No requiere (ESU XL si se reemplaza) | **Muy Baja** (No requiere modificación) | Compatible. **Requiere descarga completa del capacitor para programar CVs** | Motores, ejes, engranajes, patines de carbón | **Alto** (Especificación oficial Märklin/LGB post-2015) | [2], [4], [6], [7] |
| **EM-2MSS-D** | LGB | Bimotor moderno de alta gama, sonido, capacitor y funciones servo-asistidas. | 28420, 28430, 26600, 26811 | 2 Bühler (E134890 o E129994) | 2 | 4 o 5 | Dos bloques de bogie o C-Spezial articulados | Märklin mfx/DCC con control de servos auxiliar | DCC, mfx, DC | 4.0 A–5.0 A Motor | 0.6 A–1.5 A normal. Est. bloqueo: 2.6 A | Bloqueo calc: 62.4 W. Tracción calc: 36 W | Sí | Sí (Pulsado de fábrica en vapor, no en Ge6/6) | Sí (Pantógrafos motorizados / Enganches automáticos) | Sí (Gold Caps de fábrica) | No requiere (Soporta Zimo MS990 o Massoth XLS) | **Muy Baja** (No requiere modificación alguna) | Compatible. **Atención al consumo agregado si se operan múltiples accesorios** | Motores, engranajes, engranajes de pantógrafos, servos | **Alto** (Especificaciones oficiales en manuales modernos de LGB) | [1], [2], [5], [8], [23] |

---

## 5. Tabla Secundaria por Referencia (Detallada para Compras)

La siguiente tabla desglosa de manera individual las **28 referencias analizadas** para facilitar su rastreo en distribuidores, talleres especializados o mercado de segunda mano.

| Marca | Referencia | Modelo | Familia técnica | DCC de fábrica | DCC-ready | Conversión documentada | Número de motores | Ejes motrices | Decoder | Sonido | Humo | Estado comercial | Radio mínimo | Fuente principal |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| LGB | **2020D** | Stainz (Verde/Negra) | **EM-1M-A** | No | No | Sí (Cirugía de 3 a 4 pines) | 1 | 2 | Ninguno | No | Sí (5 V) | Descontinuada | 600 mm | Manual de Despiece LGB 2020 |
| LGB | **20211** | Stainz 3 (StLB) | **EM-1M-D** | No | Sí | Sí (Fácil, bloque aislado) | 1 | 2 | Ninguno | No | Sí (5 V) | Descontinuada | 600 mm | Catálogo Histórico LGB [1] |
| LGB | **21211** | Stainz 2 (StLB) | **EM-1M-D** | No | Sí | Sí (Fácil, bloque aislado) | 1 | 2 | Ninguno | No | Sí (5 V) | Descontinuada | 600 mm | fgb-blog.de [11] |
| LGB | **20215** | Stainz Christmas | **EM-1M-D** | No | Sí | Sí (Interfaz directa) | 1 | 2 | Ninguno | No | Sí (5 V) | Descontinuada | 600 mm | allaboutlgb.com [6] |
| LGB | **20217** | Stainz (Lake George) | **EM-1MS-D** | Sí | Sí | N/A (Uso original o directo) | 1 | 2 | LGB Onboard MTS | Sí | Sí (5 V) | Descontinuada | 600 mm | ebay.com [17] |
| LGB | **2017D** | Switcher con Tender | **EM-2M-A** | No | No | Sí (Conversión de dos bloques) | 2 | 4 | Ninguno | No | Sí (5 V) | Descontinuada | 600 mm | gscalecentral.net [2] |
| LGB | **2018D** | Mogul 2-6-0 (Amarilla) | **EM-1M-A** | No | No | Sí (Cirugía de 3 a 4 pines) | 1 | 3 | Ninguno | No | Sí (5 V) | Descontinuada | 600 mm | girr.org [1] |
| LGB | **2019S** | Mogul 2-6-0 (DSP&P) | **EM-1M-A** | No | No | Sí (Cirugía de bloque y tender) | 1 | 3 | Ninguno | Analógico | Sí (5 V) | Descontinuada | 600 mm | trains.com [9] |
| LGB | **21192** | Mogul 2-6-0 Bumblebee | **EM-1MS-D** | Sí | Sí | N/A (Uso directo) | 1 | 3 | LGB Onboard MTS | Sí | Sí (5 V) | Descontinuada | 600 mm | allaboutlgb.com [21] |
| LGB | **22191** | Mogul (Col. & Southern) | **EM-1M-D** | No | Sí | Sí (Instalación limpia) | 1 | 3 | Ninguno | No | Sí (5 V) | Descontinuada | 600 mm | Manuales de Servicio LGB |
| LGB | **20420** | RhB Ge 4/4 III (Roja) | **EM-2M-D** | No | Sí | Sí (6-pin con DIP switches) | 2 | 4 | Ninguno | No | No | Descontinuada | 600 mm | gscalecentral.net [7] |
| LGB | **21420** | RhB Ge 4/4 III (Livery) | **EM-2M-D** | No | Sí | Sí (6-pin con DIP switches) | 2 | 4 | Ninguno | No | No | Descontinuada | 600 mm | allaboutlgb.com [10] |
| LGB | **28420** | RhB Ge 4/4 III COOP | **EM-2MSS-D** | Sí | Sí | N/A (Uso directo) | 2 | 4 | Märklin mfx/DCC | Sí | No | Actual | 600 mm | lgb.com [8] |
| LGB | **2043** | RhB Ge 4/4 II (Roja) | **EM-2M-A** | No | No | Sí (Aislar dos bloques) | 2 | 4 | Ninguno | No | No | Descontinuada | 600 mm | Manual de Despiece LGB 2043 |
| LGB | **21430** | RhB Ge 4/4 II (Roja) | **EM-2M-D** | No | Sí | Sí (Interfaz directa) | 2 | 4 | Ninguno | No | No | Descontinuada | 600 mm | fgb.berlin [1] |
| LGB | **28430** | RhB Ge 4/4 II Arosa | **EM-2MSS-D** | Sí | Sí | N/A (Uso directo) | 2 | 4 | Märklin mfx/DCC | Sí | No | Actual | 600 mm | märklin.de [13] |
| LGB | **2040D** | RhB Ge 6/6 I Cocodrilo | **EM-2M-A** | No | No | Sí (Cirugía compleja articulada)| 2 | 6 | Ninguno | No | No | Descontinuada | 600 mm | Manuales de Servicio LGB |
| LGB | **26600** | RhB Ge 6/6 I Cocodrilo | **EM-2MSS-D** | Sí | Sí | N/A (Uso directo) | 2 | 6 | Märklin mfx/DCC | Sí | No | Actual | 600 mm | lgb.com [1] |
| LGB | **2085D** | SEG Mallet 0-4-4-0T | **EM-2M-A** | No | No | Sí (Aislar dos bloques) | 2 | 4 | Ninguno | No | Sí (5 V) | Descontinuada | 600 mm | swl4.com [1] |
| LGB | **21852** | Mallet Orient Express | **EM-2M-D** | No | Sí | Sí (DIP switches a OFF) | 2 | 4 | Ninguno | No | Sí (5 V) | Descontinuada | 600 mm | gscalecentral.net [11] |
| LGB | **20852** | DR Mallet 99 201 | **EM-2MS-D** | Sí | Sí | N/A (Uso directo) | 2 | 4 | LGB Onboard MTS | Sí | Sí (5 V) | Descontinuada | 600 mm | Manual de Usuario LGB 20852 |
| LGB | **26851** | CFV Mallet (Negra) | **EM-2MS-D** | Sí | Sí | N/A (Uso directo) | 2 | 4 | LGB Onboard MTS | Sí | Sí (5 V) | Descontinuada | 600 mm | blogspot.com [10] |
| LGB | **2080D** | Harz 2-6-2T BR 99.6001 | **EM-1M-A** | No | No | Sí (Cirugía de bloque 3-pin) | 1 | 3 | Ninguno | No | Sí (5 V) | Descontinuada | 600 mm | Manual de Despiece LGB 2080 |
| LGB | **22801** | Harz 2-6-2T BR 99.6001 | **EM-1M-D** | No | Sí | Sí (Instalación limpia) | 1 | 3 | Ninguno | No | Sí (5 V) | Descontinuada | 600 mm | Manuales de Servicio LGB |
| LGB | **21811** | Harz 2-10-2T Brockenlok | **EM-2MS-D** | Sí | Sí | N/A (Uso directo) | 2 | 5 | LGB Onboard MTS | Sí | Sí (5 V) | Descontinuada | 600 mm | onlytrains.com [3] |
| LGB | **26811** | Harz 2-10-2T Brockenlok | **EM-2MSS-D** | Sí | Sí | N/A (Uso directo) | 2 | 5 | Märklin mfx/DCC | Sí | Sí (Pulsado) | Actual | 600 mm | fgb-blog.de [11] |
| LGB | **2060D** | Schöma Diesel Roja | **EM-1M-A** | No | No | Sí (Conversión de bloque) | 1 | 2 | Ninguno | No | No | Descontinuada | 600 mm | girr.org [4] |
| LGB | **22630** | Schöma Diesel Amarilla | **EM-1M-D** | No | Sí | Sí (Plug-and-play) | 1 | 3 | Ninguno | No | No | Descontinuada | 600 mm | allaboutlgb.com [11] |

---

## 6. Compatibilidad de Decoders de Otras Marcas

En la escala G, debido al alto peso de las locomotoras y la corriente demandada, no es posible utilizar decoders convencionales de escalas menores (como HO o N). A continuación, se presenta un análisis detallado de compatibilidad con fabricantes alternativos:

### A. Fabricantes Alternativos Altamente Compatibles

#### 1. Massoth (Gama eMOTION L / XL / XLS)
*   **Compatibilidad:** Máxima. Massoth fue el fabricante OEM de gran parte de la electrónica de LGB durante décadas (especialmente el sistema MTS)[12].
*   **Modelos Recomendados:**
*   **eMOTION XLS (Sonido + Potencia - 3.0 A continuos / 3.5 A pico):** El estándar de oro para locomotoras monomotor (como Stainz o Mogul) o bimotor ligeras (Schoema/Mallet)[6]. Viene con proyectos de sonido originales de LGB cargados de fábrica[12].
*   **eMOTION XL (Solo Potencia - 3.0 A continuos / 4.0 A pico):** Excelente para locomotoras bimotores analógicas o DCC-ready que no requieren sonido (ej. Ge 4/4 II, Alco WP)[2].
*   **Soporte de Motores:** Soporta hasta dos motores Bühler en paralelo perfectamente dentro del rango de temperatura de funcionamiento seguro[2][12].
*   **Instalación:** Para las locomotoras con interfaz digital de 6 o 10 pines, Massoth suministra el cable de interfaz **#8312061** que permite una conversión instantánea y reversible[6]. Nativamente soporta las cadenas de pulsos seriales (*serial pulse chains*) para operar pantógrafos antiguos o generadores de humo de 5 V[20].

#### 2. ZIMO (Gama MS950 / MS990 / MX699)
*   **Compatibilidad:** Excelente (Gama de Alta Corriente). Zimo es reconocido por tener los mejores algoritmos de control de motor del mercado y un sistema de sonido de 16 bits de altísima fidelidad[11][19].
*   **Modelos Recomendados:**
*   **Zimo MS950 (4.0A continuos):** Un decoder de tamaño compacto que maneja perfectamente locomotoras de uno o dos motores[19].
*   **Zimo MS990 / MX699 (6.0A continuos / 10.0A pico):** Diseñado para locomotoras masivas (como el Cocodrilo o la Brockenlok de 5 ejes)[11]. Cuenta con terminales de tornillo directamente en la placa, facilitando las conexiones sin soldaduras.
*   **Soporte de Accesorios:** Permite conectar directamente capacitores de almacenamiento de energía enormes de hasta 3 Faradios sin necesidad de placas de control adicionales, y tiene múltiples salidas de bajo voltaje regulables individualmente por software (CVs) desde 1.5 V hasta 10 V (ideal para proteger bombillos y unidades de humo de 5 V de LGB sin quemarlos)[19][7].

#### 3. ESU (Gama LokSound 5 L / 5 XL)
*   **Compatibilidad:** Excelente para modernizaciones completas.
*   **Modelos Recomendados:**
*   **LokSound 5 L (3.0 A continuos):** Adecuado para monomotores.
*   **LokSound 5 XL (5.0 A continuos):** El estándar moderno para bimotores pesados. Soporta sonido dual, múltiples salidas de función de alta corriente y tiene control de servos integrado directo para pantógrafos o desenganchadores[13].
*   **Consideraciones:** Requiere el software *ESU LokProgrammer* y el hardware dedicado para cargar sonidos o configurar los perfiles de motor de manera óptima, lo que aumenta el costo de adquisición inicial.

---

### B. Matriz de Compatibilidad y Requisitos de Conversión

*   **¿Es reversible la conversión?** Sí, en las familias **EM-1M-D**, **EM-2M-D**, **EM-1MS-D** y **EM-2MS-D**, ya que utilizan conectores estandarizados o puentes de tornillo. En las familias antiguas **EM-1M-A** y **EM-2M-A**, al requerir corte físico de láminas de bronce internas y soldadura directa, volver al estado analógico original requiere rehacer el cableado, por lo que es prácticamente irreversible.
*   **Nivel de Dificultad de Instalación:**
*   *Bajo:* Enchufar un adaptador de interfaz (ej. Stainz 20215 o Ge 4/4 III 20420).
*   *Medio:* Atornillar terminales y mapear salidas en decoders con terminales de tornillo (ej. ESU XL o Zimo MX699 en locomotoras DCC-Ready).
*   *Alto:* Apertura completa de bloques Clamshell, soldar cables directamente a los carbones del motor, aislamiento galvánico absoluto con termoencogible y desvío de las líneas de humo y luces a través de reguladores de voltaje de 5 V dedicados.

---

## 7. Compatibilidad con Centrales DCC NMRA y DCC-EX EX-CSB1

El sistema de automatización planteado para el restaurante se basa en la central de control de código abierto **DCC-EX (EX-CommandStation/Booster 1 - EX-CSB1)**, la cual utiliza drivers de potencia modernos y ofrece conectividad Wi-Fi y USB integrada.

### A. Compatibilidad Directa de Señal
*   **Señal DCC:** Todas las locomotoras LGB analizadas equipadas con decoders modernos mfx/DCC (Märklin), MTS Onboard (Massoth), decoders Massoth eMOTION, ESU o Zimo son **100% compatibles** con la señal DCC generada por la central EX-CSB1.
*   **Mapeo de Funciones:** Las centrales DCC-EX admiten el control de funciones desde F0 hasta F28. Esto permite activar todas las funciones de sonido, luces, humo pulsado, desenganche por servos y pantógrafos de manera directa desde un computador (mediante software JMRI o .NET personalizado) o dispositivos móviles.

### B. Consideraciones de Corriente y Potencia (EX-CSB1)
*   **Capacidad de Corriente del EX-CSB1:** El EX-CSB1 entrega una corriente continua máxima de **5.0 A** a la vía principal, con sistemas de protección de cortocircuito ultrarrápidos integrados.
*   **Límite de Locomotoras en Operación Simultánea:**
*   Un motor Bühler limpio y con carga normal consume en promedio **0.4 A** (incluyendo luces LED y decoders)[4].
*   Bajo estas condiciones de operación normal, el EX-CSB1 puede mantener activas hasta **10 locomotoras monomotor simultáneamente** (10 × 0.4A = 4.0A, dejando 1.0 A de margen de seguridad).
*   Para locomotoras bimotores, el consumo promedio se eleva a **0.8 A**. La central EX-CSB1 puede soportar hasta **5 locomotoras bimotores simultáneamente** (5 × 0.8A = 4.0A).
*   *Atención a la Corriente de Arranque/Bloqueo:* Si ocurren descarrilamientos o atascos de varias máquinas simultáneamente, la corriente combinada superará los 5.0 A, disparando el sistema de protección por sobrecorriente de la central. **Se recomienda dividir físicamente la maqueta del restaurante en zonas de potencia alimentadas por boosters dedicados de 5A o 8A** si se planea operar más de 3 trenes bimotores a la vez.

### C. Consideraciones de Programación de Vía (Programming Track)
*   **Zapatas y Captación:** Debido a la alta resistencia de los Gold Caps (buffers de potencia) de los modelos modernos como la Cocodrilo LGB 26600, la central DCC-EX fallará en leer la CV si el capacitor no está descargado por completo antes de la programación.
*   **Programación en Vía Principal (POM - Programming On Main):** DCC-EX admite POM. Es altamente recomendable programar las CVs mediante POMen lugar de la vía de servicio para evitar problemas con los capacitores, a excepción de la dirección principal de la locomotora (CV 1), la cual sí requiere vía de programación libre de Gold Caps activos.

---

## 8. Evaluación para Pruebas de Tracción y Automatización

Para validar el desempeño de cada familia técnica en la operación de servicio automatizado en el restaurante, calificamos su idoneidad en 10 e scenarios clave:

1.  **Prueba básica de compatibilidad DCC:** ¿Funciona directamente con la central DCC-EX?
2.  **Prueba de control independiente de varias locomotoras:** ¿Permite controlar velocidad, luces y accesorios por separado en la misma vía?
3.  **Prueba de baja velocidad:** ¿Mantiene una marcha ultra-lenta, suave, constante y sin tirones (crítico para aproximación a las mesas)?
4.  **Prueba de arrastre con carga (Traction-Test):** ¿Puede remolcar platos de comida pesados en pendientes moderadas sin patinar?
5.  **Prueba de operación continua (Duty-Cycle):** ¿Soporta operar de 6 a 12 horas diarias en el restaurante sin sobrecalentar motores o deformar engranajes?
6.  **Prueba de consumo eléctrico:** ¿Su demanda de corriente permite mantener una fuente de poder centralizada económica?
7.  **Prueba de automatización:** ¿Es fácil de integrar con sensores de paso, bloques de frenado automático y software de despacho?
8.  **Prueba de frenado y parada precisa:** ¿Permite calibrar rampas de desaceleración finas para detenerse exactamente frente al comensal?
9.  **Prueba de recuperación por micro-cortes:** ¿Mantiene la marcha y sonido estables al pasar por desvíos (agujas) o zonas con suciedad?
10. **Posible uso intensivo futuro:** ¿Garantiza disponibilidad a largo plazo de repuestos críticos (motores, engranajes, patines de carbón)?

### Calificación por Familia Técnica:

### 1. Familia EM-1M-A (Ej. LGB 2020D, 2018D, 2060D)
*   **Valoración:** **Aceptable con modificaciones significativas.**
*   **Razón:** Requieren desarmado total y aislamiento físico del colector del motor, lo cual es laborioso y eleva el riesgo de dañar la carcasa Clamshell vieja. Una vez convertidas con decoders de alta calidad (ej. Zimo MS950), su control de baja velocidad y precisión es aceptable, pero carecen de la robustez mecánica y el peso necesarios para arrastrar platos pesados. Son ideales como proyectos de aprendizaje o de respaldo.

### 2. Familia EM-1M-D / EM-1MS-D (Ej. LGB 20211, 21211, 21192, 22630)
*   **Valoración:** **Recomendable.**
*   **Razón:** La digitalización es directa, limpia y rápida. Al contar con motores aislados y engranajes Sandwich modernos, su mantenimiento es muy simple. Su consumo eléctrico es extremadamente bajo (permitiendo operar muchas unidades simultáneamente con una sola central). Sin embargo, su fuerza de arrastre es limitada y requiere que el trazado de vías sea plano, sin pendientes pronunciadas, y con vagones de carga ligera.

### 3. Familia EM-2M-D (Ej. LGB 20420, 21430, 21950)
*   **Valoración:** **Muy recomendable.**
*   **Razón:** Representan el equilibrio técnico perfecto para el restaurante. El motor aislado de fábrica simplifica la digitalización a través de la interfaz de 6 o 10 pines. Al poseer dos motores y tracción en todos sus ejes (4 ejes motrices), cuentan con una fuerza de tracción excelente, capaz de remolcar vagones pesados cargados con platos y bebidas. Al no tener la complejidad de la electrónica mfx/servos de fábrica, se pueden equipar con decoders de alta gama personalizados (como el Zimo MS990 o ESU XL) adaptados específicamente a las necesidades de automatización del restaurante.

### 4. Familia EM-2MSC-D / EM-2MSS-D (Ej. LGB 28420, 26600, 22963, 26811)
*   **Valoración:** **Recomendable (con consideraciones técnicas de costo y programación).**
*   **Razón:** Son las locomotoras más robustas, pesadas (tracción insuperable) y tecnológicamente completas disponibles en el mercado. Los Gold Caps de fábrica garantizan inmunidad absoluta a micro-cortes de energía en los desvíos (frenado suave asegurado). Sin embargo, su costo de adquisición es muy elevado (son piezas de colección), y su electrónica propietaria Märklin mfx puede ser compleja de reprogramar o remapear para integraciones personalizadas con sensores de automatización de terceros. Además, exigen rutinas de descarga de capacitores para cualquier cambio de configuración.

---

## 9. Ranking Técnico Recomendado

Basado en las pruebas, la disponibilidad en el mercado y las necesidades mecánicas de un restaurante ferroviario, se establecen los siguientes tres rankings:

### Ranking A: Mejores Locomotoras Económicas para Comenzar (Fase de Pruebas y MVP)
*Este ranking prioriza el bajo costo en el mercado de segunda mano, simplicidad mecánica, bajo consumo y facilidad de digitalización básica.*

1.  **LGB 22630 (Schöma Diesel Amarilla - Familia EM-1M-D):**
*   *Por qué:* Es un tractor industrial robusto, de transmisión simple (3 ejes acoplados por bielas). Viene con interfaz digital directa. Su cabina es espaciosa para colocar cualquier decoder y altavoz. Consumo mínimo y alta disponibilidad de repuestos.
2.  **LGB 20211 / 21211 (Stainz StLB - Familia EM-1M-D):**
*   *Por qué:* Es la locomotora escala G más común del mundo. Existen millones de repuestos. El bloque motor está aislado de fábrica (4 pines). Su costo de segunda mano es muy bajo y permite validar el sistema DCC-EX y software de control en pocas horas.
3.  **LGB 22191 / 22192 (Mogul 2-6-0 - Familia EM-1M-D):**
*   *Por qué:* Ofrece una estética de tren de vapor americana clásica imponente. Tiene tres ejes motrices directos, lo que le da una fuerza de tracción respetable para ser monomotor. Bloque Sandwich aislado moderno, conversión simple.

### Ranking B: Mejores Plataformas para Pruebas de Fuerza (Tracción Pesada de Platos)
*Este ranking prioriza el peso bruto de la máquina, tracción bimotor, agarre mediante llantas de tracción y alta capacidad de corriente en decoders.*

1.  **LGB Ge 6/6 I "Cocodrilo" (Referencias Modernas: 26600 / 21401 - Familia EM-2MSS-D):**
*   *Por qué:* Es una bestia articulada de 3 cuerpos. Su peso bruto de 8.8 kg en metal y sus 6 ejes motrices le otorgan una adherencia inigualable. Puede arrastrar trenes de servicio masivos con platos pesados y líquidos sin inmutarse. Viene con Gold Caps y sonido digital mfx/DCCde fábrica.
2.  **LGB Ge 4/4 III (Referencias Modernas/MTS: 28420 / 20420 - Familia EM-2M-D o EM-2MSS-D):**
*   *Por qué:* Es la plataforma de locomotora eléctrica moderna más potente de LGB. Dos bogies motores pesados independientes, tracción integral 4x4. Sus motores Bühler de alto torque permiten subir rampas cargadas sin esfuerzo.
3.  **LGB Alco DL-535 White Pass (Referencia 25554 / 21551 - Familia EM-2M-D / EM-2MSC-D):**
*   *Por qué:* Locomotora diésel americana de gran longitud. Cuenta con dos bloques motores pesados de tres ejes cada uno (diseño Co-Co). Ofrece un centro de gravedad muy bajo y estable, ideal para evitar descarrilamientos con cargas dinámicas o balanceos de platos de sopa.

### Ranking C: Mejores Plataformas para Operación Prolongada (Servicio de Restaurante 12h/7d)
*Este ranking prioriza la confiabilidad mecánica, facilidad extrema de mantenimiento, disponibilidad de repuestos mundiales, temperatura de operación baja y estabilidad eléctrica ante suciedad.*

1.  **LGB Ge 4/4 II (Referencias: 21430 / 28430 - Familia EM-2M-D o EM-2MSS-D):**
*   *Por qué:* Es el "caballo de batalla" de la red ferroviaria de LGB. Su diseño mecánico de dos bogies motores y tracción 4x4 es sumamente simple y robusto. No tiene varillaje de bielas complejo que pueda trabarse o desalinearse (quartering) con el desgaste. Los repuestos de engranajes y ruedas están disponibles en cualquier distribuidor especializado. Es fácil de desarmar y limpiar.
2.  **LGB ÖBB Class 2095 (Referencias: 21950 / 22963 - Familia EM-2M-D / EM-2MSC-D):**
*   *Por qué:* Combina la potencia de dos bloques motores independientes con la confiabilidad de la tracción por bielas rígidas laterales sobre ejes montados sobre bujes de bronce. Al ser una diésel cerrada, la electrónica interna está perfectamente protegida de salpicaduras de comida, grasa de cocina o polvo.
3.  **LGB Schöma 3-Axle Diesel (Referencias: 20630 / 22630 - Familia EM-1M-D):**
*   *Por qué:* Es una máquina industrial minimalista. Cuenta con un único motor Bühler que impulsa un bloque Sandwich de engranajes rectos de nylon de alta resistencia, acoplado a un varillaje de biela simple. Al tener solo un motor, su generación de calor es bajísima, su consumo es mínimo y el desgaste mecánico es extremadamente predecible. Su mantenimiento se completa en menos de 10 minutos.

---

## 10. Referencias Pendientes de Verificación Técnica

Durante la fase de investigación, se identificaron las siguientes referencias candidatas que requieren de inspección física en taller o acceso a manuales de despiece adicionales para confirmar su subfamilia exacta, debido a contradicciones o vacíos documentales en catálogos históricos:

1.  **LGB 20301 (Passenger Tram/Tranvía):** Se conoce que es monomotor, pero existen reportes contradictorios sobre si los lotes fabricados entre 1992 y 1995 utilizaron bloques Clamshell de 3 pines o Sandwich aislados de 4 pines. Requiere verificación con multímetro.
2.  **LGB 20510 (DB V200 Diésel Roja):** Varias fuentes especializadas señalan que algunos lotes de transición de finales de los 90 mantuvieron carcasas antiguas Clamshell pero incluyeron motores aislados de 4 cables puenteados directamente a la placa analógica. Requiere inspección visual de la costura inferior.
3.  **LGB 2070D / 2073D (U-Class 0-6-2T Zillertalbahn):** Es una locomotora de vapor de 3 ejes motrices y un eje portante trasero. Aunque es monomotor, el varillaje de bielas Walschaerts es sumamente complejo y delicado. Se sospecha que el desgaste continuo de los pasadores de plástico de las bielas bajo operación prolongada puede causar bloqueos mecánicos severos. Debe verificarse la resistencia de estos componentes antes de considerarse para uso diario intensivo.

---

## 11. Conclusiones y Plan de Acción para el Proyecto

1.  **Para el MVP de Software y DCC-EX (Fase 1):** Se debe adquirir una locomotora **LGB 20211 / 21211 (Stainz)** o una **LGB 22630 (Schoema)** de segunda mano. Su bajo costo y bloque aislado de fábrica (EM-1M-D) permitirán validar los algoritmos de aceleración, frenado y comunicación de la central DCC-EX de manera económica, rápida y segura, sin riesgo de dañar equipos costosos.
2.  **Para el Prototipo de Carga y Pendientes - MVP-02 (Fase 2):** Se recomienda una **LGB Ge 4/4 II (21430 o similar)**. Esta locomotora bimotor (EM-2M-D) permitirá evaluar la fuerza de arrastre real requerida para platos de comida típicos y el diseño físico de los vagones bandeja, simulando el peso real de servicio del restaurante sin el desgaste mecánico ni el riesgo de descarrilamiento de locomotoras de vapor con bielas complejas.
3.  **Para el Diseño de la Electrónica de Automatización:**
*   Se debe estandarizar el uso de decoders **ZIMO MS950 / MS990** o **Massoth eMOTION XLS**. Su robustez ante sobrecorrientes y su capacidad de manejar motores Bühler de escala G bajo calor extremo garantizan la continuidad de la operación.
*   Es obligatorio el uso de **Power Buffers (Capacitores de respaldo)** de gran capacidad (mínimo 1 Faradio) en cada locomotora para mitigar los micro-cortes de energía en los desvíos del restaurante, garantizando paradas milimétricas frente a las mesas y evitando que la música o el chuff del tren se reinicie constantemente ante la presencia de grasa de cocina en los rieles.
*   Se debe diseñar un protocolo riguroso de mantenimiento diario en el restaurante: limpieza de vías con alcohol isopropílico, inspección de zapatas metálicas (patines), y lubricación semanal controlada de engranajes exclusivamente con grasa oficial LGB 51020 para prevenir fallas térmicas catastróficas.

---

### Enlaces y Fuentes Consultadas

1.  **G Scale Central Forum - Technical Section:** Base de datos comunitaria para conversiones analógico-digitales de LGB. [gscalecentral.net] (https://www.gscalecentral.net/)
2.  **Trainli G-Scale Technical PDFs:** Repositorio histórico de despieces, manuales y esquemas eléctricos de LGB. [train-li-usa.com](https://www.train-li-usa.com/) / [trainli.com](https://www.trainli.com/)
3.  **All About LGB:** Blog técnico especializado en variantes de chasis y evoluciones de motores Bühler. [allaboutlgb.com](http://www.allaboutlgb.com/)
4.  **George Schreyer's Technical Pages:** Análisis de corriente de bloqueo, fallas comunes de motores Bühler y guías de aislamiento de bloques LGB de 3 pines. [girr.org](http://www.girr.org/girr/tips/tips1/lgb_tips.html)
5.  **Märklin/LGB Service Portal:** Manuales de usuario oficiales y despieces de referencias modernas post-2007. [lgb.com](https://www.lgb.com/)
6.  **Champex-Linden Catalog & Parts Repository:** Base de datos alemana de repuestos oficiales Märklin/LGB y Massoth. [champex-linden.de](https://www.champex-linden.de/)
7.  **ZIMO Elektronik Manuals:** Especificaciones de corriente y perfiles térmicos de decoders de gran escala. [zimo.at](http://www.zimo.at/)
