namespace Stainer.Web.Application.ReadModels;

public sealed record CommandResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string Message);

public sealed record UserMutationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string UserId,
    string Username,
    string DisplayName,
    bool Enabled,
    IReadOnlyList<string> Roles,
    string Message);

public sealed record WorkflowDraftMutationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string WorkflowDefinitionId,
    string WorkflowVersionId,
    string Code,
    string Name,
    int VersionNo,
    string VersionLabel,
    string Status,
    string Message);

public sealed record TaskWorkflowCandidateResponse(
    string WorkflowVersionId,
    string WorkflowDefinitionId,
    string WorkflowCode,
    string WorkflowName,
    string VersionLabel);

public sealed record TaskCreationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    bool RequiresSelection,
    string Message,
    string? TaskId,
    string? TaskCode,
    IReadOnlyList<string> CandidatePrimaryAntibodyCodes,
    IReadOnlyList<TaskWorkflowCandidateResponse> CandidateWorkflows);

public sealed record ReagentScanConfirmationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string SessionId,
    int EmptyCount,
    int ValidCount,
    int InvalidCount,
    string Message);

public sealed record EngineeringWriteResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string EntityId,
    string Message);

public sealed record PreflightValidationReportResponse(
    bool Ok,
    int TaskCount,
    int IssueCount,
    IReadOnlyList<PreflightValidationIssueResponse> Issues);

public sealed record PreflightValidationIssueResponse(
    string Area,
    string Code,
    string Message);
