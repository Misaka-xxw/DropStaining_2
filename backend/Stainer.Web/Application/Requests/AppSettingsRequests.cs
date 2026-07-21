namespace Stainer.Web.Application.Requests;

// 保存应用运行参数（设置模块：通讯 + 运行/设备参数）。单行配置，scope 固定 default。
public sealed record SaveAppSettingsRequest(
    string CommandId,
    string? DataInterface,
    string? HostAddress,
    int? HeartbeatSec,
    decimal? ReagentBottleCapacityMl,
    decimal? ReagentTargetTempC,
    decimal? WorkTargetTempC,
    decimal? NeedleGapMm,
    string Reason);
