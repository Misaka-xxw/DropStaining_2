namespace Stainer.Web.Application.Requests;

public sealed record StartDeviceInitializationRequest(string CommandId);

public sealed record RetryDeviceInitializationRequest(
    string CommandId,
    string Reason);

public sealed record ConfigureMockDeviceFaultRequest(
    string CommandId,
    string ModuleCode,
    string FaultType,
    string Reason,
    string? ErrorCode = null,
    string? Message = null);

public sealed record ClearMockDeviceFaultRequest(
    string CommandId,
    string Reason,
    string? ModuleCode = null);

public sealed record SetThermalPointRequest(
    string CommandId,
    int TargetTemperatureDeciC,
    bool IsEnabled);

public sealed record SetThermalBoardRequest(
    string CommandId,
    int TargetTemperatureDeciC,
    bool IsEnabled);

public sealed record SetCoolingRequest(
    string CommandId,
    int TargetTemperatureDeciC,
    bool IsEnabled);

public sealed record ConfigureThermalFaultRequest(
    string CommandId,
    string TargetType,
    string FaultType,
    string Reason,
    string? DrawerCode = null,
    int? SlotNo = null);

public sealed record ClearThermalFaultRequest(
    string CommandId,
    string TargetType,
    string Reason,
    string? DrawerCode = null,
    int? SlotNo = null);
