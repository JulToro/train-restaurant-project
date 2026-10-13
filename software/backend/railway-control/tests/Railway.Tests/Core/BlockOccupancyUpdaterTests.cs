using Railway.Core.Domain;
using Railway.Core.Services;
using Xunit;

namespace Railway.Tests.Core;

public class BlockOccupancyUpdaterTests
{
    [Theory]
    [InlineData(SensorState.Active, BlockOccupancy.Occupied)]
    [InlineData(SensorState.Inactive, BlockOccupancy.Free)]
    public void Apply_KnownStateUpdatesSensorAndInitiallyUnknownBlock(
        SensorState state, BlockOccupancy expectedOccupancy)
    {
        var sensor = new Sensor("I2S1", "Sensor B1");
        var block = new Block("B1", "Bloque de entrada");

        var changed = BlockOccupancyUpdater.Apply(sensor, block, state);

        Assert.True(changed);
        Assert.Equal(state, sensor.State);
        Assert.Equal(expectedOccupancy, block.Occupancy);
    }

    [Theory]
    [InlineData(SensorState.Active, SensorState.Inactive, BlockOccupancy.Free)]
    [InlineData(SensorState.Inactive, SensorState.Active, BlockOccupancy.Occupied)]
    public void Apply_StateTransitionChangesOccupancy(
        SensorState initialState, SensorState nextState, BlockOccupancy expectedOccupancy)
    {
        var sensor = new Sensor("I2S1", "Sensor B1");
        var block = new Block("B1", "Bloque de entrada");
        BlockOccupancyUpdater.Apply(sensor, block, initialState);

        var changed = BlockOccupancyUpdater.Apply(sensor, block, nextState);

        Assert.True(changed);
        Assert.Equal(nextState, sensor.State);
        Assert.Equal(expectedOccupancy, block.Occupancy);
    }

    [Theory]
    [InlineData(SensorState.Active, BlockOccupancy.Occupied)]
    [InlineData(SensorState.Inactive, BlockOccupancy.Free)]
    public void Apply_RepeatedStateDoesNotReportAnotherOccupancyChange(
        SensorState state, BlockOccupancy expectedOccupancy)
    {
        var sensor = new Sensor("I2S1", "Sensor B1");
        var block = new Block("B1", "Bloque de entrada");
        BlockOccupancyUpdater.Apply(sensor, block, state);

        var changed = BlockOccupancyUpdater.Apply(sensor, block, state);

        Assert.False(changed);
        Assert.Equal(state, sensor.State);
        Assert.Equal(expectedOccupancy, block.Occupancy);
    }

    [Theory]
    [InlineData(SensorState.Unknown, BlockOccupancy.Unknown)]
    [InlineData(SensorState.Active, BlockOccupancy.Occupied)]
    [InlineData(SensorState.Inactive, BlockOccupancy.Free)]
    public void Apply_UnknownStatePreservesPreviousSensorAndBlockState(
        SensorState initialState, BlockOccupancy expectedOccupancy)
    {
        var sensor = new Sensor("I2S1", "Sensor B1");
        var block = new Block("B1", "Bloque de entrada");
        if (initialState != SensorState.Unknown)
            BlockOccupancyUpdater.Apply(sensor, block, initialState);

        var changed = BlockOccupancyUpdater.Apply(sensor, block, SensorState.Unknown);

        Assert.False(changed);
        Assert.Equal(initialState, sensor.State);
        Assert.Equal(expectedOccupancy, block.Occupancy);
    }
}
