using Railway.Jmri.Client;
using Microsoft.Extensions.Configuration;


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


await using var client = new JmriWebSocketClient(options);

client.MessageReceived += message =>
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] JMRI: {message}");

client.Connected += () => Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Conectado a JMRI.");
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


