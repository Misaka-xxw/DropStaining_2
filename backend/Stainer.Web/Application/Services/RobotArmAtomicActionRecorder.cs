using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

// 原子动作记录器（Ports）：把原子动作的净效果落到现有 Mock 运行状态与流水账。
// 这样 Mock 模式下原子动作可被系统其余部分（孪生页面、Step5 测试 API）观测到，
// 而不必再为测试 API 另补一层适配。第一阶段只提供写 DB 的 Mock 实现；
// 后续真实硬件落地时，原子动作服务本身无需改动。
public interface IRobotArmAtomicActionRecorder
{
    Task RecordAsync(RobotArmAtomicActionContext context, CancellationToken cancellationToken = default);
}

// Mock 实现：复用 RobotArmState / NeedleState / PipettingOperation / AuditLog，
// 写入语义与 MotionControlService / EngineeringPipettingService 的 Mock 状态保持一致。
public sealed class MockStateAtomicActionRecorder(StainerDbContext dbContext) : IRobotArmAtomicActionRecorder
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly StainerDbContext _dbContext = dbContext;

    public async Task RecordAsync(RobotArmAtomicActionContext context, CancellationToken cancellationToken = default)
    {
        await EnsureSeededAsync(cancellationToken);

        // 机械臂：动作结束停在安全高度，清空故障态。
        var arm = await _dbContext.RobotArmStates.SingleAsync(cancellationToken);
        arm.IsConnected = true;
        arm.IsHomed = true;
        arm.Status = MotionStatuses.Idle;
        arm.CurrentZUm = context.FinalZUm;
        arm.CurrentTargetPointCode = context.Action;
        arm.CurrentCommandId = context.CommandId;
        arm.DeviceCommandExecutionId = context.CommandId;
        arm.LastErrorCode = null;
        arm.LastErrorMessage = null;
        arm.UpdatedAtUtc = DateTimeOffset.UtcNow;

        // 针头：吸入/排出调整液量；清洗类动作清空针头。
        var needleCode = NormalizeNeedleCode(context.NeedleCode);
        var needle = await _dbContext.NeedleStates.SingleAsync(x => x.NeedleCode == needleCode, cancellationToken);
        if (context.ClearsNeedle)
        {
            needle.LoadedSourceType = NeedleLoadSourceTypes.Empty;
            needle.LoadedReagentCode = null;
            needle.SourceBottleId = null;
            needle.DabBatchId = null;
            needle.SystemLiquidSourceType = null;
            needle.SourcePositionCode = null;
            needle.VolumeUl = 0;
            needle.NeedsWash = false;
            needle.Status = MotionStatuses.Idle;
        }
        else
        {
            needle.VolumeUl = Math.Max(0, needle.VolumeUl + context.NetVolumeUl);
            needle.NeedsWash = true;
            needle.Status = MotionStatuses.Completed;
        }

        needle.IsConnected = true;
        needle.LastErrorCode = null;
        needle.LastErrorMessage = null;
        needle.UpdatedAtUtc = DateTimeOffset.UtcNow;

        // 流水账：复用 PipettingOperation + AuditLog，与现有 motion.operation.* 风格一致。
        var operation = new PipettingOperation
        {
            OperationType = MapOperationType(context.Action),
            Status = DeviceCommandStatus.Completed,
            NeedleCode = needleCode,
            ExecutionMode = PipettingExecutionModes.Single,
            TargetPointCode = context.Action,
            SourceType = NeedleLoadSourceTypes.Empty,
            VolumeUl = Math.Abs(context.NetVolumeUl),
            DeviceCommandExecutionId = context.CommandId,
            ParametersJson = JsonSerializer.Serialize(new
            {
                action = context.Action,
                reason = context.Reason,
                netVolumeUl = context.NetVolumeUl,
                clearsNeedle = context.ClearsNeedle,
                finalZUm = context.FinalZUm
            }, JsonOptions),
            CompletedAtUtc = DateTimeOffset.UtcNow
        };
        _dbContext.PipettingOperations.Add(operation);
        _dbContext.AuditLogs.Add(new AuditLog
        {
            Action = $"atomic.action.{context.Action.ToLowerInvariant()}",
            EntityType = "PipettingOperation",
            EntityId = operation.Id,
            Message = JsonSerializer.Serialize(new
            {
                context.Action,
                needleCode,
                netVolumeUl = context.NetVolumeUl,
                clearsNeedle = context.ClearsNeedle,
                finalZUm = context.FinalZUm,
                commandId = context.CommandId
            }, JsonOptions),
            CreatedAtUtc = DateTimeOffset.UtcNow
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureSeededAsync(CancellationToken cancellationToken)
    {
        if (!await _dbContext.RobotArmStates.AnyAsync(cancellationToken))
        {
            _dbContext.RobotArmStates.Add(new RobotArmState { IsHomed = false, Status = MotionStatuses.Idle, UpdatedAtUtc = DateTimeOffset.UtcNow });
        }

        if (!await _dbContext.NeedleStates.AnyAsync(cancellationToken))
        {
            _dbContext.NeedleStates.AddRange(
                new NeedleState { NeedleCode = NeedleCodes.Needle1, NeedleNo = 1, UpdatedAtUtc = DateTimeOffset.UtcNow },
                new NeedleState { NeedleCode = NeedleCodes.Needle2, NeedleNo = 2, UpdatedAtUtc = DateTimeOffset.UtcNow });
        }

        if (_dbContext.ChangeTracker.HasChanges())
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private static string NormalizeNeedleCode(string? needleCode)
    {
        var normalized = needleCode?.Trim();
        if (string.IsNullOrWhiteSpace(normalized) || normalized.Equals("1", StringComparison.OrdinalIgnoreCase))
        {
            return NeedleCodes.Needle1;
        }

        if (normalized.Equals("2", StringComparison.OrdinalIgnoreCase))
        {
            return NeedleCodes.Needle2;
        }

        return NeedleCodes.All.FirstOrDefault(x => x.Equals(normalized, StringComparison.OrdinalIgnoreCase)) ?? NeedleCodes.Needle1;
    }

    private static string MapOperationType(string action) => action switch
    {
        RobotAtomicActions.TakeLiquid => PipettingOperationTypes.Aspirate,
        RobotAtomicActions.PrepareMix => PipettingOperationTypes.Dispense,
        RobotAtomicActions.DispenseLiquid => PipettingOperationTypes.Dispense,
        RobotAtomicActions.WashInner => PipettingOperationTypes.WashNeedle,
        RobotAtomicActions.WashOuter => PipettingOperationTypes.WashNeedle,
        _ => PipettingOperationTypes.Move
    };
}
