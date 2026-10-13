namespace Railway.Jmri.Client
{
    public class JmriClientOptions
    {
        public string WebSocketUrl { get; set; } = string.Empty;
        public int ReconnectIntervalSeconds { get; set; } 
        public int MaxReconnectAttempts { get; set; }
        public int HeartbeatIntervalSeconds { get; set; } = 5;
    }
}