namespace Stainer.Web.Domain.Entities;

// 试剂位对象配置（DB 持久化）。配置模块"试剂对象详情"的坐标修正(X/Y) + 孔位Z高度，
// 按 rackCode(R1-R40) 单行 upsert，镜像 PrecisionCalibrationProfile 的单行-per-key + 幂等 + 审计模式。
// 这是上位机侧的工程配置档案，与 coordinate_points 版本治理（生产坐标基线）互不干扰。
public sealed class ReagentPositionConfig
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string RackCode { get; set; } = "R1";
    public decimal? CalibratedXMm { get; set; }       // X 坐标修正 / mm
    public decimal? CalibratedYMm { get; set; }       // Y 坐标修正 / mm
    public decimal? SafeZMm { get; set; }              // Z-Travel 安全移动高度 / mm
    public decimal? LiquidDetectZMm { get; set; }     // Z-Start 探液高度 / mm
    public decimal? AspirateEndZMm { get; set; }       // Z-End 针尖最大下降深度 / mm
    public decimal? DispenseZMm { get; set; }         // Z-Dispens 吸液/排液高度 / mm
    public int? RoiLeft { get; set; }                 // 扫码 ROI Left（每试剂位独立）
    public int? RoiTop { get; set; }                  // 扫码 ROI Top
    public int? RoiWidth { get; set; }                // 扫码 ROI Width
    public int? RoiHeight { get; set; }               // 扫码 ROI Height
    public int? PipetteVolumeUl { get; set; }           // 通道移液测试默认液量 / μL
    public string? PipetteNeedleCode { get; set; }       // 通道移液测试默认控制针（Z1/Z2/双针）
    public string? PipetteLiquidClassCode { get; set; }  // 通道移液测试默认液体类型 code
    public bool Enabled { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}
