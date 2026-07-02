using Stainer.Web.Application.Devices;

namespace Stainer.Web.Application.ReadModels;

public sealed record DeviceInitializationCheckResponse(
    string Id,
    int StepNo,
    string ModuleCode,
    string Status,
    string? ErrorCode,
    string Message,
    DateTimeOffset? StartedAtUtc,
    DateTimeOffset? CompletedAtUtc,
    IReadOnlyDictionary<string, object?> Result);

public sealed record DeviceInitializationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string? RunId,
    string Status,
    string DeviceMode,
    string AdapterName,
    int AttemptNo,
    string? RetryOfRunId,
    DateTimeOffset? StartedAtUtc,
    DateTimeOffset? CompletedAtUtc,
    IReadOnlyList<DeviceInitializationCheckResponse> Checks,
    string Message);

public sealed record DeviceFaultMutationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string Message,
    DeviceStatusSnapshot State);

public sealed record ThermalStateResponse(
    bool Ready,
    IReadOnlyList<ThermalPointResponse> Points,
    CoolingStateResponse Cooling,
    DateTimeOffset GeneratedAtUtc);

public sealed record ThermalPointResponse(
    string Id,
    string DrawerCode,
    int BoardNo,
    int SlotNo,
    int PointNo,
    int CurrentTemperatureDeciC,
    int TargetTemperatureDeciC,
    bool IsEnabled,
    bool IsConnected,
    string Status,
    string? FaultCode,
    string? FaultMessage,
    DateTimeOffset UpdatedAtUtc);

public sealed record CoolingStateResponse(
    string Id,
    int CurrentTemperatureDeciC,
    int TargetTemperatureDeciC,
    bool IsEnabled,
    bool IsConnected,
    string Status,
    string? FaultCode,
    string? FaultMessage,
    DateTimeOffset UpdatedAtUtc);

public sealed record ThermalMutationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string Message,
    ThermalStateResponse State);

public sealed record TemperatureTelemetryResponse(
    string Id,
    string SourceType,
    string SourceId,
    string? DrawerCode,
    int? BoardNo,
    int? SlotNo,
    int? PointNo,
    int CurrentTemperatureDeciC,
    int TargetTemperatureDeciC,
    bool IsEnabled,
    bool IsConnected,
    string Status,
    string? FaultCode,
    DateTimeOffset RecordedAtUtc);
