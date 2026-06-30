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
