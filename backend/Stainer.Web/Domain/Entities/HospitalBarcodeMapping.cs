namespace Stainer.Web.Domain.Entities;

public sealed class HospitalBarcodeMapping
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string HospitalCode { get; set; } = string.Empty;
    public string PrimaryAntibodyCode { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
