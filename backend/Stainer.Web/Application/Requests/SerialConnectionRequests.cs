namespace Stainer.Web.Application.Requests;

// 保存串口连接配置（调试模块 COM 设置）。deviceKey 由路由 {deviceKey} 提供，不放在请求体内（避免与路由参数同名导致 body 推断冲突）。
public sealed record SaveSerialConnectionRequest(
    string CommandId,
    string? PortName,
    int? BaudRate,
    int? DataBits,
    string? Parity,
    string? StopBits,
    string? Handshake,
    int? ReadTimeoutMilliseconds,
    int? WriteTimeoutMilliseconds,
    string Reason);
