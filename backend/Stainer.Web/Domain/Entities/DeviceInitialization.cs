namespace Stainer.Web.Domain.Entities;

public sealed class DeviceInitializationRun
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CommandId { get; set; } = string.Empty;
    public string Status { get; set; } = DeviceInitializationStatus.Running;
    public string DeviceMode { get; set; } = string.Empty;
    public string AdapterName { get; set; } = string.Empty;
    public int AttemptNo { get; set; } = 1;
    public string? RetryOfRunId { get; set; }
    public string? RequestedByUserId { get; set; }
    public string? FailureCode { get; set; }
    public string? Message { get; set; }
    public DateTimeOffset StartedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public User? RequestedByUser { get; set; }
    public DeviceInitializationRun? RetryOfRun { get; set; }
    public ICollection<DeviceInitializationRun> RetryRuns { get; set; } = new List<DeviceInitializationRun>();
    public ICollection<DeviceInitializationCheck> Checks { get; set; } = new List<DeviceInitializationCheck>();
}

public sealed class DeviceInitializationCheck
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DeviceInitializationRunId { get; set; } = string.Empty;
    public int StepNo { get; set; }
    public string ModuleCode { get; set; } = string.Empty;
    public string Status { get; set; } = DeviceInitializationCheckStatus.Pending;
    public string? ErrorCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string ResultJson { get; set; } = "{}";
    public DateTimeOffset? StartedAtUtc { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }

    public DeviceInitializationRun? DeviceInitializationRun { get; set; }
}

public static class DeviceInitializationStatus
{
    public const string Running = "Running";
    public const string Ready = "Ready";
    public const string Failed = "Failed";
}

public static class DeviceInitializationCheckStatus
{
    public const string Pending = "Pending";
    public const string Running = "Running";
    public const string Succeeded = "Succeeded";
    public const string Failed = "Failed";
    public const string TimedOut = "TimedOut";
    public const string Unknown = "Unknown";
}
