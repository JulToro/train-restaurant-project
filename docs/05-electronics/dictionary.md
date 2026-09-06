# Diccionario DCC y Control Ferroviario

## Términos principales

| Término                           | Significado                                                                                                                                                                      |
| --------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **DCC**                           | **Digital Command Control**. Protocolo digital utilizado en modelismo ferroviario para enviar energía y órdenes a las locomotoras a través de los mismos rieles.                 |
| **Command Station**               | Central de mando del sistema DCC. Recibe órdenes desde un computador, throttle u otro controlador y genera los paquetes digitales DCC que se envían a la vía.                    |
| **DCC-EX**                        | Proyecto open source de software y firmware para construir y controlar una central DCC utilizando hardware compatible.                                                           |
| **EX-CSB1**                       | Hardware de DCC-EX que integra microcontrolador, Command Station, generación de señal DCC, drivers de potencia, USB y Wi-Fi en una sola placa.                                   |
| **Arduino**                       | Plataforma de hardware y software basada en microcontroladores programables. Se utiliza ampliamente para automatización, sensores y control electrónico.                         |
| **Arduino Mega 2560**             | Modelo de Arduino con gran cantidad de entradas y salidas. Puede ejecutar el firmware DCC-EX y funcionar como cerebro de una central DCC construida de forma DIY.                |
| **Microcontrolador**              | Pequeño computador integrado dentro de una placa electrónica. Ejecuta instrucciones y controla sensores, motores, comunicaciones y otros dispositivos.                           |
| **Motor Shield**                  | Placa electrónica de potencia que permite que un microcontrolador controle cargas eléctricas mayores de las que podría manejar directamente.                                     |
| **EX-MotorShield8874**            | Motor Shield diseñado para DCC-EX. Permite generar la señal DCC con suficiente corriente para alimentar la vía y las locomotoras.                                                |
| **Booster**                       | Equipo encargado de aumentar o distribuir la potencia de la señal DCC. Se utiliza cuando una instalación necesita alimentar más trenes o dividir la vía en varios sectores.      |
| **Decoder DCC**                   | Dispositivo electrónico instalado dentro de una locomotora. Recibe las órdenes DCC de los rieles y controla motor, velocidad, dirección, luces, sonido y otras funciones.        |
| **Address / Dirección DCC**       | Número único asignado a una locomotora o accesorio para que la central pueda enviarle órdenes específicas.                                                                       |
| **Throttle**                      | Control utilizado para manejar una locomotora. Permite modificar velocidad, sentido y funciones. Puede ser físico, virtual o estar integrado en una aplicación.                  |
| **CV**                            | **Configuration Variable**. Parámetros configurables dentro de un decoder DCC, como dirección, aceleración, frenado, velocidad máxima, luces y comportamiento del motor.         |
| **MAIN**                          | Vía principal utilizada para circulación normal de trenes.                                                                                                                       |
| **PROG / Programming Track**      | Vía aislada utilizada para programar y leer la configuración de los decoders DCC.                                                                                                |
| **Turnout**                       | Desvío ferroviario que permite cambiar un tren de una vía a otra.                                                                                                                |
| **Turnout Decoder**               | Decoder DCC especializado en controlar desvíos, servos, relés u otros accesorios.                                                                                                |
| **Servo**                         | Pequeño motor de posicionamiento que puede mover un desvío a una posición específica.                                                                                            |
| **Sensor**                        | Dispositivo que detecta eventos físicos, como el paso de un tren, presencia, posición, distancia o estado de una vía.                                                            |
| **Occupancy Detection**           | Sistema que permite detectar si existe una locomotora o vagón ocupando un determinado tramo de vía.                                                                              |
| **Block / Sector**                | Tramo de vía dividido eléctrica o lógicamente para facilitar control, detección de ocupación, seguridad o distribución de potencia.                                              |
| **Power District**                | Sector eléctrico independiente alimentado normalmente por un booster propio. Permite distribuir mejor la corriente y aislar fallos.                                              |
| **Rail Bus / Track Bus**          | Cableado principal que lleva la alimentación y señal DCC desde la central o booster hasta diferentes puntos de la vía.                                                           |
| **Feeder**                        | Cable corto que conecta el Track Bus directamente con los rieles.                                                                                                                |
| **Firmware**                      | Software instalado directamente dentro de un microcontrolador o dispositivo electrónico. DCC-EX puede funcionar como firmware dentro de un Arduino u otro hardware compatible.   |
| **Software**                      | Programa que corre normalmente en un computador, servidor, teléfono o tablet. Ejemplos: JMRI o una aplicación propia en C#.                                                      |
| **JMRI**                          | **Java Model Railroad Interface**. Software open source para controlar, configurar y automatizar ferrocarriles a escala desde un computador.                                     |
| **DecoderPro**                    | Componente de JMRI utilizado principalmente para configurar decoders DCC mediante una interfaz gráfica.                                                                          |
| **PanelPro**                      | Componente de JMRI utilizado para crear paneles visuales, controlar desvíos, sensores, señales y automatizaciones.                                                               |
| **API**                           | **Application Programming Interface**. Interfaz mediante la cual una aplicación puede comunicarse con otro software o dispositivo.                                               |
| **Serial / Puerto Serial**        | Forma de comunicación utilizada frecuentemente entre un computador y un microcontrolador. Una conexión USB puede presentarse al sistema operativo como un puerto serial.         |
| **USB**                           | Interfaz física utilizada para conectar el computador con la central DCC o microcontrolador. USB no es DCC; solo transporta las órdenes entre el PC y la central.                |
| **Wi-Fi**                         | Comunicación inalámbrica que puede utilizarse entre el computador, dispositivos móviles y una central DCC compatible.                                                            |
| **Ethernet**                      | Comunicación de red mediante cable. Puede utilizarse para conectar centrales DCC o controladores a la red del restaurante.                                                       |
| **TCP/IP**                        | Conjunto de protocolos de red utilizados para enviar información entre dispositivos conectados mediante Ethernet o Wi-Fi.                                                        |
| **I/O**                           | **Input / Output**. Entradas y salidas electrónicas utilizadas para conectar sensores, servos, LEDs, relés y otros dispositivos.                                                 |
| **GPIO**                          | **General Purpose Input/Output**. Pines configurables de un microcontrolador que pueden utilizarse como entradas o salidas.                                                      |
| **Driver de motor**               | Circuito electrónico que controla la potencia entregada a un motor. Un Motor Shield incluye uno o varios drivers de motor.                                                       |
| **Current Sense**                 | Medición electrónica de la corriente consumida por una vía, motor o circuito. Puede utilizarse para protección y detección de ocupación.                                         |
| **Stall Current**                 | Corriente máxima que consume un motor cuando recibe energía pero físicamente no puede girar. Es un dato crítico para seleccionar decoder, fuente, booster y cableado.            |
| **Corriente continua**            | Cantidad de corriente que un dispositivo puede manejar de forma sostenida sin sobrecalentarse o dañarse.                                                                         |
| **Peak Current**                  | Corriente máxima que un dispositivo puede soportar durante un período corto.                                                                                                     |
| **Voltaje de vía**                | Voltaje eléctrico aplicado a los rieles. En escala G suele ser mayor que en escalas pequeñas y debe ajustarse según motores, decoders y central utilizados.                      |
| **Fuente de alimentación**        | Equipo que convierte la energía de la red eléctrica en el voltaje DC requerido por la central, booster y demás componentes.                                                      |
| **Short Circuit / Cortocircuito** | Situación en la que los rieles o conductores quedan conectados con muy baja resistencia, provocando una corriente elevada. Las centrales y boosters deben contar con protección. |
| **Auto-Reverser**                 | Dispositivo que detecta y corrige automáticamente problemas de polaridad en bucles de retorno o ciertas configuraciones de vía.                                                  |
| **Accessory Decoder**             | Decoder especializado en controlar accesorios como desvíos, señales, relés o iluminación.                                                                                        |
| **DIY**                           | **Do It Yourself**. Significa construir o ensamblar un sistema por cuenta propia utilizando componentes separados en lugar de comprar un producto completamente terminado.       |
| **Open Source**                   | Software o hardware cuyo diseño o código está disponible públicamente para estudiar, modificar y adaptar.                                                                        |
| **Locomotive Consist**            | Configuración donde dos o más locomotoras trabajan juntas y reciben órdenes coordinadas como si fueran una sola unidad.                                                          |
| **Speed Step**                    | Nivel discreto de velocidad utilizado por DCC. Los sistemas normalmente soportan 14, 28 o 128 pasos de velocidad.                                                                |
| **Function**                      | Función adicional controlada mediante DCC, como luces, sonido, bocina, humo u otros accesorios de la locomotora.                                                                 |
| **F0, F1, F2...**                 | Identificadores utilizados para activar funciones específicas dentro de un decoder DCC.                                                                                          |
| **Route**                         | Secuencia lógica de desvíos, sectores o acciones necesarias para enviar un tren desde un punto hasta otro.                                                                       |
| **Interlocking**                  | Lógica de seguridad que evita configurar rutas incompatibles o permitir que dos trenes entren simultáneamente en un mismo tramo peligroso.                                       |
| **Dispatcher**                    | Sistema o persona encargada de asignar trenes, rutas y movimientos dentro de la red ferroviaria.                                                                                 |
| **Automation**                    | Conjunto de reglas que permiten ejecutar movimientos automáticamente utilizando información de sensores, rutas, estados y condiciones.                                           |

## Relación básica entre los componentes

```text
Computador
    │
    │ USB / Wi-Fi / Ethernet
    ▼
Command Station
    │
    │ Señal DCC + potencia
    ▼
Rieles
    │
    ▼
Decoder DCC
    │
    ▼
Motor / luces / funciones
```

En una instalación automatizada también pueden intervenir:

```text
Sensores
   │
   ▼
Controlador / Command Station
   │
   ▼
Software de control
   │
   ▼
Rutas / desvíos / trenes
```

## Dos formas comunes de construir una central DCC-EX

### Opción integrada

```text
Computador
    │
    ▼
EX-CSB1
    │
    ▼
Rieles
```

El EX-CSB1 ya integra microcontrolador, Command Station y etapa de potencia principal.

### Opción DIY

```text
Computador
    │
    ▼
Arduino Mega 2560
    │
    ▼
EX-MotorShield8874
    │
    ▼
Rieles
```

En este caso, Arduino ejecuta DCC-EX y el Motor Shield proporciona la potencia necesaria para alimentar la vía.

## Conceptos importantes para el proyecto del restaurante

* La cantidad de locomotoras que una central puede gestionar lógicamente no es igual a la cantidad que puede alimentar físicamente.
* La cantidad real de trenes simultáneos dependerá principalmente del consumo eléctrico de cada locomotora.
* En trenes de escala G con dos motores, es importante medir la corriente normal y la corriente de bloqueo.
* Para instalaciones grandes será necesario dividir la vía en sectores eléctricos.
* Los boosters permiten añadir potencia a nuevos sectores.
* Los sensores permitirán conocer la posición real de los trenes.
* El software de control deberá gestionar rutas, ocupación, seguridad, velocidad y estado de cada tren.
