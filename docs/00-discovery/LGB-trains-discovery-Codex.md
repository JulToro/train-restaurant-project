# Investigación técnica LGB (escala G): DCC, tracción y mantenimiento

Fecha de corte: 6 de septiembre de 2026. Escala G, vía 45 mm, dos carriles. Marca LGB (desde 2007 Märklin).

## Método y límites

Se priorizaron manuales y despieces LGB/Märklin, catálogos históricos y guías de conversión Massoth/ZIMO/ESU. F = fabricante; M = manual/despiece; C = catálogo; U = usuario; I = inferencia; NF = no encontrado. No se generalizan corriente, peso, fuerza, temperatura, radio o potencia si el fabricante no los publica. El voltaje 0–24 V de un manual no es consumo y no se calcula P=V×I sin V e I de la misma locomotora.

## Familias técnicas

| Familia | Configuración y referencias | Motores/bloques/ejes | Electrónica y funciones | Corriente y consumo | Conversión/EX-CSB1 | Repuestos y confianza | Fuentes |
|---|---|---|---|---|---|---|---|
| LGB-1M-OLD | Bloque antiguo de carcasa partida; 2010/2020D, 2018D, 2060D, 2080D, 2019S | 1/1/2–3; caja, engranajes y bielas antiguas | Sin DCC; algunas placas de sonido analógico; humo según lote | NF; no publicado por fabricante | Aislar motor de carril; Massoth XL/XLS, ESU XL o ZIMO G solo después de medir; EX-CSB1 solo tras DCC | Motores/carbones/patines pueden repetirse; confianza media | [GIRR DCC](https://www.girr.org/girr/tips/tips5/dcc_tips.html), [Massoth interfaz](https://www.massoth.de/wp-content/uploads/2019/11/Massoth-Workshop_Digitale-Gartenbahn_2015-Maerz.pdf) |
| LGB-1M-DIRECT | Interfaz directa y un bloque; 20214, 20215, 20211, 21211, 21761, 20212 | 1/1/2; caja Stainz | DCC-ready, humo en vapor; 20214 manual recomienda decoder 55027 | NF; manual pide controlador >1 A | Baja-media; reversible con puente/interfaz; EX-CSB1 directo después del decoder | 20214: motor E126050, captación E171326, ruedas E178288/E178284 y aro E126174; confianza alta para 20214 | [Manual 20214](https://static.maerklin.de/damcontent/33/e3/33e338081993f633b32d6d7452d9cb671467807303.pdf), [Massoth Stainz](https://www.massoth.de/wp-content/uploads/2020/04/LGB-2x212-Stainz_EN_massoth_umbaubericht.pdf) |
| LGB-1M-SOUND | Un motor, versiones modernas 20752 y 20753 | 1/1/2 | Märklin/LGB mfx/DCC sound, humo y luces | NF | DCC directo; Massoth XLS, ESU L o ZIMO G recableados; EX-CSB1 por DCC | Ambas comparten E126050, E126006, E185962, E144092/E144089, E171326 y E144497; placa no demostrada idéntica; confianza alta mecánica | [Catálogo 2018](https://champex-linden.de/cl_downloads/download_fremddokumente_2018/2018_lgb_neuheiten_de.pdf), [Typenverzeichnis](https://champex-linden.de/spremberger-stadtbahn-11-alias-kleine-dicke) |
| LGB-MOGUL-1M | Un bloque, Mogul: 20280, 20282, 20283, 20284, 21192, 22191, 24182, 26192 | 1/1/3 | MTS, interfaz o mfx/DCC según lote; sonido/humo en versiones modernas | NF | Media; Massoth XLS, ESU L/XL, ZIMO G; EX-CSB1 tras aislamiento | 20280/82/83 comparten E129994, E133761, E315534 y ruedas E174467/E174451/E298231; 20284 cambia ruedas/tender; confianza alta subgrupo | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index.html), [GIRR tender](https://www.girr.org/girr/tips/tips9/lgb_2017_tips.html) |
| LGB-HG3-1M | Cremallera, revisiones separadas: 20275, 21470, 21471, 22470, 20471 | 1/1/3; engranaje HG3/3 y bielas | 20275 mfx/DCC factory; 21470/22470/20471 ready; 21471 MTS sin sonido | 21470 exige 1,5 A de decoder; 22470 interfaz 3 A; consumo NF | Baja-media ready; 55020/55027; funciones OEM pueden exigir recableado; EX-CSB1 por DCC | E134666/E177543/E177545 compartidos parcialmente; ejes/humo cambian; confianza alta | [Manual 2147x](https://www.beathis.ch/lgb/20471/2147x_062005.pdf), [22470](https://www.beathis.ch/lgb/20471/22470.pdf), [20275](https://static.maerklin.de/damcontent/e3/d7/e3d7809fdfbcdc16d854390ae8f0d1cc1667885379.pdf) |
| LGB-E10-2M-READY | Dos bogies y dos motores: 20750; 21751 es revisión mfx/DCC distinta | 2/2/4 | 20750 interfaz para decoder 3 A, sin sonido; 21751 mfx/DCC | 3 A es capacidad del decoder recomendada, no consumo | Baja-media; Massoth XLS documentado, ESU XL/ZIMO MS990; EX-CSB1 tras instalar | 20750: E188198, E188314, E177820; confianza alta | [Manual](https://www.trainli.com/USER-GUIDE-PDF/New%20LGB%20Engines/lgb-20750-user-guide.pdf), [Massoth](https://www.massoth.de/wp-content/uploads/2020/04/LGB-20750-DB-E10_EN_massoth_umbaubericht.pdf) |
| LGB-GE4-2M | Dos bogies; 20420, 21420, 21430, 21431, 28443, 28445, 28446 | 2/2/4 | Ready/MTS o mfx/DCC; pantógrafos y placas cambian | NF | Baja-media; Massoth XL/XLS, ESU XL, ZIMO MS990; pantógrafo requiere adaptador; EX-CSB1 DCC | 28443/45/46 comparten E162585, E126050, E133761, E142325/E142327, E195622, E130625; 21430/31 comparten mecánica pero no toda la iluminación; confianza alta en subgrupo | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index3.html), [Massoth panto](https://www.massoth.de/en/category/massoth-wiki/tips-und-tricks/) |
| LGB-GE6-2M | Articulada, dos bloques y seis ejes: 22061, 22062, 26600, 26602 | 2/2/6 | MTS o mfx/DCC según lote, sonido en modernas | NF; E345896 y E274352 aparecen en fuentes distintas | ESU XL, ZIMO MS990 o Massoth solo tras verificar corriente; EX-CSB1 por DCC | No agrupar motores hasta inspección; confianza media | [Publicaciones LGB](https://www.lgb-trains.com/lib/articles/en/items.html), [ZIMO](https://www.zimo.at/web2010/products/ms-sound-decoder-grossbahn.htm) |
| LGB-ALCO-2M | Co-Co, dos cajas encapsuladas: 25554–25558 | 2/2/4, 12 captaciones | 25554 MTS; 25555–58 mfx/DCC sound | NF; manual 25554 pide controlador >1 A | Media; ESU XL, Massoth XL/XLS, ZIMO MS990; EX-CSB1 directo solo DCC | 25554: E129994, E163760, E184714, E170039/E163768/E163789/E163773, E126174, E184715; confianza alta para 25554 | [Despiece](https://static.maerklin.de/damcontent/37/a2/37a219f07ca5a30e6be16636a2d0bd5a1434543447.pdf), [Manual](https://www.manualslib.de/manual/75796/Lgb-25554.html), [Typenverzeichnis](https://champex-linden.de/alco-dl535-e) |
| LGB-TRACKCLEAN-1M-AUX | Limpiavías 21671/21672; tracción y motor/ruedas de limpieza | 1 + auxiliar/1/2 | mfx/DCC sound; funciones F11/F27 y consumibles | NF | DCC directo, pero mapa auxiliar y corriente deben medirse; EX-CSB1 DCC | Motor Bühler y ruedas E132064; no es locomotora de arrastre; confianza alta | [Manual](https://www.manualslib.com/manual/3226112/Lgb-21671.html), [Descripción](https://www.hobbyco.net/lgb/lgb-locos/track-cleaning-locomotive-l21671) |
| LGB-MALLET-2M | Dos bloques articulados: 2085D, 20852, 21852, 26851 | 2/2/4 | DC, MTS o sound según lote | NF | Alta en antiguos; ESU XL/Massoth/ZIMO tras aislamiento; EX-CSB1 solo después | No hay prueba de caja/placa común; confianza baja-media | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index.html) |

## Tabla consolidada con las columnas solicitadas

La tabla anterior es legible para comparar plataformas. Esta matriz conserva literalmente los campos pedidos; NF significa que no se publicó o no se verificó para toda la familia.

| Familia técnica | Marca | Configuración interna | Referencias que utilizan la configuración | Motores | Bloques motores | Ejes motrices | Transmisión | Decoder de fábrica | Protocolos | Corriente decoder | Consumo conocido | Potencia conocida o calculada | Sonido | Humo | Servos | Capacitor | Decoders alternativos | Dificultad de conversión | Compatibilidad EX-CSB1 | Repuestos compartidos | Nivel de confianza | Fuentes |
|---|---|---|---|---:|---:|---:|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| LGB-1M-OLD | LGB | Bloque antiguo no aislado, una motriz | 2010/2020D, 2018D, 2060D, 2080D | 1 | 1 | 2–3 | Caja antigua/bielas | Ninguno | DC; DCC tras conversión | NF | NF | NF | No DCC | Lote | No | NF | Massoth XL/XLS, ESU XL, ZIMO G | Alta | Sí después de aislar | Motor/carbones/patines según lote | Medio | [GIRR](https://www.girr.org/girr/tips/tips5/dcc_tips.html) |
| LGB-1M-DIRECT | LGB | Bloque aislado con interfaz directa | 20214, 20215, 20211, 21211, 21761 | 1 | 1 | 2 | Caja Stainz | Ninguno | DC; DCC | NF; controlador >1 A en 20214 | NF | NF | No en 20214 | Sí | No | NF | LGB 55027, Massoth XLS, ESU L | Baja-media | Sí tras decoder | E126050 en 20214 | Alto (20214) | [Manual 20214](https://static.maerklin.de/damcontent/33/e3/33e338081993f633b32d6d7452d9cb671467807303.pdf) |
| LGB-1M-SOUND | LGB/Märklin | Un bloque, mfx/DCC moderno | 20752, 20753 | 1 | 1 | 2 | Caja compacta | OEM mfx/DCC | DC, DCC, mfx | NF | NF | NF | Sí | Sí | NF | NF | Massoth XLS, ESU L, ZIMO G | Baja | Sí por DCC | E126050, E126006, E185962, E144497 | Alto mecánico | [Catálogo](https://champex-linden.de/cl_downloads/download_fremddokumente_2018/2018_lgb_neuheiten_de.pdf) |
| LGB-MOGUL-1M | LGB | Un bloque, tres motrices | 20280/82/83/84, 21192, 22191 | 1 | 1 | 3 | Reductor y bielas Mogul | MTS, interfaz o mfx/DCC | DC, MTS, DCC/mfx | NF | NF | NF | Según lote | Sí en vapor | No | NF | Massoth XLS, ESU L/XL, ZIMO G | Media | Sí tras aislamiento/DCC | E129994/E133761; 20284 cambia ruedas | Alto subgrupo | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index.html) |
| LGB-HG3-1M | LGB | Cremallera, una motriz | 20275, 21470, 21471, 22470, 20471 | 1 | 1 | 3 | HG3/3 y cremallera | 367667, MTS o interfaz | DC, DCC, MTS | 1,5 A o 3 A según interfaz; no consumo | NF | NF | 20275 sí; 21471 no | Según lote | No | NF | 55020/55027, ESU/ZIMO/Massoth cableados | Baja-media ready | Sí por DCC | E134666/E177543/E177545 parciales | Alto | [Manual 2147x](https://www.beathis.ch/lgb/20471/2147x_062005.pdf) |
| LGB-E10-2M-READY | LGB | Dos bogies, cuatro motrices | 20750, 21751 (revisión distinta) | 2 | 2 | 4 | Dos reductores | Interfaz 3 A o mfx/DCC | DC, DCC, mfx | 3 A de decoder recomendado, no consumo | NF | NF | 21751 sí | No | No | NF | Massoth XLS, ESU XL, ZIMO MS990 | Baja-media | Sí tras decoder | E188198/E188314/E177820 | Alto | [Massoth](https://www.massoth.de/wp-content/uploads/2020/04/LGB-20750-DB-E10_EN_massoth_umbaubericht.pdf) |
| LGB-GE4-2M | LGB/Märklin | Dos bogies, cuatro motrices | 20420, 21420, 21430/31, 28443/45/46 | 2 | 2 | 4 | Dos reductores | Ready/MTS o mfx/DCC | DC, DCC, mfx | NF | NF | NF | Modernas sí | No | Según lote | NF | Massoth XL/XLS, ESU XL, ZIMO MS990 | Baja-media | Sí por DCC | E126050/E133761; 28443/45/46 comparten E162585 | Alto subgrupo | [Massoth](https://www.massoth.de/en/category/massoth-wiki/tips-und-tricks/) |
| LGB-GE6-2M | LGB/Märklin | Articulada, seis motrices | 22061, 22062, 26600, 26602 | 2 | 2 | 6 | Dos articulados | MTS o mfx/DCC | DC, DCC, mfx | NF | NF | NF | Modernas sí | NF | No probado | NF | ESU XL, ZIMO MS990 | Media-alta | Sí por DCC | Motor contradicho E345896/E274352 | Medio | [LGB publicaciones](https://www.lgb-trains.com/lib/articles/en/items.html) |
| LGB-ALCO-2M | LGB | Co-Co, cajas encapsuladas | 25554–25558 | 2 | 2 | 4 | Dos reductores | MTS o mfx/DCC sound | DC, MTS, DCC, mfx | NF | NF | NF | Sí | No | No | NF | ESU XL, Massoth XL/XLS, ZIMO MS990 | Media | Solo DCC en modernas; 25554 recableado | E129994/E163760/E184714/E126174 | Alto 25554 | [Despiece](https://static.maerklin.de/damcontent/37/a2/37a219f07ca5a30e6be16636a2d0bd5a1434543447.pdf) |
| LGB-TRACKCLEAN-1M-AUX | LGB/Märklin | Tracción más motor de limpieza | 21671, 21672 | 1 + auxiliar | 1 | 2 | Caja limpiavías | mfx/DCC sound | DC, DCC, mfx | NF | NF | NF | Sí | Ruedas de limpieza | No | NF | Massoth/ESU/ZIMO con mapa AUX | Media-alta | Sí por DCC | Motor Bühler/E132064 | Alto identificación | [Manual](https://www.manualslib.com/manual/3226112/Lgb-21671.html) |
| LGB-MALLET-2M | LGB | Dos bloques articulados | 2085D, 20852, 21852, 26851 | 2 | 2 | 4 | Dos reductores/bielas | DC, MTS o sound | DC, MTS, DCC tras conversión | NF | NF | NF | Según lote | Sí vapor | No | NF | ESU XL, Massoth XL/XLS, ZIMO MS990 | Alta antiguos | Sí tras conversión | No probado caja/placa común | Bajo-medio | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index.html) |

## Tabla individual por referencia

| Marca | Referencia | Modelo | Familia | DCC fábrica | DCC-ready | Conversión documentada | Motores | Ejes motrices | Decoder | Sonido | Humo | Estado | Radio mínimo | Fuente |
|---|---|---|---|---|---|---|---:|---:|---|---|---|---|---|---|
| LGB | 2010/2020D | Stainz antigua | 1M-OLD | No | No | Sí, aislamiento | 1 | 2 | Ninguno | No | Lote | Descontinuada | NF | [GIRR](https://www.girr.org/girr/tips/tips5/dcc_tips.html) |
| LGB | 2018D | Mogul antigua | 1M-OLD | No | No | Sí | 1 | 3 | Ninguno | No | Lote | Descontinuada | NF | [GIRR](https://www.girr.org/girr/tips/tips5/dcc_tips.html) |
| LGB | 2060D | Schöma | 1M-OLD | No | No | Sí | 1 | 2 | Ninguno | No | No | Descontinuada | NF | [GIRR](https://www.girr.org/girr/tips/tips5/dcc_tips.html) |
| LGB | 2080D | Harz | 1M-OLD | No | No | Sí | 1 | 3 | Ninguno | No | Sí | Descontinuada | NF | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index.html) |
| LGB | 20214 | Stainz Richter | 1M-DIRECT | No | Sí | Sí, 55027 | 1 | 2 | Interfaz directa | No | Sí | Colección | NF | [Manual](https://static.maerklin.de/damcontent/33/e3/33e338081993f633b32d6d7452d9cb671467807303.pdf) |
| LGB | 20211 | Stainz StLB | 1M-DIRECT | No | Sí, lote | Sí | 1 | 2 | Interfaz | No | Lote | Descontinuada | NF | [Publicaciones](https://www.lgb-trains.com/lib/articles/en/items.html) |
| LGB | 21211 | Stainz | 1M-DIRECT | No | Sí | Sí | 1 | 2 | Interfaz | No | Lote | Descontinuada | NF | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index.html) |
| LGB | 21761 | Spremberger | 1M-DIRECT | No | Sí | Sí | 1 | 2 | Preparada | No | Sí | Descontinuada | NF | [Typenverzeichnis](https://champex-linden.de/spremberger-stadtbahn-11-alias-kleine-dicke) |
| LGB | 20752 | DR 99 5015 | 1M-SOUND | Sí | N/A | N/A | 1 | 2 | mfx/DCC sound | Sí | Sí | Descontinuada | NF | [Catálogo](https://champex-linden.de/cl_downloads/download_fremddokumente_2018/2018_lgb_neuheiten_de.pdf) |
| LGB | 20753 | DR 99 5016 | 1M-SOUND | Sí | N/A | N/A | 1 | 2 | mfx/DCC sound | Sí | Sí | Reciente | NF | [Descripción](https://www.ceneo.pl/188788758) |
| LGB | 20280 | Mogul | MOGUL-1M | Sí/lote | N/A | N/A | 1 | 3 | mfx/DCC | Sí | Sí | Descontinuada | NF | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index.html) |
| LGB | 20282 | Mogul Coca-Cola | MOGUL-1M | Sí | N/A | N/A | 1 | 3 | mfx/DCC | Sí | Sí | Descontinuada | NF | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index.html) |
| LGB | 20283 | Mogul D&S | MOGUL-1M | Sí | N/A | N/A | 1 | 3 | mfx/DCC | Sí | Sí | Descontinuada | NF | [Catálogo 2021](https://www.bahnorama.ch/data/downloads/LGB/archiv/LGB_Neuheiten_2021.pdf) |
| LGB | 20284 | Mogul Nevada | MOGUL-1M | Sí | N/A | N/A | 1 | 3 | mfx/DCC | Sí | Sí | Reciente | NF | [Catálogo 2024](https://www.marklin.gr/media/pdf/Catalogs/LGB/2024/EN_lgb_nh2024_Online.pdf) |
| LGB | 21192 | Mogul Bumblebee | MOGUL-1M | MTS/lote | Sí/lote | Sí | 1 | 3 | LGB onboard | Sí | Sí | Descontinuada | NF | [GIRR](https://www.girr.org/girr/tips/tips9/lgb_2017_tips.html) |
| LGB | 22191 | Mogul C&S | MOGUL-1M | No/lote | Sí/lote | Sí | 1 | 3 | Interfaz | No | Sí | Descontinuada | NF | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index.html) |
| LGB | 20275 | HG3/3 1067 | HG3-1M | Sí | N/A | N/A | 1 | 3 | 367667 | Sí | Sí | Descontinuada | NF | [Manual](https://static.maerklin.de/damcontent/e3/d7/e3d7809fdfbcdc16d854390ae8f0d1cc1667885379.pdf) |
| LGB | 21470 | HG3/3 1067 | HG3-1M | No | Sí | Sí | 1 | 3 | Interfaz | No | Sí | Descontinuada | NF | [Manual](https://www.beathis.ch/lgb/20471/2147x_062005.pdf) |
| LGB | 21471 | HG3/3 1068 | HG3-1M | Sí, MTS | No | Sí, recambio | 1 | 3 | MTS onboard | No | Sí | Descontinuada | NF | [Manual](https://www.beathis.ch/lgb/20471/2147x_062005.pdf) |
| LGB | 22470 | HG3/3 1058 | HG3-1M | No | Sí | Sí, 55027 | 1 | 3 | Interfaz | No | Sí | Descontinuada | NF | [Manual](https://www.beathis.ch/lgb/20471/22470.pdf) |
| LGB | 20471 | HG3/3 1067 | HG3-1M | No | Sí | Sí, 55020/MX699KV | 1 | 3 | Directa | No | Sí | Descontinuada | NF | [Conversión](https://www.beathis.ch/lgb/20471/20471.html) |
| LGB | 20750 | DB E10 | E10-2M | No | Sí | Sí, Massoth | 2 | 4 | Interfaz 3 A | No | No | Descontinuada | NF | [Manual](https://www.trainli.com/USER-GUIDE-PDF/New%20LGB%20Engines/lgb-20750-user-guide.pdf) |
| LGB | 21751 | DB E10 | E10-2M revisión | Sí | N/A | N/A | 2 | 4 | mfx/DCC | Sí | No | Descontinuada | NF | [Modelo](https://www.modellbau-wiki.de/wiki/DB-Baureihe_E_10) |
| LGB | 20420 | Ge 4/4 III | GE4-2M | Lote | Sí/lote | Sí | 2 | 4 | Interfaz/MTS | Lote | No | Descontinuada | NF | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index3.html) |
| LGB | 21420 | Ge 4/4 III | GE4-2M | No | Sí | Sí | 2 | 4 | Interfaz | No | No | Descontinuada | NF | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index3.html) |
| LGB | 21430 | Ge 4/4 III | GE4-2M | No | Sí | Sí | 2 | 4 | Interfaz | No | No | Descontinuada | NF | [Train-Li](https://www.train-li-usa.com/page/lgb-user-guide-manual) |
| LGB | 21431 | Ge 4/4 III | GE4-2M | No | Sí | Sí | 2 | 4 | Interfaz | No | No | Descontinuada | NF | [Train-Li](https://www.train-li-usa.com/page/lgb-user-guide-manual) |
| LGB | 28443 | Ge 4/4 | GE4-2MS | Sí | N/A | N/A | 2 | 4 | mfx/DCC | Sí | No | Descontinuada | NF | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index3.html) |
| LGB | 28445 | Ge 4/4 | GE4-2MS | Sí | N/A | N/A | 2 | 4 | mfx/DCC | Sí | No | Descontinuada | NF | [Märklin archive](https://static.maerklin.de/) |
| LGB | 28446 | Ge 4/4 Glacier Express | GE4-2MS | Sí | N/A | N/A | 2 | 4 | mfx/DCC | Sí | No | Descontinuada | NF | [Publicaciones](https://www.lgb-trains.com/lib/articles/en/items.html) |
| LGB | 22061 | Ge 6/6 II | GE6-2M | Lote | N/A | N/A | 2 | 6 | mfx/DCC/MTS | Sí/lote | NF | Lote | NF | [Publicaciones](https://www.lgb-trains.com/lib/articles/en/items.html) |
| LGB | 22062 | Ge 6/6 II | GE6-2M | Sí | N/A | N/A | 2 | 6 | mfx/DCC | Sí | NF | Descontinuada | NF | [Publicaciones](https://www.lgb-trains.com/lib/articles/en/items.html) |
| LGB | 26600 | Ge 6/6 I | GE6-2M | Sí | N/A | N/A | 2 | 6 | mfx/DCC | Sí | NF | Colección | NF | [Publicaciones](https://www.lgb-trains.com/lib/articles/en/items.html) |
| LGB | 25554 | Alco DL535 | ALCO-2M | Sí, MTS | No | Sí, recableado | 2 | 4 | LGB onboard | Sí | No | Descontinuada | NF | [Despiece](https://static.maerklin.de/damcontent/37/a2/37a219f07ca5a30e6be16636a2d0bd5a1434543447.pdf) |
| LGB | 25555 | Alco DL535 SP | ALCO-2M | Sí | N/A | N/A | 2 | 4 | mfx/DCC | Sí | No | Descontinuada | NF | [Typenverzeichnis](https://champex-linden.de/alco-dl535-e) |
| LGB | 25556 | Alco DL535 ON Rail | ALCO-2M | Sí | N/A | N/A | 2 | 4 | mfx/DCC | Sí | No | Descontinuada | NF | [Typenverzeichnis](https://champex-linden.de/alco-dl535-e) |
| LGB | 25558 | Alco DL535 SP | ALCO-2M | Sí | N/A | N/A | 2 | 4 | mfx/DCC | Sí | No | Descontinuada | NF | [Typenverzeichnis](https://champex-linden.de/alco-dl535-e) |
| LGB | 21671 | Limpiavías | TRACKCLEAN | Sí | N/A | N/A | 1+aux | 2 | mfx/DCC | Sí | No | 2021/baja disponibilidad | 600 mm | [Manual](https://www.manualslib.com/manual/3226112/Lgb-21671.html) |
| LGB | 21672 | Limpiavías USA | TRACKCLEAN | Sí | N/A | N/A | 1+aux | 2 | mfx/DCC | Sí | No | 2021/baja disponibilidad | 600 mm | [Catálogo](https://www.gaugemasterretail.com/media/downloads/LGB_New_2021.pdf) |
| LGB | 20852 | Mallet DR | MALLET-2M | Sí, MTS | N/A | Recableado | 2 | 4 | LGB onboard | Sí | Sí | Descontinuada | NF | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index.html) |
| LGB | 26851 | Mallet CFV | MALLET-2M | Lote | N/A | Recableado | 2 | 4 | MTS/mfx | Sí | Sí | Descontinuada | NF | [LGB manuals](https://www.lgb-trains.com/lib/manuals/en/index.html) |

## Decoders alternativos

| Decoder | Datos confirmados | Alcance |
|---|---|---|
| LGB 55029 | Motor <4 A; AUX1–4 0,9 A; AUX5–6 1,75 A; mtc27; buffer 55429 | Directo solo en referencias de la lista mtc27; otras requieren adaptador. [Manual](https://www.modellbauland.ch/data/dokumente/00055617/LGB%2055029%20Anleitung.pdf) |
| ESU LokSound 5 XL | 4 A continuo, 5 A máximo, DCC/MM/SX/M4, 12 salidas, 4 servos, PowerPack, dos Bühler/Mabuchi | Candidato principal para 2M; recableado, no universal. [Ficha](https://www.esu.eu/produkte/loksound/loksound-5-xl/) |
| Massoth XLS-M1 | Dos motores, máximo 3 A, 7–24 V, buffer, 8 funciones, servo | Interfaz M1; manual alemán/inglés contradice 3 A frente a 1,5 A. [Manual](https://www.massoth.de/wp-content/uploads/2025/05/991073_BDA_XLSM1_250525.pdf) |
| Massoth XLS-Onboard 8216100 | Sustituye Onboard LGB y reutiliza conexiones | No universal; corriente NF. [Ficha](https://www.massoth.de/en/news20260817/) |
| ZIMO MS990 | 6 A conjunto, 10 A pico según tabla G | 6 A incluye motor, sonido y funciones; cableado. [Ficha](https://www.zimo.at/web2010/products/ms-sound-decoder-grossbahn.htm) |
| PIKO 36500 | 5 A continuo motor, DCC/mfx/MM | Candidato cableado; compatibilidad LGB no demostrada. [Manual/producto](https://www.piko-shop.de/de/artikel/g-piko-smartdecoder-gartenbahn-33963.html) |
| Digitrax DG583S | 5 A total motor+funciones | Candidato recableado. [Selector](https://www.digitrax.com/products/engine-matrix/decoder/tables/) |
| SoundTraxx TSU4400 | 7–27 V; 4 A máximo de bloqueo; 6 salidas de 400 mA; audio 3 W/8 ohm | Conversión cableada; otra cantidad de salidas aparece en hoja de precios. [Ficha](https://soundtraxx.com/products/tsunami2-digital-sound-decoders/tsu-4400/) |
| TCS G8 | 4/8 A según condición de ficha; ocho funciones de 500 mA | Candidato sin prueba LGB directa. [Ficha](https://drupal.tcsdcc.com/1303) |
| Train-Li | No se encontró decoder propio | Repositorio de manuales, no fabricante alternativo. [Guías](https://www.train-li-usa.com/page/lgb-user-guide-manual) |

## EX-CSB1

EX-CSB1 acepta 12–25 V y ofrece dos salidas DCC/PWM de hasta 5 A pico por salida, con protección térmica y de sobrecorriente; no se debe diseñar como 5 A continuos garantizados. Toda locomotora que documente DCC, incluida mfx/DCC, funciona por DCC con dirección y pasos. mfx no es necesario para moverla.

La vía de programación está normalmente limitada a 250 mA. PROGBOOST elimina temporalmente el límite; ACK ON y R diagnostican lectura. El ACK esperado es aproximadamente 60 mA durante 6 ms ±1 ms. Un buffer puede impedir la lectura inicial. Fuentes: [EX-CSB1](https://dcc-ex.com/ex-csb1/index.html), [diagnóstico ACK](https://dcc-ex.com/reference/tools/diagnostic-d-ack-command.html).

Medir una locomotora por distrito con motor, sonido, humo y accesorios activos; añadir booster cuando la suma real se acerque al límite. No unir salidas para crear 10 A en una vía. La central no proporciona posición absoluta: automatización y parada requieren sensores externos.

## Evaluación

| Familia | DCC | Baja velocidad | Tracción | Continua | Automatización | Juicio |
|---|---|---|---|---|---|---|
| 1M-OLD | Conversión | Depende de desgaste | Baja-moderada | Tras revisión | Buena con decoder | Aceptable con modificaciones |
| 1M-DIRECT | Decoder enchufable | Buena | Moderada | Buena | Buena | Recomendable |
| 1M-SOUND | DCC directo | Buena | Moderada | Buena, vigilar OEM | Buena | Recomendable |
| Mogul | Variable por lote | Buena | Moderada-alta | Buena si caja/tender limpios | Buena | Recomendable |
| HG3/3 | Ready/factory | Buena pero cremallera delicada | Moderada | Media | Buena | Aceptable con modificaciones |
| E10 2M | Muy buena | Muy buena | Alta | Buena | Muy buena | Muy recomendable |
| GE4 2M | Muy buena | Muy buena | Alta | Muy buena | Buena | Muy recomendable |
| GE6 2M | DCC moderno | Muy buena | Muy alta | Buena con temperatura | Buena | Recomendable |
| Alco 2M | Directa solo modernas | Buena | Alta | Buena | Buena | Recomendable |
| Limpiavías | Directa | Buena | No es plataforma de arrastre | Auxiliar añade desgaste | Compleja | Aceptable con modificaciones |
| Mallet 2M | Variable | Media | Alta | Media por bielas | Buena | Aceptable con modificaciones |

## Rankings

A, económicas para comenzar: 20214 Stainz; 20750 E10 si se desea fuerza; 20280/20282/20283 Mogul verificando lote; 20752/20753 si se prefiere DCC/mfx y humo instalados. Precio y consumo deben confirmarse en cada unidad usada.

B, fuerza: GE6/6 26600/22062; GE4/4 28443/28445/28446 o 21430/21431; Alco 25554–58; E10 20750.

C, operación prolongada: GE4/4 2M con funciones delicadas desconectables; E10 20750 con XL/XLS medido; Alco 25554–58; Stainz 20214 para carga ligera.

## Vacíos y contradicciones

22061/22065 muestran E345896 frente a E274352 en fuentes distintas. XLS-M1 muestra 3 A en alemán y 1,5 A en inglés. ESU XL distingue 5 A máximo de 4 A continuo. TSU4400 tiene discrepancia de salidas entre ficha y hoja de precios. No se localizaron corrientes LGB de marcha/bloqueo, fuerza, temperatura ni radios para la mayoría; se descartan las cifras genéricas del borrador anterior.

20275 tiene observaciones de usuario sobre holgura, captación y eje delantero; son de una unidad. [Informe](https://www.ig1.de/allerlei/36-andere-modellbahn-anlagen/251-lgb-2m-teil-iii-zahnradstrecke-sowie-einige-tipps-und-tricks). Un taller describe rehabilitación de una 20280 y cambio a MSD3; demuestra mantenimiento posible, no vida útil certificada. [Caso](https://www.gscalecentral.net/threads/considerations-when-buying-a-new-loco-lgb-piko.317763/page-2).

20301, 20510 y 2070D/2073D quedan pendientes por falta de despiece y revisión de interfaz verificable. No se incluyen como plataformas aptas.

**Conclusión:** comenzar con 20214 o 20750 y un decoder de escala G con corriente publicada y medidas propias. Para tracción, probar GE4/GE6 y Alco en distritos separados. Las mfx/DCC son compatibles con EX-CSB1 por DCC; identificación automática y funciones exclusivas mfx no son requisitos.
