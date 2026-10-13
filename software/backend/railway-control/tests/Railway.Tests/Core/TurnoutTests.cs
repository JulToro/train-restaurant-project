using Railway.Core.Domain;
using Xunit;

namespace Railway.Tests.Core;

public class TurnoutTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesAndSetStateToUnknown()
    {
        // Arrange
        var id = "T1";
        var name = "Desvío Principal";

        // Act
        var turnout = new Turnout(id, name);

        // Assert
        Assert.Equal(id, turnout.Id);
        Assert.Equal(name, turnout.Name);
        Assert.Equal(TurnoutPosition.Unknown, turnout.Position);
    }

    [Fact]
    public void SetPosition_ShouldChangePositionCorrectly()
    {
        // Arrange
        var turnout = new Turnout("T1", "Desvío Principal");
        turnout.SetPosition(TurnoutPosition.Closed);

        // Act
        turnout.SetPosition(TurnoutPosition.Thrown);

        // Assert
        Assert.Equal(TurnoutPosition.Thrown, turnout.Position);
    }
}
