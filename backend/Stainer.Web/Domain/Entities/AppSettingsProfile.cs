namespace Stainer.Web.Domain.Entities;

// 应用运行参数配置（DB 持久化）。设置模块中"影响实际行为"的运行/通讯参数（单行），
// 镜像 WashValveConfigProfile / SerialConnectionProfile 的单行-per-key + 幂等 + 审计模式。
// 纯界面偏好（日志保留条数、自动预检、最大可见步骤）不在此处，仍由前端 localStorage 维护。
public sealed class AppSettingsProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ScopeKey { get; set; } = AppSettingsScopeKeys.Default;
    public string? DataInterface { get; set; }      // 数据接口
    public string? HostAddress { get; set; }        // 主机地址
    public int? HeartbeatSec { get; set; }          // 心跳(秒)
    public decimal? ReagentBottleCapacityMl { get; set; } // 试剂瓶容量 mL
    public decimal? ReagentTargetTempC { get; set; }      // 试剂目标温度 ℃
    public decimal? WorkTargetTempC { get; set; }         // 工作目标温度 ℃
    public decimal? NeedleGapMm { get; set; }             // 针间距 mm
    public bool Enabled { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}

public static class AppSettingsScopeKeys
{
    public const string Default = "default";
}
