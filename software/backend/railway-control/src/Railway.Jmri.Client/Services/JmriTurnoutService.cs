using System.Text.Json;
using Railway.Core.Domain;
using Railway.Jmri.Client.Parsing;

namespace Railway.Jmri.Client.Services;

public class JmriTurnoutService
{
    private readonly IJmriConnection _client;
    private static long _nextRequestId;

    public JmriTurnoutService(IJmriConnection client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _client = client;
    }

    public async Task SetTurnoutPosition(string turnoutId, TurnoutPosition position, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(turnoutId);

        var state = position switch
        {
            TurnoutPosition.Closed => 2,
            TurnoutPosition.Thrown => 4,
            _ => throw new ArgumentOutOfRangeException(
                nameof(position),
                "Solo se puede solicitar Closed o Thrown.")
        };

        var requestId = Interlocked.Increment(ref _nextRequestId);
        var confirmation = new TaskCompletionSource<TurnoutPosition>(TaskCreationOptions.RunContinuationsAsynchronously);

        void OnMessageReceived(string message)
        {
            if (!JmriTurnoutEventParser.TryParse(
                message, turnoutId, out var reportedPosition))
                return;

            using var document = JsonDocument.Parse(message);
            var root = document.RootElement;

            if (!root.TryGetProperty("id", out var id) ||
                id.ValueKind != JsonValueKind.Number ||
                !id.TryGetInt64(out var responseId) ||
                responseId != requestId)
                return;

            confirmation.TrySetResult(reportedPosition);

        }

        void OnDisconnected(string reason)
        {
            confirmation.TrySetException(
                new IOException($"Conexion perdida: {reason}"));
        }

        _client.MessageReceived += OnMessageReceived;
        _client.Disconnected += OnDisconnected;

        try
        {
            var message = JsonSerializer.Serialize(new
            {
                id = requestId,
                type = "turnout",
                method = "post",
                data = new { name = turnoutId, state }
            });
            await _client.SendAsync(message, cancellationToken);

            var reportedPosition = await confirmation.Task.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);

            if (reportedPosition != position)
            {
                throw new InvalidOperationException(
                    $"Se solicito {position}, pero JMRI reporto {reportedPosition}.");
            }
        }
        finally
        {
            _client.MessageReceived -= OnMessageReceived;
            _client.Disconnected -= OnDisconnected;
        }

    }
}