namespace Railway.Core.Domain;

public sealed class Turnout
{
    public string Id { get; }
    public string Name { get; }
    public TurnoutPosition Position { get; private set; }

    public Turnout(string id, string name)
    {
        Id = id;
        Name = name;
        Position = TurnoutPosition.Unknown;
    }

    public void SetPosition(TurnoutPosition position) => Position = position;
}

public enum TurnoutPosition
{
    Unknown,
    Closed,
    Thrown
}