using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Services;
using Stainer.Web.Domain.Entities;

namespace Stainer.Tests;

/// <summary>
/// 覆盖本轮新增的 fluidics 契约：POST /api/fluidics/wash-stop 与 POST /api/fluidics/level-thresholds。
/// </summary>
public sealed class FluidicsWashStopAndThresholdTests
{
    [Fact]
    public async Task Wash_stop_stops_pwm0_and_is_idempotent()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin", "admin");

        // 先发起一次 wash（由 PWM0 驱动）
        var washResp = await client.PostAsJsonAsync("/api/fluidics/wash", new
        {
            commandId = "cmd-washstop-wash",
            targetPointCode = "WashInnerLeft",
            speedPercent = 60,
            durationMs = 2000,
            reason = "wash before stop"
        });
        var washBody = await washResp.Content.ReadAsStringAsync();
        Assert.True(washResp.IsSuccessStatusCode, $"wash setup failed: {(int)washResp.StatusCode} {washBody}");

        var stopResp = await client.PostAsJsonAsync("/api/fluidics/wash-stop", new
        {
            commandId = "cmd-washstop-stop",
            reason = "stop sample wash"
        });
        Assert.True(stopResp.IsSuccessStatusCode, $"wash-stop failed: {(int)stopResp.StatusCode}");
        var stopped = await stopResp.Content.ReadFromJsonAsync<FluidicsMutationResponse>();
        Assert.NotNull(stopped);
        var pwm0 = stopped!.State.Pumps.Single(x => x.PwmChannelCode == "PWM0");
        Assert.Equal(0, pwm0.SpeedPercent);
        Assert.Equal(FluidicsStatuses.Stopped, pwm0.Status);

        // 幂等：相同 commandId + 相同 body 重放，应返回 Replayed=true 且状态不变
        var replayResp = await client.PostAsJsonAsync("/api/fluidics/wash-stop", new
        {
            commandId = "cmd-washstop-stop",
            reason = "stop sample wash"
        });
        Assert.True(replayResp.IsSuccessStatusCode, $"wash-stop replay failed: {(int)replayResp.StatusCode}");
        var replay = await replayResp.Content.ReadFromJsonAsync<FluidicsMutationResponse>();
        Assert.NotNull(replay);
        Assert.True(replay!.Replayed, "wash-stop replay should be idempotent");
        Assert.Equal(FluidicsStatuses.Stopped, replay.State.Pumps.Single(x => x.PwmChannelCode == "PWM0").Status);
    }

    [Fact]
    public async Task Wash_stop_requires_admin_role()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "operator", "operator");

        var forbidden = await client.PostAsJsonAsync("/api/fluidics/wash-stop", new
        {
            commandId = "cmd-washstop-forbidden",
            reason = "operator cannot stop wash"
        });
        Assert.Equal(HttpStatusCode.Forbidden, forbidden.StatusCode);
    }

    [Fact]
    public async Task Level_thresholds_update_thresholds_without_changing_current_volume()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin", "admin");

        var before = await client.GetFromJsonAsync<FluidicsStateResponse>("/api/fluidics/state");
        var waterBefore = before!.LiquidLevels.Single(x => x.SourceType == LiquidSourceTypes.SystemWater);

        var updated = await PostJsonAsync<FluidicsMutationResponse>(client, "/api/fluidics/level-thresholds", new
        {
            commandId = "cmd-threshold-set",
            sourceType = LiquidSourceTypes.SystemWater,
            lowThresholdUl = 150000,
            fullThresholdUl = 950000,
            reason = "adjust thresholds"
        });
        var waterAfter = updated.State.LiquidLevels.Single(x => x.SourceType == LiquidSourceTypes.SystemWater);
        Assert.Equal(150000, waterAfter.LowThresholdUl);
        Assert.Equal(950000, waterAfter.FullThresholdUl);
        // 阈值配置不得改动实际容量与当前液量
        Assert.Equal(waterBefore.CapacityUl, waterAfter.CapacityUl);
        Assert.Equal(waterBefore.CurrentVolumeUl, waterAfter.CurrentVolumeUl);
    }

    [Fact]
    public async Task Level_thresholds_clamp_negative_and_over_capacity_uniformly()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin", "admin");

        var baseline = await client.GetFromJsonAsync<FluidicsStateResponse>("/api/fluidics/state");
        var capacity = baseline!.LiquidLevels.Single(x => x.SourceType == LiquidSourceTypes.SystemWater).CapacityUl;

        var negLow = await PostJsonAsync<FluidicsMutationResponse>(client, "/api/fluidics/level-thresholds", new
        {
            commandId = "cmd-threshold-neg",
            sourceType = LiquidSourceTypes.SystemWater,
            lowThresholdUl = -50,
            reason = "negative low clamps to 0"
        });
        Assert.Equal(0, negLow.State.LiquidLevels.Single(x => x.SourceType == LiquidSourceTypes.SystemWater).LowThresholdUl);

        var overFull = await PostJsonAsync<FluidicsMutationResponse>(client, "/api/fluidics/level-thresholds", new
        {
            commandId = "cmd-threshold-over-full",
            sourceType = LiquidSourceTypes.SystemWater,
            fullThresholdUl = capacity + 999999,
            reason = "full over capacity clamps to capacity"
        });
        Assert.Equal(capacity, overFull.State.LiquidLevels.Single(x => x.SourceType == LiquidSourceTypes.SystemWater).FullThresholdUl);

        // low 同样截断到 capacity（此前不一致：low 不截断）
        var overLow = await PostJsonAsync<FluidicsMutationResponse>(client, "/api/fluidics/level-thresholds", new
        {
            commandId = "cmd-threshold-over-low",
            sourceType = LiquidSourceTypes.SystemWater,
            lowThresholdUl = capacity + 999999,
            reason = "low over capacity clamps to capacity"
        });
        Assert.Equal(capacity, overLow.State.LiquidLevels.Single(x => x.SourceType == LiquidSourceTypes.SystemWater).LowThresholdUl);
    }

    private static WebApplicationFactory<Program> CreateFactory()
    {
        var root = Path.Combine(TestPaths.TempRoot, "stainer-washstop-tests", Guid.NewGuid().ToString("N"));
        var settings = new Dictionary<string, string?>
        {
            ["ConnectionStrings:StainerDatabase"] = $"Data Source={Path.Combine(root, "stainer.db")}",
            ["MachineExecutor:LeasePath"] = Path.Combine(root, $"machine-executor-{Guid.NewGuid():N}.lock"),
            ["Safety:LogDirectory"] = Path.Combine(root, "logs"),
            ["Device:Mode"] = "Mock",
            ["Device:HardwareAvailable"] = "false",
            ["Device:StartupInitialization:Enabled"] = "false"
        };
        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            foreach (var pair in settings)
            {
                builder.UseSetting(pair.Key, pair.Value);
            }
            builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(settings));
        });
    }

    private static async Task LoginAsync(HttpClient client, string username, string role)
    {
        var response = await client.PostAsJsonAsync("/api/login", new { username, password = "123456", role });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static async Task<T> PostJsonAsync<T>(HttpClient client, string url, object request)
    {
        var response = await client.PostAsJsonAsync(url, request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<T>();
        Assert.NotNull(body);
        return body!;
    }
}
