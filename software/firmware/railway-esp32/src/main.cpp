// Todo lo que aparece después de // es un comentario: no se ejecuta.
// #include permite usar las herramientas de Arduino, como Serial y millis().
#include <Arduino.h>

// constexpr define una constante cuyo valor se conoce al compilar el programa.
// unsigned long es un tipo de número entero sin signo (no guarda negativos).
// El signo = asigna el valor; el punto y coma ; termina la instrucción.
constexpr unsigned long SERIAL_BAUD_RATE = 115200; // Velocidad del puerto serie.
constexpr unsigned long HEARTBEAT_INTERVAL_MS = 5000; // 5000 ms = 5 segundos.

// char representa un carácter. char NOMBRE[] guarda una secuencia de caracteres.
// Las comillas dobles delimitan el texto; [] deja que C++ calcule su tamaño.
// Estos nombres en MAYÚSCULAS son una convención para identificar constantes.
constexpr char DEVICE_ID[] = "railway-esp32-01"; // Identificación de esta placa.
constexpr char FIRMWARE_VERSION[] = "0.1.0"; // Versión de este programa.

// Una variable puede cambiar de valor durante la ejecución.
// Esta guarda cuándo enviamos el último mensaje de actividad, en milisegundos.
// Está fuera de las funciones para conservar su valor entre llamadas a loop().
unsigned long lastHeartbeat = 0;

// Una función agrupa instrucciones. () contiene sus parámetros; aquí no hay ninguno.
// void significa que la función no devuelve un resultado a quien la llama.
// Las llaves { y } delimitan el cuerpo de la función.
// Arduino llama a setup() una sola vez al encender o reiniciar el ESP32.
void setup() {
    // Serial es el objeto que usamos para comunicarnos con el monitor serie.
    // El punto . permite llamar a una de sus funciones, en este caso begin().
    // El valor entre paréntesis indica la velocidad: el monitor debe usar la misma.
    Serial.begin(SERIAL_BAUD_RATE);

    // Pausa el programa 1000 milisegundos para dar un margen al iniciar el puerto.
    delay(1000);

    // println() escribe y después pasa a la siguiente línea.
    Serial.println(); // Sin texto, solo agrega un salto de línea.
    Serial.println("==========================="); // Separador visual.
    Serial.println(" Railway ESP32 Controller"); // Nombre del programa.
    Serial.println("===========================");
    Serial.println("[BOOT] Firmware started"); // Mensaje de arranque.

    // print() escribe sin saltar de línea: el identificador aparecerá a continuación.
    Serial.print("[INFO] Device: ");
    Serial.println(DEVICE_ID); // Escribe el valor de la constante y termina la línea.

    Serial.print("[INFO] Version: ");
    Serial.println(FIRMWARE_VERSION); // Muestra la versión del firmware.

    // Es un mensaje definido por nosotros: indica que llegamos a este punto.
    Serial.println("[INFO] Status: RUNNING");

    // millis() devuelve los milisegundos transcurridos desde el inicio del programa.
    // Guardamos el momento actual para esperar 5 segundos antes del primer latido.
    lastHeartbeat = millis();
}

// Después de setup(), Arduino llama a loop() una y otra vez mientras está encendido.
void loop() {
    // const impide modificar esta variable después de inicializarla.
    // now es una variable local: se crea en cada llamada y solo existe en esta función.
    const unsigned long now = millis();

    // if ejecuta el bloque entre llaves solo si se cumple la condición entre paréntesis.
    // La resta calcula cuánto tiempo ha pasado desde el último mensaje.
    // >= significa "mayor o igual que": aquí preguntamos si ya pasaron 5000 ms.
    // Con unsigned long, esta resta también funciona cuando millis() vuelve a cero.
    if (now - lastHeartbeat >= HEARTBEAT_INTERVAL_MS) {
        // Actualiza el momento del último latido para empezar a contar el siguiente.
        lastHeartbeat = now;

        // Heartbeat significa "latido": permite observar que el bucle sigue ejecutándose.
        // Este mensaje no comprueba la red, los sensores ni los servos.
        Serial.print("[HEARTBEAT] Uptime: ");

        // / divide: convertimos milisegundos a segundos.
        // UL indica que 1000 es unsigned long. La división entera descarta decimales.
        Serial.print(now / 1000UL);
        Serial.println(" seconds"); // Completa el mensaje y pasa a otra línea.
    }

    // Pausa 1 ms y permite que el sistema atienda otras tareas.
    // La espera de 5 segundos se controla con millis(), sin una pausa larga en loop().
    delay(1);
}
