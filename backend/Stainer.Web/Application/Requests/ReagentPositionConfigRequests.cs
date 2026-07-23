namespace Stainer.Web.Application.Requests;

// 保存试剂位对象配置（坐标修正 X/Y + 孔位 Z 高度）。rackCode 由路由 {rackCode} 提供（权威）。
public sealed record SaveReagentPositionConfigRequest(
    string CommandId,
    decimal? CalibratedXMm,
    decimal? CalibratedYMm,
    decimal? SafeZMm,
    decimal? LiquidDetectZMm,
    decimal? AspirateEndZMm,
    decimal? DispenseZMm,
    int? RoiLeft,
    int? RoiTop,
    int? RoiWidth,
    int? RoiHeight,
    int? PipetteVolumeUl,
    string? PipetteNeedleCode,
    string? PipetteLiquidClassCode,
    string Reason);
