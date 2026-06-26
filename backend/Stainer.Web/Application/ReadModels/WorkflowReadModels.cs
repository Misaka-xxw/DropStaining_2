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
    DateTimeOffset? PublishedAtUtc,
    DateTimeOffset? RetiredAtUtc,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    int StepCount,
    int ReagentRequirementCount);

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
    DateTimeOffset? RetiredAtUtc,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    IReadOnlyList<WorkflowStepResponse> Steps,
    IReadOnlyList<WorkflowReagentRequirementResponse> ReagentRequirements);

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

public sealed record WorkflowReagentRequirementResponse(
    string Id,
    string ReagentCode,
    string? ReagentName,
    int? RequiredVolumeUl,
    bool IsRequired);

public sealed record WorkflowVersionMaintenanceResponse(
    string WorkflowDefinitionId,
    string WorkflowVersionId,
    string Code,
    string Name,
    string WorkflowType,
    string Description,
    bool IsEnabled,
    int VersionNo,
    string VersionLabel,
    string Status,
    string ChangeNote,
    DateTimeOffset? PublishedAtUtc,
    DateTimeOffset? RetiredAtUtc,
    IReadOnlyList<WorkflowStepResponse> Steps,
    IReadOnlyList<WorkflowReagentRequirementResponse> ReagentRequirements);

public sealed record PublishValidationResponse(
    string WorkflowVersionId,
    string Result,
    int FailCount,
    int WarningCount,
    IReadOnlyList<PublishValidationIssueResponse> Issues);

public sealed record PublishValidationIssueResponse(
    string Severity,
    string Area,
    string Code,
    string Message);

public sealed record PrimaryAntibodyMappingResponse(
    string Id,
    string PrimaryAntibodyCode,
    string WorkflowVersionId,
    string WorkflowCode,
    string WorkflowName,
    string VersionLabel,
    string WorkflowStatus,
    bool IsEnabled,
    DateTimeOffset CreatedAtUtc);

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
