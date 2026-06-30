namespace Stainer.Web.Domain.Entities;

public sealed class SampleScanSession
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SessionCode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset StartedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public string? CreatedByUserId { get; set; }

    public User? CreatedByUser { get; set; }
    public ICollection<SampleScanItem> Items { get; } = new List<SampleScanItem>();
}

public sealed class SampleScanItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SampleScanSessionId { get; set; } = string.Empty;
    public string? SlotCode { get; set; }
    public string ScanKind { get; set; } = string.Empty;
    public string ScanStatus { get; set; } = string.Empty;
    public string? RawCode { get; set; }
    public string? NormalizedCode { get; set; }
    public string? PrimaryAntibodyCode { get; set; }
    public string? ErrorReason { get; set; }
    public string DeviceStatus { get; set; } = string.Empty;
    public DateTimeOffset ScannedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public SampleScanSession? SampleScanSession { get; set; }
}

public static class SampleScanKind
{
    public const string TonglingPrimaryAntibody = "TonglingPrimaryAntibody";
    public const string HospitalQr = "HospitalQr";
    public const string Empty = "Empty";
    public const string Damaged = "Damaged";
}

public static class SampleScanStatus
{
    public const string Valid = "VALID";
    public const string Empty = "EMPTY";
    public const string Invalid = "INVALID";
    public const string TimedOut = "TIMED_OUT";
    public const string DeviceDisconnected = "DEVICE_DISCONNECTED";
    public const string Failed = "FAILED";
}
