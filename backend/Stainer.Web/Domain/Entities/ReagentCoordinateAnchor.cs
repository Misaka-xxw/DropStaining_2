namespace Stainer.Web.Domain.Entities;

/// <summary>
/// 试剂区坐标锚点：表达一个试剂列的首尾坐标，其余坐标由插值服务自动计算。
/// 复用已有 CoordinateProfile / CoordinateProfileVersion 版本体系，保证可追踪、可回滚。
/// </summary>
public sealed class ReagentCoordinateAnchor
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CoordinateProfileId { get; set; } = string.Empty;
    public string? CoordinateProfileVersionId { get; set; }
    public int ColumnNo { get; set; }
    public string? ColumnCode { get; set; }
    public int SlotCount { get; set; } = 8;
    public double? StartXUm { get; set; }
    public double? StartYUm { get; set; }
    public double? StartZUm { get; set; }
    public double? EndXUm { get; set; }
    public double? EndYUm { get; set; }
    public double? EndZUm { get; set; }
    public bool IsEnabled { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public CoordinateProfile? CoordinateProfile { get; set; }
    public CoordinateProfileVersion? CoordinateProfileVersion { get; set; }
}