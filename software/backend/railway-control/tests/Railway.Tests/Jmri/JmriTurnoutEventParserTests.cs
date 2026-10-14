using System.Text.Json;
using Railway.Core.Domain;
using Railway.Jmri.Client.Parsing;
using Xunit;

namespace Railway.Tests.Jmri;

public class JmriTurnoutEventParserTests
{
    [Theory]
    [InlineData(2, TurnoutPosition.Closed)]
    [InlineData(4, TurnoutPosition.Thrown)]
    [InlineData(0, TurnoutPosition.Unknown)]
    [InlineData(8, TurnoutPosition.Unknown)]
    public void TryParse_MapsReportedState(int state, TurnoutPosition expected)
    {
        var message = JsonSerializer.Serialize(new
        {
            type = "turnout",
            data = new { name = "I2T1", state }
        });

        var parsed = JmriTurnoutEventParser.TryParse(message, "I2T1", out var position);

        Assert.True(parsed);
        Assert.Equal(expected, position);
    }

    [Theory]
    [InlineData("not json")]
    [InlineData("null")]
    [InlineData("[]")]
    [InlineData("{}")]
    [InlineData("{\"type\":3,\"data\":{\"name\":\"I2T1\",\"state\":2}}")]
    [InlineData("{\"type\":\"sensor\",\"data\":{\"name\":\"I2T1\",\"state\":2}}")]
    [InlineData("{\"type\":\"turnout\"}")]
    [InlineData("{\"type\":\"turnout\",\"data\":null}")]
    [InlineData("{\"type\":\"turnout\",\"data\":{\"state\":2}}")]
    [InlineData("{\"type\":\"turnout\",\"data\":{\"name\":12,\"state\":2}}")]
    [InlineData("{\"type\":\"turnout\",\"data\":{\"name\":\"I2T2\",\"state\":2}}")]
    [InlineData("{\"type\":\"turnout\",\"data\":{\"name\":\"I2T1\"}}")]
    [InlineData("{\"type\":\"turnout\",\"data\":{\"name\":\"I2T1\",\"state\":\"2\"}}")]
    [InlineData("{\"type\":\"turnout\",\"data\":{\"name\":\"I2T1\",\"state\":2.5}}")]
    [InlineData("{\"type\":\"turnout\",\"data\":{\"name\":\"I2T1\",\"state\":2147483648}}")]
    public void TryParse_IgnoresMalformedOrUnrelatedMessages(string message)
    {
        var parsed = JmriTurnoutEventParser.TryParse(message, "I2T1", out var position);

        Assert.False(parsed);
        Assert.Equal(TurnoutPosition.Unknown, position);
    }
}
