namespace Stainer.Web.Application.ReadModels;

// 机械臂只读 Bridge 会话一次 Open/Close/状态查询的对外结果分类（稳定错误码）。
public enum RobotArmConnectionOutcome
{
    Success,
    Failure,
    Blocked,
    PipeUnavailable,
    ConnectTimeout,
    ResponseTimeout,
    Canceled,
    Disconnected,
    ProtocolError
}

// 机械臂只读 Bridge 会话一次调用的稳定、脱敏响应。
//   - Outcome：稳定错误码；
//   - Message：固定安全文本（传输/协议错误）或 Bridge 已脱敏的协议消息（Completed）；
//   - BridgeStatus / BlockReason：仅 Completed 时透出 Bridge 的已脱敏协议字段；
// 不含路径、堆栈、原始异常文本或管道内部信息。
public sealed record RobotArmConnectionResponse(
    string CommandId,
    bool Replayed,
    RobotArmConnectionOutcome Outcome,
    string BridgeStatus,
    string Message,
    string? BlockReason,
    DateTimeOffset RequestedAtUtc);
