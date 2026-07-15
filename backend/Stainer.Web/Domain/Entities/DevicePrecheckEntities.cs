namespace Stainer.Web.Domain.Entities;

/// <summary>
/// 最近一次完整预检（POST /api/prechecks）的持久化报告。
/// 供 GET /api/run/preflight 的只读校验复用，使“一键检测”与启动门禁形成同一可信链路；
/// 报告带 GeneratedAtUtc，preflight 按有效期判定是否仍可信。
/// </summary>
public sealed class DevicePrecheckRun
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string CommandId { get; set; } = string.Empty;
    public string DeviceMode { get; set; } = string.Empty;
    public string RunMode { get; set; } = string.Empty;
    public bool Ok { get; set; }
    public DateTimeOffset GeneratedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public string ChecksJson { get; set; } = "[]";
}
