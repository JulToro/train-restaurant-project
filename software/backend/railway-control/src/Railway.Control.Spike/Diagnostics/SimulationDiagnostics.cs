using Railway.Core.Domain;

namespace Railway.Control.Spike.Diagnostics;

public sealed class SimulationDiagnostics
{
    private readonly object _sync = new object();
    private SensorState? _lastSensorState;
    private DateTimeOffset? _lastSensorEventAt;

    public void RecordSensorEvent(SensorState sensorState)
    {
        lock (_sync)
        {
            _lastSensorState = sensorState;
            _lastSensorEventAt = DateTimeOffset.UtcNow;
        }
    }

    public (SensorState? State, DateTimeOffset? ReceivedAt) ReadSensorEvent()
    {
        lock (_sync)
        {
            return (_lastSensorState, _lastSensorEventAt);
        }
    }
}