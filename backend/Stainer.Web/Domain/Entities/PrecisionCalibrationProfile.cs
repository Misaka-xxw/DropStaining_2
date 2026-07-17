namespace Stainer.Web.Domain.Entities;

// 精度校正配置（DB 持久化）。调试模块“精度校正”：移动校正（X/Y 偏差 mm）与加样校正（目标/实测 μL，可派生校正因子）。
// 按 ScopeKey 单行 upsert，镜像 SerialConnectionProfile 的“单行-per-key”模式（device_key → scope_key）。
// 注意：仅持久化校正配置，不触发任何真实机械臂/加样动作；真实执行仍 fail-closed。
public sealed class PrecisionCalibrationProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ScopeKey { get; set; } = PrecisionCalibrationScopeKeys.Move;
    public double? MoveOffsetXMm { get; set; }          // 移动 X 校正偏差 / mm
    public double? MoveOffsetYMm { get; set; }          // 移动 Y 校正偏差 / mm
    public double? DispenseTargetVolumeUl { get; set; } // 加样体积目标 / μL
    public double? DispenseMeasuredVolumeUl { get; set; } // 实测体积 / μL
    public double? DispenseCalibrationFactor { get; set; } // 派生 = measured / target（target > 0 时）
    public bool Enabled { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}

public static class PrecisionCalibrationScopeKeys
{
    public const string Move = "move";          // 移动精度校正（X/Y 偏差）
    public const string Dispense = "dispense";  // 加样精度校正（目标/实测）
}
