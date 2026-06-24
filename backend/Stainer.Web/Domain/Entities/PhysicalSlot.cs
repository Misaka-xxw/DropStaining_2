namespace Stainer.Web.Domain.Entities;

public sealed class PhysicalSlot
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DrawerId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int SlotNo { get; set; }
    public int VerticalOrderFromBottom { get; set; }
    public int HeatPointId { get; set; }
    public bool IsEnabled { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public Drawer? Drawer { get; set; }
}
