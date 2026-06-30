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
