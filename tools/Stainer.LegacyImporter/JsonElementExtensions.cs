using System.Text.Json;

namespace Stainer.LegacyImporter;

internal static class JsonElementExtensions
{
    public static string? GetStringOrNull(this JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var value) && value.ValueKind != JsonValueKind.Null
            ? value.GetString()
            : null;
    }

    public static bool? GetBoolOrNull(this JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var value) && value.ValueKind is JsonValueKind.True or JsonValueKind.False
            ? value.GetBoolean()
            : null;
    }

    public static int? GetIntOrNull(this JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var value) || value.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        return value.ValueKind switch
        {
            JsonValueKind.Number when value.TryGetInt32(out var number) => number,
            JsonValueKind.Number => (int)Math.Round(value.GetDouble(), MidpointRounding.AwayFromZero),
            JsonValueKind.String when int.TryParse(value.GetString(), out var number) => number,
            _ => null
        };
    }

    public static int? GetDeciCOrNull(this JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var value) || value.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        return value.ValueKind switch
        {
            JsonValueKind.Number => (int)Math.Round(value.GetDouble() * 10, MidpointRounding.AwayFromZero),
            JsonValueKind.String when decimal.TryParse(value.GetString(), out var number) => (int)Math.Round(number * 10, MidpointRounding.AwayFromZero),
            _ => null
        };
    }

    public static int? GetMicrolitersFromMillilitersOrNull(this JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var value) || value.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        return value.ValueKind switch
        {
            JsonValueKind.Number => (int)Math.Round(value.GetDouble() * 1000, MidpointRounding.AwayFromZero),
            JsonValueKind.String when decimal.TryParse(value.GetString(), out var number) => (int)Math.Round(number * 1000, MidpointRounding.AwayFromZero),
            _ => null
        };
    }
}
