using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;
using Xunit;

namespace Stainer.Tests;

// 单元测试：验证机械臂原子动作按工艺规定的顺序驱动底层原语。
// 直接构造 MockRobotMotionPrimitives（记录调用顺序）+ 显式高度，不启动 Web host / DB，
// 聚焦“动作顺序”这一核心契约。
public sealed class RobotArmAtomicActionTests
{
    // 用一组确定的、互不相同的高度，便于断言每一步传入的 Z 值。
    private static readonly RobotArmAtomicHeights Heights = new()
    {
        AspirateZUm = 1_000,
        MixZUm = 2_000,
        DispenseZUm = 3_000,
        WashInnerZUm = 4_000,
        WashOuterZUm = 5_000,
        SafeZUm = 90_000
    };

    private static (RobotArmAtomicActionService service, MockRobotMotionPrimitives primitives) BuildSut()
    {
        var primitives = new MockRobotMotionPrimitives();
        var service = new RobotArmAtomicActionService(primitives, Heights);
        return (service, primitives);
    }

    [Fact]
    public async Task TakeLiquid_moves_to_aspirate_height_then_aspirates_then_returns_to_safe()
    {
        var (service, primitives) = BuildSut();
        var result = await service.TakeLiquidAsync(new TakeLiquidRequest("cmd-take", "Needle1", 100));
        Assert.True(result.Ok, result.Message);
        Assert.Equal(RobotAtomicActions.TakeLiquid, result.Action);
        Assert.Equal(
        [
            RobotPrimitiveCall.MoveZ(Heights.AspirateZUm),
            RobotPrimitiveCall.Aspirate(100),
            RobotPrimitiveCall.MoveZ(Heights.SafeZUm)
        ], primitives.Calls);
    }

    [Fact]
    public async Task PrepareMix_moves_to_mix_height_then_dispenses_then_returns_to_safe()
    {
        var (service, primitives) = BuildSut();
        var result = await service.PrepareMixAsync(new PrepareMixRequest("cmd-mix", "Needle1", 80));
        Assert.True(result.Ok, result.Message);
        Assert.Equal(
        [
            RobotPrimitiveCall.MoveZ(Heights.MixZUm),
            RobotPrimitiveCall.Dispense(80),
            RobotPrimitiveCall.MoveZ(Heights.SafeZUm)
        ], primitives.Calls);
    }

    [Fact]
    public async Task DispenseLiquid_moves_to_dispense_height_then_dispenses_then_returns_to_safe()
    {
        var (service, primitives) = BuildSut();
        var result = await service.DispenseLiquidAsync(new DispenseLiquidRequest("cmd-disp", "Needle1", 60));
        Assert.True(result.Ok, result.Message);
        Assert.Equal(
        [
            RobotPrimitiveCall.MoveZ(Heights.DispenseZUm),
            RobotPrimitiveCall.Dispense(60),
            RobotPrimitiveCall.MoveZ(Heights.SafeZUm)
        ], primitives.Calls);
    }

    [Fact]
    public async Task WashInner_moves_to_wash_height_then_aspirates_wash_then_dispenses_waste_then_returns_to_safe()
    {
        var (service, primitives) = BuildSut();
        var result = await service.WashInnerAsync(new WashInnerRequest("cmd-wash-inner", "Needle1", 200, 200));
        Assert.True(result.Ok, result.Message);
        Assert.Equal(
        [
            RobotPrimitiveCall.MoveZ(Heights.WashInnerZUm),
            RobotPrimitiveCall.Aspirate(200),
            RobotPrimitiveCall.Dispense(200),
            RobotPrimitiveCall.MoveZ(Heights.SafeZUm)
        ], primitives.Calls);
    }

    [Fact]
    public async Task WashOuter_moves_to_outer_wash_height_then_washes_then_returns_to_safe()
    {
        var (service, primitives) = BuildSut();
        var result = await service.WashOuterAsync(new WashOuterRequest("cmd-wash-outer", "Needle1"));
        Assert.True(result.Ok, result.Message);
        Assert.Equal(
        [
            RobotPrimitiveCall.MoveZ(Heights.WashOuterZUm),
            RobotPrimitiveCall.WashOuter(),
            RobotPrimitiveCall.MoveZ(Heights.SafeZUm)
        ], primitives.Calls);
    }

    [Fact]
    public async Task Each_action_records_semantic_step_trace_in_order()
    {
        var (service, _) = BuildSut();
        var result = await service.TakeLiquidAsync(new TakeLiquidRequest("cmd-trace", "Needle2", 50, "load ABC"));
        var names = result.Steps.Select(x => x.Name).ToArray();
        Assert.Equal(["MoveZ→吸液高度", "Aspirate", "MoveZ→安全高度"], names);
        Assert.Contains("Needle2", result.Message);
        Assert.Contains("load ABC", result.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task TakeLiquid_rejects_non_positive_volume(int volumeUl)
    {
        var (service, primitives) = BuildSut();
        var ex = await Assert.ThrowsAsync<BusinessRuleException>(
            () => service.TakeLiquidAsync(new TakeLiquidRequest("cmd-bad", "Needle1", volumeUl)));
        Assert.Equal("atomic_action_volume_invalid", ex.Code);
        Assert.Empty(primitives.Calls); // 校验失败不应触发任何原语
    }

    [Fact]
    public async Task WashInner_rejects_non_positive_waste_volume_without_calling_primitives()
    {
        var (service, primitives) = BuildSut();
        await Assert.ThrowsAsync<BusinessRuleException>(
            () => service.WashInnerAsync(new WashInnerRequest("cmd-bad-waste", "Needle1", 200, 0)));
        Assert.Empty(primitives.Calls);
    }

    [Fact]
    public async Task Actions_require_command_id()
    {
        var (service, primitives) = BuildSut();
        await Assert.ThrowsAsync<BusinessRuleException>(
            () => service.DispenseLiquidAsync(new DispenseLiquidRequest("   ", "Needle1", 10)));
        Assert.Empty(primitives.Calls);
    }

    // [P2] 任一原语抛异常时，安全高度回零仍必须执行（动作闭环强保证）。
    [Fact]
    public async Task Safe_height_return_runs_even_when_a_primitive_throws()
    {
        var primitives = new ThrowingRobotMotionPrimitives();
        var service = new RobotArmAtomicActionService(primitives, Heights);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.TakeLiquidAsync(new TakeLiquidRequest("cmd-throw", "Needle1", 100)));

        // 期望序列：MoveZ(吸液) -> Aspirate(抛) -> MoveZ(安全)（finally 兜底）。
        Assert.Equal(
        [
            RobotPrimitiveCall.MoveZ(Heights.AspirateZUm),
            RobotPrimitiveCall.Aspirate(100),
            RobotPrimitiveCall.MoveZ(Heights.SafeZUm)
        ], primitives.Calls);
    }

    // [P1] 注入 MockStateAtomicActionRecorder 后，原子动作应更新现有 Mock 运行状态与流水账。
    [Fact]
    public async Task TakeLiquid_updates_mock_runtime_state_and_ledger_via_recorder()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        var primitives = new MockRobotMotionPrimitives();
        var recorder = new MockStateAtomicActionRecorder(dbContext);
        var service = new RobotArmAtomicActionService(primitives, Heights, recorder);

        var result = await service.TakeLiquidAsync(new TakeLiquidRequest("cmd-take-record", "Needle1", 100, "load ABC"));
        Assert.True(result.Ok, result.Message);

        var arm = await dbContext.RobotArmStates.SingleAsync();
        Assert.Equal(Heights.SafeZUm, arm.CurrentZUm);
        Assert.Equal(MotionStatuses.Idle, arm.Status);
        Assert.Equal("cmd-take-record", arm.CurrentCommandId);

        var needle = await dbContext.NeedleStates.SingleAsync(x => x.NeedleCode == NeedleCodes.Needle1);
        Assert.Equal(100, needle.VolumeUl);
        Assert.True(needle.NeedsWash);
        Assert.Equal(MotionStatuses.Completed, needle.Status);

        var operation = await dbContext.PipettingOperations.SingleAsync(x => x.DeviceCommandExecutionId == "cmd-take-record");
        Assert.Equal(PipettingOperationTypes.Aspirate, operation.OperationType);
        Assert.Equal(100, operation.VolumeUl);
        Assert.True(await dbContext.AuditLogs.AnyAsync(x => x.Action == "atomic.action.takeliquid" && x.EntityId == operation.Id));
    }

    // [P1] 清洗类动作应清空针头，并写 WashNeedle 流水。
    [Fact]
    public async Task WashOuter_clears_needle_and_records_wash_ledger()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        dbContext.NeedleStates.Add(new NeedleState { NeedleCode = NeedleCodes.Needle1, NeedleNo = 1, VolumeUl = 80, NeedsWash = true, LoadedSourceType = NeedleLoadSourceTypes.ReagentBottle, LoadedReagentCode = "ABC", UpdatedAtUtc = DateTimeOffset.UtcNow });
        dbContext.RobotArmStates.Add(new RobotArmState { IsHomed = false, Status = MotionStatuses.Idle, UpdatedAtUtc = DateTimeOffset.UtcNow });
        await dbContext.SaveChangesAsync();

        var primitives = new MockRobotMotionPrimitives();
        var recorder = new MockStateAtomicActionRecorder(dbContext);
        var service = new RobotArmAtomicActionService(primitives, Heights, recorder);

        var result = await service.WashOuterAsync(new WashOuterRequest("cmd-wash-record", "Needle1"));
        Assert.True(result.Ok, result.Message);

        var needle = await dbContext.NeedleStates.SingleAsync(x => x.NeedleCode == NeedleCodes.Needle1);
        Assert.Equal(0, needle.VolumeUl);
        Assert.False(needle.NeedsWash);
        Assert.Equal(NeedleLoadSourceTypes.Empty, needle.LoadedSourceType);

        var operation = await dbContext.PipettingOperations.SingleAsync(x => x.DeviceCommandExecutionId == "cmd-wash-record");
        Assert.Equal(PipettingOperationTypes.WashNeedle, operation.OperationType);
    }

    private static async Task<StainerDbContext> CreateMigratedContextAsync()
    {
        var databasePath = Path.Combine(TestPaths.TempRoot, "stainer-atomic-action-tests", Guid.NewGuid().ToString("N"), "stainer.db");
        var connectionString = $"Data Source={databasePath}";
        var options = new DbContextOptionsBuilder<StainerDbContext>()
            .UseSqlite(connectionString)
            .AddInterceptors(new SqlitePragmaConnectionInterceptor())
            .Options;
        var dbContext = new StainerDbContext(options);
        DatabaseInitializer.EnsureDatabaseDirectory(connectionString);
        await dbContext.Database.MigrateAsync();
        return dbContext;
    }

    // 仅用于 [P2]：Aspirate 抛异常，其余原语正常记录。
    private sealed class ThrowingRobotMotionPrimitives : IRobotMotionPrimitives
    {
        public List<RobotPrimitiveCall> Calls { get; } = new();

        public Task MoveZAsync(long zUm, CancellationToken cancellationToken = default)
        {
            Calls.Add(RobotPrimitiveCall.MoveZ(zUm));
            return Task.CompletedTask;
        }

        public Task AspirateAsync(int volumeUl, CancellationToken cancellationToken = default)
        {
            Calls.Add(RobotPrimitiveCall.Aspirate(volumeUl));
            throw new InvalidOperationException("aspirate failed");
        }

        public Task DispenseAsync(int volumeUl, CancellationToken cancellationToken = default)
        {
            Calls.Add(RobotPrimitiveCall.Dispense(volumeUl));
            return Task.CompletedTask;
        }

        public Task WashOuterAsync(CancellationToken cancellationToken = default)
        {
            Calls.Add(RobotPrimitiveCall.WashOuter());
            return Task.CompletedTask;
        }
    }
}
