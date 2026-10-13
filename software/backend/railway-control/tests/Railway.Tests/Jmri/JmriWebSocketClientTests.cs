using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Channels;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Railway.Jmri.Client;
using Xunit;

namespace Railway.Tests.Jmri;

public class JmriWebSocketClientTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    [Fact]
    public async Task ThrowingSubscriberDoesNotStopReceptionOrOtherSubscribers()
    {
        await using var server = await TestServer.StartAsync();
        var client = server.CreateClient();
        var messages = new List<string>();
        var received = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        client.MessageReceived += _ => throw new InvalidOperationException("Subscriber failed");
        client.MessageReceived += message =>
        {
            messages.Add(message);
            if (messages.Count == 2)
                received.TrySetResult();
        };

        try
        {
            await client.ConnectAsync().WaitAsync(Timeout);
            using var socket = await server.NextConnectionAsync();
            await SendAsync(socket, "first");
            await SendAsync(socket, "second");
            await received.Task.WaitAsync(Timeout);

            Assert.Equal(new[] { "first", "second" }, messages);
            Assert.Equal(JmriConnectionState.Connected, client.State);
        }
        finally
        {
            await client.DisconnectAsync().WaitAsync(Timeout);
        }
    }

    [Fact]
    public async Task MessageSubscriberCanWaitForDisconnect()
    {
        await using var server = await TestServer.StartAsync();
        var client = server.CreateClient();
        var disconnected = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        client.MessageReceived += _ => client.DisconnectAsync().GetAwaiter().GetResult();
        client.Disconnected += _ => disconnected.TrySetResult();

        try
        {
            await client.ConnectAsync().WaitAsync(Timeout);
            using var socket = await server.NextConnectionAsync();
            await SendAsync(socket, "stop");
            await disconnected.Task.WaitAsync(Timeout);
            Assert.Equal(JmriConnectionState.Disconnected, client.State);
        }
        finally
        {
            await client.DisconnectAsync().WaitAsync(Timeout);
        }
    }

    [Fact]
    public async Task RemoteCloseSubscriberCanWaitForDisconnect()
    {
        await using var server = await TestServer.StartAsync();
        var client = server.CreateClient();
        var callbackFinished = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var notifications = 0;
        client.Disconnected += _ =>
        {
            client.DisconnectAsync().GetAwaiter().GetResult();
            Interlocked.Increment(ref notifications);
            callbackFinished.TrySetResult();
        };

        try
        {
            await client.ConnectAsync().WaitAsync(Timeout);
            using var socket = await server.NextConnectionAsync();
            await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure,
                "Server stopped", CancellationToken.None).WaitAsync(Timeout);
            await callbackFinished.Task.WaitAsync(Timeout);
            Assert.Equal(JmriConnectionState.Disconnected, client.State);
            Assert.Equal(1, notifications);
        }
        finally
        {
            await client.DisconnectAsync().WaitAsync(Timeout);
        }
    }

    [Fact]
    public void ConstructorRejectsNullOptions()
    {
        Assert.Throws<ArgumentNullException>(() => new JmriWebSocketClient(null!));
    }

    [Fact]
    public async Task ConnectUpdatesStateAndNotifiesConnected()
    {
        await using var server = await TestServer.StartAsync();
        await using var client = server.CreateClient();
        var connected = NewSignal();
        client.Connected += () => connected.TrySetResult();

        Assert.Equal(JmriConnectionState.Disconnected, client.State);
        Assert.False(client.IsConnected);

        await client.ConnectAsync().WaitAsync(Timeout);
        await connected.Task.WaitAsync(Timeout);

        Assert.Equal(JmriConnectionState.Connected, client.State);
        Assert.True(client.IsConnected);
    }

    [Fact]
    public async Task ConcurrentConnectCallsAllowOnlyOneConnection()
    {
        await using var server = await TestServer.StartAsync();
        await using var client = server.CreateClient();

        var first = client.ConnectAsync();
        var second = client.ConnectAsync();

        await first.WaitAsync(Timeout);
        await Assert.ThrowsAsync<InvalidOperationException>(() => second.WaitAsync(Timeout));
        Assert.Equal(1, server.RequestCount);

        // A rejected call must not leave the semaphore locked.
        await client.DisconnectAsync().WaitAsync(Timeout);
        Assert.False(client.IsConnected);
    }

    [Fact]
    public async Task InvalidUrlDoesNotPreventLaterConnection()
    {
        await using var server = await TestServer.StartAsync();
        var options = server.CreateOptions();
        options.WebSocketUrl = "http://localhost/";
        await using var client = new JmriWebSocketClient(options);

        await Assert.ThrowsAsync<InvalidOperationException>(() => client.ConnectAsync().WaitAsync(Timeout));
        Assert.Equal(JmriConnectionState.Disconnected, client.State);

        options.WebSocketUrl = server.CreateOptions().WebSocketUrl;
        await client.ConnectAsync().WaitAsync(Timeout);
        Assert.True(client.IsConnected);
    }

    [Fact]
    public async Task CancelledConnectDoesNotContactServer()
    {
        await using var server = await TestServer.StartAsync();
        await using var client = server.CreateClient();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            client.ConnectAsync(cancellation.Token).WaitAsync(Timeout));

        Assert.Equal(JmriConnectionState.Disconnected, client.State);
        Assert.Equal(0, server.RequestCount);
    }

    [Fact]
    public async Task RejectedInitialConnectionSetsFailedState()
    {
        await using var server = await TestServer.StartAsync();
        server.RejectConnections = true;
        await using var client = server.CreateClient();

        await Assert.ThrowsAsync<WebSocketException>(() => client.ConnectAsync().WaitAsync(Timeout));

        Assert.Equal(JmriConnectionState.Failed, client.State);
        Assert.False(client.IsConnected);
    }

    [Fact]
    public async Task FragmentedMessageIsDeliveredOnceWithCompleteUtf8Text()
    {
        await using var server = await TestServer.StartAsync();
        await using var client = server.CreateClient();
        var received = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        var count = 0;
        client.MessageReceived += message =>
        {
            Interlocked.Increment(ref count);
            received.TrySetResult(message);
        };

        await client.ConnectAsync().WaitAsync(Timeout);
        using var socket = await server.NextConnectionAsync();
        var text = new string('x', 9000) + "\u00e9";
        var bytes = Encoding.UTF8.GetBytes(text);
        // Split the final UTF-8 character across fragments, beyond the receive buffer size.
        await socket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length - 1),
            WebSocketMessageType.Text, false, CancellationToken.None).WaitAsync(Timeout);
        await socket.SendAsync(new ArraySegment<byte>(bytes, bytes.Length - 1, 1),
            WebSocketMessageType.Text, true, CancellationToken.None).WaitAsync(Timeout);

        Assert.Equal(text, await received.Task.WaitAsync(Timeout));
        Assert.Equal(1, Volatile.Read(ref count));
    }

    [Fact]
    public async Task RemoteCloseReconnectsAndResumesReceivingMessages()
    {
        await using var server = await TestServer.StartAsync();
        await using var client = server.CreateClient();
        var events = new ConcurrentQueue<string>();
        var received = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        client.Connected += () => events.Enqueue("connected");
        client.Disconnected += _ => events.Enqueue("disconnected");
        client.Reconnecting += (attempt, delay) => events.Enqueue($"retry:{attempt}:{delay.TotalSeconds}");
        client.MessageReceived += message => received.TrySetResult(message);

        await client.ConnectAsync().WaitAsync(Timeout);
        using var firstSocket = await server.NextConnectionAsync();
        await CloseAsync(firstSocket);
        using var secondSocket = await server.NextConnectionAsync();
        await SendAsync(secondSocket, "after reconnect");

        Assert.Equal("after reconnect", await received.Task.WaitAsync(Timeout));
        Assert.Equal(new[] { "connected", "disconnected", "retry:1:1", "connected" }, events.ToArray());
        Assert.Equal(JmriConnectionState.Connected, client.State);
        Assert.Equal(2, server.RequestCount);
    }

    [Fact]
    public async Task ConnectedSubscriberCanResubscribeAfterReconnection()
    {
        await using var server = await TestServer.StartAsync();
        var options = server.CreateOptions();
        options.HeartbeatIntervalSeconds = 60;
        await using var client = new JmriWebSocketClient(options);
        var messages = Channel.CreateUnbounded<string>();
        var subscription = JsonSerializer.Serialize(new
        {
            type = "sensor",
            method = "get",
            data = new { name = "I2S1" }
        });
        client.Connected += () => client.SendAsync(subscription).GetAwaiter().GetResult();
        client.MessageReceived += message => messages.Writer.TryWrite(message);

        await client.ConnectAsync().WaitAsync(Timeout);
        using var firstSocket = await server.NextConnectionAsync();
        Assert.Equal(subscription, await ReceiveTextAsync(firstSocket));
        var active = "{\"type\":\"sensor\",\"data\":{\"name\":\"I2S1\",\"state\":2}}";
        await SendAsync(firstSocket, active);
        Assert.Equal(active, await messages.Reader.ReadAsync().AsTask().WaitAsync(Timeout));

        await CloseAsync(firstSocket);
        using var secondSocket = await server.NextConnectionAsync();
        Assert.Equal(subscription, await ReceiveTextAsync(secondSocket));
        var inactive = "{\"type\":\"sensor\",\"data\":{\"name\":\"I2S1\",\"state\":4}}";
        await SendAsync(secondSocket, inactive);
        Assert.Equal(inactive, await messages.Reader.ReadAsync().AsTask().WaitAsync(Timeout));

        Assert.True(client.IsConnected);
        Assert.Equal(2, server.RequestCount);
    }

    [Fact]
    public async Task DisconnectCancelsRetryDelayWithoutAnotherConnectionAttempt()
    {
        await using var server = await TestServer.StartAsync();
        await using var client = new JmriWebSocketClient(server.CreateOptions(reconnectIntervalSeconds: 60));
        var retrying = NewSignal();
        var callbackFinished = NewSignal();
        var disconnectCount = 0;
        client.Reconnecting += (_, _) => retrying.TrySetResult();
        client.Disconnected += _ =>
        {
            Interlocked.Increment(ref disconnectCount);
            callbackFinished.TrySetResult();
        };

        await client.ConnectAsync().WaitAsync(Timeout);
        using var socket = await server.NextConnectionAsync();
        await CloseAsync(socket);
        await retrying.Task.WaitAsync(Timeout);

        await client.DisconnectAsync().WaitAsync(Timeout);
        await client.DisconnectAsync().WaitAsync(Timeout);
        await callbackFinished.Task.WaitAsync(Timeout);

        Assert.Equal(JmriConnectionState.Disconnected, client.State);
        Assert.Equal(1, server.RequestCount);
        Assert.Equal(1, Volatile.Read(ref disconnectCount));
    }

    [Fact]
    public async Task FailedRetriesStopAtConfiguredLimit()
    {
        await using var server = await TestServer.StartAsync();
        await using var client = new JmriWebSocketClient(server.CreateOptions(maxReconnectAttempts: 2));
        var attempts = new ConcurrentQueue<int>();
        client.Reconnecting += (attempt, _) => attempts.Enqueue(attempt);

        await client.ConnectAsync().WaitAsync(Timeout);
        using var socket = await server.NextConnectionAsync();
        server.RejectConnections = true;
        await CloseAsync(socket);
        await WaitUntilAsync(() => client.State == JmriConnectionState.Failed && attempts.Count == 2);

        Assert.False(client.IsConnected);
        Assert.Equal(new[] { 1, 2 }, attempts.ToArray());
        Assert.Equal(3, server.RequestCount);
    }

    [Fact]
    public async Task DisposeDisconnectsAndPreventsReuse()
    {
        await using var server = await TestServer.StartAsync();
        await using var client = server.CreateClient();
        var disconnected = NewSignal();
        client.Disconnected += _ => disconnected.TrySetResult();

        await client.ConnectAsync().WaitAsync(Timeout);
        await client.DisposeAsync().AsTask().WaitAsync(Timeout);
        await disconnected.Task.WaitAsync(Timeout);
        await client.DisposeAsync().AsTask().WaitAsync(Timeout);

        Assert.Equal(JmriConnectionState.Disconnected, client.State);
        Assert.False(client.IsConnected);
        await Assert.ThrowsAsync<ObjectDisposedException>(() => client.ConnectAsync().WaitAsync(Timeout));
    }

    [Fact]
    public async Task HeartbeatSendsRepeatedPingsAndReceivesPongs()
    {
        await using var server = await TestServer.StartAsync();
        var options = server.CreateOptions();
        options.HeartbeatIntervalSeconds = 1;
        await using var client = new JmriWebSocketClient(options);
        var messages = Channel.CreateUnbounded<string>();
        client.MessageReceived += message => messages.Writer.TryWrite(message);

        await client.ConnectAsync().WaitAsync(Timeout);
        using var socket = await server.NextConnectionAsync();

        for (var heartbeat = 0; heartbeat < 3; heartbeat++)
        {
            using var ping = JsonDocument.Parse(await ReceiveTextAsync(socket));
            Assert.Equal("ping", ping.RootElement.GetProperty("type").GetString());

            await SendAsync(socket, "{\"type\":\"pong\"}");
            var response = await messages.Reader.ReadAsync().AsTask().WaitAsync(Timeout);
            using var pong = JsonDocument.Parse(response);
            Assert.Equal("pong", pong.RootElement.GetProperty("type").GetString());
        }

        Assert.Equal(JmriConnectionState.Connected, client.State);
        Assert.Equal(1, server.RequestCount);
    }

    [Fact]
    public async Task HeartbeatRestartsAfterReconnection()
    {
        await using var server = await TestServer.StartAsync();
        var options = server.CreateOptions();
        options.HeartbeatIntervalSeconds = 1;
        await using var client = new JmriWebSocketClient(options);

        await client.ConnectAsync().WaitAsync(Timeout);
        using var firstSocket = await server.NextConnectionAsync();
        using var firstPing = JsonDocument.Parse(await ReceiveTextAsync(firstSocket));
        Assert.Equal("ping", firstPing.RootElement.GetProperty("type").GetString());

        await CloseAsync(firstSocket);
        using var secondSocket = await server.NextConnectionAsync();
        using var secondPing = JsonDocument.Parse(await ReceiveTextAsync(secondSocket));
        Assert.Equal("ping", secondPing.RootElement.GetProperty("type").GetString());

        Assert.True(client.IsConnected);
        Assert.Equal(2, server.RequestCount);
    }

    private static async Task<string> ReceiveTextAsync(WebSocket socket)
    {
        using var cancellation = new CancellationTokenSource(Timeout);
        using var message = new MemoryStream();
        var buffer = new byte[256];
        WebSocketReceiveResult result;

        do
        {
            result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellation.Token);
            Assert.Equal(WebSocketMessageType.Text, result.MessageType);
            message.Write(buffer, 0, result.Count);
        }
        while (!result.EndOfMessage);

        return Encoding.UTF8.GetString(message.ToArray());
    }

    private static TaskCompletionSource NewSignal() =>
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    private static async Task WaitUntilAsync(Func<bool> condition)
    {
        using var cancellation = new CancellationTokenSource(Timeout);
        while (!condition())
            await Task.Delay(10, cancellation.Token);
    }

    private static Task CloseAsync(WebSocket socket) =>
        socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure,
            "Server stopped", CancellationToken.None).WaitAsync(Timeout);

    private static Task SendAsync(WebSocket socket, string text) =>
        socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(text)),
            WebSocketMessageType.Text, true, CancellationToken.None).WaitAsync(Timeout);

    private sealed class TestServer : IAsyncDisposable
    {
        private readonly WebApplication _app;
        private readonly Channel<WebSocket> _connections = Channel.CreateUnbounded<WebSocket>();
        private int _requestCount;
        private int _rejectConnections;
        private readonly TaskCompletionSource _stop = new(TaskCreationOptions.RunContinuationsAsynchronously);

        private TestServer(WebApplication app) => _app = app;

        public int RequestCount => Volatile.Read(ref _requestCount);

        public bool RejectConnections
        {
            get => Volatile.Read(ref _rejectConnections) != 0;
            set => Interlocked.Exchange(ref _rejectConnections, value ? 1 : 0);
        }

        public Task<WebSocket> NextConnectionAsync() =>
            _connections.Reader.ReadAsync().AsTask().WaitAsync(Timeout);

        public static async Task<TestServer> StartAsync()
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.ConfigureKestrel(options => options.Listen(IPAddress.Loopback, 0));
            var server = new TestServer(builder.Build());
            server._app.UseWebSockets();
            server._app.Map("/", async context =>
            {
                Interlocked.Increment(ref server._requestCount);
                if (server.RejectConnections)
                {
                    context.Response.StatusCode = 503;
                    return;
                }

                using var socket = await context.WebSockets.AcceptWebSocketAsync();
                server._connections.Writer.TryWrite(socket);
                await server._stop.Task;
            });
            await server._app.StartAsync();
            return server;
        }

        public JmriWebSocketClient CreateClient() => new(CreateOptions());

        public JmriClientOptions CreateOptions(int reconnectIntervalSeconds = 1, int maxReconnectAttempts = 3) => new()
        {
            WebSocketUrl = new UriBuilder(_app.Urls.Single()) { Scheme = "ws" }.Uri.AbsoluteUri,
            ReconnectIntervalSeconds = reconnectIntervalSeconds,
            MaxReconnectAttempts = maxReconnectAttempts
        };

        public async ValueTask DisposeAsync()
        {
            _stop.TrySetResult();
            await _app.DisposeAsync();
        }
    }
}
