# Investigación Técnica de Locomotoras USA Trains (Escala G / 1:29)
## Clasificación Arquitectónica, Mecánica y Eléctrica para Pruebas de Tracción y Automatización

**Estado:** Completado / Versión de Referencia Técnica  
**Fecha de Actualización:** 6 de Septiembre de 2026  
**Investigador:** Experto Técnico en Ferromodelismo Escala G, Sistemas DCC y Automatización  

---

## 1. Introducción y Objetivos

El propósito de esta investigación es analizar en profundidad la gama de locomotoras de gran escala de la marca estadounidense **USA Trains** (especializada en prototipos norteamericanos a escala 1:29 de alta fidelidad que corren sobre vía estándar de 45 mm) para evaluar su viabilidad técnica en las pruebas de tracción pesada, resistencia operativa y automatización de entrega en el restaurante ferroviario.

USA Trains es ampliamente reconocida por fabricar locomotoras pesadas con una de las mecánicas de tracción más robustas del mercado de la gran escala (gama **"Workhorse"** y gama premium **"Ultimate Series"**). A través de este análisis, **clasificaremos los modelos por su arquitectura electromecánica interna y su sistema de transmisión**, abstrayendo por completo sus decoraciones externas. Prestaremos especial atención a los factores eléctricos extremos de esta marca: la bajísima resistencia de sus motores can (tipo Mabuchi RS-545), las colosales corrientes de bloqueo (*stall*) que superan los 15 Amperios por bloque y las complejidades de su cableado original (luces LED/incandescentes con retornos de ánodo común conflictivos con DCC). Todo esto nos permitirá comparar USA Trains con LGB, PIKO y Bachmann, conformando la base analítica de nuestra futura flota.

---

## 2. Definiciones Técnicas y Diferencias Críticas

Para comprender la ingeniería de USA Trains, es imprescindible asimilar cuatro factores mecánicos y eléctricos que diferencian su material rodante de la competencia:

### A. La Realidad sobre el Concepto "DCC-Ready" en USA Trains
A diferencia de marcas como PIKO (que incluyen interfaces universales directas) o Bachmann Spectrum (zócalos de doble fila), en el ecosistema de USA Trains **"DCC Ready" no significa la presencia de un zócalo o enchufe Plug-and-Play estándar**[1][5]. 
*   **Aislamiento del Motor:** Indica estrictamente que las terminales del motor eléctrico están completamente aisladas galvánicamente de las zapatas de bronce y escobillas captadoras de corriente de las ruedas[1][4].
*   **La Placa de Retardo ("Delay Board"):** De fábrica, la gran mayoría de estas locomotoras analógicas incluyen un circuito de retardo electrónico[6][7]. En modo DC (analógico), esta placa retiene el flujo de energía hacia los motores hasta que el voltaje en la vía sube a ~8V–10V, permitiendo encender las luces, el generador de humo de alta corriente y el sistema de sonido (ej. Phoenix Sound) antes de que la máquina comience a moverse física y bruscamente[6][8].
*   **Proceso de Conversión:** Para digitalizar la locomotora, el manual oficial instruye **retirar físicamente esta placa de retardo** (sujeta por 4 tornillos de terminal)[6][7]. Al retirarse, se exponen cables pelados o bornes donde se debe realizar el cableado manual (hardwiring) directo de las terminales del motor y captación de vía hacia el decodificador DCC[6][7].

### B. Consumos Eléctricos Extremos (El Reto de los 15 Amperios por Bloque)
Los bloques de motor de USA Trains (que utilizan variantes de alto torque del motor Mabuchi RS-545SH) están diseñados para tracción pesada industrial y poseen una resistencia interna extremadamente baja de aproximadamente **1.0 Ohm** por motor[1][2].
*   **Corriente de Bloqueo en un Bloque (*Stall Current*):** Al bloquear físicamente el eje de un solo bloque de motor a un voltaje típico de G de 16V, el consumo se eleva instantáneamente a **15.0 Amperios** por Ley de Ohm ($I = V/R$)[1][2].
*   **Consumo Combinado de Bloqueo (Bimotor):** En locomotoras de dos motores conectados eléctricamente en paralelo (GP38-2, SD40-2, SD70), la resistencia combinada cae a **0.6 Ohms**[4][2]. A un voltaje nominal de 20V, un bloqueo mecánico absoluto puede demandar teóricamente **más de 30 Amperios**[4][2].
*   **Consumo Operativo Real en Vía:**
    *   *Rueda Libre (Light Engine):* 1.5 A a 1.8 A a 20V[4].
    *   *Patinado de Ruedas (*Wheel-Slip*):* 3.0 A a 3.6 A (límite físico antes de que las ruedas derrapen sobre el riel metálico)[4][5].
    *   *Arrastre de Platos Pesados:* 4.0 A a 5.5 A continuos bajo condiciones de carga máxima en pendientes[2].
*   **Solución Técnica de Protección (La Resistencia de Protección):** Para evitar que un descarrilamiento, atasco o sobrecarga fría o queme instantáneamente decoders de gama alta (como el ESU LokSound 5 XL de 5A), se recomienda instalar una **resistencia de potencia de 1.0 a 1.5 Ohms (de 10W a 25W) conectada en serie con las líneas de alimentación de los motores**[4][10]. Esto limita físicamente el pico máximo de corriente de bloqueo a un rango seguro de 8A–10A sin afectar notablemente el desempeño dinámico o el Back-EMF a velocidades operacionales normales de restaurante[1][10].

### C. Complejidades de Iluminación y Retornos de Ánodo Común
Los sistemas de iluminación originales de USA Trains presentan serios desafíos para las digitalizaciones directas:
*   **Bombillos de Bajo Voltaje (5V o 1.5V):** No están preparados para los 18V–24V de las vías DCC y se quemarán de inmediato si se conectan sin resistencias limitadoras adecuadas[14][10].
*   **Retorno Común Negativo (Common-Cathode):** Las luces originales y las placas switch inferiores comparten un retorno de tierra común negativo en el chasis[14][10]. Sin embargo, los decodificadores DCC estándar de gran escala funcionan bajo una topología de **retorno común positivo (V+)** (cable azul)[15][14]. Intentar conectar las luces compartidas al decodificador puede destruir las salidas de función del decoder o provocar un cortocircuito constante[14].
*   *Recomendación de Taller:* La técnica más confiable y limpia consiste en **desmantelar o ignorar por completo el cableado de luces de la placa base analógica original** ("Gut and Hardwire" Method) y cablear nuevos LEDs independientes con resistencias limitadoras individuales directamente a los terminales de función del decoder[1][9][10].

---

## 3. Clasificación de Familias Técnicas USA Trains

Estructuramos la siguiente codificación arquitectónica para las plataformas de USA Trains:

*   **USM-1M-A:** Un motor Mabuchi de alto torque. Chasis rígido liviano (Workhorse Series - Vaporera 0-4-0). Transmisión directa por piñones helicoidales. No posee placas electrónicas avanzadas, requiere cableado manual desde las escobillas de las ruedas.
*   **USM-2M-DR:** Dos motores Mabuchi acoplados en bloques de dos ejes (Bo-Bo). Chasis mediano diésel (Ultimate/Workhorse Switchers: Alco S-4, GE 44-Tonner, NW-2). Placa base de conmutación analógica inferior con jumpers de puente. Consumo moderado-alto.
*   **USM-2M-HEAVY:** Dos motores Mabuchi RS-545 de alta resistencia en bloques de dos ejes (Bo-Bo). Chasis diésel pesado de línea principal (Ultimate Series: GP7/GP9, GP30, GP38-2). Chasis de plástico pesado con insertos masivos de plomo. Placas de retardo analógicas desmontables mediante bornes de tornillo.
*   **USM-2M-COCO:** Dos motores Mabuchi RS-545 en bloques de tres ejes (Co-Co - Ultimate Series: SD40-2, SD70MAC, F3-A/B). Chasis de gran longitud y peso masivo (~6.5 a 8 kg). Tracción total en 6 ejes con llantas de tracción de fábrica. Alta demanda de corriente continua.
*   **USM-HEAVY-DIECAST:** Locomotoras Spectrum-rival pesadas construidas completamente en metal die-cast (ej. J1e Hudson 4-6-4, GG-1, FEF-3). Motores de alta corriente, generadores de humo de alta potencia integrados con soplador por ventilador (consumo de ~2.5A solo para humo) y cableados umbilicales complejos de alta corriente. Peso superior a los 12 kg.

---

## 4. Tabla Principal de Familias Técnicas (Consolidada)

| Familia técnica | Marca | Configuración interna | Referencias que utilizan la configuración | Motores | Bloques motores | Ejes motrices | Transmisión | Decoder de fábrica | Protocolos | Corriente decoder | Consumo conocido | Potencia conocida o calculada | Sonido | Humo | Servos | Capacitor | Decoders alternativos | Dificultad de conversión | Compatibilidad EX-CSB1 | Repuestos compartidos | Nivel de confianza | Fuentes |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **USM-1M-A** | USA Trains | Monomotor liviano de maniobras, analógico puro, sin zócalo. | R2201, R2202, R2203 | 1 Mabuchi de 5 polos | 1 | 2 | Piñones helicoidales cerrados de nylon a bielas rígidas externas | Ninguno | Analógico DC | N/A | 0.6A normal. Est. bloqueo: 4.5A | Bloqueo calc: 108W. Tracción calc: 14.4W | No | Sí (Directo a vía 24V) | No | No | SoundTraxx Eco-200, ESU LokSound 5 L, Massoth eMOTION XL | **Media-Alta** (Requiere abrir chasis compacto y soldar directamente a captadores) | Compatible de forma directa | Motor Mabuchi, escobillas de carbón, bielas de acoplamiento | **Alto** (Manuales de vapor de maniobras de USA Trains y foros) | [1], [2], [4] |
| **USM-2M-DR** | USA Trains | Bimotor de maniobras mediano, placa base con interruptores. | R22151, R22152, R22155, R22550, R22551, R22555, R22559 | 2 Mabuchi RS-545 (aislados) | 2 | 4 | Dos bloques bogie Bo-Bo independientes con bottom-plate | Ninguno | Analógico DC | N/A | 1.2A-1.8A normal. Est. bloqueo: 15.0A | Bloqueo calc: 360W. Tracción calc: 36W | No | Sí (De alta corriente 1.5A) | No | No | NCE D808SR, ESU LokSound 5 XL, Massoth eMOTION XL, Zimo MS990 | **Media** (Bypass total de placa de retardo y cableado de LEDs nuevos) | Compatible. **Se recomienda resistencia en serie para limitar picos** | Motores Mabuchi, piñones de bogie, patines de bronce | **Alto** (Fichas técnicas oficiales y manuales de S-4 y GE-44) | [1], [5], [10], [16] |
| **USM-2M-HEAVY**| USA Trains | Bimotor pesado de línea, chasis plomo, placa retardo. | R22203, R22204, R22205, R22206, R22212, R22106, R22110 | 2 Mabuchi RS-545 (aislados) | 2 | 4 | Dos bloques bogie Bo-Bo, engranajes helicoidales cerrados | Ninguno | Analógico DC | N/A | 1.5A-2.2A normal. Est. bloqueo: 20.0A | Bloqueo calc: 480W. Tracción calc: 48W | No | Sí (Pulsado de alta corriente) | No | No | NCE D808SR (8.0A), ESU LokSound 5 XL, Zimo MS990, CVP AirWire PnP | **Media-Alta** (Desmontaje complejo, bypass de placa común-negativa) | Directa. **Booster obligatorio por corrientes de arranque de 4.0A** | Bogies Bo-Bo de repuesto, piñones, llantas de tracción, LEDs | **Alto** (Guías de conversión técnica de SBS4DCC y George Schreyer) | [1], [2], [8], [9], [13] |
| **USM-2M-COCO** | USA Trains | Bimotor de tracción extrema, Co-Co 6 ejes motrices, muy pesada.| R22300, R22301, R22302, R22600, R22602 | 2 Mabuchi RS-545 pesados | 2 | 6 | Dos bloques bogie de 3 ejes con engranajes de alta reducción | Ninguno | Analógico DC | N/A | 1.8A-2.8A normal. Est. bloqueo: 30.0A | Bloqueo calc: 720W. Tracción calc: 67.2W | No | Sí (Pulsado con soplador por ventilador) | No | No | NCE D808SR, ESU LokSound 5 XL (con limitador), Zimo MS990 (6.0A) | **Alta** (Requiere desmontaje total de cables de luces y ventiladores de humo) | Compatible. **Atención: Un bloqueo total disparará el corte del EX-CSB1** | Bogies Co-Co, motores de repuesto, escobillas de carbón, ventilador | **Alto** (Despieces detallados de SD70 y SD40 de USA Trains) | [2], [4], [9], [10], [12] |
| **USM-DIECAST** | USA Trains | Piezas de metal die-cast masivas, consumo extremo humo/luces. | R20001, R22651 | 2 Mabuchi pesados (GG-1) o 1 Pittman masivo (Hudson) | 2 o 1 | 6 o 12 | Engranajes helicoidales de bronce macizo, bielas articuladas | Ninguno | Analógico DC | N/A | 2.2A-3.5A normal. Est. bloqueo: >30.0A | Bloqueo calc: 720W. Tracción calc: 84W | No | Sí (Pulsado de alta densidad de corriente >2.0A) | No | No | Zimo MS990 (6.0A continuos), ESU LokSound 5 XL | **Muy Alta** (Manejo de carrocerías de 13 kg y cableados umbilicales complejos) | Requiere Booster dedicado independiente de alta corriente (>8.0A) | Interruptores de caja de humo, ejes de bronce, engranajes cónicos | **Alto** (Manuales oficiales Spectrum y bitácoras de conversión de Phoenix) | [1], [2], [5], [6] |

---

## 5. Tabla Secundaria por Referencia (Detallada para Compras)

La siguiente tabla presenta de forma detallada e individual las **23 referencias analizadas** para simplificar su búsqueda y adquisición en el mercado de repuestos o distribuidores especializados.

| Marca | Referencia | Modelo | Familia técnica | DCC de fábrica | DCC-ready | Conversión documentada | Número de motores | Ejes motrices | Decoder | Sonido | Humo | Estado comercial | Radio mínimo | Fuente principal |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| USA Trains | **R22151** | GE 44-Ton Burlington Route | **USM-2M-DR** | No | Sí | Sí (Bypass de delay board) | 2 | 4 | Ninguno | No | Sí | Descontinuada | 600 mm | usatrains.com [7] |
| USA Trains | **R22152** | GE 44-Ton New York Central| **USM-2M-DR** | No | Sí | Sí (Bypass de delay board) | 2 | 4 | Ninguno | No | Sí | Descontinuada | 600 mm | usatrains.com [7] |
| USA Trains | **R22155** | GE 44-Ton Chessie System | **USM-2M-DR** | No | Sí | Sí (Igual a R22151) | 2 | 4 | Ninguno | No | Sí | Descontinuada | 600 mm | usatrains.com [7] |
| USA Trains | **R22157** | GE 44-Ton Santa Fe | **USM-2M-DR** | No | Sí | Sí (Igual a R22151) | 2 | 4 | Ninguno | No | Sí | Descontinuada | 600 mm | usatrains.com [7] |
| USA Trains | **R22160** | GE 44-Ton Pennsylvania RR | **USM-2M-DR** | No | Sí | Sí (Igual a R22151) | 2 | 4 | Ninguno | No | Sí | Descontinuada | 600 mm | usatrains.com [7] |
| USA Trains | **R22550** | Alco S-4 Rio Grande Black | **USM-2M-DR** | No | Sí | Sí (Desmontaje completo) | 2 | 4 | Ninguno | No | Sí (1.5A) | Descontinuada | 600 mm | lsol.com [16] |
| USA Trains | **R22551** | Alco S-4 Santa Fe Blue/Yel | **USM-2M-DR** | No | Sí | Sí (Desmontaje completo) | 2 | 4 | Ninguno | No | Sí (1.5A) | Descontinuada | 600 mm | lsol.com [16] |
| USA Trains | **R22552** | Alco S-4 Union Pacific Yellow| **USM-2M-DR** | No | Sí | Sí (Igual a R22550) | 2 | 4 | Ninguno | No | Sí (1.5A) | Descontinuada | 600 mm | lsol.com [16] |
| USA Trains | **R22555** | Alco S-4 Southern Pacific | **USM-2M-DR** | No | Sí | Sí (Igual a R22550) | 2 | 4 | Ninguno | No | Sí (1.5A) | Descontinuada | 600 mm | lsol.com [16] |
| USA Trains | **R22559** | Alco S-4 Burlington Northern | **USM-2M-DR** | No | Sí | Sí (Igual a R22550) | 2 | 4 | Ninguno | No | Sí (1.5A) | Descontinuada | 600 mm | lsol.com [16] |
| USA Trains | **R22203** | EMD GP38-2 CSX Blue/Yellow | **USM-2M-HEAVY**| No | Sí | Sí (Bypass switch board) | 2 | 4 | Ninguno | No | Sí | Actual | 1200 mm | usatrains.com [21] |
| USA Trains | **R22204** | EMD GP38-2 BNSF Orange | **USM-2M-HEAVY**| No | Sí | Sí (Bypass switch board) | 2 | 4 | Ninguno | No | Sí | Actual | 1200 mm | usatrains.com [21] |
| USA Trains | **R22205** | EMD GP38-2 D&RGW Black/Org | **USM-2M-HEAVY**| No | Sí | Sí (Igual a R22203) | 2 | 4 | Ninguno | No | Sí | Actual | 1200 mm | usatrains.com [21] |
| USA Trains | **R22206** | EMD GP38-2 Union Pacific | **USM-2M-HEAVY**| No | Sí | Sí (Igual a R22203) | 2 | 4 | Ninguno | No | Sí | Actual | 1200 mm | usatrains.com [21] |
| USA Trains | **R22207** | EMD GP38-2 ATSF Blue/Yellow| **USM-2M-HEAVY**| No | Sí | Sí (Igual a R22203) | 2 | 4 | Ninguno | No | Sí | Actual | 1200 mm | usatrains.com [21] |
| USA Trains | **R22212** | EMD GP38-2 ATSF Warbonnet | **USM-2M-HEAVY**| No | Sí | Sí (Igual a R22203) | 2 | 4 | Ninguno | No | Sí | Actual | 1200 mm | usatrains.com [21] |
| USA Trains | **R22300** | EMD SD40-2 Rio Grande | **USM-2M-COCO** | No | Sí | Sí (Hardwiring completo) | 2 | 6 | Ninguno | No | Sí (Puls.)| Actual | 1200 mm | usatrains.com [2] |
| USA Trains | **R22301** | EMD SD40-2 Santa Fe Blue/Yel| **USM-2M-COCO** | No | Sí | Sí (Hardwiring completo) | 2 | 6 | Ninguno | No | Sí (Puls.)| Actual | 1200 mm | usatrains.com [2] |
| USA Trains | **R22302** | EMD SD40-2 Union Pacific | **USM-2M-COCO** | No | Sí | Sí (Igual a R22300) | 2 | 6 | Ninguno | No | Sí (Puls.)| Actual | 1200 mm | usatrains.com [2] |
| USA Trains | **R22600** | EMD SD70MAC ATSF Warbonnet | **USM-2M-COCO** | No | Sí | Sí (Hardwiring completo) | 2 | 6 | Ninguno | No | Sí (Puls.)| Actual | 1200 mm | trainli.com [1] |
| USA Trains | **R22602** | EMD SD70MAC Union Pacific | **USM-2M-COCO** | No | Sí | Sí (Hardwiring completo) | 2 | 6 | Ninguno | No | Sí (Puls.)| Actual | 1200 mm | trainli.com [1] |
| USA Trains | **R22106** | EMD GP9 Union Pacific Yellow| **USM-2M-HEAVY**| No | Sí | Sí (Bypass delay board) | 2 | 4 | Ninguno | No | Sí | Actual | 1200 mm | onlytrains.com [3] |
| USA Trains | **R22110** | EMD GP9 Pennsylvania RR Green| **USM-2M-HEAVY**| No | Sí | Sí (Bypass delay board) | 2 | 4 | Ninguno | No | Sí | Actual | 1200 mm | onlytrains.com [3] |

---

## 6. Compatibilidad de Decoders de Otras Marcas

La digitalización de una locomotora USA Trains requiere la selección rigurosa de decodificadores de alta corriente capaces de soportar los masivos picos de demanda del motor Mabuchi RS-545:

### A. Decoders Recomendados y Compatibilidad

#### 1. NCE (Gama D808SR)
*   **Compatibilidad:** Máxima (Histórica). El D808SR fue diseñado en EE. UU. específicamente para manejar la bajísima resistencia de los motores de USA Trains[8][9].
*   **Especificaciones:** Ofrece una capacidad de **8.0 Amperios continuos y hasta 30.0 Amperios de pico (bloqueo)**[8][9]. Esto le permite manejar con absoluta holgura un bloqueo total de cualquiera de las locomotoras pesadas de la Ultimate Series sin necesidad de instalar resistencias limitadoras de corriente en serie[8][9].
*   **Desventaja:** Es un decodificador silencioso (no cuenta con módulo de sonido integrado), por lo que requiere instalar una placa de sonido auxiliar Phoenix Sound conectada en paralelo[8].

#### 2. ESU (Gama LokSound 5 XL)
*   **Compatibilidad:** Excelente (Con adición obligatoria de resistencia de protección)[13].
*   **Especificaciones:** Soporta hasta **5.0 Amperios continuos** y un pico de corriente de 10.0A para el motor[13]. Cuenta con sonido digital de 16 bits de altísima fidelidad y perfiles de sonido de motores diésel EMD y Alco excelentes[13].
*   **Limitación de Corriente Obligatoria:** Dado que el bloqueo absoluto de los motores duales de USA Trains puede superar los 15 Amperios, **es estrictamente obligatorio soldar una resistencia de potencia de 1.0 a 1.5 Ohms (de 10W a 25W) en serie con los terminales del motor al instalar el LokSound 5 XL**[4][10]. Esta resistencia limitará físicamente el pico máximo de bloqueo a un nivel seguro de ~8A, protegiendo los transistores del decoder de una falla catastrófica térmica ante descarrilamientos o atascos de transmisión[1][10].

#### 3. ZIMO (Gama MS990 / MX699)
*   **Compatibilidad:** Excelente. Zimo destaca por fabricar los decodificadores con mejor gestión térmica y mayor cantidad de salidas auxiliares del mercado de ferromodelismo[12].
*   **Especificaciones:** El **MS990** maneja de forma continua hasta **6.0 Amperios** de corriente de motor y soporta picos de hasta 12.0 Amperios, ofreciendo un excelente perfil de protección de sobrecorriente por software ajustable mediante CVs[12]. Sus múltiples salidas de función de bajo voltaje ajustable son perfectas para gobernar las luces originales compartidas compartidas de USA Trains sin fundirlas.

---

### B. Análisis de Reversibilidad de Conversión

*   **¿Es reversible la conversión?** En un 100% únicamente si se conserva intacto el arnés del cableado de la placa de delay y conmutación analógica original de fábrica[6][7]. No obstante, debido a las complejidades del retorno común negativo y los bombillos de bajo voltaje, la gran mayoría de instaladores profesionales eligen desmantelar por completo toda la electrónica analógica original (*Gut and Hardwire Method*)[1][9]. Este desmantelamiento total de placas analógicas propietarias hace que la conversión sea **prácticamente irreversible**, obligando a re-cablear todo el sistema de forma manual si se desea volver al formato analógico DC original.

---

## 7. Compatibilidad con Centrales DCC y DCC-EX EX-CSB1

Nuestra central de pruebas de código abierto basada en Arduino **DCC-EX EX-CSB1** entrega una salida de corriente máxima constante de **5.0 A** en vía principal. El comportamiento de las locomotoras USA Trains bajo este sistema presenta las siguientes restricciones críticas:

### A. Compatibilidad de Señal DCC
*   Las señales DCC de la central DCC-EX son decodificadas de forma perfecta por los decodificadores NCE, ESU, Zimo o Massoth montados en las máquinas. Todos los parámetros de velocidad, sentido de marcha y activación de luces responden con absoluta precisión.

### B. Restricciones de Corriente Críticas (El Límite del EX-CSB1)
*   **Corriente Operativa:** Una locomotora bimotor pesada de la Ultimate Series (ej. GP38-2 o SD70MAC) cargada con vagones en trazado plano consume de **2.0A a 3.0A constantes** de forma regular[2][4].
*   **Límite Físico de Flota:** 
    *   La central EX-CSB1 (5.0A) **solo puede alimentar y operar una única locomotora pesada de USA Trains simultáneamente en la maqueta** ($1 \times 3.0\text{A} = 3.0\text{A}$, dejando 2.0A de margen de seguridad para el arranque o encendido de humo).
    *   Intentar operar dos locomotoras de la Ultimate Series de manera simultánea superará de forma continua los 5.0A, disparando instantáneamente la protección térmica de la central y apagando el sistema.
*   **Picos de Arranque y Atascos (Cortocircuito Virtual):** Debido a que la corriente de bloqueo de estas máquinas supera los 15 Amperios, **cualquier atasco mecánico, descarrilamiento corto o fricción severa en las bielas de las locomotoras USA Trains será interpretado instantáneamente por la central DCC-EX como un cortocircuito absoluto en la vía**, disparando la protección ultrarrápida del motor shield.
*   **Recomendación de Diseño Obligatoria:** Para habilitar una operación confiable libre de falsos disparos en el restaurante, es obligatorio alimentar el tendido de vías mediante **Boosters G-Scale industriales independientes con una capacidad mínima de 8.0 Amperios a 10.0 Amperios continuos por sector de vía**.

---

## 8. Evaluación para Pruebas de Tracción y Automatización

Evaluamos la idoneidad operacional de las familias de USA Trains bajo los 10 escenarios críticos de servicio diario continuado en el restaurante:

### Calificación de Idoneidad por Familia Técnica:

### 1. Familia USM-1M-A (Ej. Vaporera 0-4-0 Switcher Workhorse)
*   **Valoración:** **Aceptable con adición de Keep-Alive.**
*   **Razón:** Es una locomotora monomotor muy eficiente en consumo de corriente, lo que permite que sea operada fácilmente en centrales estándar sin boosters costosos. Su transmisión de bielas cortas es confiable, pero su corta captación eléctrica la hace muy propensa a detenerse sobre desvíos sucios. Exige la soldadura obligatoria de un capacitor Keep-Alive de gran capacidad para asegurar paradas precisas y marcha fluida.

### 2. Familia USM-2M-DR (Ej. Alco S-4, GE 44-Tonner Switchers)
*   **Valoración:** **Recomendable.**
*   **Razón:** Estas locomotoras de maniobra medianas ofrecen un excelente equilibrio técnico. Su tracción Bo-Bo (4 ejes motrices) les otorga una excelente fuerza de arrastre para jalar vajillas moderadamente pesadas en trazados planos. Al no tener la inmensa masa de las diésel de línea principal, su consumo eléctrico es menor (~1.5A), permitiendo operar múltiples unidades con el EX-CSB1 de forma segura. La única consideración compleja es el cableado de su sistema de luces LED directional en taller.

### 3. Familia USM-2M-HEAVY (Ej. EMD GP7, GP9, GP38-2 Ultimate Series)
*   **Valoración:** **Muy recomendable (con boosters de potencia).**
*   **Razón:** Representan el estándar de oro americano en tracción y robustez diésel. Su chasis masivo con insertos de plomo y tracción 4x4 les permite subir pendientes moderadas cargadas con vajilla pesada sin esfuerzo alguno. El mantenimiento de sus bogies Bo-Bo es sumamente sencillo. Sin embargo, su alta demanda de corriente de arranque exige de forma obligatoria boosters de potencia independientes en el restaurante.

### 4. Familia USM-2M-COCO (Ej. EMD SD40-2, SD70MAC Ultimate Series)
*   **Valoración:** **Recomendable con restricciones de diseño físico de vía.**
*   **Razón:** Son las locomotoras diésel más potentes y pesadas de la escala G. Su tracción integral en 6 ejes (Co-Co) y su masa de hasta 8 kg garantizan una adherencia colosal; pueden remolcar trenes enteros cargados con comidas pesadas y jarras de líquidos sin el menor atisbo de derrape. No obstante, **su gran longitud exige radios de curva sumamente amplios ($R > 1.2$ metros o R3/R5)**, lo que restringe drásticamente la flexibilidad del trazado físico del restaurante. Además, su consumo eléctrico extremo exige fuentes centralizadas de 10A y boosters industriales obligatorios.

---

## 9. Ranking Técnico Recomendado para USA Trains G

Basados en el análisis electromecánico de consumo, torque, complejidad de conversión y resistencia diaria, establecemos los siguientes rankings:

### Ranking A: Mejores Locomotoras Económicas para Comenzar (Fase MVP y Pruebas)
*Este ranking prioriza el bajo precio de segunda mano, la simplicidad de conversión, bajo consumo y estabilidad mecánica.*

1.  **USA Trains R22601 / R22602 (GE 44-Tonner Switcher - Familia USM-2M-DR):**
    *   *Por qué:* Es una locomotora de maniobras de tamaño contenido y consumo moderado. Al remover la carrocería, el acceso a la placa de conmutación analógica es limpio. No requiere desarmados mecánicos complejos. Su tracción integral Bo-Bo es sumamente suave y silenciosa.
2.  **USA Trains R22301 / R22302 (Alco S-4 Switcher - Familia USM-2M-DR):**
    *   *Por qué:* Excelente tractor industrial clásico. Su mecánica de engranajes cerrados es robusta y muy fácil de lubricar. Ofrece un amplio espacio en su carrocería plástica para montar decoders XL, altavoces de gran diámetro y circuitos limitadores de corriente.
3.  **USA Trains R22106 / R22110 (EMD GP9 - Familia USM-2M-HEAVY):**
    *   *Por qué:* Es un modelo de línea media de gran popularidad. Su chasis Bo-Bo es el más confiable y de menor mantenimiento de la Ultimate Series. Su precio de adquisición en el mercado de segunda mano es significativamente inferior al de las diésel Co-Co de tres ejes.

### Ranking B: Mejores Plataformas para Pruebas de Fuerza (Tracción Pesada de Platos)
*Este ranking prioriza la masa bruta de la locomotora (adherencia), tracción total por ruedas motrices pesadas y alta tolerancia a sobrecorrientes en decoders.*

1.  **USA Trains R22600 / R22602 (EMD SD70MAC - Familia USM-2M-COCO):**
    *   *Por qué:* Es una bestia de tracción diésel moderna de 6 ejes (Co-Co). Su peso masivo superior a los 7.5 kg y sus ejes acoplados de gran torque garantizan que la locomotora nunca patinará ni perderá tracción en rampas inclinadas, incluso remolcando platos con bebidas y sopas pesadas.
2.  **USA Trains R22300 / R22302 (EMD SD40-2 - Familia USM-2M-COCO):**
    *   *Por qué:* Ofrece la misma configuración Co-Co masiva de tres ejes por bogie que la SD70MAC pero bajo la carrocería clásica de una SD40-2. Su adherencia sobre rieles metálicos es formidable, permitiendo un flujo continuo de tracción con excelente disipación de calor en sus dos motores can.
3.  **USA Trains R22204 / R22206 (EMD GP38-2 - Familia USM-2M-HEAVY):**
    *   *Por qué:* Es el caballo de batalla diésel estadounidense de 4 ejes (Bo-Bo). Chasis pesado con bloques motorizados compactos que ejercen una fuerza de arrastre sobresaliente, ideal para validar el límite de carga útil del restaurante sin la inmensa longitud de los modelos de 3 ejes.

### Ranking C: Mejores Plataformas para Operación Prolongada (Confiabilidad y Sostenibilidad 12h/7d)
*Este ranking prioriza la simplicidad de la transmisión mecánica (baja cantidad de bielas o engranajes expuestos), robustez térmica, facilidad de lubricación estanca y estabilidad ante grasa de cocina.*

1.  **USA Trains R22301 / R22302 (Alco S-4 Switcher - Familia USM-2M-DR):**
    *   *Por qué:* Es una de las locomotoras de maniobra más robustas jamás fabricadas. Sus dos bogies Bo-Bo están completamente cerrados, protegiendo los piñones y ejes de manera estanca contra cualquier vapor de cocina, polvo o grasa en el restaurante. Al no tener varillajes de bielas expuestos ni transmisiones cardán articuladas, el desgaste mecánico es prácticamente inexistente bajo uso intensivo de 12 horas diarias.
2.  **USA Trains R22206 / R22212 (EMD GP38-2 - Familia USM-2M-HEAVY):**
    *   *Por qué:* Excelente estabilidad térmica de sus dos motores Mabuchi. Sus engranajes helicoidales internos de nylon de alta densidad y ejes de bronce autolubricados toleran ciclos de trabajo diarios prolongados con bajísima fricción, requiriendo engrase periódico únicamente cada 100 horas de operación.
3.  **USA Trains R22151 / R22152 (GE 44-Tonner Switcher - Familia USM-2M-DR):**
    *   *Por qué:* Tractor compacto minimalista de maniobras industriales. Tracción integral Bo-Bo directa por piñones cerrados. Su menor masa reduce drásticamente el desgaste por fricción en las pestañas de las ruedas en curvas y consume la mitad de corriente que las pesadas diésel de línea principal, minimizando la acumulación de calor interno sobre la electrónica.

---

## 10. Referencias Pendientes de Verificación Técnica

Identificamos las siguientes referencias dentro de la gama de USA Trains que requieren inspección física en banco de pruebas o manuales de despiece adicionales para certificar sus detalles electromecánicos debido a contradicciones o vacíos en catálogos históricos:

1.  **USA Trains R2201 (0-4-0 Steam Switcher):** Se requiere abrir físicamente el chasis compacto para verificar si el retorno de la luz de la caldera y la cabina comparte un común negativo integrado en la fundición de antimonio del bloque motor, lo cual complicaría la instalación de decoders con salidas aisladas.
2.  **USA Trains R20001 (NYC J1e Hudson 4-6-4 Die-Cast Metal):** Esta imponente locomotora de vapor de 13 kg posee una arquitectura de control interna sumamente compleja. Debemos verificar si su generador de humo pulsado por ventilador opera a través de un motor de 5V independiente que requiera una salida de función aislada y regulada en el decodificador, o si puede alimentarse de forma directa mediante la tensión de vía DCC de 24V.

---

## 11. Conclusión y Comparativa Global de Flota (LGB vs. PIKO vs. Bachmann vs. USA Trains)

Con la incorporación de **USA Trains** a nuestra matriz analítica de la gran escala, disponemos del panorama más completo de ingeniería ferroviaria para la toma de decisiones del restaurante:

### A. Matriz Comparativa Global de Gran Escala

| Factor Técnico / Marca | **LGB (Märklin)** | **PIKO G (Alemania)** | **Bachmann G (Spectrum)** | **USA Trains (Ultimate)** |
| :--- | :--- | :--- | :--- | :--- |
| **Escala de Fabricación** | 1:22.5 (Vía métrica europea) | 1:22.5 / 1:26 | 1:20.3 (Fn3 - Vía Estrecha) | **1:29 (Escala Estándar EE. UU.)** |
| **Materiales de Carrocería** | Plástico LURAN-S y algunos modelos de metal die-cast. | Plástico ABS de alta resistencia con plomos. | Resinas pesadas, plásticos y metal die-cast pesado. | **Plásticos pesados de gran calibre e insertos masivos de plomo**. |
| **Aislamiento de Motores** | Complejo en antiguos (3-pin). Excelente en modernos (4-pin). | **Excelente (100% aislados de fábrica con conectores de 4 pines)**. | Complejo en Big Hauler. Excelente en gama Spectrum (PnP). | **Excelente (100% aislados galvánicamente de fábrica)**. |
| **Facilidad de Mantenimiento** | Media. Caja partida verticalmente Clamshell antigua. | **Excelente. Caja Sandwich con bottom-plate de acceso directo**. | Media. Requiere desarmar bielas o tenders en vapor pesado. | **Excelente. Bogies independientes con bottom-plate desmontable**. |
| **Consumo Eléctrico Promedio**| **Bajo (Motores Bühler de 5-7 polos muy eficientes ~0.4A–0.8A)**. | Medio (Motores can de alta precisión ~0.5A–1.0A). | Medio-Alto (Motores Pittman y transmisiones por cardán ~0.8A–1.8A). | **Extremo (Motores Mabuchi RS-545 de baja resistencia ~1.5A–3.0A)**. |
| **Corriente de Bloqueo (*Stall*)**| ~1.3A nominal por motor (2.6A bimotor). | ~1.5A nominal por motor (3.0A bimotor). | ~1.5A a 2.5A por motor (3.5A a 4.5A bimotor). | **Extrema (~15A nominal por motor / >30A combinados bimotor)**. |
| **Sistemas Digitales de Fábrica**| Decoders mfx/DCC con Gold Caps instalados de fábrica. | Decoders SmartDecoder XP 5.1 G de 5.0A con RailCom. | Clásico Dummy Board con zócalo 12/11-pin para decoders de terceros. | Ninguno (DC Analógica con delay board). Requiere hardwiring total. |
| **Fuerza de Arrastre de Vajilla** | Excelente (Alta adherencia por peso en metal y ruedas LGB). | Buena (Limitada en pendientes pronunciadas por peso ligero). | Excelente (Gran torque en engranajes articulados). | **Insuperable (La mayor adherencia y masa del mercado escala G)**. |
| **Radios de Curva Requeridos** | **Muy flexibles (Diseñados para R1 = 600 mm en toda la gama)**. | **Muy flexibles (Diseñados para R1 = 600 mm en toda la gama)**. | Exigentes (La mayoría de Spectrum pesadas exigen R > 1200 mm). | Exigentes (Diésel de línea y Co-Co exigen R > 1200 mm). |

---

### B. Plan de Acción de Selección de Flota para el Restaurante

1.  **Fase 1 - Prototipo de Laboratorio y Pruebas de Software (DCC-EX):**  
    *Adquirir:* **PIKO GE 25-Ton Switcher (38500)** o **LGB Stainz 20211/21211 (EM-1M-D)**. Sus bloques modernos de 4 pines aislados de fábrica y su bajísimo consumo (<0.5A) garantizan un entorno de pruebas de software 100% seguro y de bajo costo para validar los algoritmos de la central DCC-EX sin riesgo de cortocircuitos.
2.  **Fase 2 - Flota de Servicio Diario de Entrega de Platos (Caballos de Batalla):**  
    *Adquirir:* **PIKO DR BR 132 "Ludmilla" (37540 / 37542)** o **USA Trains Alco S-4 Switcher (R22550 / R22551)**. 
    *   *Si la prioridad es la facilidad de mantenimiento:* Elegir la **Ludmilla de PIKO**, gracias a su compuerta de acceso rápido por techo y chasis sellado Sandwich.
    *   *Si la prioridad es la fuerza bruta de arrastre y robustez de chasis:* Elegir el **S-4 de USA Trains**, gracias a su masiva adherencia por peso y bogies Bo-Bo estancos al polvo y grasa de cocina. Ambos modelos operan perfectamente en curvas de radio medio ($R = 600\text{ mm}$ a $R = 900\text{ mm}$).
3.  **Fase 3 - Tren de Gran Espectáculo Visual y Servicio de Mesas Especiales:**  
    *Adquirir:* **LGB Ge 6/6 I Cocodrilo (26600)** o **USA Trains EMD SD70MAC (R22602)**. El Cocodrilo articulado de LGB de metal macizo o la masiva locomotora diésel SD70MAC de USA Trains capturarán la atención total de los clientes del restaurante gracias a su realismo e imponente tamaño. Ambas exigen el uso de **curvas de radio amplio ($R > 1.2$ metros)** y la instalación obligatoria de **Boosters de potencia independientes de 10 Amperios** en las vías debido a su alto consumo eléctrico combinado.
4.  **Estandarización de Estabilizadores de Energía:**  
    Es de carácter **crítico y obligatorio** la instalación de un **Power Buffer (Capacitador Stay-Alive de mínimo 1 Faradio)** en cada locomotora digitalizada. Esto garantizará inmunidad absoluta ante los micro-cortes de energía causados por la grasa evaporada suspendida de la cocina que se deposita en los rieles, asegurando una marcha suave, sonido ininterrumpido y detenciones de entrega de comida milimétricas frente a las mesas de los comensales.

---

### Enlaces y Fuentes Consultadas

1.  **George Schreyer's Technical Pages - USA Trains Guides:** Análisis de corriente de bloqueo, winding resistance de motores Mabuchi RS-545 y soluciones para el cableado de ánodo común en Ultimate Series. [girr.org](http://www.girr.org/girr/tips/tips2/usat_tips.html)
2.  **USA Trains Official Website:** Manuales de usuario, diagramas de despiece, catálogo histórico de referencias de la Ultimate Series y repuestos de bogies. [usatrains.com](http://www.usatrains.com/)
3.  **G Scale Central - USA Trains DCC Conversions:** Bitácoras de instalación de decodificadores ESU XL y NCE D808, y métodos de bypass para la placa de retardo. [gscalecentral.net](https://www.gscalecentral.net/)
4.  **SBS4DCC Large Scale DCC Guides:** Especificaciones térmicas de los decodificadores NCE D808SR y esquemas para la instalación de resistencias limitadoras de 1.0 Ohm. [sbs4dcc.com](https://www.sbs4dcc.com/)
5.  **CVP AirWire Technical Resources:** Manuales de instalación para receptores de batería directos específicos para modelos SD70 y GP38 de USA Trains. [cvpusa.com](https://www.cvpusa.com/)
6.  **ESU Electronic Solutions Ulm:** Manual de usuario de la gama de decodificadores LokSound 5 XL y configuraciones de BEMF para motores Mabuchi RS-545 de G-scale. [esu.eu](https://www.esu.eu/)
