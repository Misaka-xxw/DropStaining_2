namespace Stainer.Web.Domain.Entities;

public sealed class Drawer
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public int HeatBoardId { get; set; }
    public bool IsEnabled { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<PhysicalSlot> PhysicalSlots { get; set; } = new List<PhysicalSlot>();
}
