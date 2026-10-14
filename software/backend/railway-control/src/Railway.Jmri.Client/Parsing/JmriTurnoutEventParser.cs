using System.Text.Json;
using Railway.Core.Domain;

namespace Railway.Jmri.Client.Parsing;

public static class JmriTurnoutEventParser
{
    public static bool TryParse(string message, string systemName, out TurnoutPosition position)
    {
        // Implement the parsing logic for JMRI turnout events here.
        position = TurnoutPosition.Unknown;

        try
        {
            using var document = JsonDocument.Parse(message);
            var root = document.RootElement;

            if(root.ValueKind != JsonValueKind.Object ||
            !root.TryGetProperty("type", out var type) ||
            type.ValueKind != JsonValueKind.String ||
            type.GetString() != "turnout" ||
            !root.TryGetProperty("data", out var data) ||
            data.ValueKind != JsonValueKind.Object ||
            !data.TryGetProperty("name", out var name) ||
            name.ValueKind != JsonValueKind.String ||
            name.GetString() != systemName ||
            !data.TryGetProperty("state", out var state) ||
            state.ValueKind != JsonValueKind.Number ||
            !state.TryGetInt32(out var value))
            {
                return false;
            }

            position = value switch
            {
                2 => TurnoutPosition.Closed,
                4 => TurnoutPosition.Thrown,
                _ => TurnoutPosition.Unknown
            };

            // Attempt to parse the message and set the position accordingly.
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}