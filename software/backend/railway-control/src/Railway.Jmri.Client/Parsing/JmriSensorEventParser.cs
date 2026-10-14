using System.Text.Json;
using Railway.Core.Domain;

namespace Railway.Jmri.Client.Parsing;

public static class JmriSensorEventParser
{
    // Implement the parsing logic for JMRI sensor events here.
    public static bool TryParse(string message, string systemName, out SensorState state)
    {
        state = SensorState.Unknown;
        
        try
        {
            using var document = JsonDocument.Parse(message);
            var root = document.RootElement;

            if(root.ValueKind != JsonValueKind.Object ||
            !root.TryGetProperty("type", out var type) ||
            type.ValueKind != JsonValueKind.String ||
            type.GetString() != "sensor" ||
            !root.TryGetProperty("data", out var data) ||
            data.ValueKind != JsonValueKind.Object ||
            !data.TryGetProperty("name", out var name) ||
            name.ValueKind != JsonValueKind.String ||
            name.GetString() != systemName ||
            !data.TryGetProperty("state", out var sensorState) ||
            sensorState.ValueKind != JsonValueKind.Number ||
            !sensorState.TryGetInt32(out var value))
            {
                return false;
            }

            state = value switch
            {
                2 => SensorState.Active,
                4 => SensorState.Inactive,
                _ => SensorState.Unknown
            };

            return true;

        }
        catch (JsonException)
        {
            return false;
        }
    }
}