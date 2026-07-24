using Stainer.Web.Application.Devices.SoconBridge;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;

namespace Stainer.Web.Application.Services;

// 工程受控的机械臂只读连接服务。内部调用 ISoconBridgeClient 的 Open/Close/Bridge 状态查询，
// 是 Stainer 主项目接入 SOCON Bridge 只读会话的唯一受控入口。
//
// 范围限定（不可回退）：
//   - 只接入 Open/Close 及协议中已存在的 Bridge 状态查询；不连接真机、不增加机械臂动作；
//   - 不引用 SOCON SDK / 厂商 DLL / net452 Bridge 项目；不调用 IRobotMotionPrimitives 或任何机械臂动作服务。
//
// Fail-closed 规则（不可回退）：只有在 Real 模式且 Device:HardwareAvailable=true 时才调用真实
// Named Pipe 客户端；Mock、硬件不可用或配置不完整时直接抛 BusinessRuleException(409) 失败闭合，
// 绝不把真实连接请求静默转成 Mock 成功。Bridge 响应是运行时连接状态的唯一来源；本服务不在
// 数据库保存任何“连接状态”（CommandReceipt 仅记录命令幂等/审计，不是连接状态）。
// 不自动启动 Bridge、不自动连接、不自动重试、不自动重连。
public sealed class RobotArmConnectionService(
    IConfiguration configuration,
    ISoconBridgeClient bridgeClient,
    DeviceModeService deviceModeService,
    CommandIdempotencyService idempotencyService)
{
    public Task<RobotArmConnectionResponse> OpenReadOnlySessionAsync(
        RobotArmConnectionRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default) =>
        ExecuteSessionCommandAsync(
            "open",
            request,
            actor,
            ct => bridgeClient.OpenConfiguredReadOnlySessionAsync(ct),
            cancellationToken);

    public Task<RobotArmConnectionResponse> CloseReadOnlySessionAsync(
        RobotArmConnectionRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default) =>
        ExecuteSessionCommandAsync(
            "close",
            request,
            actor,
            ct => bridgeClient.CloseConfiguredReadOnlySessionAsync(ct),
            cancellationToken);

    // 只读状态查询（GetBridgeStatus）。GET 语义，不走命令幂等；仍受 Real+HardwareAvailable 门禁约束。
    public async Task<RobotArmConnectionResponse> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        EnsureRealBridgeAllowed();
        var result = await bridgeClient.GetBridgeStatusAsync(cancellationToken);
        return Map(result, commandId: "status", replayed: false);
    }

    private Task<RobotArmConnectionResponse> ExecuteSessionCommandAsync(
        string operation,
        RobotArmConnectionRequest request,
        AuthenticatedUser actor,
        Func<CancellationToken, Task<SoconBridgeResponseResult>> callBridge,
        CancellationToken cancellationToken)
    {
        var commandId = RequireValue(request.CommandId, "commandId");

        // 命令幂等：同一 commandId 重放时返回已存储结果，不再次调用 Bridge（与现有工程命令一致）。
        return idempotencyService.RunAsync(
            commandId,
            $"socon_bridge.session.{operation}",
            request,
            actor,
            async () =>
            {
                // 门禁放在幂等 lambda 内（与 EngineeringPipettingService.EnsureMockMode 同位）：
                // 门禁失败时事务回滚，不落 CommandReceipt。
                EnsureRealBridgeAllowed();

                var result = await callBridge(cancellationToken);
                var response = Map(result, commandId, replayed: false);
                return new CommandExecutionResult<RobotArmConnectionResponse>(
                    response,
                    "SoconBridgeSession",
                    result.RequestId);
            },
            cancellationToken);
    }

    private void EnsureRealBridgeAllowed()
    {
        if (!deviceModeService.IsReal)
        {
            throw new BusinessRuleException(
                "socon_bridge_real_mode_required",
                "SoconBridge read-only session requires Real device mode.",
                StatusCodes.Status409Conflict);
        }

        if (!IsHardwareAvailable)
        {
            throw new BusinessRuleException(
                "socon_bridge_hardware_unavailable",
                "SoconBridge read-only session requires available hardware.",
                StatusCodes.Status409Conflict);
        }
    }

    private bool IsHardwareAvailable =>
        bool.TryParse(configuration["Device:HardwareAvailable"], out var available) && available;

    // 纯映射：把 ISoconBridgeClient 的结构化结果转换为对外的稳定、脱敏响应。
    // 传输/协议错误一律映射为固定安全消息；只有 Completed 才透出 Bridge 已脱敏的协议
    // message / blockReason / bridgeStatus。绝不暴露 ErrorMessage / 异常文本 / 路径 / 堆栈。
    internal static RobotArmConnectionResponse Map(
        SoconBridgeResponseResult result,
        string commandId,
        bool replayed)
    {
        var (outcome, message) = result.Status switch
        {
            SoconBridgeExchangeStatus.Completed => result.Outcome switch
            {
                SoconBridgeOutcome.Success => (RobotArmConnectionOutcome.Success, result.Message ?? string.Empty),
                SoconBridgeOutcome.Blocked => (RobotArmConnectionOutcome.Blocked, result.Message ?? string.Empty),
                _ => (RobotArmConnectionOutcome.Failure, result.Message ?? string.Empty),
            },
            SoconBridgeExchangeStatus.PipeUnavailable => (RobotArmConnectionOutcome.PipeUnavailable, "SoconBridge endpoint is unavailable."),
            SoconBridgeExchangeStatus.ConnectTimeout => (RobotArmConnectionOutcome.ConnectTimeout, "SoconBridge did not respond within the connect timeout."),
            SoconBridgeExchangeStatus.ResponseTimeout => (RobotArmConnectionOutcome.ResponseTimeout, "SoconBridge did not respond within the response timeout."),
            SoconBridgeExchangeStatus.Canceled => (RobotArmConnectionOutcome.Canceled, "SoconBridge request was canceled."),
            SoconBridgeExchangeStatus.Disconnected => (RobotArmConnectionOutcome.Disconnected, "SoconBridge closed the connection before completing."),
            _ => (RobotArmConnectionOutcome.ProtocolError, "SoconBridge returned a protocol error."),
        };

        // 非 Completed 时不透出 Bridge 任何字段（可能为空或不安全），只保留固定安全消息。
        var bridgeStatus = result.Status == SoconBridgeExchangeStatus.Completed
            ? result.BridgeStatus ?? string.Empty
            : string.Empty;
        var blockReason = result.Status == SoconBridgeExchangeStatus.Completed
            ? result.BlockReason
            : null;

        return new RobotArmConnectionResponse(
            commandId,
            replayed,
            outcome,
            bridgeStatus,
            message,
            blockReason,
            DateTimeOffset.UtcNow);
    }

    private static string RequireValue(string? value, string fieldName)
    {
        var normalized = value?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BusinessRuleException($"{fieldName}_required", $"{fieldName} is required.", StatusCodes.Status400BadRequest);
        }

        return normalized;
    }
}
