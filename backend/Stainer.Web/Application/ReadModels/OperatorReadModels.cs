namespace Stainer.Web.Application.ReadModels;

public sealed record OperatorSnapshotResponse(
    string Status,
    bool Initialized,
    string DeviceMode,
    string? RunId,
    string? RunCode,
    string? CurrentMajorStepCode,
    bool PauseRequested,
    bool StopRequested,
    OperatorUserResponse ActiveUser,
    OperatorSystemResponse System,
    IReadOnlyList<OperatorChannelResponse> Channels,
    IReadOnlyList<string> Alarms,
    IReadOnlyList<AlarmResponse> AlarmDetails,
    IReadOnlyList<string> Logs,
    IReadOnlyList<OperatorEventResponse> RecentEvents,
    DeviceInitializationResponse Initialization,
    ThermalStateResponse Thermal,
    FluidicsStateResponse Fluidics,
    IReadOnlyList<OperatorNeedleResponse> Needles,
    IReadOnlyList<OperatorResourceLeaseResponse> ResourceLeases,
    IReadOnlyList<OperatorDeviceCommandResponse> DeviceCommands,
    IReadOnlyList<OperatorDabPositionResponse> DabPositions);

public sealed record OperatorUserResponse(
    string Id,
    string Username,
    string DisplayName,
    string Role,
    IReadOnlyList<string> Roles);

public sealed record OperatorSystemResponse(
    bool ControllerOnline,
    bool RoboticArmHome,
    bool ReagentCooling,
    bool SampleScannerOnline,
    bool ReagentScannerOnline,
    bool LiquidSensor,
    bool NeedleWash,
    bool PureWaterOk,
    bool PbsOk,
    bool WasteTankFull,
    bool ToxicTankFull,
    decimal ReagentTemperatureC);

public sealed record OperatorChannelResponse(
    int Id,
    string DrawerCode,
    string? ChannelBatchId,
    string Status,
    int Progress,
    string CurrentStep,
    string? ExperimentType,
    string? WorkflowVersionId,
    string? WorkflowCode,
    string? WorkflowName,
    string? WorkflowVersionLabel,
    string WorkflowSelectionStatus,
    bool WorkflowLocked,
    bool CanSelectWorkflow,
    bool CanChangeWorkflow,
    IReadOnlyList<OperatorSlideResponse> Slides);

public sealed record OperatorSlideResponse(
    string Id,
    string? StainingTaskId,
    int Slot,
    string SlotCode,
    string Barcode,
    string SampleIdentifier,
    string ProtocolCode,
    string? WorkflowName,
    string? WorkflowVersionLabel,
    string? WorkflowVersionId,
    string? AntibodyCode,
    string? ConfirmedPrimaryAntibodyCode,
    string? InputMode,
    string? CompatibilityValidationStatus,
    string? CompatibilityValidationMessage,
    string Status,
    string CurrentStep,
    int Progress);

public sealed record OperatorEventResponse(
    string Id,
    string Type,
    string Title,
    string Detail,
    string Status,
    DateTimeOffset OccurredAtUtc,
    string Href);

public sealed record OperatorNeedleResponse(
    string NeedleCode,
    string Status,
    bool IsConnected,
    string LoadedSourceType,
    string? LoadedReagentCode,
    string? SourcePositionCode,
    string? DabBatchId,
    int VolumeUl,
    bool NeedsWash,
    string? CurrentCommandId,
    string? LastErrorCode,
    string? LastErrorMessage,
    DateTimeOffset UpdatedAtUtc);

public sealed record OperatorResourceLeaseResponse(
    string Id,
    string ResourceCode,
    string ResourceType,
    string Status,
    string? MachineRunId,
    string? WorkflowStepExecutionId,
    string? DeviceCommandExecutionId,
    string? WaitReason,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? AcquiredAtUtc,
    DateTimeOffset? ReleasedAtUtc);

public sealed record OperatorDeviceCommandResponse(
    string Id,
    string CommandType,
    string Status,
    string? WorkflowStepExecutionId,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? CommandSentAtUtc,
    DateTimeOffset? AcknowledgedAtUtc,
    DateTimeOffset? CompletedAtUtc);

public sealed record OperatorDabPositionResponse(
    string Id,
    string Code,
    int PositionNo,
    bool IsEnabled,
    string Status,
    string? ActiveDabBatchId,
    string? BatchStatus,
    string? CleaningStatus,
    string? DabASource,
    string? DabBSource,
    int? SlideCount,
    int? RemainingVolumeUl,
    DateTimeOffset? PreparedAtUtc,
    DateTimeOffset? ExpiresAtUtc,
    DateTimeOffset? UpdatedAtUtc);
