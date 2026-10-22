# Resumen del montaje y las pruebas: ESP32, PCA9685 y servo

**Fecha de actualización:** 20 de octubre de 2026  
**Propósito:** dejar constancia del inventario, la instalación del entorno Arduino, el cableado y las pruebas hechas hasta ahora.

## 1. Inventario identificado

| Elemento | Identificación y uso |
|---|---|
| ESP32 | Placa de desarrollo con módulo ESP32-D; se programa por USB desde Arduino IDE. |
| PCA9685 HW-170 | Controlador PWM I²C de 16 canales y 12 bits. La dirección probada fue `0x40`. |
| Servo | Microservo MG90S de **rotación continua**, con señal (naranja/amarillo), positivo (rojo) y tierra (marrón/café). La prueba confirmó que funciona con la PCA9685. |
| Fuente de laboratorio | Fuente DC programable NANKADF. Se ajustó a 5.00 V y límite de corriente de 1.000 A. |
| Multímetro | Redline PM8232, usado para comprobar voltaje DC. |
| Protoboard y jumpers | Para sujetar la ESP32 y llevar señales a la PCA9685. |
| Otros módulos en el inventario | Driver de motores TC1508A (salidas Motor-A/Motor-B e INT/DR) y dos sensores: un módulo HW-477 V0.2, probablemente de efecto Hall, y un interruptor magnético de vidrio tipo reed. No se probaron en esta sesión. |

## 2. Arduino IDE y configuración inicial

1. Se descargó e instaló **Arduino IDE** desde la página oficial de Arduino: [arduino.cc/en/software](https://www.arduino.cc/en/software).
2. En Arduino IDE se instaló el soporte de placas **ESP32 de Espressif** desde el Gestor de placas.
3. Se seleccionó la placa **ESP32 Dev Module** y el puerto serial correspondiente a la ESP32 conectada por USB.
4. Se usó **115200 baudios** para el Monitor Serie.
5. Arduino IDE es la aplicación que compila y carga el firmware a la ESP32. No se convierte en una librería dentro de .NET ni de JMRI: esas aplicaciones siguen siendo independientes; la ESP32 ejecuta el programa que se le carga.

En el registro de carga apareció el paquete ESP32 Arduino **3.3.12** y el mensaje de reinicio por RTS. La carga terminó con verificación de datos correcta (*Hash of data verified*).

## 3. Librería instalada

Se instaló desde **Sketch → Include Library → Manage Libraries…** buscando:

- **Adafruit PWM Servo Driver Library**, de Adafruit.
- El encabezado que usa el programa es `Adafruit_PWMServoDriver.h`.

Al principio la compilación fallaba con `Adafruit_PWMServoDriver.h: No such file or directory`. Después de instalar/revisar la librería, el programa compiló y se cargó correctamente. El registro de carga confirmó que el firmware se escribió y verificó en la ESP32.

## 4. Cableado realizado

### ESP32 ↔ PCA9685: alimentación lógica y comunicación

| ESP32 | PCA9685 | Función |
|---|---|---|
| GPIO 21 | SDA | Datos I²C |
| GPIO 22 | SCL | Reloj I²C |
| GND | GND | Tierra común |
| 3V3 | VCC | Alimentación lógica del módulo |

Los pines GND de la ESP32 están conectados internamente entre sí; se puede usar cualquiera que esté marcado **GND**. El pin indicado como “38” en la conversación se entiende como la posición/contacto de GND identificado en la placa, **no como GPIO38**.

### Fuente externa ↔ alimentación del servo en la PCA9685

| Fuente de laboratorio | PCA9685 | Función |
|---|---|---|
| Positivo rojo, ajustado a 5.00 V | V+ | Alimentación de los servos |
| Negativo/COM | GND | Retorno y referencia común |

**VCC y V+ tienen funciones distintas:** VCC alimenta la lógica de la placa; V+ alimenta los servos. La tierra de la fuente debe ser común con la tierra de la PCA9685 y la ESP32. No se debe alimentar el servo desde el pin 3V3 de la ESP32.

En las fotos se observó conectado V+ al positivo de la fuente, y GND de la PCA9685 a GND de la ESP32. El multímetro midió aproximadamente **5 V entre V+ y GND** en la placa.

### Servo ↔ canal de la PCA9685

En cada grupo de tres pines, la serigrafía de la placa indica las filas:

- **Amarillo:** señal PWM.
- **Rojo:** V+ / positivo del servo.
- **Negro:** GND / tierra del servo.

Conecta el cable de señal del MG90S a la fila amarilla, el rojo a la fila roja y el marrón/café a la fila negra. El canal 0 es el primer grupo de tres pines del extremo donde comienza la numeración de canales; en la orientación de la placa mostrada, se tomó el grupo de la izquierda como canal 0. El canal 5 sería el sexto grupo.

El pin **OE** quedó conectado con un puente a **GND de la PCA9685** para mantener habilitadas las salidas. OE es activo en bajo. En algunos módulos hay una resistencia que ya lo mantiene bajo, pero en este montaje se dejó conectado a GND para asegurar el estado. El cable naranja de la conexión I²C va a **SCL**, no a OE.

## 5. Pruebas y resultados

### Escáner I²C

Se cargó un programa de escaneo con `Wire.begin(21, 22)`. El Monitor Serie encontró repetidamente el dispositivo en **`0x40`**.

Esto confirma que la ESP32 ve un dispositivo I²C en esa dirección y que hay comunicación básica por SDA/SCL. No confirma por sí solo que las salidas PWM funcionen ni que el servo esté bien.

### Prueba final del servo de rotación continua

La prueba que funcionó inicializó la PCA9685 y alternó pulsos en el canal 0:

```cpp
#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>

Adafruit_PWMServoDriver pca(0x40);

void setup() {
  Serial.begin(115200);
  Wire.begin(21, 22);  // SDA, SCL
  pca.begin();
  pca.setPWMFreq(50);
  delay(500);
}

void loop() {
  Serial.println("Prueba: 1600 us");
  pca.writeMicroseconds(0, 1600);
  delay(2000);

  Serial.println("Parada: 1700 us");
  pca.writeMicroseconds(0, 1700);
  delay(2000);

  Serial.println("Prueba: 1800 us");
  pca.writeMicroseconds(0, 1800);
  delay(2000);

  Serial.println("Parada: 1700 us");
  pca.writeMicroseconds(0, 1700);
  delay(2000);
}
```

El `setup()` utilizado inicializa el Monitor Serie a 115200 baudios, I²C con SDA en GPIO 21 y SCL en GPIO 22, inicia la PCA9685 en la dirección `0x40` y fija la frecuencia en 50 Hz. `writeMicroseconds(canal, pulso)` configura el pulso de control en microsegundos.

**Resultado observado:** el servo se movió con los pulsos de 1600 y 1800 µs y se detuvo con 1700 µs. En un servo de rotación continua, estos valores controlan sentido/velocidad, no una posición angular. El punto de parada de 1700 µs es el valor observado para este servo; puede variar entre unidades.

### Fuente y multímetro

La fuente se ajustó a **5.00 V** con límite de corriente de **1.000 A**. Con la salida activada se observó corriente que normalmente volvía a 0 A y, por momentos, marcaba aproximadamente 0.004–0.012 A. Se comprobó alrededor de **5 V entre V+ y GND**. El límite de 1 A es el máximo permitido por el ajuste, no la corriente que necesariamente está consumiendo el circuito.

Un multímetro conectado a V+ y GND verifica la alimentación, pero no permite ver correctamente los pulsos PWM. El PCA9685 no “detecta” el servo: genera una señal PWM y el servo responde si tiene alimentación y señal correctas.

## 6. Estado actual

La configuración final funciona: la ESP32 detecta la PCA9685 en `0x40`, y el canal 0 controla el servo de rotación continua. La conexión de OE a GND mantiene habilitadas las salidas. También se confirmó que el riel V+ debe recibir alimentación externa para mover el servo; alimentar únicamente la ESP32 por USB no alimenta V+.

La prueba previa de alto/bajo con multímetro no mostró un cambio claro y se repitió con dos módulos, pero eso no demuestra que ambos estuvieran dañados. La prueba funcional posterior, con V+ externo, OE a GND y pulsos apropiados para rotación continua, sí confirmó el funcionamiento del montaje que se probó.

## 7. Seguridad y comprobaciones antes de volver a energizar

1. Apaga **OUT** en la fuente antes de cambiar cualquier cable.
2. Conserva la fuente en **5.00 V** y el límite de corriente en **1.000 A** para esta prueba; no subas el voltaje para intentar forzar el movimiento.
3. Revisa la polaridad y que el conector del servo coincida con las filas señal, V+ y GND de la placa.
4. Asegura que fuente, PCA9685 y ESP32 compartan GND.
5. Vuelve a encender OUT solo después de revisar el cableado.

> Nota: en una foto de la placa se lee “V+ 6V Max”. Aunque el anuncio del producto menciona rangos distintos, para este montaje se está usando 5 V, que queda dentro de esa indicación impresa.
