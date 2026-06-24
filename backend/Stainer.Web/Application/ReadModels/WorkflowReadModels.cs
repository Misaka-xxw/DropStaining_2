namespace Stainer.Web.Application.ReadModels;

public sealed record WorkflowSummaryResponse(
    string Id,
    string Code,
    string Name,
    string WorkflowType,
    string Description,
    bool IsEnabled,
    IReadOnlyList<WorkflowVersionSummaryResponse> Versions);

public sealed record WorkflowVersionSummaryResponse(
    string Id,
    int VersionNo,
    string VersionLabel,
    string Status,
    DateTimeOffset? PublishedAtUtc);

public sealed record WorkflowDetailResponse(
    string Id,
    string Code,
    string Name,
    string WorkflowType,
    string Description,
    bool IsEnabled,
    IReadOnlyList<WorkflowVersionDetailResponse> Versions);

public sealed record WorkflowVersionDetailResponse(
    string Id,
    int VersionNo,
    string VersionLabel,
    string Status,
    string ChangeNote,
    DateTimeOffset? PublishedAtUtc,
    IReadOnlyList<WorkflowStepResponse> Steps);

public sealed record WorkflowStepResponse(
    string Id,
    int StepNo,
    string MajorStepCode,
    string StepName,
    string ActionType,
    string? ReagentCode,
    int? VolumeUl,
    int? DurationSeconds,
    int? TargetTemperatureDeciC,
    string FailureStrategy);

public sealed record ProtocolCompatResponse(
    string Id,
    string Code,
    string Name,
    string Version,
    string Description,
    string Status);

public sealed record DashboardQueryReservationResponse(
    string Status,
    int SlideCount,
    int ReagentCount,
    int AlarmCount);
