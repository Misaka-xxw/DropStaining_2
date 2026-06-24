namespace Stainer.Web.Domain.Entities;

public sealed class CoordinateProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string OriginDefinition { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<CoordinatePoint> CoordinatePoints { get; set; } = new List<CoordinatePoint>();
}
