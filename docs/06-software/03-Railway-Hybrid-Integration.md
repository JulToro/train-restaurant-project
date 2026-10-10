# Integración híbrida — DCC por riel, batería y accesorios

## 1. Principio

La aplicación no debe depender de la fuente de energía del tren. Un tren puede ser DCC por riel o batería; para Traffic Service ambos tienen rutas, límites y estados equivalentes.

```text
Traffic Service
   ├─ DccTrainController       → JMRI → DCC-EX/CSB1 → riel → decoder
   └─ BatteryTrainController   → red → receptor/ESP32 → motor a batería
```

## 2. Sensores

La detección de ocupación debe ser independiente de la alimentación del tren. Se priorizan reed/Hall, ópticos o RFID sobre medición exclusiva de consumo en riel.

Arquitectura inicial propuesta:

```text
Imán en tren → sensor reed/Hall fijo → ESP32 → Wi-Fi/MQTT → Traffic Service
```

El ESP32 puede publicar estado de sensor y health. JMRI puede recibir/representar esos estados mediante el adaptador elegido; no es obligatorio que cada sensor pase físicamente por CSB1.

## 3. Desvíos con servo

El PCA9685 es un expansor PWM, no un controlador de red autónomo. El ESP32 recibe una orden y controla el PCA9685 por I²C; el PCA9685 genera la señal del servo.

```text
Orden T1 = THROWN → ESP32 → I²C → PCA9685 → servo → desvío
```

Queda pendiente elegir el protocolo del ESP32: MQTT recomendado para telemetría/control de accesorios, o una integración compatible con DCC accesorios.

## 4. Parada de emergencia

La parada de emergencia tiene dos planos:

| Plano | Acción |
|---|---|
| Lógico | Traffic Service ordena `Stop` al DCC/CSB1 y a cada controlador de tren a batería. |
| Físico futuro | Un pulsador cableado corta o inhibe la potencia/enable de ambos sistemas sin depender de Wi-Fi, JMRI o .NET. |

Apagar la potencia DCC no detiene por sí mismo un tren alimentado por batería.

## 5. Restricción eléctrica

Un tren a batería no debe unir su alimentación de motor con el DCC presente en los rieles. Sus tomas de riel deben estar aisladas/desconectadas o gestionadas mediante una conmutación diseñada para ese propósito.

## 6. Contratos de software

```csharp
public interface ITrainController
{
    Task SetSpeedAsync(TrainId trainId, int speed);
    Task StopAsync(TrainId trainId);
    Task SetDirectionAsync(TrainId trainId, Direction direction);
}

public interface IAccessoryController
{
    Task SetTurnoutAsync(TurnoutId turnoutId, TurnoutPosition position);
    IAsyncEnumerable<AccessoryEvent> SubscribeAsync();
}
```

La implementación DCC y la implementación batería/ESP32 son intercambiables para el caso de uso de tráfico.
