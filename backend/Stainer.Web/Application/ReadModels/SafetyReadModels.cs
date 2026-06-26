namespace Stainer.Web.Application.ReadModels;

public sealed record DeviceModeStatusResponse(
    string CurrentMode,
    string ConfiguredMode,
    bool IsMock,
    bool IsReal,
    bool RealDeviceHealthCheckComplete,
    bool CanStartRuns,
    bool RestartRequiredForChanges,
    string? PendingRequestedMode,
    string Source,
    string Message);

public sealed record DeviceModeChangeResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string CurrentMode,
    string RequestedMode,
    bool RestartRequired,
    string Message);

public sealed record ExecutorLeaseStatusResponse(
    bool IsOwner,
    bool ReadOnlyMode,
    string OwnerId,
    string LockPath,
    string? FailureReason);

public sealed record StartupRecoveryReportResponse(
    int RunsScanned,
    int CommandsMarkedUnknown,
    int StepsMarkedUnknown,
    int RunsMarkedFaulted,
    int AlarmsCreated,
    DateTimeOffset RecoveredAtUtc,
    string Message);

public sealed record DatabaseMaintenanceReportResponse(
    bool Ok,
    string DatabasePath,
    string SqliteVersion,
    bool ForeignKeysEnabled,
    string JournalMode,
    int BusyTimeoutMilliseconds,
    bool CanReadWrite,
    bool IntegrityOk,
    int AppliedMigrationCount,
    int PendingMigrationCount,
    string Message);

public sealed record DatabaseBackupResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string BackupPath,
    bool IntegrityBeforeOk,
    bool IntegrityAfterOk,
    DateTimeOffset CreatedAtUtc,
    string Message);

public sealed record DatabaseRestoreResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string BackupPath,
    bool IntegrityOk,
    bool RestartRequired,
    string Message);

public sealed record PreHardwareReadinessResponse(
    bool Ok,
    DateTimeOffset CheckedAtUtc,
    IReadOnlyList<PreHardwareReadinessCheckResponse> Checks,
    IReadOnlyList<string> BlockingReasons);

public sealed record PreHardwareReadinessCheckResponse(
    string Code,
    bool Ok,
    string Message,
    string Severity = "Blocker");
