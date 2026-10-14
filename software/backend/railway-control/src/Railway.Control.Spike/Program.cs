using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Railway.Jmri.Client;
using Railway.Core.Domain;
using Railway.Core.Services;
using Railway.Jmri.Client.Parsing;
using Railway.Jmri.Client.Services;

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
var turnout = new Turnout("I2T1", "Desvio principal");

// 2. Crear el cliente
await using var client = new JmriWebSocketClient(options);

client.MessageReceived += message =>
{
    UpdateBlockOccupancy(message, sensor, block);

    if (JmriTurnoutEventParser.TryParse(message, turnout.Id, out var position))
    {
        turnout.SetPosition(position);
        Console.WriteLine($"Turnout {turnout.Id} state: {position}");
    }
};

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


        var turnoutSubscription = JsonSerializer.Serialize(new
        {
            type = "turnout",
            method = "get",
            data = new { name = turnout.Id }
        });
        client.SendAsync(turnoutSubscription).GetAwaiter().GetResult();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Solicitud de suscripcion enviada: {turnout.Id}.");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(
            $"[{DateTime.Now:HH:mm:ss}] No se pudo enviar suscripciones: {ex.Message}");
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

    var turnoutService = new JmriTurnoutService(client);

    await ConsoleMenuAsync(turnoutService, turnout.Id);
}
catch (Exception ex)
{
    Console.WriteLine($"No se pudo establecer la conexion con JMRI: {ex.Message}");
}

static void UpdateBlockOccupancy(string message, Sensor sensor, Block block)
{
    if (!JmriSensorEventParser.TryParse(message, sensor.Id, out var state))
        return;

    if (BlockOccupancyUpdater.Apply(sensor, block, state))
    {
        Console.WriteLine(
            $"[{DateTime.Now:HH:mm:ss}] {sensor.Id}: {sensor.State}" +
            $" -> {block.Id}: {block.Occupancy}");
    }
}


static async Task ConsoleMenuAsync(JmriTurnoutService turnoutService, string turnoutId)
{
    var requestedPosition = TurnoutPosition.Unknown;
    while (true)
    {
        Console.WriteLine("=== Menú de Control de Ferrocarril ===");
        Console.WriteLine("1. CLOSED");
        Console.WriteLine("2. THROWN");
        Console.WriteLine("0. Salir");
        Console.Write("Seleccione una opción: ");
        var option = Console.ReadLine();

        if (option is null or "0")
        { 
            return; 
        }

        if (option is not "1" and not "2")
        {
            Console.WriteLine("Opción no válida.");
            continue;
        }


        switch (option)
        {
            case "1":
                requestedPosition = TurnoutPosition.Closed;
                break;
            case "2":
                requestedPosition = TurnoutPosition.Thrown;
                break;
            default:
                Console.WriteLine("Opción no válida.");
                break;
        }
        
        try
        {
            await turnoutService.SetTurnoutPosition(turnoutId, requestedPosition, CancellationToken.None);
            
            Console.WriteLine($"Posición confirmada por JMRI: {requestedPosition}.");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cambiar la posición del desvío: {ex.Message}");
        }

    }
}