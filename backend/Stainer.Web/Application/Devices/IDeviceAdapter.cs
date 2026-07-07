namespace Stainer.Web.Application.Devices;

public interface IDeviceAdapter
{
    string Mode { get; }

    string Name { get; }

    Task<DeviceStatusSnapshot> GetStatusAsync(CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> GetHealthAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> InitializeModuleAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> ScanSampleAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> ScanReagentAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> QueryLisAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> SetTemperatureAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> SetCoolingAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> RunPumpAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> ReadLiquidLevelsAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> MixAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> MoveRobotAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> OperateNeedlesAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> PipetteAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> WashNeedlesAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> PrepareDabAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceCommandResult> ExecuteWorkflowActionAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default);

    Task<DeviceFaultControlResult> ConfigureFaultAsync(DeviceFaultCommand command, CancellationToken cancellationToken = default);

    Task<DeviceFaultControlResult> ClearFaultsAsync(DeviceFaultClearCommand command, CancellationToken cancellationToken = default);
}

public sealed record DeviceCommandContext(
    string CommandId,
    string? CorrelationId,
    string? Actor,
    string Source);

public sealed record DeviceOperationRequest(
    DeviceCommandContext Context,
    string ModuleCode,
    string Action,
    IReadOnlyDictionary<string, object?> Parameters);

public sealed record DeviceCommandResult(
    bool Ok,
    string Status,
    string ModuleCode,
    string Action,
    string? ErrorCode,
    string Message,
    DateTimeOffset StartedAtUtc,
    DateTimeOffset CompletedAtUtc,
    bool Acknowledged,
    IReadOnlyDictionary<string, object?> Data);

public sealed record DeviceStatusSnapshot(
    string Mode,
    string AdapterName,
    bool Ready,
    long Version,
    DateTimeOffset UpdatedAtUtc,
    IReadOnlyList<DeviceModuleStatusSnapshot> Modules,
    IReadOnlyList<DeviceFaultPlanSnapshot> FaultPlans);

public sealed record DeviceModuleStatusSnapshot(
    string ModuleCode,
    string ConnectionStatus,
    string CurrentAction,
    string? TargetParametersJson,
    string? CurrentParametersJson,
    string? LastErrorCode,
    string? LastErrorMessage,
    DateTimeOffset UpdatedAtUtc,
    long Version);

public sealed record DeviceFaultPlanSnapshot(
    string Id,
    string ModuleCode,
    string FaultType,
    string? ErrorCode,
    string Message,
    string Reason,
    string CommandId,
    string? OperatorUserId,
    string? OperatorUsername,
    bool Active,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? ClearedAtUtc);

public sealed record DeviceFaultCommand(
    string ModuleCode,
    string FaultType,
    string? ErrorCode,
    string? Message,
    string Reason,
    string CommandId,
    string? OperatorUserId,
    string? OperatorUsername);

public sealed record DeviceFaultClearCommand(
    string? ModuleCode,
    string Reason,
    string CommandId,
    string? OperatorUserId,
    string? OperatorUsername);

public sealed record DeviceFaultControlResult(
    bool Ok,
    string Message,
    DeviceStatusSnapshot State);

public static class DeviceCommandStatuses
{
    public const string Succeeded = "Succeeded";
    public const string Failed = "Failed";
    public const string TimedOut = "TimedOut";
    public const string Unknown = "Unknown";
    public const string NotSupported = "NotSupported";
    public const string Offline = "Offline";
    public const string NotConfigured = "NotConfigured";
}

public static class DeviceConnectionStatuses
{
    public const string Unknown = "Unknown";
    public const string Connected = "Connected";
    public const string Disconnected = "Disconnected";
    public const string Faulted = "Faulted";
    public const string Offline = "Offline";
    public const string NotConfigured = "NotConfigured";
}

public static class DeviceFaultTypes
{
    public const string FailNextCommand = "FailNextCommand";
    public const string TimeoutNextCommand = "TimeoutNextCommand";
    public const string Disconnect = "Disconnect";
    public const string ReturnUnknown = "ReturnUnknown";
    public const string PersistentModuleFailure = "PersistentModuleFailure";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        FailNextCommand,
        TimeoutNextCommand,
        Disconnect,
        ReturnUnknown,
        PersistentModuleFailure
    };
}

public static class DeviceModules
{
    public const string Controller = "controller";
    public const string Cooling = "cooling";
    public const string SampleScanner = "sample-scanner";
    public const string ReagentScanner = "reagent-scanner";
    public const string Lis = "lis";
    public const string Temperature = "temperature";
    public const string Pump = "pump";
    public const string Mixer = "mixer";
    public const string LiquidLevel = "liquid-level";
    public const string RobotArm = "robot-arm";
    public const string Needles = "needles";
    public const string Pipette = "pipette";
    public const string NeedleWash = "needle-wash";
    public const string Dab = "dab";
    public const string Workflow = "workflow";

    public static readonly IReadOnlyList<string> All =
    [
        Controller,
        Cooling,
        SampleScanner,
        ReagentScanner,
        Lis,
        Temperature,
        Pump,
        Mixer,
        LiquidLevel,
        RobotArm,
        Needles,
        Pipette,
        NeedleWash,
        Dab,
        Workflow
    ];
}

public static class ReagentQrCommands
{
    public const string StartScan = "TL_QR_START_SCAN";
    public const string ResetScan = "TL_QR_RESET_SCAN";
    public const string GetScanStatus = "TL_QR_GET_SCAN_STATUS";
    public const string GetText = "TL_QR_GET_TEXT";
    public const string ClearText = "TL_QR_CLEAR_TEXT";
    public const string PutText = "TL_QR_PUT_TEXT";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        StartScan,
        ResetScan,
        GetScanStatus,
        GetText,
        ClearText,
        PutText
    };
}

public static class ReagentQrScanStatusCodes
{
    public const ushort Idle = 0;
    public const ushort Scanning = 1;
}
