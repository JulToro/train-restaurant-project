using Railway.Core.Domain;

namespace Railway.Core.Services;

public static class BlockOccupancyUpdater
{
    public static bool Apply(Sensor sensor, Block block, SensorState state)
    {
        if(state is not(SensorState.Active or SensorState.Inactive))
            return false;

        var previousOccupancy = block.Occupancy;

        sensor.SetState(state);

        if(state == SensorState.Active)
            block.MarkOccupied();
        else if(state == SensorState.Inactive)
            block.MarkFree();

        return block.Occupancy != previousOccupancy;
    }
}