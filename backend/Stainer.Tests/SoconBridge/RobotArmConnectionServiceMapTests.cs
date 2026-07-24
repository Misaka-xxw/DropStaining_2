using Stainer.Web.Application.Devices.SoconBridge;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Services;

namespace Stainer.Tests.SoconBridge;

/// <summary>
/// RobotArmConnectionService.Map 的纯单元测试：把 ISoconBridgeClient 的结构化结果映射为
/// 对外稳定、脱敏响应。覆盖 Success/Failure/Blocked、所有传输/协议错误，以及“原始异常文本与
/// 路径绝不进入对外响应”的脱敏断言。
/// </summary>
public class RobotArmConnectionServiceMapTests
{
    [Fact]
    public void Completed_Success_映射为Success并透出已脱敏的Bridge字段()
    {
        var result = SoconBridgeResponseResult.Completed(
            "req-1",
            "OpenConfiguredReadOnlySession",
            success: true,
            bridgeStatus: "DeploymentValidated",
            message: "SessionOpen",
            blockReason: null,
            warnings: [],
            details: null);

        var response = RobotArmConnectionService.Map(result, "cmd-1", replayed: false);

        Assert.Equal("cmd-1", response.CommandId);
        Assert.False(response.Replayed);
        Assert.Equal(RobotArmConnectionOutcome.Success, response.Outcome);
        Assert.Equal("SessionOpen", response.Message);
        Assert.Equal("DeploymentValidated", response.BridgeStatus);
        Assert.Null(response.BlockReason);
    }

    [Fact]
    public void Completed_Blocked_映射为Blocked并保留blockReason()
    {
        var result = SoconBridgeResponseResult.Completed(
            "req-1",
            "OpenConfiguredReadOnlySession",
            success: false,
            bridgeStatus: "RealReadOnlyNotEnabled",
            message: "BLOCKED",
            blockReason: "RealReadOnlyNotEnabled",
            warnings: [],
            details: null);

        var response = RobotArmConnectionService.Map(result, "cmd-1", replayed: false);

        Assert.Equal(RobotArmConnectionOutcome.Blocked, response.Outcome);
        Assert.Equal("RealReadOnlyNotEnabled", response.BlockReason);
        Assert.Equal("BLOCKED", response.Message);
    }

    [Fact]
    public void Completed_Failure_映射为Failure且不带blockReason()
    {
        var result = SoconBridgeResponseResult.Completed(
            "req-1",
            "GetBridgeStatus",
            success: false,
            bridgeStatus: "Offline",
            message: "NotSupported",
            blockReason: null,
            warnings: [],
            details: null);

        var response = RobotArmConnectionService.Map(result, "cmd-1", replayed: false);

        Assert.Equal(RobotArmConnectionOutcome.Failure, response.Outcome);
        Assert.Null(response.BlockReason);
    }

    [Theory]
    [InlineData(SoconBridgeExchangeStatus.PipeUnavailable, RobotArmConnectionOutcome.PipeUnavailable, "SoconBridge endpoint is unavailable.")]
    [InlineData(SoconBridgeExchangeStatus.ConnectTimeout, RobotArmConnectionOutcome.ConnectTimeout, "SoconBridge did not respond within the connect timeout.")]
    [InlineData(SoconBridgeExchangeStatus.ResponseTimeout, RobotArmConnectionOutcome.ResponseTimeout, "SoconBridge did not respond within the response timeout.")]
    [InlineData(SoconBridgeExchangeStatus.Canceled, RobotArmConnectionOutcome.Canceled, "SoconBridge request was canceled.")]
    [InlineData(SoconBridgeExchangeStatus.Disconnected, RobotArmConnectionOutcome.Disconnected, "SoconBridge closed the connection before completing.")]
    public void 传输错误_映射为固定安全消息_且不泄漏原始异常与路径(
        SoconBridgeExchangeStatus status,
        RobotArmConnectionOutcome expectedOutcome,
        string expectedMessage)
    {
        // 即便底层 ErrorMessage 含敏感路径/管道内部信息，对外响应也必须使用固定安全消息。
        var result = SoconBridgeResponseResult.ForStatus(
            status,
            "req-1",
            "raw: Access to C:\\Users\\secret\\SoconBridge.config.local.json denied; pipe-internal-handle=0x42");

        var response = RobotArmConnectionService.Map(result, "cmd-1", replayed: false);

        Assert.Equal(expectedOutcome, response.Outcome);
        Assert.Equal(expectedMessage, response.Message);
        Assert.Equal(string.Empty, response.BridgeStatus);
        Assert.Null(response.BlockReason);
        Assert.DoesNotContain("secret", response.Message);
        Assert.DoesNotContain("C:", response.Message);
        Assert.DoesNotContain("config.local.json", response.Message);
        Assert.DoesNotContain("pipe-internal", response.Message);
        Assert.DoesNotContain("0x42", response.Message);
    }

    [Fact]
    public void ProtocolError_映射为ProtocolError固定消息_不泄漏细节()
    {
        var result = SoconBridgeResponseResult.ForProtocolError(
            SoconBridgeProtocolErrorKind.RequestIdMismatch,
            "req-1",
            "raw mismatch detail referencing /home/secret/node-7");

        var response = RobotArmConnectionService.Map(result, "cmd-1", replayed: false);

        Assert.Equal(RobotArmConnectionOutcome.ProtocolError, response.Outcome);
        Assert.Equal("SoconBridge returned a protocol error.", response.Message);
        Assert.DoesNotContain("secret", response.Message);
        Assert.DoesNotContain("node-7", response.Message);
    }

    [Fact]
    public void Replayed标志由调用方传入()
    {
        var result = SoconBridgeResponseResult.Completed(
            "req-1", "Ping", true, "Idle", "Pong", null, [], null);

        var replayed = RobotArmConnectionService.Map(result, "cmd-1", replayed: true);

        Assert.True(replayed.Replayed);
    }
}
