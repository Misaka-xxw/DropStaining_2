namespace Stainer.Web.Application.Requests;

public sealed record CreateUserRequest(
    string CommandId,
    string Username,
    string DisplayName,
    string Password,
    IReadOnlyList<string> Roles);

public sealed record UpdateUserDisplayNameRequest(
    string CommandId,
    string DisplayName);

public sealed record SetUserEnabledRequest(
    string CommandId,
    bool Enabled);

public sealed record ResetUserPasswordRequest(
    string CommandId,
    string NewPassword);

public sealed record SetUserRolesRequest(
    string CommandId,
    IReadOnlyList<string> Roles);

public sealed record CreateHeTaskRequest(
    string CommandId,
    string WorkflowVersionId,
    string SlotCode);

public sealed record CreateIhcTaskRequest(
    string CommandId,
    string InputMode,
    string RawCode,
    string SlotCode,
    string? SelectedPrimaryAntibodyCode,
    string? SelectedWorkflowVersionId);

public sealed record ReagentScanInputItem(
    string Position,
    string ScanResult,
    string? RawBarcode,
    string? LocatorCode,
    DateOnly? ExpirationDate);

public sealed record ConfirmReagentScanRequest(
    string CommandId,
    IReadOnlyList<ReagentScanInputItem> Items);

public sealed record CalibrateCoordinatePointRequest(
    string CommandId,
    string ProfileCode,
    string PointCode,
    long? CalibratedXUm,
    long? CalibratedYUm,
    long? SafeZUm,
    long? AspirateZUm,
    long? DispenseZUm,
    string Reason);

public sealed record SaveLiquidClassRequest(
    string CommandId,
    string Code,
    string Name,
    int? AspirateSpeedUlPerSecond,
    int? DispenseSpeedUlPerSecond,
    int? LeadingAirGapUl,
    int? TrailingAirGapUl,
    int? ExcessVolumeUl,
    int? PreWetCycles,
    int? MixCycles,
    bool IsEnabled,
    string Reason);

public sealed record SaveDeviceProfileRequest(
    string CommandId,
    string Code,
    string Name,
    bool IsActive,
    string Reason);
