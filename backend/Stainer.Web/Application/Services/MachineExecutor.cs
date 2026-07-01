using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.Devices;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class MachineExecutor(IRuntimeEventPublisher eventPublisher, IDeviceAdapter deviceAdapter)
{
    private readonly Channel<MachineExecutorCommand> commands = Channel.CreateUnbounded<MachineExecutorCommand>();
    private readonly ConcurrentDictionary<string, ControlFlags> flags = new(StringComparer.Ordinal);
    private IServiceScopeFactory? scopeFactory;

    public void Attach(IServiceScopeFactory serviceScopeFactory)
    {
        scopeFactory = serviceScopeFactory;
    }

    public ValueTask EnqueueStartAsync(string runId, CancellationToken cancellationToken = default)
    {
        return commands.Writer.WriteAsync(new MachineExecutorCommand(runId, MachineExecutorCommandType.Start, null), cancellationToken);
    }

    public ValueTask EnqueueResumeAsync(string runId, CancellationToken cancellationToken = default)
    {
        var control = flags.GetOrAdd(runId, _ => new ControlFlags());
        control.PauseRequested = false;
        return commands.Writer.WriteAsync(new MachineExecutorCommand(runId, MachineExecutorCommandType.Resume, null), cancellationToken);
    }

    public ValueTask EnqueueRedoAsync(string runId, string reason, CancellationToken cancellationToken = default)
    {
        var control = flags.GetOrAdd(runId, _ => new ControlFlags());
        control.FaultMessage = null;
        return commands.Writer.WriteAsync(new MachineExecutorCommand(runId, MachineExecutorCommandType.Redo, reason), cancellationToken);
    }

    public void RequestPause(string runId)
    {
        flags.GetOrAdd(runId, _ => new ControlFlags()).PauseRequested = true;
    }

    public void RequestStop(string runId)
    {
        flags.GetOrAdd(runId, _ => new ControlFlags()).StopRequested = true;
    }

    public void RequestFault(string runId, string message)
    {
        flags.GetOrAdd(runId, _ => new ControlFlags()).FaultMessage = string.IsNullOrWhiteSpace(message) ? "Injected fault." : message.Trim();
    }

    public async Task RunAsync(CancellationToken stoppingToken)
    {
        if (scopeFactory is null)
        {
            throw new InvalidOperationException("MachineExecutor scope factory was not attached.");
        }

        await foreach (var command in commands.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
                var dabLifecycleService = scope.ServiceProvider.GetRequiredService<DabLifecycleService>();
                await ProcessCommandAsync(dbContext, dabLifecycleService, command, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                await RecordExecutorExceptionAsync(command.RunId, ex);
            }
        }
    }

    private async Task RecordExecutorExceptionAsync(string runId, Exception exception)
    {
        if (scopeFactory is null)
        {
            return;
        }

        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            var run = await LoadRunAsync(dbContext, runId, CancellationToken.None);
            if (run is null)
            {
                return;
            }

            run.Status = RuntimeLedgerStatus.Faulted;
            run.FaultMessage = exception.Message;
            var currentStep = run.WorkflowExecutions
                .SelectMany(x => x.StepExecutions)
                .FirstOrDefault(x => x.Status == RuntimeLedgerStatus.Running)
                ?? run.WorkflowExecutions
                    .SelectMany(x => x.StepExecutions)
                    .FirstOrDefault(x => x.Status == RuntimeLedgerStatus.Pending);
            if (currentStep is not null)
            {
                currentStep.Status = RuntimeLedgerStatus.Unknown;
            }

            await AddAlarmAsync(dbContext, runId, "executor_exception", "Critical", exception.Message, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            eventPublisher.Publish(runId, "run.executor_exception", exception.Message);
        }
        catch
        {
            // The executor must not crash the web host while trying to record its own failure.
        }
    }

    private async Task ProcessCommandAsync(
        StainerDbContext dbContext,
        DabLifecycleService dabLifecycleService,
        MachineExecutorCommand command,
        CancellationToken cancellationToken)
    {
        switch (command.Type)
        {
            case MachineExecutorCommandType.Start:
            case MachineExecutorCommandType.Resume:
                await ExecuteRunUntilBlockedAsync(dbContext, dabLifecycleService, command.RunId, cancellationToken);
                break;
            case MachineExecutorCommandType.Redo:
                await RedoCurrentMajorStepAsync(dbContext, command.RunId, command.Payload ?? "Redo requested.", cancellationToken);
                await ExecuteRunUntilBlockedAsync(dbContext, dabLifecycleService, command.RunId, cancellationToken);
                break;
        }
    }

    private async Task ExecuteRunUntilBlockedAsync(
        StainerDbContext dbContext,
        DabLifecycleService dabLifecycleService,
        string runId,
        CancellationToken cancellationToken)
    {
        var control = flags.GetOrAdd(runId, _ => new ControlFlags());
        var run = await LoadRunAsync(dbContext, runId, cancellationToken);
        if (run is null
            || run.Status == RuntimeLedgerStatus.Completed
            || run.Status == RuntimeLedgerStatus.Stopped
            || run.Status == RuntimeLedgerStatus.Faulted)
        {
            return;
        }

        var now = DateTimeOffset.UtcNow;
        run.Status = RuntimeLedgerStatus.Running;
        run.StartedAtUtc ??= now;
        run.PauseRequested = false;
        run.StopRequested = false;
        foreach (var batch in run.ChannelBatches)
        {
            batch.Status = RuntimeLedgerStatus.Running;
            batch.StartedAtUtc ??= now;
            batch.WorkflowLockedAtUtc ??= now;
            batch.WorkflowSelectionStatus = WorkflowSelectionStatus.Locked;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        PublishMachineState(run, "Run started or resumed.");
        eventPublisher.Publish(MachineEventMessage.Create(
            MachineEventTypes.DeviceConnectionChanged,
            run.Id,
            "MachineRun",
            run.Id,
            "engineer",
            new Dictionary<string, object?>
            {
                ["connected"] = true,
                ["adapter"] = deviceAdapter.Name,
                ["mode"] = deviceAdapter.Mode,
                ["message"] = $"{deviceAdapter.Name} is selected."
            }));

        while (!cancellationToken.IsCancellationRequested)
        {
            run = await LoadRunAsync(dbContext, runId, cancellationToken);
            if (run is null)
            {
                return;
            }

            if (run.Status == RuntimeLedgerStatus.Faulted)
            {
                return;
            }

            var step = run.WorkflowExecutions
                .SelectMany(x => x.StepExecutions)
                .Where(x => x.Status == RuntimeLedgerStatus.Pending)
                .OrderBy(x => x.WorkflowExecution!.SlideTask!.SlotCode)
                .ThenBy(x => x.StepNo)
                .FirstOrDefault();

            if (step is null)
            {
                await CompleteRunAsync(dbContext, run, cancellationToken);
                return;
            }

            var stepCompleted = await ExecuteStepAsync(dbContext, dabLifecycleService, run, step, cancellationToken);
            if (!stepCompleted)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(control.FaultMessage))
            {
                await FaultRunAsync(dbContext, run.Id, step.Id, control.FaultMessage, cancellationToken);
                control.FaultMessage = null;
                return;
            }

            if (control.StopRequested)
            {
                await StopRunAsync(dbContext, run.Id, cancellationToken);
                control.StopRequested = false;
                return;
            }

            if (control.PauseRequested)
            {
                await PauseRunAsync(dbContext, run.Id, cancellationToken);
                return;
            }
        }
    }

    private async Task<bool> ExecuteStepAsync(
        StainerDbContext dbContext,
        DabLifecycleService dabLifecycleService,
        MachineRun run,
        WorkflowStepExecution step,
        CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        run.CurrentMajorStepCode = step.MajorStepCode;
        step.Status = RuntimeLedgerStatus.Running;
        step.StartedAtUtc ??= now;
        step.WorkflowExecution!.Status = RuntimeLedgerStatus.Running;
        step.WorkflowExecution.StartedAtUtc ??= now;
        step.WorkflowExecution.SlideTask!.Status = RuntimeLedgerStatus.Running;
        step.WorkflowExecution.SlideTask.ChannelBatch!.Status = RuntimeLedgerStatus.Running;

        var command = new DeviceCommandExecution
        {
            MachineRunId = run.Id,
            WorkflowStepExecutionId = step.Id,
            CommandType = step.ActionType,
            Status = DeviceCommandStatus.Planned,
            PayloadJson = JsonSerializer.Serialize(new
            {
                step.StepNo,
                step.MajorStepCode,
                step.ReagentCode,
                step.VolumeUl
            }),
            CreatedAtUtc = now
        };
        dbContext.DeviceCommandExecutions.Add(command);
        await dbContext.SaveChangesAsync(cancellationToken);
        PublishSlideTaskState(run.Id, step.WorkflowExecution.SlideTask, step.StepName);
        PublishWorkflowStep(run.Id, step, MachineEventTypes.WorkflowStepStarted);

        command.Status = DeviceCommandStatus.CommandSent;
        command.CommandSentAtUtc = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        var deviceResult = await ExecuteDeviceActionAsync(step, command, cancellationToken);
        if (deviceResult.Acknowledged)
        {
            command.Status = DeviceCommandStatus.Acknowledged;
            command.AcknowledgedAtUtc = DateTimeOffset.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var deviceOutcomeUnknown = deviceResult.Status is DeviceCommandStatuses.Unknown or DeviceCommandStatuses.TimedOut
            || (IsDabStep(step)
                && deviceResult.Data.TryGetValue("faultType", out var faultType)
                && string.Equals(Convert.ToString(faultType), DeviceFaultTypes.Disconnect, StringComparison.OrdinalIgnoreCase));
        var businessOk = false;
        if (deviceResult.Ok)
        {
            businessOk = await ApplyBusinessEffectsAsync(dbContext, dabLifecycleService, run, step, command, deviceResult, cancellationToken);
        }
        else if (IsDabStep(step))
        {
            var dabFailure = await dabLifecycleService.HandlePreparationNotCompletedFromDeviceAsync(
                run,
                step,
                deviceResult,
                deviceOutcomeUnknown,
                cancellationToken);
            if (!dabFailure.Ok)
            {
                await AddAlarmAsync(dbContext, run.Id, dabFailure.ErrorCode!, "Critical", dabFailure.Message, cancellationToken);
                if (dabFailure.Batch is not null)
                {
                    PublishDabBatchChanged(run.Id, dabFailure.Batch, deviceOutcomeUnknown ? "unknown" : "failed");
                }
            }
        }

        var ok = deviceResult.Ok && businessOk;
        command.Status = ok
            ? DeviceCommandStatus.Completed
            : deviceOutcomeUnknown ? DeviceCommandStatus.Unknown : DeviceCommandStatus.Failed;
        command.CompletedAtUtc = DateTimeOffset.UtcNow;
        command.ResultJson = JsonSerializer.Serialize(new
        {
            ok,
            adapter = deviceAdapter.Name,
            mode = deviceAdapter.Mode,
            deviceResult.Status,
            deviceResult.ErrorCode,
            deviceResult.Message,
            deviceResult.StartedAtUtc,
            deviceResult.CompletedAtUtc,
            deviceResult.Data
        });

        step.Status = ok
            ? RuntimeLedgerStatus.Completed
            : deviceOutcomeUnknown ? RuntimeLedgerStatus.Unknown : RuntimeLedgerStatus.Failed;
        step.CompletedAtUtc = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        PublishWorkflowStep(run.Id, step, MachineEventTypes.WorkflowStepCompleted);
        PublishSlideTaskState(run.Id, step.WorkflowExecution.SlideTask, step.StepName);

        if (!ok)
        {
            await FaultRunAsync(
                dbContext,
                run.Id,
                step.Id,
                deviceResult.Message,
                cancellationToken,
                deviceOutcomeUnknown ? "device_command_unknown" : "device_command_failed",
                deviceOutcomeUnknown);
            return false;
        }

        return true;
    }

    private async Task<bool> ApplyBusinessEffectsAsync(
        StainerDbContext dbContext,
        DabLifecycleService dabLifecycleService,
        MachineRun run,
        WorkflowStepExecution step,
        DeviceCommandExecution command,
        DeviceCommandResult deviceResult,
        CancellationToken cancellationToken)
    {
        if (IsDabStep(step))
        {
            return await ApplyDabAsync(dbContext, dabLifecycleService, run, step, command, deviceResult, cancellationToken);
        }

        if (IsTemperatureStep(step))
        {
            eventPublisher.Publish(MachineEventMessage.Create(
                MachineEventTypes.TemperatureChanged,
                run.Id,
                "WorkflowStepExecution",
                step.Id,
                null,
                new Dictionary<string, object?>
                {
                    ["workflowStepExecutionId"] = step.Id,
                    ["slideTaskId"] = step.WorkflowExecution?.SlideTaskId,
                    ["majorStepCode"] = step.MajorStepCode,
                    ["currentTemperatureDeciC"] = deviceResult.Data.GetValueOrDefault("currentTemperatureDeciC") ?? 420,
                    ["targetTemperatureDeciC"] = deviceResult.Data.GetValueOrDefault("currentTemperatureDeciC") ?? 420,
                    ["adapter"] = deviceAdapter.Name,
                    ["message"] = deviceResult.Message
                }));
        }

        if (!string.IsNullOrWhiteSpace(step.ReagentCode) && (step.VolumeUl ?? 0) > 0)
        {
            return await ConsumeReagentAsync(dbContext, run, step, command, step.ReagentCode!, step.VolumeUl!.Value, cancellationToken);
        }

        return true;
    }

    private Task<DeviceCommandResult> ExecuteDeviceActionAsync(
        WorkflowStepExecution step,
        DeviceCommandExecution command,
        CancellationToken cancellationToken)
    {
        var moduleCode = ResolveDeviceModule(step);
        var request = new DeviceOperationRequest(
            new DeviceCommandContext(command.Id, command.Id, "system", nameof(MachineExecutor)),
            moduleCode,
            step.ActionType,
            new Dictionary<string, object?>
            {
                ["machineRunId"] = command.MachineRunId,
                ["workflowStepExecutionId"] = step.Id,
                ["stepNo"] = step.StepNo,
                ["majorStepCode"] = step.MajorStepCode,
                ["reagentCode"] = step.ReagentCode,
                ["volumeUl"] = step.VolumeUl,
                ["targetTemperatureDeciC"] = 420
            });

        if (IsDabStep(step))
        {
            return deviceAdapter.PrepareDabAsync(request, cancellationToken);
        }

        if (IsTemperatureStep(step))
        {
            return deviceAdapter.SetTemperatureAsync(request, cancellationToken);
        }

        var action = step.ActionType.ToLowerInvariant();
        if (action.Contains("needle") && action.Contains("wash"))
        {
            return deviceAdapter.WashNeedlesAsync(request, cancellationToken);
        }

        if (action.Contains("wash"))
        {
            return deviceAdapter.RunPumpAsync(request, cancellationToken);
        }

        if (action.Contains("mix"))
        {
            return deviceAdapter.MixAsync(request, cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(step.ReagentCode) && (step.VolumeUl ?? 0) > 0)
        {
            return deviceAdapter.PipetteAsync(request, cancellationToken);
        }

        return deviceAdapter.ExecuteWorkflowActionAsync(request, cancellationToken);
    }

    private static string ResolveDeviceModule(WorkflowStepExecution step)
    {
        if (IsDabStep(step)) return DeviceModules.Dab;
        if (IsTemperatureStep(step)) return DeviceModules.Temperature;
        var action = step.ActionType.ToLowerInvariant();
        if (action.Contains("needle")) return DeviceModules.NeedleWash;
        if (action.Contains("wash")) return DeviceModules.Pump;
        if (action.Contains("mix")) return DeviceModules.Mixer;
        if (!string.IsNullOrWhiteSpace(step.ReagentCode) && (step.VolumeUl ?? 0) > 0) return DeviceModules.Pipette;
        return DeviceModules.Workflow;
    }

    private async Task<bool> ConsumeReagentAsync(
        StainerDbContext dbContext,
        MachineRun run,
        WorkflowStepExecution step,
        DeviceCommandExecution command,
        string reagentCode,
        int volumeUl,
        CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var placements = await dbContext.ReagentRackPlacements
            .Where(x => x.RemovedAtUtc == null)
            .Include(x => x.ReagentRackPosition)
            .Include(x => x.ReagentBottle)
            .Where(x => x.ReagentBottle!.ReagentCode == reagentCode
                && x.ReagentBottle.Status == "Available"
                && x.ReagentBottle.ExpirationDate >= today
                && x.ReagentBottle.RemainingVolumeUl > 0)
            .ToListAsync(cancellationToken);

        var available = placements.Sum(x => x.ReagentBottle!.RemainingVolumeUl);
        if (available < volumeUl)
        {
            await AddAlarmAsync(dbContext, run.Id, "reagent_insufficient", "Critical", $"Reagent {reagentCode} is insufficient. Required {volumeUl} ul, available {available} ul.", cancellationToken);
            return false;
        }

        var remaining = volumeUl;
        foreach (var placement in placements.OrderBy(x => x.ReagentBottle!.RemainingVolumeUl))
        {
            if (remaining <= 0)
            {
                break;
            }

            var bottle = placement.ReagentBottle!;
            var used = Math.Min(remaining, bottle.RemainingVolumeUl);
            bottle.RemainingVolumeUl -= used;
            bottle.UpdatedAtUtc = DateTimeOffset.UtcNow;
            remaining -= used;

            dbContext.ReagentConsumptions.Add(new ReagentConsumption
            {
                MachineRunId = run.Id,
                WorkflowStepExecutionId = step.Id,
                ReagentBottleId = bottle.Id,
                ReagentCode = reagentCode,
                VolumeUl = used,
                CreatedAtUtc = DateTimeOffset.UtcNow
            });
            dbContext.DispenseExecutions.Add(new DispenseExecution
            {
                DeviceCommandExecutionId = command.Id,
                ReagentBottleId = bottle.Id,
                ReagentCode = reagentCode,
                VolumeUl = used,
                SourcePositionCode = placement.ReagentRackPosition?.Code,
                TargetSlotCode = step.WorkflowExecution!.SlideTask!.SlotCode,
                Status = DeviceCommandStatus.Completed,
                CreatedAtUtc = DateTimeOffset.UtcNow
            });
            dbContext.AuditLogs.Add(new AuditLog
            {
                Action = "run.reagent_consumption",
                EntityType = "ReagentBottle",
                EntityId = bottle.Id,
                Message = JsonSerializer.Serialize(new { runId = run.Id, reagentCode, volumeUl = used, remainingUl = bottle.RemainingVolumeUl }),
                CreatedAtUtc = DateTimeOffset.UtcNow
            });

            if (bottle.RemainingVolumeUl == 0)
            {
                await AddAlarmAsync(dbContext, run.Id, "reagent_depleted", "Warning", $"Bottle {bottle.FullBarcode} for {reagentCode} is depleted.", cancellationToken);
                eventPublisher.Publish(MachineEventMessage.Create(
                    MachineEventTypes.ReagentBottleDepleted,
                    run.Id,
                    "ReagentBottle",
                    bottle.Id,
                    null,
                    new Dictionary<string, object?>
                    {
                        ["reagentBottleId"] = bottle.Id,
                        ["fullBarcode"] = bottle.FullBarcode,
                        ["reagentCode"] = reagentCode,
                        ["remainingVolumeUl"] = bottle.RemainingVolumeUl,
                        ["message"] = $"Bottle {bottle.FullBarcode} for {reagentCode} is depleted."
                    }));
            }
        }

        return true;
    }

    private async Task<bool> ApplyDabAsync(
        StainerDbContext dbContext,
        DabLifecycleService dabLifecycleService,
        MachineRun run,
        WorkflowStepExecution step,
        DeviceCommandExecution command,
        DeviceCommandResult deviceResult,
        CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var dabBatch = await FindDabBatchForStepAsync(dbContext, step, cancellationToken);
        if (dabBatch is null)
        {
            await AddAlarmAsync(dbContext, run.Id, "dab_batch_not_prepared", "Critical", "No formal DAB batch is assigned to this DAB workflow step.", cancellationToken);
            return false;
        }

        if (dabBatch.Status is DabBatchStatus.PendingPreparation or DabBatchStatus.Preparing)
        {
            var preparation = await dabLifecycleService.CompletePreparationFromDeviceAsync(
                dabBatch.Id,
                run,
                step,
                command,
                deviceResult,
                cancellationToken);
            if (!preparation.Ok)
            {
                await AddAlarmAsync(dbContext, run.Id, preparation.ErrorCode!, "Critical", preparation.Message, cancellationToken);
                return false;
            }

            PublishDabBatchChanged(run.Id, preparation.Batch!, "prepared");
            return true;
        }

        if (dabBatch.Status == DabBatchStatus.Available)
        {
            return await ConsumeAvailableDabBatchAsync(dbContext, run, step, command, dabBatch, now, cancellationToken);
        }

        await AddAlarmAsync(dbContext, run.Id, "dab_batch_unavailable", "Critical", $"DAB batch {dabBatch.Id} is {dabBatch.Status} and cannot be used.", cancellationToken);
        return false;
    }

    private async Task<DabBatch?> FindDabBatchForStepAsync(
        StainerDbContext dbContext,
        WorkflowStepExecution step,
        CancellationToken cancellationToken)
    {
        var stainingTaskId = step.WorkflowExecution?.SlideTask?.StainingTaskId;
        if (string.IsNullOrWhiteSpace(stainingTaskId))
        {
            return null;
        }

        var candidates = await dbContext.DabBatches
            .Include(x => x.DabMixPosition)
            .Include(x => x.Tasks)
            .Include(x => x.ReagentReservations)
            .ThenInclude(x => x.ReagentBottle)
            .Where(x => x.Tasks.Any(task => task.StainingTaskId == stainingTaskId)
                && x.Status != DabBatchStatus.Cleaned
                && x.Status != DabBatchStatus.LegacyUnverified)
            .ToListAsync(cancellationToken);
        return candidates
            .OrderByDescending(x => x.Status == DabBatchStatus.Preparing)
            .ThenByDescending(x => x.Status == DabBatchStatus.PendingPreparation)
            .ThenBy(x => x.CreatedAtUtc)
            .FirstOrDefault();
    }

    private async Task<bool> ConsumeAvailableDabBatchAsync(
        StainerDbContext dbContext,
        MachineRun run,
        WorkflowStepExecution step,
        DeviceCommandExecution command,
        DabBatch batch,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        if (batch.ExpiresAtUtc <= now)
        {
            batch.Status = DabBatchStatus.Expired;
            batch.CleaningStatus = DabCleaningStatus.Required;
            batch.DabMixPosition!.Status = DabMixPositionStatus.AwaitingCleaning;
            batch.DabMixPosition.UpdatedAtUtc = now;
            PublishDabBatchChanged(run.Id, batch, "expired");
            await AddAlarmAsync(dbContext, run.Id, "dab_expired", "Critical", "A DAB batch is expired. Clean DAB mix positions before continuing.", cancellationToken);
            return false;
        }

        var volume = Math.Max(step.VolumeUl ?? DabFormula.VolumePerSlideUl, DabFormula.VolumePerSlideUl);
        if (batch.RemainingVolumeUl < volume)
        {
            await AddAlarmAsync(dbContext, run.Id, "dab_batch_insufficient", "Critical", $"DAB batch {batch.Id} does not have enough remaining volume.", cancellationToken);
            return false;
        }

        batch.RemainingVolumeUl -= volume;
        batch.UsedVolumeUl += volume;
        batch.UpdatedAtUtc = now;
        if (batch.RemainingVolumeUl == 0)
        {
            batch.Status = DabBatchStatus.Depleted;
            batch.CleaningStatus = DabCleaningStatus.Required;
            batch.DabMixPosition!.Status = DabMixPositionStatus.AwaitingCleaning;
            batch.DabMixPosition.UpdatedAtUtc = now;
        }

        PublishDabBatchChanged(run.Id, batch, "consumed");
        dbContext.DabBatchUsages.Add(new DabBatchUsage
        {
            DabBatch = batch,
            MachineRunId = run.Id,
            WorkflowStepExecutionId = step.Id,
            VolumeUl = volume,
            CreatedAtUtc = now
        });
        dbContext.DispenseExecutions.Add(new DispenseExecution
        {
            DeviceCommandExecutionId = command.Id,
            ReagentCode = "DAB",
            VolumeUl = volume,
            SourcePositionCode = batch.PositionCode,
            TargetSlotCode = step.WorkflowExecution!.SlideTask!.SlotCode,
            Status = DeviceCommandStatus.Completed,
            CreatedAtUtc = now
        });
        dbContext.AuditLogs.Add(new AuditLog
        {
            Action = "run.dab_consumption",
            EntityType = "DabBatch",
            EntityId = batch.Id,
            Message = JsonSerializer.Serialize(new { runId = run.Id, volumeUl = volume, remainingUl = batch.RemainingVolumeUl }),
            CreatedAtUtc = now
        });
        return true;
    }

    private async Task RedoCurrentMajorStepAsync(StainerDbContext dbContext, string runId, string reason, CancellationToken cancellationToken)
    {
        var run = await LoadRunAsync(dbContext, runId, cancellationToken);
        if (run is null || run.Status != RuntimeLedgerStatus.Faulted)
        {
            return;
        }

        var targetStep = run.WorkflowExecutions
            .SelectMany(x => x.StepExecutions)
            .Where(x => x.Status is RuntimeLedgerStatus.Unknown or RuntimeLedgerStatus.Failed)
            .OrderByDescending(x => x.StartedAtUtc ?? x.CreatedAtUtc)
            .FirstOrDefault();
        if (targetStep is null)
        {
            return;
        }

        var major = targetStep.MajorStepCode;
        var majorSteps = run.WorkflowExecutions
            .SelectMany(x => x.StepExecutions)
            .Where(x => x.WorkflowExecutionId == targetStep.WorkflowExecutionId && x.MajorStepCode == major)
            .ToList();

        if (!await ValidateMockDeviceStateAsync(dbContext, run, cancellationToken))
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return;
        }

        foreach (var step in majorSteps)
        {
            if (!await ValidateStepResourcesAsync(dbContext, run, step, cancellationToken))
            {
                await dbContext.SaveChangesAsync(cancellationToken);
                return;
            }

            step.Status = RuntimeLedgerStatus.Pending;
            step.StartedAtUtc = null;
            step.CompletedAtUtc = null;
            step.RedoCount++;
        }

        run.Status = RuntimeLedgerStatus.Running;
        run.FaultMessage = null;
        run.CurrentMajorStepCode = major;
        dbContext.AuditLogs.Add(new AuditLog
        {
            Action = "run.redo_major_step",
            EntityType = "MachineRun",
            EntityId = run.Id,
            Message = JsonSerializer.Serialize(new { majorStepCode = major, reason }),
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
        await dbContext.SaveChangesAsync(cancellationToken);
        PublishMachineState(run, $"Redo major step {major}.");
    }

    private async Task<bool> ValidateMockDeviceStateAsync(StainerDbContext dbContext, MachineRun run, CancellationToken cancellationToken)
    {
        var hasActiveDeviceProfile = await dbContext.DeviceProfiles
            .AsNoTracking()
            .AnyAsync(x => x.IsActive, cancellationToken);
        var hasActiveCoordinateProfile = await dbContext.CoordinateProfiles
            .AsNoTracking()
            .AnyAsync(x => x.IsActive && x.Status == "Active", cancellationToken);
        if (hasActiveDeviceProfile && hasActiveCoordinateProfile)
        {
            return true;
        }

        await AddAlarmAsync(
            dbContext,
            run.Id,
            "redo_device_not_ready",
            "Critical",
            "Mock device state is not ready before redo. Active device and coordinate profiles are required.",
            cancellationToken);
        return false;
    }

    private async Task<bool> ValidateStepResourcesAsync(StainerDbContext dbContext, MachineRun run, WorkflowStepExecution step, CancellationToken cancellationToken)
    {
        if (IsDabStep(step))
        {
            var availableBatches = await dbContext.DabBatches
                .Where(x => x.Status == RuntimeLedgerStatus.Available)
                .ToListAsync(cancellationToken);
            var expired = availableBatches.Any(x => x.ExpiresAtUtc <= DateTimeOffset.UtcNow);
            if (expired)
            {
                await AddAlarmAsync(dbContext, run.Id, "redo_dab_expired", "Critical", "DAB is expired before redo.", cancellationToken);
                return false;
            }
        }

        if (!string.IsNullOrWhiteSpace(step.ReagentCode) && (step.VolumeUl ?? 0) > 0 && !IsDabStep(step))
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var available = await dbContext.ReagentRackPlacements
                .Where(x => x.RemovedAtUtc == null)
                .Include(x => x.ReagentBottle)
                .Where(x => x.ReagentBottle!.ReagentCode == step.ReagentCode
                    && x.ReagentBottle.Status == "Available"
                    && x.ReagentBottle.ExpirationDate >= today)
                .SumAsync(x => x.ReagentBottle!.RemainingVolumeUl, cancellationToken);
            if (available < step.VolumeUl)
            {
                await AddAlarmAsync(dbContext, run.Id, "redo_reagent_insufficient", "Critical", $"Reagent {step.ReagentCode} is insufficient before redo.", cancellationToken);
                return false;
            }
        }

        return true;
    }

    private async Task PauseRunAsync(StainerDbContext dbContext, string runId, CancellationToken cancellationToken)
    {
        var run = await LoadRunAsync(dbContext, runId, cancellationToken);
        if (run is null)
        {
            return;
        }

        run.Status = RuntimeLedgerStatus.Paused;
        run.PauseRequested = true;
        foreach (var batch in run.ChannelBatches)
        {
            batch.Status = RuntimeLedgerStatus.Paused;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        PublishMachineState(run, "Run paused after current atomic action.");
    }

    private async Task StopRunAsync(StainerDbContext dbContext, string runId, CancellationToken cancellationToken)
    {
        var run = await LoadRunAsync(dbContext, runId, cancellationToken);
        if (run is null)
        {
            return;
        }

        run.Status = RuntimeLedgerStatus.Stopped;
        run.StopRequested = true;
        var now = DateTimeOffset.UtcNow;
        run.CompletedAtUtc = now;
        foreach (var batch in run.ChannelBatches)
        {
            batch.Status = RuntimeLedgerStatus.Stopped;
            batch.CompletedAtUtc ??= now;
        }

        foreach (var slideTask in run.WorkflowExecutions.Select(x => x.SlideTask).Where(x => x is not null).Cast<SlideTask>())
        {
            if (slideTask.Status is RuntimeLedgerStatus.Pending or RuntimeLedgerStatus.Running or RuntimeLedgerStatus.Paused)
            {
                slideTask.Status = RuntimeLedgerStatus.Stopped;
            }
        }

        foreach (var step in run.WorkflowExecutions.SelectMany(x => x.StepExecutions).Where(x => x.Status == RuntimeLedgerStatus.Pending))
        {
            step.Status = RuntimeLedgerStatus.Stopped;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        PublishMachineState(run, "Run stopped after current atomic action.");
    }

    private async Task FaultRunAsync(
        StainerDbContext dbContext,
        string runId,
        string stepId,
        string message,
        CancellationToken cancellationToken,
        string alarmCode = "mock_fault",
        bool markUnknown = true)
    {
        var run = await LoadRunAsync(dbContext, runId, cancellationToken);
        if (run is null)
        {
            return;
        }

        var step = run.WorkflowExecutions.SelectMany(x => x.StepExecutions).FirstOrDefault(x => x.Id == stepId);
        if (step is not null)
        {
            step.Status = markUnknown ? RuntimeLedgerStatus.Unknown : RuntimeLedgerStatus.Failed;
        }

        run.Status = RuntimeLedgerStatus.Faulted;
        run.FaultMessage = message;
        foreach (var batch in run.ChannelBatches)
        {
            batch.Status = RuntimeLedgerStatus.Faulted;
        }

        await AddAlarmAsync(dbContext, runId, alarmCode, "Critical", message, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        PublishMachineState(run, message);
    }

    private async Task CompleteRunAsync(StainerDbContext dbContext, MachineRun run, CancellationToken cancellationToken)
    {
        run.Status = RuntimeLedgerStatus.Completed;
        var now = DateTimeOffset.UtcNow;
        run.CompletedAtUtc = now;
        foreach (var workflow in run.WorkflowExecutions)
        {
            workflow.Status = RuntimeLedgerStatus.Completed;
            workflow.CompletedAtUtc = now;
            workflow.SlideTask!.Status = RuntimeLedgerStatus.WaitingUnload;
            workflow.SlideTask.ChannelBatch!.Status = RuntimeLedgerStatus.Completed;
            workflow.SlideTask.ChannelBatch.CompletedAtUtc ??= now;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        PublishMachineState(run, "Run completed and slides are waiting unload.");
    }

    private async Task AddAlarmAsync(StainerDbContext dbContext, string runId, string code, string severity, string message, CancellationToken cancellationToken)
    {
        if (!await dbContext.Alarms.AnyAsync(x => x.MachineRunId == runId && x.Code == code && x.Status == "Active", cancellationToken))
        {
            var alarm = new Alarm
            {
                MachineRunId = runId,
                Code = code,
                Severity = severity,
                Message = message,
                Status = "Active",
                CreatedAtUtc = DateTimeOffset.UtcNow
            };
            dbContext.Alarms.Add(alarm);
            eventPublisher.Publish(MachineEventMessage.Create(
                MachineEventTypes.AlarmRaised,
                runId,
                "Alarm",
                alarm.Id,
                null,
                new Dictionary<string, object?>
                {
                    ["alarmId"] = alarm.Id,
                    ["code"] = code,
                    ["severity"] = severity,
                    ["status"] = alarm.Status,
                    ["message"] = message
                }));
        }
    }

    private void PublishMachineState(MachineRun run, string message)
    {
        eventPublisher.Publish(MachineEventMessage.Create(
            MachineEventTypes.MachineStateChanged,
            run.Id,
            "MachineRun",
            run.Id,
            null,
            new Dictionary<string, object?>
            {
                ["runId"] = run.Id,
                ["runCode"] = run.RunCode,
                ["status"] = run.Status,
                ["currentMajorStepCode"] = run.CurrentMajorStepCode,
                ["faultMessage"] = run.FaultMessage,
                ["message"] = message
            }));
    }

    private void PublishSlideTaskState(string runId, SlideTask slideTask, string? currentStep)
    {
        eventPublisher.Publish(MachineEventMessage.Create(
            MachineEventTypes.SlideTaskStateChanged,
            runId,
            "SlideTask",
            slideTask.Id,
            null,
            new Dictionary<string, object?>
            {
                ["slideTaskId"] = slideTask.Id,
                ["slotCode"] = slideTask.SlotCode,
                ["taskType"] = slideTask.TaskType,
                ["status"] = slideTask.Status,
                ["currentStep"] = currentStep
            }));
    }

    private void PublishWorkflowStep(string runId, WorkflowStepExecution step, string eventType)
    {
        eventPublisher.Publish(MachineEventMessage.Create(
            eventType,
            runId,
            "WorkflowStepExecution",
            step.Id,
            null,
            new Dictionary<string, object?>
            {
                ["workflowStepExecutionId"] = step.Id,
                ["workflowExecutionId"] = step.WorkflowExecutionId,
                ["slideTaskId"] = step.WorkflowExecution?.SlideTaskId,
                ["stepNo"] = step.StepNo,
                ["majorStepCode"] = step.MajorStepCode,
                ["stepName"] = step.StepName,
                ["actionType"] = step.ActionType,
                ["reagentCode"] = step.ReagentCode,
                ["volumeUl"] = step.VolumeUl,
                ["status"] = step.Status,
                ["redoCount"] = step.RedoCount
            }));
    }

    private void PublishDabBatchChanged(string runId, DabBatch batch, string changeType)
    {
        eventPublisher.Publish(MachineEventMessage.Create(
            MachineEventTypes.DabBatchChanged,
            runId,
            "DabBatch",
            batch.Id,
            null,
            new Dictionary<string, object?>
            {
                ["dabBatchId"] = batch.Id,
                ["positionCode"] = batch.PositionCode,
                ["status"] = batch.Status,
                ["changeType"] = changeType,
                ["remainingVolumeUl"] = batch.RemainingVolumeUl,
                ["preparedAtUtc"] = batch.PreparedAtUtc,
                ["expiresAtUtc"] = batch.ExpiresAtUtc
            }));
    }

    private async Task<MachineRun?> LoadRunAsync(StainerDbContext dbContext, string runId, CancellationToken cancellationToken)
    {
        return await dbContext.MachineRuns
            .Include(x => x.ChannelBatches)
            .ThenInclude(x => x.SlideTasks)
            .Include(x => x.WorkflowExecutions)
            .ThenInclude(x => x.SlideTask)
            .ThenInclude(x => x!.ChannelBatch)
            .Include(x => x.WorkflowExecutions)
            .ThenInclude(x => x.StepExecutions)
            .SingleOrDefaultAsync(x => x.Id == runId, cancellationToken);
    }

    private static bool IsDabStep(WorkflowStepExecution step)
    {
        return step.MajorStepCode.Contains("DAB", StringComparison.OrdinalIgnoreCase)
            || step.ActionType.Contains("DAB", StringComparison.OrdinalIgnoreCase)
            || string.Equals(step.ReagentCode, "DAB", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsTemperatureStep(WorkflowStepExecution step)
    {
        return step.MajorStepCode.Contains("HEAT", StringComparison.OrdinalIgnoreCase)
            || step.MajorStepCode.Contains("TEMP", StringComparison.OrdinalIgnoreCase)
            || step.ActionType.Contains("HEAT", StringComparison.OrdinalIgnoreCase)
            || step.ActionType.Contains("TEMP", StringComparison.OrdinalIgnoreCase);
    }

    private sealed record MachineExecutorCommand(string RunId, string Type, string? Payload);

    private static class MachineExecutorCommandType
    {
        public const string Start = "Start";
        public const string Resume = "Resume";
        public const string Redo = "Redo";
    }

    private sealed class ControlFlags
    {
        public volatile bool PauseRequested;
        public volatile bool StopRequested;
        public string? FaultMessage;
    }
}

public sealed class MachineExecutorHostedService(
    MachineExecutor executor,
    IServiceScopeFactory scopeFactory,
    MachineExecutorLeaseService leaseService,
    SafetyLogWriter safetyLogWriter) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!leaseService.TryAcquire())
        {
            await safetyLogWriter.WriteAsync(
                "runtime",
                "Error",
                "MachineExecutor lease is unavailable. This instance is read-only for execution.",
                new SafetyLogContext(Source: "MachineExecutorHostedService"),
                cancellationToken: stoppingToken);
            return;
        }

        executor.Attach(scopeFactory);
        try
        {
            await safetyLogWriter.WriteAsync(
                "runtime",
                "Information",
                "MachineExecutor lease acquired.",
                new SafetyLogContext(Source: "MachineExecutorHostedService"),
                cancellationToken: stoppingToken);
            await executor.RunAsync(stoppingToken);
        }
        finally
        {
            leaseService.Release();
        }
    }
}
