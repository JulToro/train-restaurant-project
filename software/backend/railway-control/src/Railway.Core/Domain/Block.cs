namespace Railway.Core.Domain;

public sealed class Block
{
    public string Id { get; }
    public string Name { get; }
    public BlockOccupancy Occupancy { get; private set; }

    public Block(string id, string name)
    {
        Id = id;
        Name = name;
        Occupancy = BlockOccupancy.Unknown;
    }

    public void MarkOccupied() => Occupancy = BlockOccupancy.Occupied;

    public void MarkFree() => Occupancy = BlockOccupancy.Free;
}

public enum BlockOccupancy
{
    Unknown,
    Free,
    Occupied
}