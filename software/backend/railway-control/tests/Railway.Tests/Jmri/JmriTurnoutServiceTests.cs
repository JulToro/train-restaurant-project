using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Nodes;
using Railway.Core.Domain;
using Railway.Jmri.Client;
using Railway.Jmri.Client.Services;
using Xunit;

namespace Railway.Tests.Jmri;

public class JmriTurnoutServiceTests
{
    private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(15);

    [Fact]
    public void Constructor_RejectsNullConnection()
    {
        Assert.Throws<ArgumentNullException>(() => new JmriTurnoutService(null!));
    }

    [Theory]
    [InlineData(TurnoutPosition.Closed, 2)]
    [InlineData(TurnoutPosition.Thrown, 4)]
    public async Task SetTurnoutPosition_SendsCommandAndAcceptsImmediateConfirmation(
        TurnoutPosition position, int state)
    {
        var connection = new FakeConnection();
        var service = new JmriTurnoutService(connection);
        connection.OnSend = (message, _) =>
        {
            using var document = JsonDocument.Parse(message);
            var command = document.RootElement;
            Assert.Equal("turnout", command.GetProperty("type").GetString());
            Assert.Equal("post", command.GetProperty("method").GetString());
            Assert.Equal("I2T1", command.GetProperty("data").GetProperty("name").GetString());
            Assert.Equal(state, command.GetProperty("data").GetProperty("state").GetInt32());
            Assert.True(command.GetProperty("id").GetInt64() > 0);
            Assert.Equal(1, connection.MessageSubscriberCount);
            Assert.Equal(1, connection.DisconnectSubscriberCount);
            connection.Receive(Response(message, state));
            return Task.CompletedTask;
        };

        await service.SetTurnoutPosition("I2T1", position, CancellationToken.None).WaitAsync(TestTimeout);

        Assert.Single(connection.SentMessages);
        AssertNoSubscribers(connection);
    }

    [Theory]
    [InlineData(4, "Thrown")]
    [InlineData(0, "Unknown")]
    public async Task SetTurnoutPosition_RejectsDifferentOrUnknownPosition(int state, string reported)
    {
        var connection = new FakeConnection();
        var service = new JmriTurnoutService(connection);
        connection.OnSend = (message, _) =>
        {
            connection.Receive(Response(message, state));
            return Task.CompletedTask;
        };

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.SetTurnoutPosition("I2T1", TurnoutPosition.Closed, CancellationToken.None).WaitAsync(TestTimeout));

        Assert.Contains("Closed", error.Message);
        Assert.Contains(reported, error.Message);
        AssertNoSubscribers(connection);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetTurnoutPosition_ReportsCorrelatedJmriError(bool includeMessage)
    {
        var connection = new FakeConnection();
        var service = new JmriTurnoutService(connection);
        connection.OnSend = (message, _) =>
        {
            var response = JsonNode.Parse(message)!.AsObject();
            response["type"] = "error";
            var data = new JsonObject { ["code"] = 404 };
            if (includeMessage)
                data["message"] = "Turnout not found";
            response["data"] = data;
            connection.Receive(response.ToJsonString());
            return Task.CompletedTask;
        };

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.SetTurnoutPosition("I2T1", TurnoutPosition.Closed, CancellationToken.None).WaitAsync(TestTimeout));

        Assert.Contains("Error de JMRI", error.Message);
        Assert.Contains(includeMessage ? "Turnout not found" : "404", error.Message);
        AssertNoSubscribers(connection);
    }

    [Theory]
    [InlineData("invalid-json")]
    [InlineData("array")]
    [InlineData("missing-id")]
    [InlineData("string-id")]
    [InlineData("different-id")]
    [InlineData("different-turnout")]
    [InlineData("different-type")]
    [InlineData("unrelated-error")]
    [InlineData("missing-state")]
    public async Task SetTurnoutPosition_IgnoresUnrelatedMessagesUntilMatchingResponse(string scenario)
    {
        var connection = new FakeConnection();
        var service = new JmriTurnoutService(connection);
        var operation = service.SetTurnoutPosition("I2T1", TurnoutPosition.Closed, CancellationToken.None);
        var request = Assert.Single(connection.SentMessages);
        var response = JsonNode.Parse(Response(request, 2))!.AsObject();

        switch (scenario)
        {
            case "missing-id": response.Remove("id"); break;
            case "string-id": response["id"] = "invalid"; break;
            case "different-id": response["id"] = response["id"]!.GetValue<long>() + 1; break;
            case "different-turnout": response["data"]!["name"] = "I2T2"; break;
            case "different-type": response["type"] = "sensor"; break;
            case "unrelated-error":
                response["id"] = response["id"]!.GetValue<long>() + 1;
                response["type"] = "error";
                response["data"] = new JsonObject { ["code"] = 500, ["message"] = "Other request failed" };
                break;
            case "missing-state": response["data"]!.AsObject().Remove("state"); break;
        }

        connection.Receive(scenario switch
        {
            "invalid-json" => "not json",
            "array" => "[]",
            _ => response.ToJsonString()
        });
        Assert.False(operation.IsCompleted);

        connection.Receive(Response(request, 2));
        await operation.WaitAsync(TestTimeout);
        AssertNoSubscribers(connection);
    }

    [Fact]
    public async Task SetTurnoutPosition_ReportsDisconnectionWhileWaiting()
    {
        var connection = new FakeConnection();
        var service = new JmriTurnoutService(connection);
        var operation = service.SetTurnoutPosition("I2T1", TurnoutPosition.Closed, CancellationToken.None);

        connection.LoseConnection("JMRI stopped");
        var error = await Assert.ThrowsAsync<IOException>(() => operation.WaitAsync(TestTimeout));

        Assert.Contains("JMRI stopped", error.Message);
        AssertNoSubscribers(connection);
    }

    [Fact]
    public async Task SetTurnoutPosition_CancelsPendingConfirmation()
    {
        var connection = new FakeConnection();
        var service = new JmriTurnoutService(connection);
        using var cancellation = new CancellationTokenSource();
        var operation = service.SetTurnoutPosition("I2T1", TurnoutPosition.Closed, cancellation.Token);

        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => operation.WaitAsync(TestTimeout));

        AssertNoSubscribers(connection);
    }

    [Fact]
    public async Task SetTurnoutPosition_PropagatesSendFailureAndRemovesSubscribers()
    {
        var connection = new FakeConnection();
        var service = new JmriTurnoutService(connection);
        var failure = new WebSocketException("Send failed");
        connection.OnSend = (_, _) => Task.FromException(failure);

        var error = await Assert.ThrowsAsync<WebSocketException>(() =>
            service.SetTurnoutPosition("I2T1", TurnoutPosition.Closed, CancellationToken.None).WaitAsync(TestTimeout));

        Assert.Same(failure, error);
        AssertNoSubscribers(connection);
    }

    [Fact]
    public async Task SetTurnoutPosition_TimesOutWithoutConfirmation()
    {
        var connection = new FakeConnection();
        var service = new JmriTurnoutService(connection);

        await Assert.ThrowsAsync<TimeoutException>(() =>
            service.SetTurnoutPosition("I2T1", TurnoutPosition.Closed, CancellationToken.None).WaitAsync(TestTimeout));

        AssertNoSubscribers(connection);
    }

    [Fact]
    public async Task SetTurnoutPosition_CorrelatesConcurrentOrdersIndependently()
    {
        var connection = new FakeConnection();
        var service = new JmriTurnoutService(connection);
        var first = service.SetTurnoutPosition("I2T1", TurnoutPosition.Closed, CancellationToken.None);
        var second = service.SetTurnoutPosition("I2T2", TurnoutPosition.Thrown, CancellationToken.None);
        using var firstDocument = JsonDocument.Parse(connection.SentMessages[0]);
        using var secondDocument = JsonDocument.Parse(connection.SentMessages[1]);
        Assert.NotEqual(firstDocument.RootElement.GetProperty("id").GetInt64(),
            secondDocument.RootElement.GetProperty("id").GetInt64());

        connection.Receive(Response(connection.SentMessages[1], 4, "I2T2"));
        await second.WaitAsync(TestTimeout);
        Assert.False(first.IsCompleted);
        Assert.Equal(1, connection.MessageSubscriberCount);

        connection.Receive(Response(connection.SentMessages[0], 2));
        await first.WaitAsync(TestTimeout);
        AssertNoSubscribers(connection);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SetTurnoutPosition_RejectsEmptyIdentifierWithoutSending(string? turnoutId)
    {
        var connection = new FakeConnection();
        var service = new JmriTurnoutService(connection);

        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            service.SetTurnoutPosition(turnoutId!, TurnoutPosition.Closed, CancellationToken.None));

        Assert.Empty(connection.SentMessages);
        AssertNoSubscribers(connection);
    }

    [Fact]
    public async Task SetTurnoutPosition_RejectsUnknownPositionWithoutSending()
    {
        var connection = new FakeConnection();
        var service = new JmriTurnoutService(connection);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            service.SetTurnoutPosition("I2T1", TurnoutPosition.Unknown, CancellationToken.None));

        Assert.Empty(connection.SentMessages);
        AssertNoSubscribers(connection);
    }

    private static string Response(string request, int state, string turnoutId = "I2T1")
    {
        using var document = JsonDocument.Parse(request);
        return JsonSerializer.Serialize(new
        {
            id = document.RootElement.GetProperty("id").GetInt64(),
            type = "turnout",
            data = new { name = turnoutId, state }
        });
    }

    private static void AssertNoSubscribers(FakeConnection connection)
    {
        Assert.Equal(0, connection.MessageSubscriberCount);
        Assert.Equal(0, connection.DisconnectSubscriberCount);
    }

    private sealed class FakeConnection : IJmriConnection
    {
        public JmriConnectionState State { get; private set; } = JmriConnectionState.Connected;
        public event Action? Connected { add { } remove { } }
        public event Action<int, TimeSpan>? Reconnecting { add { } remove { } }
        public event Action<string>? Disconnected;
        public event Action<string>? MessageReceived;
        public List<string> SentMessages { get; } = new();
        public Func<string, CancellationToken, Task>? OnSend { get; set; }
        public int MessageSubscriberCount => MessageReceived?.GetInvocationList().Length ?? 0;
        public int DisconnectSubscriberCount => Disconnected?.GetInvocationList().Length ?? 0;

        public Task SendAsync(string message, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            SentMessages.Add(message);
            return OnSend?.Invoke(message, cancellationToken) ?? Task.CompletedTask;
        }

        public void Receive(string message) => MessageReceived?.Invoke(message);

        public void LoseConnection(string reason)
        {
            State = JmriConnectionState.Disconnected;
            Disconnected?.Invoke(reason);
        }

        public Task ConnectAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task DisconnectAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
