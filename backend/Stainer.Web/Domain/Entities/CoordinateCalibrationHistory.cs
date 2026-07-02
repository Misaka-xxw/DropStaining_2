namespace Stainer.Web.Domain.Entities;

public sealed class CoordinateCalibrationHistory
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CoordinatePointId { get; set; } = string.Empty;
    public string? CoordinateProfileVersionId { get; set; }
    public string? SourceCoordinateProfileVersionId { get; set; }
    public long? PreviousXUm { get; set; }
    public long? PreviousYUm { get; set; }
    public long? NewXUm { get; set; }
    public long? NewYUm { get; set; }
    public long? NewZUm { get; set; }
    public long? SafeZUm { get; set; }
    public long? LiquidDetectZUm { get; set; }
    public long? DispenseZUm { get; set; }
    public long? ActionOffsetXUm { get; set; }
    public long? ActionOffsetYUm { get; set; }
    public long? ActionOffsetZUm { get; set; }
    public string ChangeSummaryJson { get; set; } = "{}";
    public string ValidationResultJson { get; set; } = "{}";
    public string Reason { get; set; } = string.Empty;
    public string? CalibratedByUserId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public CoordinatePoint? CoordinatePoint { get; set; }
    public CoordinateProfileVersion? CoordinateProfileVersion { get; set; }
    public CoordinateProfileVersion? SourceCoordinateProfileVersion { get; set; }
    public User? CalibratedByUser { get; set; }

    public long? AspirateZUm
    {
        get => LiquidDetectZUm;
        set => LiquidDetectZUm = value;
    }
}
