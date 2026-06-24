namespace Stainer.Web.Domain.Entities;

public sealed class CoordinatePoint
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CoordinateProfileId { get; set; } = string.Empty;
    public string PointCode { get; set; } = string.Empty;
    public string PointType { get; set; } = string.Empty;
    public long? PresetXUm { get; set; }
    public long? PresetYUm { get; set; }
    public long? CalibratedXUm { get; set; }
    public long? CalibratedYUm { get; set; }
    public long? SafeZUm { get; set; }
    public long? AspirateZUm { get; set; }
    public long? DispenseZUm { get; set; }
    public bool RequiresCalibration { get; set; }
    public bool IsEnabled { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public CoordinateProfile? CoordinateProfile { get; set; }
    public ICollection<CoordinateCalibrationHistory> CalibrationHistory { get; set; } = new List<CoordinateCalibrationHistory>();
}
