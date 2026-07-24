using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Stainer.Web.Application.Devices.SoconBridge;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Services;
using Stainer.Web.Infrastructure.Data;
using Stainer.Web.Infrastructure.Devices.SoconBridge;

namespace Stainer.Tests.SoconBridge;

/// <summary>
/// 机械臂只读 Bridge 会话工程 API 的集成测试（WebApplicationFactory + 假 ISoconBridgeClient）。
/// 覆盖：Real+硬件可用时 Open/Close 调用客户端；Success/Failure/Blocked/超时/取消；Mock 模式与
/// 硬件不可用时 fail-closed 不调用真实客户端；重复/重放按 Bridge 结果返回；HTTP 状态；敏感信息不泄漏；
/// DI 解析正确。全程不启动真实 Bridge、不加载 SDK、不访问硬件。
/// </summary>
public class RobotArmConnectionApiTests
{
    [Fact]
    public async Task Real模式_Open成功_调用客户端并返回Success()
    {
        var fake = new FakeSoconBridgeClient();
        await using var factory = CreateFactory(RealHardwareAvailable, fake);
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await OpenEngineeringSessionAsync(client, "open-success");

        var response = await client.PostAsJsonAsync(OpenUrl, new { commandId = "cmd-open-success", reason = "open read-only session" });
        var body = await ReadOkAsync<RobotArmConnectionResponse>(response);

        Assert.Equal(RobotArmConnectionOutcome.Success, body.Outcome);
        Assert.Equal(1, fake.OpenCalls);
        Assert.Equal(0, fake.CloseCalls);
    }

    [Fact]
    public async Task Real模式_Close成功_调用Close客户端()
    {
        var fake = new FakeSoconBridgeClient();
        await using var factory = CreateFactory(RealHardwareAvailable, fake);
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await OpenEngineeringSessionAsync(client, "close-success");

        var response = await client.PostAsJsonAsync(CloseUrl, new { commandId = "cmd-close-success", reason = "close read-only session" });
        var body = await ReadOkAsync<RobotArmConnectionResponse>(response);

        Assert.Equal(RobotArmConnectionOutcome.Success, body.Outcome);
        Assert.Equal(1, fake.CloseCalls);
        Assert.Equal(0, fake.OpenCalls);
    }

    [Fact]
    public async Task Real模式_Open返回Blocked_透出blockReason()
    {
        var fake = new FakeSoconBridgeClient(openSelector: _ => SoconBridgeResponseResult.Completed(
            "req", "OpenConfiguredReadOnlySession", success: false, "RealReadOnlyNotEnabled", "BLOCKED",
            "RealReadOnlyNotEnabled", [], null));
        await using var factory = CreateFactory(RealHardwareAvailable, fake);
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await OpenEngineeringSessionAsync(client, "open-blocked");

        var response = await client.PostAsJsonAsync(OpenUrl, new { commandId = "cmd-open-blocked", reason = "blocked" });
        var body = await ReadOkAsync<RobotArmConnectionResponse>(response);

        Assert.Equal(RobotArmConnectionOutcome.Blocked, body.Outcome);
        Assert.Equal("RealReadOnlyNotEnabled", body.BlockReason);
    }

    [Fact]
    public async Task Real模式_Open返回Failure_映射为Failure()
    {
        var fake = new FakeSoconBridgeClient(openSelector: _ => SoconBridgeResponseResult.Completed(
            "req", "OpenConfiguredReadOnlySession", success: false, "Offline", "NotSupported", null, [], null));
        await using var factory = CreateFactory(RealHardwareAvailable, fake);
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await OpenEngineeringSessionAsync(client, "open-failure");

        var response = await client.PostAsJsonAsync(OpenUrl, new { commandId = "cmd-open-failure", reason = "failure" });
        var body = await ReadOkAsync<RobotArmConnectionResponse>(response);

        Assert.Equal(RobotArmConnectionOutcome.Failure, body.Outcome);
        Assert.Null(body.BlockReason);
    }

    [Theory]
    [InlineData(SoconBridgeExchangeStatus.ResponseTimeout, RobotArmConnectionOutcome.ResponseTimeout)]
    [InlineData(SoconBridgeExchangeStatus.Canceled, RobotArmConnectionOutcome.Canceled)]
    public async Task Real模式_超时与取消_按稳定Outcome返回200(SoconBridgeExchangeStatus status, RobotArmConnectionOutcome expected)
    {
        var fake = new FakeSoconBridgeClient(openSelector: _ => SoconBridgeResponseResult.ForStatus(status, "req", "raw secret path C:\\x"));
        await using var factory = CreateFactory(RealHardwareAvailable, fake);
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await OpenEngineeringSessionAsync(client, "open-" + status);

        var response = await client.PostAsJsonAsync(OpenUrl, new { commandId = $"cmd-open-{status}", reason = status.ToString() });
        var body = await ReadOkAsync<RobotArmConnectionResponse>(response);

        Assert.Equal(expected, body.Outcome);
        Assert.DoesNotContain("secret", body.Message);
        Assert.DoesNotContain("C:", body.Message);
    }

    [Fact]
    public async Task Mock模式_Open_fail_closed_不调用真实客户端()
    {
        var fake = new FakeSoconBridgeClient();
        var mockMode = new Dictionary<string, string?> { ["Device:Mode"] = DeviceModes.Mock };
        await using var factory = CreateFactory(mockMode, fake);
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await OpenEngineeringSessionAsync(client, "mock");

        var response = await client.PostAsJsonAsync(OpenUrl, new { commandId = "cmd-open-mock", reason = "mock mode" });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal(0, fake.OpenCalls);
    }

    [Fact]
    public async Task 硬件不可用_Open_fail_closed_不调用真实客户端()
    {
        var fake = new FakeSoconBridgeClient();
        var hwUnavailable = new Dictionary<string, string?>
        {
            ["Device:Mode"] = DeviceModes.Real,
            ["Device:HardwareAvailable"] = "false",
            ["Device:UseMockWhenHardwareUnavailable"] = "false"
        };
        await using var factory = CreateFactory(hwUnavailable, fake);
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await OpenEngineeringSessionAsync(client, "hw-unavailable");

        var response = await client.PostAsJsonAsync(OpenUrl, new { commandId = "cmd-open-hw", reason = "hw unavailable" });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal(0, fake.OpenCalls);
    }

    [Fact]
    public async Task 重复Open_不同commandId_每次调用Bridge()
    {
        var fake = new FakeSoconBridgeClient(openSelector: call => call == 1
            ? SoconBridgeResponseResult.Completed("req", "OpenConfiguredReadOnlySession", true, "DeploymentValidated", "SessionOpen", null, [], null)
            : SoconBridgeResponseResult.Completed("req", "OpenConfiguredReadOnlySession", false, "SessionOpen", "SessionAlreadyOpen", null, [], null));
        await using var factory = CreateFactory(RealHardwareAvailable, fake);
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await OpenEngineeringSessionAsync(client, "repeat");

        var first = await client.PostAsJsonAsync(OpenUrl, new { commandId = "cmd-open-repeat-1", reason = "first open" });
        var second = await client.PostAsJsonAsync(OpenUrl, new { commandId = "cmd-open-repeat-2", reason = "second open" });
        var firstBody = await ReadOkAsync<RobotArmConnectionResponse>(first);
        var secondBody = await ReadOkAsync<RobotArmConnectionResponse>(second);

        Assert.Equal(RobotArmConnectionOutcome.Success, firstBody.Outcome);
        Assert.Equal(RobotArmConnectionOutcome.Failure, secondBody.Outcome);
        Assert.Equal("SessionAlreadyOpen", secondBody.Message);
        Assert.Equal(2, fake.OpenCalls);
    }

    [Fact]
    public async Task 同一commandId重放_不再次调用Bridge且标记Replayed()
    {
        var fake = new FakeSoconBridgeClient();
        await using var factory = CreateFactory(RealHardwareAvailable, fake);
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await OpenEngineeringSessionAsync(client, "replay");

        var first = await client.PostAsJsonAsync(OpenUrl, new { commandId = "cmd-open-replay", reason = "open" });
        var replayed = await client.PostAsJsonAsync(OpenUrl, new { commandId = "cmd-open-replay", reason = "open" });
        var firstBody = await ReadOkAsync<RobotArmConnectionResponse>(first);
        var replayedBody = await ReadOkAsync<RobotArmConnectionResponse>(replayed);

        Assert.False(firstBody.Replayed);
        Assert.True(replayedBody.Replayed);
        Assert.Equal(1, fake.OpenCalls);
    }

    [Fact]
    public async Task GET状态_Real模式_调用GetBridgeStatus()
    {
        var fake = new FakeSoconBridgeClient();
        await using var factory = CreateFactory(RealHardwareAvailable, fake);
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");

        var response = await client.GetAsync(StatusUrl);
        var body = await ReadOkAsync<RobotArmConnectionResponse>(response);

        Assert.Equal(RobotArmConnectionOutcome.Success, body.Outcome);
        Assert.Equal(1, fake.StatusCalls);
    }

    [Fact]
    public async Task GET状态_Mock模式_fail_closed()
    {
        var fake = new FakeSoconBridgeClient();
        await using var factory = CreateFactory(new Dictionary<string, string?> { ["Device:Mode"] = DeviceModes.Mock }, fake);
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");

        var response = await client.GetAsync(StatusUrl);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal(0, fake.StatusCalls);
    }

    [Fact]
    public async Task 传输错误_响应体不泄漏敏感路径()
    {
        var fake = new FakeSoconBridgeClient(openSelector: _ => SoconBridgeResponseResult.ForStatus(
            SoconBridgeExchangeStatus.PipeUnavailable, "req", "Access to C:\\Users\\secret\\SoconBridge.config.local.json denied"));
        await using var factory = CreateFactory(RealHardwareAvailable, fake);
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await OpenEngineeringSessionAsync(client, "leak");

        var response = await client.PostAsJsonAsync(OpenUrl, new { commandId = "cmd-open-leak", reason = "leak check" });
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.DoesNotContain("secret", body);
        Assert.DoesNotContain("config.local.json", body);
        Assert.DoesNotContain("C:", body);
    }

    [Fact]
    public async Task DI解析_注册真实NamedPipe客户端与连接服务()
    {
        await using var factory = CreateFactory(new Dictionary<string, string?> { ["Device:Mode"] = DeviceModes.Mock }, fakeClient: null);
        using var _ = factory.CreateClient();
        await using var scope = factory.Services.CreateAsyncScope();

        var bridgeClient = scope.ServiceProvider.GetRequiredService<ISoconBridgeClient>();
        var options = scope.ServiceProvider.GetRequiredService<SoconBridgeClientOptions>();
        var connectionService = scope.ServiceProvider.GetRequiredService<RobotArmConnectionService>();

        Assert.IsType<NamedPipeSoconBridgeClient>(bridgeClient);
        Assert.Equal(SoconBridgeTransport.DefaultPipeName, options.PipeName);
        Assert.NotNull(connectionService);
    }

    private const string OpenUrl = "/api/engineering/robot-arm/read-only-session/open";
    private const string CloseUrl = "/api/engineering/robot-arm/read-only-session/close";
    private const string StatusUrl = "/api/engineering/robot-arm/read-only-session/status";

    private static readonly Dictionary<string, string?> RealHardwareAvailable = new()
    {
        ["Device:Mode"] = DeviceModes.Real,
        ["Device:HardwareAvailable"] = "true",
        ["Device:UseMockWhenHardwareUnavailable"] = "false",
        ["Device:DebugMode"] = "false"
    };

    private static WebApplicationFactory<Program> CreateFactory(
        Dictionary<string, string?>? overrides = null,
        FakeSoconBridgeClient? fakeClient = null)
    {
        var databasePath = Path.Combine(TestPaths.TempRoot, "stainer-robot-arm-connection-tests", Guid.NewGuid().ToString("N"), "stainer.db");
        var settings = new Dictionary<string, string?>
        {
            ["ConnectionStrings:StainerDatabase"] = $"Data Source={databasePath}",
            ["MachineExecutor:LeasePath"] = Path.Combine(Path.GetDirectoryName(databasePath)!, $"machine-executor-{Guid.NewGuid():N}.lock"),
            ["Safety:LogDirectory"] = Path.Combine(Path.GetDirectoryName(databasePath)!, "logs"),
            ["Device:Mode"] = DeviceModes.Mock,
            ["Device:StartupInitialization:Enabled"] = "false"
        };
        if (overrides is not null)
        {
            foreach (var pair in overrides)
            {
                settings[pair.Key] = pair.Value;
            }
        }

        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            foreach (var pair in settings)
            {
                builder.UseSetting(pair.Key, pair.Value);
            }

            builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(settings));
            if (fakeClient is not null)
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<ISoconBridgeClient>();
                    services.AddSingleton<ISoconBridgeClient>(fakeClient);
                });
            }
        });
    }

    private static async Task LoginAsync(HttpClient client, string role)
    {
        var response = await client.PostAsJsonAsync("/api/login", new { username = "admin", password = "123456", role });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static async Task OpenEngineeringSessionAsync(HttpClient client, string suffix)
    {
        var response = await client.PostAsJsonAsync("/api/engineering/session", new
        {
            commandId = $"cmd-eng-session-{suffix}",
            password = "123456",
            reason = $"robot-arm connection test {suffix}",
            target = "socon-bridge-read-only-session"
        });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static async Task<T> ReadOkAsync<T>(HttpResponseMessage response)
    {
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<T>();
        Assert.NotNull(body);
        return body!;
    }

    // 假 ISoconBridgeClient：记录调用次数，按调用序号返回脚本化结果。绝不接触真实管道/SDK/硬件。
    private sealed class FakeSoconBridgeClient : ISoconBridgeClient
    {
        private readonly Func<int, SoconBridgeResponseResult> _openSelector;
        private readonly Func<int, SoconBridgeResponseResult> _closeSelector;
        private readonly Func<int, SoconBridgeResponseResult> _statusSelector;

        public int OpenCalls;
        public int CloseCalls;
        public int StatusCalls;

        public FakeSoconBridgeClient(
            Func<int, SoconBridgeResponseResult>? openSelector = null,
            Func<int, SoconBridgeResponseResult>? closeSelector = null,
            Func<int, SoconBridgeResponseResult>? statusSelector = null)
        {
            _openSelector = openSelector ?? (_ => SoconBridgeResponseResult.Completed(
                "req", "OpenConfiguredReadOnlySession", true, "DeploymentValidated", "SessionOpen", null, [], null));
            _closeSelector = closeSelector ?? (_ => SoconBridgeResponseResult.Completed(
                "req", "CloseConfiguredReadOnlySession", true, "SessionClosed", "SessionClosed", null, [], null));
            _statusSelector = statusSelector ?? (_ => SoconBridgeResponseResult.Completed(
                "req", "GetBridgeStatus", true, "Offline", "OK", null, [], null));
        }

        public Task<SoconBridgeResponseResult> OpenConfiguredReadOnlySessionAsync(CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref OpenCalls);
            return Task.FromResult(_openSelector(OpenCalls));
        }

        public Task<SoconBridgeResponseResult> CloseConfiguredReadOnlySessionAsync(CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref CloseCalls);
            return Task.FromResult(_closeSelector(CloseCalls));
        }

        public Task<SoconBridgeResponseResult> GetBridgeStatusAsync(CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref StatusCalls);
            return Task.FromResult(_statusSelector(StatusCalls));
        }

        public Task<SoconBridgeResponseResult> PingAsync(CancellationToken cancellationToken) =>
            GetBridgeStatusAsync(cancellationToken);
    }
}
