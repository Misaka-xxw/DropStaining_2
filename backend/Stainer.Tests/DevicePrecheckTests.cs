using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Tests;

/// <summary>
/// 数字孪生“检测”真实预检契约测试：覆盖一键/单项接口、阈值边界、单项故障隔离、
/// commandId 幂等、运行中动作互锁、只读 preflight 不触发硬件动作、以及 11 绿但业务 issue 失败时 canStart=false。
/// </summary>
public sealed class DevicePrecheckTests
{
    private static readonly string[] ExpectedCheckIds =
    [
        "device.controller.connected",
        "motion.arm.home",
        "thermal.cooling.connected",
        "scanner.sample.online",
        "scanner.reagent.online",
        "fluidics.level-sensors.readable",
        "motion.needle-wash.ready",
        "fluidics.system-water.available",
        "fluidics.pbs.available",
        "fluidics.waste.not-full",
        "fluidics.toxic-waste.not-full"
    ];

    [Fact]
    public async Task Run_all_returns_eleven_unique_passing_checks_in_seeded_mock()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");

        var report = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-precheck-all-1" });

        var ids = report.Checks.Select(c => c.CheckId).ToList();
        Assert.Equal(11, ids.Count);
        Assert.Equal(ExpectedCheckIds, ids);
        Assert.Equal(11, ids.Distinct().Count());
        Assert.All(report.Checks, c =>
        {
            Assert.True(c.Blocking);
            Assert.False(string.IsNullOrWhiteSpace(c.Label));
            Assert.False(string.IsNullOrWhiteSpace(c.Category));
            Assert.Equal(PrecheckStatuses.Passed, c.Status); // 全量真实检查在已 seed 的 Mock 环境下应全部通过
        });
        Assert.True(report.Ok);
        Assert.False(string.IsNullOrWhiteSpace(report.ReportId));
    }

    [Fact]
    public async Task Run_one_returns_only_the_target_check()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-seed-runone" });

        var one = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks/fluidics.system-water.available", new { commandId = "cmd-one-water" });

        Assert.Single(one.Checks);
        Assert.Equal("fluidics.system-water.available", one.Checks[0].CheckId);
        Assert.Equal(PrecheckStatuses.Passed, one.Checks[0].Status);
    }

    [Fact]
    public async Task Unknown_check_id_returns_explicit_business_error()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");

        var resp = await client.PostAsJsonAsync("/api/prechecks/no.such.check", new { commandId = "cmd-unknown" });
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        var body = await resp.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.NotNull(body);
        Assert.Equal("precheck_check_unknown", body!["code"]);
    }

    [Fact]
    public async Task Thermal_fault_only_fails_cooling_check_in_preflight()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-seed-thermal" });

        var fault = await client.PostAsJsonAsync("/api/thermal/faults", new
        {
            commandId = "cmd-cooling-fault",
            targetType = "Cooling",
            faultType = "Disconnected",
            reason = "cooling offline isolation test"
        });
        Assert.True(fault.IsSuccessStatusCode);

        var preflight = await GetJsonAsync<PreflightValidationReportResponse>(client, "/api/run/preflight");
        var byId = preflight.Checks!.ToDictionary(c => c.CheckId);

        Assert.NotEqual(PrecheckStatuses.Passed, byId["thermal.cooling.connected"].Status);
        Assert.Equal(PrecheckStatuses.Passed, byId["motion.arm.home"].Status);
        Assert.Equal(PrecheckStatuses.Passed, byId["fluidics.system-water.available"].Status);
        Assert.Equal(PrecheckStatuses.Passed, byId["motion.needle-wash.ready"].Status);
        Assert.False(preflight.CanStart);
    }

    [Fact]
    public async Task Motion_arm_not_homed_only_fails_arm_check_in_preflight()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-seed-motion" });

        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            var arm = await db.RobotArmStates.SingleAsync();
            arm.IsHomed = false;
            await db.SaveChangesAsync();
        }

        var preflight = await GetJsonAsync<PreflightValidationReportResponse>(client, "/api/run/preflight");
        var byId = preflight.Checks!.ToDictionary(c => c.CheckId);

        Assert.NotEqual(PrecheckStatuses.Passed, byId["motion.arm.home"].Status);
        Assert.Equal(PrecheckStatuses.Passed, byId["thermal.cooling.connected"].Status);
        Assert.Equal(PrecheckStatuses.Passed, byId["fluidics.waste.not-full"].Status);
    }

    [Fact]
    public async Task Mock_arm_home_check_moves_arm_to_home_coordinates()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");

        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            db.RobotArmStates.Add(new RobotArmState
            {
                IsHomed = false,
                IsConnected = true,
                Status = MotionStatuses.Idle,
                CurrentTargetPointCode = "R23",
                CurrentXUm = 420_000,
                CurrentYUm = 210_000,
                CurrentZUm = 7_000
            });
            await db.SaveChangesAsync();
        }

        var report = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks/motion.arm.home", new { commandId = "cmd-arm-home-coordinates" });
        Assert.Equal(PrecheckStatuses.Passed, Assert.Single(report.Checks).Status);

        await using var verify = factory.Services.CreateAsyncScope();
        var verifyDb = verify.ServiceProvider.GetRequiredService<StainerDbContext>();
        var armAfter = await verifyDb.RobotArmStates.AsNoTracking().SingleAsync();
        Assert.True(armAfter.IsHomed);
        Assert.Equal("Home", armAfter.CurrentTargetPointCode);
        Assert.Equal(0, armAfter.CurrentXUm);
        Assert.Equal(0, armAfter.CurrentYUm);
        Assert.Equal(0, armAfter.CurrentZUm);
    }

    [Fact]
    public async Task Preflight_rejects_homed_flag_when_arm_coordinates_are_not_home()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-seed-home-coordinate-check" });

        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            var arm = await db.RobotArmStates.SingleAsync();
            arm.IsHomed = true;
            arm.CurrentTargetPointCode = "Home";
            arm.CurrentXUm = 1_000;
            await db.SaveChangesAsync();
        }

        var preflight = await GetJsonAsync<PreflightValidationReportResponse>(client, "/api/run/preflight");
        var armCheck = preflight.Checks!.Single(c => c.CheckId == "motion.arm.home");
        Assert.Equal(PrecheckStatuses.Failed, armCheck.Status);
        Assert.Equal("robot_arm_home_coordinate_invalid", armCheck.Code);
        Assert.False(preflight.CanStart);
    }

    [Fact]
    public async Task Needle_needs_wash_or_has_volume_fails_in_preflight()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-seed-needle" });

        // NeedsWash=true → 洗针检查失败（只读路径不洗针）
        await MutateNeedleAsync(factory, n => n.NeedsWash = true);
        var preflight1 = await GetJsonAsync<PreflightValidationReportResponse>(client, "/api/run/preflight");
        Assert.NotEqual(PrecheckStatuses.Passed, preflight1.Checks!.Single(c => c.CheckId == "motion.needle-wash.ready").Status);

        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-reseed-needle" }); // 重新洗针复位

        // VolumeUl>0 → 洗针检查失败
        await MutateNeedleAsync(factory, n => { n.VolumeUl = 50; n.NeedsWash = false; });
        var preflight2 = await GetJsonAsync<PreflightValidationReportResponse>(client, "/api/run/preflight");
        Assert.NotEqual(PrecheckStatuses.Passed, preflight2.Checks!.Single(c => c.CheckId == "motion.needle-wash.ready").Status);
    }

    [Fact]
    public async Task Liquid_threshold_boundaries_fail_at_or_beyond_limits()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-seed-thresholds" });

        // 纯水：CurrentVolumeUl == LowThresholdUl → 失败（边界 <=）
        await SetLiquidVolumeAsync(factory, LiquidSourceTypes.SystemWater, await GetThresholdAsync(factory, LiquidSourceTypes.SystemWater, low: true));
        Assert.Equal(PrecheckStatuses.Failed, await RunOneStatusAsync(client, "fluidics.system-water.available", "cmd-water-eq-low"));

        // 纯水：CurrentVolumeUl == LowThresholdUl + 1 → 通过
        await SetLiquidVolumeAsync(factory, LiquidSourceTypes.SystemWater, await GetThresholdAsync(factory, LiquidSourceTypes.SystemWater, low: true) + 1);
        Assert.Equal(PrecheckStatuses.Passed, await RunOneStatusAsync(client, "fluidics.system-water.available", "cmd-water-above-low"));

        // PBS：等于低阈值 → 失败
        await SetLiquidVolumeAsync(factory, LiquidSourceTypes.Pbs, await GetThresholdAsync(factory, LiquidSourceTypes.Pbs, low: true));
        Assert.Equal(PrecheckStatuses.Failed, await RunOneStatusAsync(client, "fluidics.pbs.available", "cmd-pbs-eq-low"));

        // 废液：CurrentVolumeUl == FullThresholdUl → 失败（边界 >=）
        await SetLiquidVolumeAsync(factory, LiquidSourceTypes.Waste, await GetThresholdAsync(factory, LiquidSourceTypes.Waste, low: false));
        Assert.Equal(PrecheckStatuses.Failed, await RunOneStatusAsync(client, "fluidics.waste.not-full", "cmd-waste-eq-full"));

        // 废液：FullThresholdUl - 1 → 通过
        await SetLiquidVolumeAsync(factory, LiquidSourceTypes.Waste, await GetThresholdAsync(factory, LiquidSourceTypes.Waste, low: false) - 1);
        Assert.Equal(PrecheckStatuses.Passed, await RunOneStatusAsync(client, "fluidics.waste.not-full", "cmd-waste-below-full"));

        // 排毒桶：等于满阈值 → 失败
        await SetLiquidVolumeAsync(factory, LiquidSourceTypes.ToxicWaste, await GetThresholdAsync(factory, LiquidSourceTypes.ToxicWaste, low: false));
        Assert.Equal(PrecheckStatuses.Failed, await RunOneStatusAsync(client, "fluidics.toxic-waste.not-full", "cmd-toxic-eq-full"));
    }

    [Fact]
    public async Task Missing_or_disconnected_liquid_data_never_passes()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-seed-missing" });

        // 缺数据：删除纯水源 → Unavailable（绝不能默认通过）
        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            db.LiquidContainerStates.RemoveRange(db.LiquidContainerStates.Where(x => x.SourceType == LiquidSourceTypes.SystemWater));
            await db.SaveChangesAsync();
        }
        Assert.Equal(PrecheckStatuses.Unavailable, await RunOneStatusAsync(client, "fluidics.system-water.available", "cmd-water-missing"));

        // 断连/故障：PBS 断连 → Failed
        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            var pbs = await db.LiquidContainerStates.SingleAsync(x => x.SourceType == LiquidSourceTypes.Pbs);
            pbs.IsConnected = false;
            pbs.FaultCode = "sensor_offline";
            await db.SaveChangesAsync();
        }
        Assert.Equal(PrecheckStatuses.Failed, await RunOneStatusAsync(client, "fluidics.pbs.available", "cmd-pbs-disconnected"));
    }

    [Fact]
    public async Task Same_command_id_is_idempotent_and_does_not_repeat_actions()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");

        var first = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-idempotent" });
        var replay = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-idempotent" });

        Assert.Equal(first.ReportId, replay.ReportId); // 重放返回同一份报告，未重新执行
        await using var scope = factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
        Assert.Equal(1, await db.CommandReceipts.CountAsync(x => x.CommandId == "cmd-idempotent"));
        // 机械臂回零只应发生一次（重放不再回零）
        Assert.Equal(1, await db.PipettingOperations.CountAsync(x => x.OperationType == PipettingOperationTypes.Home));
    }

    [Fact]
    public async Task Active_run_blocks_arm_home_and_needle_wash_actions()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-seed-interlock" });

        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            db.MachineRuns.Add(new MachineRun { Status = RuntimeLedgerStatus.Running, RunCode = "RUN-1" });
            await db.SaveChangesAsync();
        }

        var arm = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks/motion.arm.home", new { commandId = "cmd-arm-blocked" });
        Assert.Single(arm.Checks);
        Assert.Equal(PrecheckStatuses.Failed, arm.Checks[0].Status);
        Assert.Equal("precheck_action_blocked_run_active", arm.Checks[0].Code);

        var needle = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks/motion.needle-wash.ready", new { commandId = "cmd-needle-blocked" });
        Assert.Equal(PrecheckStatuses.Failed, needle.Checks[0].Status);
        Assert.Equal("precheck_action_blocked_run_active", needle.Checks[0].Code);

        // 只读类检查在运行中仍可执行（不触发危险动作）
        var cooling = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks/thermal.cooling.connected", new { commandId = "cmd-cooling-during-run" });
        Assert.Equal(PrecheckStatuses.Passed, cooling.Checks[0].Status);
    }

    [Fact]
    public async Task Preflight_is_read_only_and_does_not_home_the_arm()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-seed-readonly" });

        // 强制机械臂未回零
        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            var arm = await db.RobotArmStates.SingleAsync();
            arm.IsHomed = false;
            await db.SaveChangesAsync();
        }

        var preflight = await GetJsonAsync<PreflightValidationReportResponse>(client, "/api/run/preflight");
        Assert.NotEqual(PrecheckStatuses.Passed, preflight.Checks!.Single(c => c.CheckId == "motion.arm.home").Status);

        // preflight 必须保持只读：机械臂依然未回零（未触发回零动作）
        await using var verify = factory.Services.CreateAsyncScope();
        var verifyDb = verify.ServiceProvider.GetRequiredService<StainerDbContext>();
        var armAfter = await verifyDb.RobotArmStates.AsNoTracking().SingleAsync();
        Assert.False(armAfter.IsHomed);
    }

    [Fact]
    public async Task All_checks_pass_but_business_issues_keep_canStart_false()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        // 主动检测使 11 项设备检查全部就绪（回零、洗针等）
        var precheck = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-allgreen" });
        Assert.All(precheck.Checks, c => Assert.Equal(PrecheckStatuses.Passed, c.Status));

        var preflight = await GetJsonAsync<PreflightValidationReportResponse>(client, "/api/run/preflight");
        // 11 项设备检查全绿
        Assert.All(preflight.Checks!, c => Assert.Equal(PrecheckStatuses.Passed, c.Status));
        // 但业务条件未满足（无已确认染色任务/无试剂扫码）→ canStart 必须为 false
        Assert.False(preflight.CanStart);
        Assert.Contains(preflight.Issues, x => !string.Equals(x.Severity, "Warning", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Paused_run_also_blocks_arm_home_and_needle_wash()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-seed-paused" });

        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            db.MachineRuns.Add(new MachineRun { Status = RuntimeLedgerStatus.Paused, RunCode = "RUN-PAUSED" });
            await db.SaveChangesAsync();
        }

        var arm = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks/motion.arm.home", new { commandId = "cmd-arm-paused" });
        Assert.Equal(PrecheckStatuses.Failed, arm.Checks[0].Status);
        Assert.Equal("precheck_action_blocked_run_active", arm.Checks[0].Code);

        var needle = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks/motion.needle-wash.ready", new { commandId = "cmd-needle-paused" });
        Assert.Equal("precheck_action_blocked_run_active", needle.Checks[0].Code);
    }

    [Fact]
    public async Task Preflight_does_not_pass_controller_or_scanners_without_a_probe_record()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");

        // 既未一键检测也未设备初始化：无任何探测记录，Mock 也不再无条件判通过
        var preflight = await GetJsonAsync<PreflightValidationReportResponse>(client, "/api/run/preflight");
        var byId = preflight.Checks!.ToDictionary(c => c.CheckId);
        Assert.NotEqual(PrecheckStatuses.Passed, byId["device.controller.connected"].Status);
        Assert.NotEqual(PrecheckStatuses.Passed, byId["scanner.sample.online"].Status);
        Assert.NotEqual(PrecheckStatuses.Passed, byId["scanner.reagent.online"].Status);
    }

    [Fact]
    public async Task Single_check_does_not_unlock_controller_or_scanner_in_preflight()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");

        // 仅做单项（单项检测不写完整报告）
        var one = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks/thermal.cooling.connected", new { commandId = "cmd-single-cooling" });
        Assert.Equal(PrecheckStatuses.Passed, one.Checks[0].Status);

        var preflight = await GetJsonAsync<PreflightValidationReportResponse>(client, "/api/run/preflight");
        var byId = preflight.Checks!.ToDictionary(c => c.CheckId);
        Assert.NotEqual(PrecheckStatuses.Passed, byId["device.controller.connected"].Status);
        Assert.NotEqual(PrecheckStatuses.Passed, byId["scanner.sample.online"].Status);
    }

    [Fact]
    public async Task Run_all_then_preflight_reads_controller_and_scanners_from_report()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-all-report" });

        var preflight = await GetJsonAsync<PreflightValidationReportResponse>(client, "/api/run/preflight");
        var byId = preflight.Checks!.ToDictionary(c => c.CheckId);
        Assert.Equal(PrecheckStatuses.Passed, byId["device.controller.connected"].Status);
        Assert.Equal(PrecheckStatuses.Passed, byId["scanner.sample.online"].Status);
        Assert.Equal(PrecheckStatuses.Passed, byId["scanner.reagent.online"].Status);
    }

    [Fact]
    public async Task Single_failed_check_invalidates_stale_full_report()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");

        // 1) 一键检测全部通过，保存全量报告
        var all = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-stale-all" });
        Assert.All(all.Checks, c => Assert.Equal(PrecheckStatuses.Passed, c.Status));

        // 2) 让试剂扫码器探测失败（Mock 注入持续故障）
        var fault = await client.PostAsJsonAsync("/api/device/mock-faults", new
        {
            commandId = "cmd-reagent-fault",
            moduleCode = "reagent-scanner",
            faultType = "PersistentModuleFailure",
            reason = "reagent scanner offline regression"
        });
        Assert.True(fault.IsSuccessStatusCode, $"scanner fault setup failed: {(int)fault.StatusCode}");

        // 3) 单项复测试剂扫码器 → 失败（同时使旧全量报告失效）
        var one = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks/scanner.reagent.online", new { commandId = "cmd-stale-single" });
        Assert.NotEqual(PrecheckStatuses.Passed, one.Checks[0].Status);

        // 4) preflight：旧报告已失效，试剂扫码器不再 Passed，且 ok=false / canStart=false
        var preflight = await GetJsonAsync<PreflightValidationReportResponse>(client, "/api/run/preflight");
        var reagent = preflight.Checks!.Single(c => c.CheckId == "scanner.reagent.online");
        Assert.NotEqual(PrecheckStatuses.Passed, reagent.Status);
        Assert.False(preflight.Ok);
        Assert.False(preflight.CanStart);
    }

    [Fact]
    public async Task Level_sensors_fail_when_required_container_missing()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-seed-levelsensor" });

        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            db.LiquidContainerStates.RemoveRange(db.LiquidContainerStates.Where(x => x.SourceType == LiquidSourceTypes.Pbs));
            await db.SaveChangesAsync();
        }

        var one = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks/fluidics.level-sensors.readable", new { commandId = "cmd-level-missing" });
        Assert.NotEqual(PrecheckStatuses.Passed, one.Checks[0].Status);
    }

    [Fact]
    public async Task Mock_level_sensor_read_refreshes_stale_sample_timestamps()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-seed-stale-levels" });

        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            var liquids = await db.LiquidContainerStates.ToListAsync();
            foreach (var liquid in liquids)
            {
                liquid.UpdatedAtUtc = DateTimeOffset.UtcNow.AddDays(-2);
            }
            await db.SaveChangesAsync();
        }

        var readStartedAt = DateTimeOffset.UtcNow;
        var one = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks/fluidics.level-sensors.readable", new { commandId = "cmd-refresh-stale-levels" });
        Assert.Equal(PrecheckStatuses.Passed, one.Checks[0].Status);

        await using var verify = factory.Services.CreateAsyncScope();
        var verifyDb = verify.ServiceProvider.GetRequiredService<StainerDbContext>();
        var refreshed = await verifyDb.LiquidContainerStates.AsNoTracking().ToListAsync();
        Assert.NotEmpty(refreshed);
        Assert.All(refreshed, liquid => Assert.True(liquid.UpdatedAtUtc >= readStartedAt));
    }

    [Fact]
    public async Task Level_sensors_fail_on_out_of_range_reading()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-seed-oor" });

        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            var water = await db.LiquidContainerStates.SingleAsync(x => x.SourceType == LiquidSourceTypes.SystemWater);
            water.CurrentVolumeUl = water.CapacityUl + 1000; // 越界（> 容量），且不修正 LevelStatus，验证严格区间校验
            await db.SaveChangesAsync();
        }

        var one = await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks/fluidics.level-sensors.readable", new { commandId = "cmd-level-oor" });
        Assert.NotEqual(PrecheckStatuses.Passed, one.Checks[0].Status);
    }

    [Fact]
    public async Task Preflight_ok_and_canStart_are_consistent()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin");
        await PostJsonAsync<PrecheckReportResponse>(client, "/api/prechecks", new { commandId = "cmd-ok-canstart" });

        var preflight = await GetJsonAsync<PreflightValidationReportResponse>(client, "/api/run/preflight");
        Assert.Equal(preflight.CanStart, preflight.Ok); // 两者始终一致，不再出现 ok=true/canStart=false
        Assert.False(preflight.CanStart); // 11 绿但业务 issue 未通过
    }

    private static async Task<string> RunOneStatusAsync(HttpClient client, string checkId, string commandId)
    {
        var one = await PostJsonAsync<PrecheckReportResponse>(client, $"/api/prechecks/{checkId}", new { commandId });
        return one.Checks.Single().Status;
    }

    private static async Task MutateNeedleAsync(WebApplicationFactory<Program> factory, Action<NeedleState> mutate)
    {
        await using var scope = factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
        foreach (var needle in await db.NeedleStates.ToListAsync())
        {
            mutate(needle);
        }
        await db.SaveChangesAsync();
    }

    private static async Task SetLiquidVolumeAsync(WebApplicationFactory<Program> factory, string sourceType, int volumeUl)
    {
        await using var scope = factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
        var liquid = await db.LiquidContainerStates.SingleAsync(x => x.SourceType == sourceType);
        liquid.CurrentVolumeUl = Math.Clamp(volumeUl, 0, liquid.CapacityUl);
        await db.SaveChangesAsync();
    }

    private static async Task<int> GetThresholdAsync(WebApplicationFactory<Program> factory, string sourceType, bool low)
    {
        await using var scope = factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
        var liquid = await db.LiquidContainerStates.AsNoTracking().SingleAsync(x => x.SourceType == sourceType);
        return low ? liquid.LowThresholdUl : liquid.FullThresholdUl;
    }

    private static WebApplicationFactory<Program> CreateFactory()
    {
        var root = Path.Combine(TestPaths.TempRoot, "stainer-precheck-tests", Guid.NewGuid().ToString("N"));
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

    private static async Task LoginAsync(HttpClient client, string role)
    {
        var response = await client.PostAsJsonAsync("/api/login", new { username = role, password = "123456", role });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static async Task<T> PostJsonAsync<T>(HttpClient client, string url, object request)
    {
        var response = await client.PostAsJsonAsync(url, request);
        var body = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, $"{url} failed: {(int)response.StatusCode} {body}");
        var result = await response.Content.ReadFromJsonAsync<T>();
        Assert.NotNull(result);
        return result!;
    }

    private static async Task<T> GetJsonAsync<T>(HttpClient client, string url)
    {
        var response = await client.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, $"{url} failed: {(int)response.StatusCode} {body}");
        var result = await response.Content.ReadFromJsonAsync<T>();
        Assert.NotNull(result);
        return result!;
    }
}
