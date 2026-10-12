namespace Railway.Core.Domain;

public sealed class Sensor
{
    public string Id { get; }
    public string Name { get; }
    public SensorState State { get; private set; }

    public Sensor(string id, string name)
    {
        Id = id;
        Name = name;
        State = SensorState.Unknown;
    }

    public void SetState(SensorState state) => State = state;
}

public enum SensorState
{
    Unknown,
    Inactive,
    Active
}