namespace Stainer.Web.Domain.Entities;

public sealed class ThermalPointState
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DrawerCode { get; set; } = string.Empty;
    public int BoardNo { get; set; }
    public int SlotNo { get; set; }
    public int PointNo { get; set; }
    public int CurrentTemperatureDeciC { get; set; } = 250;
    public int TargetTemperatureDeciC { get; set; } = 250;
    public bool IsEnabled { get; set; }
    public bool IsConnected { get; set; } = true;
    public string Status { get; set; } = ThermalStatuses.Off;
    public string? FaultCode { get; set; }
    public string? FaultMessage { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class CoolingUnitState
{
    public const string SingletonId = "cooling";

    public string Id { get; set; } = SingletonId;
    public int CurrentTemperatureDeciC { get; set; } = 80;
    public int TargetTemperatureDeciC { get; set; } = 80;
    public bool IsEnabled { get; set; } = true;
    public bool IsConnected { get; set; } = true;
    public string Status { get; set; } = ThermalStatuses.Stable;
    public string? FaultCode { get; set; }
    public string? FaultMessage { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class TemperatureTelemetry
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SourceType { get; set; } = string.Empty;
    public string SourceId { get; set; } = string.Empty;
    public string? DrawerCode { get; set; }
    public int? BoardNo { get; set; }
    public int? SlotNo { get; set; }
    public int? PointNo { get; set; }
    public int CurrentTemperatureDeciC { get; set; }
    public int TargetTemperatureDeciC { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsConnected { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? FaultCode { get; set; }
    public DateTimeOffset RecordedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

public static class ThermalStatuses
{
    public const string Off = "Off";
    public const string Heating = "Heating";
    public const string Cooling = "Cooling";
    public const string Returning = "Returning";
    public const string Stable = "Stable";
    public const string Faulted = "Faulted";
    public const string Unknown = "Unknown";
}

public static class ThermalFaultTypes
{
    public const string OverTemperature = "OverTemperature";
    public const string HeatingTimeout = "HeatingTimeout";
    public const string SensorFailure = "SensorFailure";
    public const string Disconnected = "Disconnected";
    public const string TemperatureDeviation = "TemperatureDeviation";
    public const string Unknown = "Unknown";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        OverTemperature,
        HeatingTimeout,
        SensorFailure,
        Disconnected,
        TemperatureDeviation,
        Unknown
    };
}

public static class ThermalTelemetrySourceTypes
{
    public const string Point = "ThermalPoint";
    public const string Cooling = "CoolingUnit";
}
