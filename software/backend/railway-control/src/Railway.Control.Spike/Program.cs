using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Railway.Jmri.Client;
using Railway.Core.Domain;
using Railway.Core.Services;

Console.WriteLine("==================================================");
Console.WriteLine("  Railway Control Spike - Diagnóstico Inicial    ");
Console.WriteLine("  Ambiente: Simulado / Dominio Aislado (.NET 10)  ");
Console.WriteLine("==================================================\n");

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var section = configuration.GetRequiredSection("Jmri");

var options = new JmriClientOptions
{
    WebSocketUrl = section["WebSocketUrl"]
        ?? throw new InvalidOperationException("Falta Jmri:WebSocketUrl."),

    ReconnectIntervalSeconds = int.Parse(
        section["ReconnectIntervalSeconds"]
        ?? throw new InvalidOperationException(
            "Falta Jmri:ReconnectIntervalSeconds.")),

    MaxReconnectAttempts = int.Parse(
        section["MaxReconnectAttempts"]
        ?? throw new InvalidOperationException(
            "Falta Jmri:MaxReconnectAttempts.")),

    HeartbeatIntervalSeconds = int.Parse(
        section["HeartbeatIntervalSeconds"]
        ?? throw new InvalidOperationException(
            "Falta Jmri:HeartbeatIntervalSeconds."))
};

// 1. Crear las entidades.
var sensor = new Sensor("I2S1", "Sensor de ocupacion B1");
var block = new Block("B1", "Bloque de entrada");


// 2. Crear el cliente
await using var client = new JmriWebSocketClient(options);

client.MessageReceived += message =>
    UpdateBlockOccupancy(message, sensor, block);

client.Connected += () =>
{
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Conectado a JMRI.");

    try
    {
        var subscription = JsonSerializer.Serialize(new
        {
            type = "sensor",
            method = "get",
            data = new { name = sensor.Id }
        });

        // Connected se ejecuta en la cola de notificaciones, independiente de la recepcion.
        client.SendAsync(subscription).GetAwaiter().GetResult();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Solicitud de suscripcion enviada: {sensor.Id}.");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(
            $"[{DateTime.Now:HH:mm:ss}] No se pudo enviar la suscripcion a {sensor.Id}: {ex.Message}");
    }
};


client.Disconnected += reason => Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Desconectado: {reason}");
client.Reconnecting += (attempt, delay) =>
    Console.WriteLine(
        $"[{DateTime.Now:HH:mm:ss}] Reconexion: intento {attempt}, " +
        $"espera de {delay.TotalSeconds} segundos.");

try
{
    await client.ConnectAsync();

    Console.WriteLine("Presiona Enter para salir.");
    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine($"No se pudo establecer la conexion con JMRI: {ex.Message}");
}

static void UpdateBlockOccupancy(string message, Sensor sensor, Block block)
{
    using var document = JsonDocument.Parse(message);
    var root = document.RootElement;

    if(root.ValueKind != JsonValueKind.Object || 
       !root.TryGetProperty("type", out var type) ||
       type.GetString() != "sensor"
       )
        return;

    var data = root.GetProperty("data");

    if(data.GetProperty("name").GetString() != sensor.Id)
        return;
    
    var state = data.GetProperty("state").GetInt32() switch
    {
        2 => SensorState.Active,
        4 => SensorState.Inactive,
        _ => SensorState.Unknown
    };

    if(BlockOccupancyUpdater.Apply(sensor, block, state))
    {
        Console.WriteLine(
            $"[{DateTime.Now:HH:mm:ss}] {sensor.Id}: {sensor.State}" +
            $" -> {block.Id}: {block.Occupancy}");
    }
}


