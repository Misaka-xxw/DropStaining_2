using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Tests;

// 调试模块“精度校正”后端持久化测试。
// 覆盖：默认值回读、move/dispense 保存、加样校正因子派生、命令幂等、scope/范围校验。
// 与 ScannerControlServiceTests 同样的临时 SQLite + EnsureCreatedAsync 模式。
public sealed class PrecisionCalibrationConfigServiceTests
{
    // 测试库不持久化真实 User 行：AuditLog.ActorUserId 为可空 FK（SetNull），
    // 服务对空白 UserId 做 null 合并（与 ScannerControlService 一致），故这里用空 UserId。
    private static readonly AuthenticatedUser Admin = new(string.Empty, "admin", "管理员", "admin", ["admin"]);

    [Fact]
    public async Task Get_returns_documented_defaults_before_any_save()
    {
        await using var ctx = await CreateContextAsync();
        var move = await ctx.Service.GetAsync(PrecisionCalibrationScopeKeys.Move);
        var dispense = await ctx.Service.GetAsync(PrecisionCalibrationScopeKeys.Dispense);

        Assert.Equal(0d, move.MoveOffsetXMm);
        Assert.Equal(0d, move.MoveOffsetYMm);
        Assert.Null(move.DispenseTargetVolumeUl);
        Assert.Null(dispense.DispenseMeasuredVolumeUl);
        Assert.Equal(100d, dispense.DispenseTargetVolumeUl);
        Assert.Null(dispense.DispenseCalibrationFactor);
    }

    [Fact]
    public async Task Save_move_persists_offsets_and_writes_audit()
    {
        await using var ctx = await CreateContextAsync();

        var saved = await ctx.Service.SaveAsync(
            PrecisionCalibrationScopeKeys.Move,
            new SavePrecisionCalibrationRequest("cmd-move-1", 0.012d, -0.345d, null, null, "移动校正确认"),
            Admin);

        Assert.True(saved.Ok);
        Assert.False(saved.Replayed);
        var reread = await ctx.Service.GetAsync(PrecisionCalibrationScopeKeys.Move);
        Assert.Equal(0.012d, reread.MoveOffsetXMm);
        Assert.Equal(-0.345d, reread.MoveOffsetYMm);
        Assert.True(await ctx.DbContext.AuditLogs.AnyAsync(x =>
            x.Action == "precision_calibration.save"
            && x.EntityId == saved.EntityId));
        // reason 为非 ASCII 中文，System.Text.Json 默认转义，故解析 JSON 校验 reason 属性而非原始字符串。
        var audit = await ctx.DbContext.AuditLogs.SingleAsync(x =>
            x.Action == "precision_calibration.save" && x.EntityId == saved.EntityId);
        using var doc = JsonDocument.Parse(audit.Message);
        Assert.Equal("移动校正确认", doc.RootElement.GetProperty("reason").GetString());
    }

    [Fact]
    public async Task Save_dispense_computes_calibration_factor_from_measured_over_target()
    {
        await using var ctx = await CreateContextAsync();

        await ctx.Service.SaveAsync(
            PrecisionCalibrationScopeKeys.Dispense,
            new SavePrecisionCalibrationRequest("cmd-dispense-1", null, null, 100d, 98d, "加样校正确认"),
            Admin);

        var reread = await ctx.Service.GetAsync(PrecisionCalibrationScopeKeys.Dispense);
        Assert.Equal(100d, reread.DispenseTargetVolumeUl);
        Assert.Equal(98d, reread.DispenseMeasuredVolumeUl);
        Assert.Equal(0.98d, reread.DispenseCalibrationFactor);
    }

    [Fact]
    public async Task Repeated_command_id_is_idempotent_and_does_not_duplicate()
    {
        await using var ctx = await CreateContextAsync();

        var first = await ctx.Service.SaveAsync(
            PrecisionCalibrationScopeKeys.Move,
            new SavePrecisionCalibrationRequest("cmd-idem-1", 0.5d, 0.5d, null, null, "幂等首执"),
            Admin);
        var replay = await ctx.Service.SaveAsync(
            PrecisionCalibrationScopeKeys.Move,
            new SavePrecisionCalibrationRequest("cmd-idem-1", 0.5d, 0.5d, null, null, "幂等首执"),
            Admin);

        Assert.False(first.Replayed);
        Assert.True(replay.Replayed);
        // 仅首执行写入真实校正行 + 一条审计；幂等重放不应产生第二行或第二条审计。
        Assert.Single(ctx.DbContext.PrecisionCalibrationProfiles);
        Assert.Equal(1, await ctx.DbContext.AuditLogs.CountAsync(x => x.Action == "precision_calibration.save"));
    }

    [Fact]
    public async Task Invalid_scope_key_is_rejected()
    {
        await using var ctx = await CreateContextAsync();
        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => ctx.Service.GetAsync("foo"));
        Assert.Equal("scope_key_invalid", ex.Code);
    }

    [Fact]
    public async Task Out_of_range_offset_is_rejected()
    {
        await using var ctx = await CreateContextAsync();
        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => ctx.Service.SaveAsync(
            PrecisionCalibrationScopeKeys.Move,
            new SavePrecisionCalibrationRequest("cmd-bad-1", 99999d, 0d, null, null, "超范围"),
            Admin));
        Assert.Equal("move_offset_x_mm_invalid", ex.Code);
    }

    private static async Task<TestContext> CreateContextAsync()
    {
        var databasePath = Path.Combine(Path.GetTempPath(), "stainer-precision-calibration-tests", Guid.NewGuid().ToString("N"), "stainer.db");
        Directory.CreateDirectory(Path.GetDirectoryName(databasePath)!);
        var options = new DbContextOptionsBuilder<StainerDbContext>()
            .UseSqlite($"Data Source={databasePath}")
            .Options;
        var dbContext = new StainerDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();
        var service = new PrecisionCalibrationConfigService(dbContext, new CommandIdempotencyService(dbContext));
        return new TestContext(dbContext, service);
    }

    private sealed record TestContext(StainerDbContext DbContext, PrecisionCalibrationConfigService Service) : IAsyncDisposable
    {
        public async ValueTask DisposeAsync() => await DbContext.DisposeAsync();
    }
}
