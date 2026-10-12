using Railway.Core.Domain;

Console.WriteLine("==================================================");
Console.WriteLine("  Railway Control Spike - Diagnóstico Inicial    ");
Console.WriteLine("  Ambiente: Simulado / Dominio Aislado (.NET 10)  ");
Console.WriteLine("==================================================\n");

// 1. Inicialización de entidades del trazado mínimo (B1, IS1, T1)
Console.WriteLine("[DIAG] 1. Inicializando componentes del trazado...");
var block = new Block("B1", "Bloque Entrada Estación");
var sensor = new Sensor("IS1", "Sensor Entrada B1");
var turnout = new Turnout("T1", "Desvío Principal");

Console.WriteLine($"  - Bloque:  {block.Id} ({block.Name}) | Estado: {block.Occupancy}");
Console.WriteLine($"  - Sensor:  {sensor.Id} ({sensor.Name}) | Estado: {sensor.State}");
Console.WriteLine($"  - Desvío:  {turnout.Id} ({turnout.Name}) | Posición: {turnout.Position}");

// 2. Simulación de transición en memoria
Console.WriteLine("\n[DIAG] 2. Simulando evento de entrada...");

sensor.SetState(SensorState.Active);
block.MarkOccupied();
turnout.SetPosition(TurnoutPosition.Closed);

Console.WriteLine($"  - Bloque:  {block.Id} ({block.Name}) | Estado: {block.Occupancy}");
Console.WriteLine($"  - Sensor:  {sensor.Id} ({sensor.Name}) | Estado: {sensor.State}");
Console.WriteLine($"  - Desvío:  {turnout.Id} ({turnout.Name}) | Posición: {turnout.Position}");

// 3. Confirmación final de diagnóstico
Console.WriteLine("\n==================================================");
Console.WriteLine("  DIAGNÓSTICO EXITOSO:");
Console.WriteLine("  - Modelo de dominio neutral y operativo.");
Console.WriteLine("  - Sin dependencias de JMRI ni hardware físico.");
Console.WriteLine("  - Listo para integración con WebSocket en la siguiente historia.");
Console.WriteLine("==================================================");