namespace Stainer.Web.Domain.Entities;

public sealed class PumpChannelState
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string PwmChannelCode { get; set; } = string.Empty;
    public int PwmChannelNo { get; set; }
    public string DrawerCode { get; set; } = string.Empty;
    public int SpeedPercent { get; set; }
    public string Direction { get; set; } = PumpDirections.Stopped;
    public string Status { get; set; } = FluidicsStatuses.Idle;
    public bool IsConnected { get; set; } = true;
    public string? TargetPointCode { get; set; }
    public int? DurationMs { get; set; }
    public string? CurrentCommandId { get; set; }
    public string? MachineRunId { get; set; }
    public string? WorkflowStepExecutionId { get; set; }
    public string? DeviceCommandExecutionId { get; set; }
    public string? FaultCode { get; set; }
    public string? FaultMessage { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class MixerChannelState
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DrawerCode { get; set; } = string.Empty;
    public int ChannelNo { get; set; }
    public string Status { get; set; } = FluidicsStatuses.Idle;
    public bool IsConnected { get; set; } = true;
    public string? CurrentRoundKey { get; set; }
    public string? CurrentCommandId { get; set; }
    public string? MachineRunId { get; set; }
    public string? WorkflowStepExecutionId { get; set; }
    public string? DeviceCommandExecutionId { get; set; }
    public string? FaultCode { get; set; }
    public string? FaultMessage { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class LiquidContainerState
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SourceType { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsWaste { get; set; }
    public int CapacityUl { get; set; }
    public int CurrentVolumeUl { get; set; }
    public int LowThresholdUl { get; set; }
    public int FullThresholdUl { get; set; }
    public string LevelStatus { get; set; } = LiquidLevelStatuses.Normal;
    public bool IsConnected { get; set; } = true;
    public string? FaultCode { get; set; }
    public string? FaultMessage { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class FluidicsTelemetry
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SourceType { get; set; } = string.Empty;
    public string SourceId { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? PwmChannelCode { get; set; }
    public string? DrawerCode { get; set; }
    public string? LiquidSourceType { get; set; }
    public int? SpeedPercent { get; set; }
    public string? Direction { get; set; }
    public int? CurrentVolumeUl { get; set; }
    public int? CapacityUl { get; set; }
    public string? TargetPointCode { get; set; }
    public string? CommandId { get; set; }
    public string? MachineRunId { get; set; }
    public string? WorkflowStepExecutionId { get; set; }
    public string? DeviceCommandExecutionId { get; set; }
    public string? FaultCode { get; set; }
    public DateTimeOffset RecordedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

public static class PumpDirections
{
    public const string Forward = "Forward";
    public const string Reverse = "Reverse";
    public const string Stopped = "Stopped";
}

public static class FluidicsStatuses
{
    public const string Idle = "Idle";
    public const string Running = "Running";
    public const string Completed = "Completed";
    public const string Stopped = "Stopped";
    public const string TimedOut = "TimedOut";
    public const string Faulted = "Faulted";
    public const string Disconnected = "Disconnected";
    public const string Unknown = "Unknown";
}

public static class FluidicsFaultTypes
{
    public const string Failure = "Failure";
    public const string Timeout = "Timeout";
    public const string SensorFailure = "SensorFailure";
    public const string Disconnected = "Disconnected";
    public const string Unknown = "Unknown";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Failure,
        Timeout,
        SensorFailure,
        Disconnected,
        Unknown
    };
}

public static class LiquidSourceTypes
{
    public const string SystemWater = "SystemWater";
    public const string Pbs = "PBS";
    public const string Waste = "Waste";
    public const string ToxicWaste = "ToxicWaste";

    public static readonly IReadOnlyList<string> All = [SystemWater, Pbs, Waste, ToxicWaste];
}

public static class LiquidLevelStatuses
{
    public const string Normal = "Normal";
    public const string Low = "Low";
    public const string Empty = "Empty";
    public const string Full = "Full";
    public const string SensorFault = "SensorFault";
    public const string Disconnected = "Disconnected";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Normal,
        Low,
        Empty,
        Full,
        SensorFault,
        Disconnected
    };
}

public static class FluidicsTelemetrySourceTypes
{
    public const string Pump = "PumpChannel";
    public const string Mixer = "MixerChannel";
    public const string LiquidLevel = "LiquidLevel";
}

public static class FluidicsTelemetryEventTypes
{
    public const string PumpChanged = "PumpChanged";
    public const string MixerChanged = "MixerChanged";
    public const string LiquidLevelChanged = "LiquidLevelChanged";
    public const string FaultConfigured = "FaultConfigured";
    public const string FaultCleared = "FaultCleared";
}
