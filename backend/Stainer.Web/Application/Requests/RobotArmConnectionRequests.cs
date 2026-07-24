namespace Stainer.Web.Application.Requests;

// 工程受控的机械臂只读 Bridge 会话 Open/Close 请求。
// 仅携带工程命令幂等与审计所需字段（CommandId / Reason / Target / DangerousOperationConfirmed）。
// 不携带 COM、波特率、sdkDirectory、NodeID 等硬件配置——这些仍由 Bridge 本机配置负责，
// 客户端请求体只发送 { requestId, command }。
public sealed record RobotArmConnectionRequest(
    string CommandId,
    string Reason,
    string? Target = null,
    bool DangerousOperationConfirmed = false);
