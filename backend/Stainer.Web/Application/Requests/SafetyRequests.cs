namespace Stainer.Web.Application.Requests;

public sealed record DeviceModeChangeRequest(
    string CommandId,
    string DeviceMode,
    string Reason);

public sealed record DatabaseBackupRequest(
    string CommandId,
    string? OutputDirectory = null);

public sealed record DatabaseRestoreRequest(
    string CommandId,
    string BackupPath,
    string Reason);
