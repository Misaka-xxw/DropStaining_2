namespace Stainer.Web.Domain.Entities;

public sealed class ReagentRackPosition
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Code { get; set; } = string.Empty;
    public int PositionNo { get; set; }
    public int ColumnNo { get; set; }
    public int RowNo { get; set; }
    public int ScannerChannelNo { get; set; }
    public string ScannerChannelCode { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
