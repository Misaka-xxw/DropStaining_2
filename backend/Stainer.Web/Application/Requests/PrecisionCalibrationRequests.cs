namespace Stainer.Web.Application.Requests;

// 保存精度校正配置（调试模块“精度校正”）。scopeKey 由路由 {scopeKey} 提供（权威），
// 不放入请求体，避免 minimal API 对全简单属性 record 的 body 推断冲突（与 SaveSerialConnectionRequest 一致）。
public sealed record SavePrecisionCalibrationRequest(
    string CommandId,
    double? MoveOffsetXMm,
    double? MoveOffsetYMm,
    double? DispenseTargetVolumeUl,
    double? DispenseMeasuredVolumeUl,
    string Reason);
