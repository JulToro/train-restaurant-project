namespace Railway.Jmri.Client
{
    public interface IJmriConnection : IAsyncDisposable
    {
        JmriConnectionState State { get; }

        event Action? Connected;
        event Action<string>? Disconnected;
        event Action<int, TimeSpan>? Reconnecting;
        event Action<string>? MessageReceived;


        Task SendAsync(string message, CancellationToken cancellationToken = default);
        Task ConnectAsync(CancellationToken cancellationToken = default);
        Task DisconnectAsync(CancellationToken cancellationToken = default);
    }
}