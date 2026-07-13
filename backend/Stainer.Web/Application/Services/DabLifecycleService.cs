using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.Devices;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class DabLifecycleService(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService,
    FluidicsControlService fluidicsControlService)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<IReadOnlyList<DabMixPositionResponse>> ListPositionsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.DabMixPositions
            .AsNoTracking()
            .OrderBy(x => x.PositionNo)
            .Select(x => new DabMixPositionResponse(
                x.Id,
                x.Code,
                x.PositionNo,
                x.IsEnabled,
                x.Status,
                x.ActiveDabBatchId,
                x.UpdatedAtUtc))
            .ToListAsync(cancellationToken);
    }

    public async Task<DabBatchResponse> GetBatchAsync(string batchId, CancellationToken cancellationToken = default)
    {
        var batch = await BatchQuery(asTracking: false)
            .SingleOrDefaultAsync(x => x.Id == batchId, cancellationToken);
        if (batch is null)
        {
            throw new BusinessRuleException("dab_batch_not_found", "DAB batch was not found.", StatusCodes.Status404NotFound);
        }

        return ToResponse(batch, null, false, "DAB batch loaded.");
    }

    public Task<DabBatchResponse> CreateBatchAsync(
        CreateDabBatchRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "dab.batch.create",
            request,
            actor,
            async () =>
            {
                var taskIds = request.TaskIds
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct(StringComparer.Ordinal)
                    .ToList();
                if (taskIds.Count == 0 || taskIds.Count != request.TaskIds.Count)
                {
                    throw new BusinessRuleException("dab_tasks_invalid", "At least one distinct non-empty taskId is required.");
                }

                if (string.IsNullOrWhiteSpace(request.DabAReagentBottleId)
                    || string.IsNullOrWhiteSpace(request.DabBReagentBottleId)
                    || string.Equals(request.DabAReagentBottleId, request.DabBReagentBottleId, StringComparison.Ordinal))
                {
                    throw new BusinessRuleException("dab_source_bottles_invalid", "Distinct DAB A and DAB B source bottles are required.");
                }

                var tasks = await dbContext.StainingTasks
                    .Include(x => x.WorkflowVersion)
                    .ThenInclude(x => x!.Steps)
                    .Where(x => taskIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);
                if (tasks.Count != taskIds.Count)
                {
                    throw new BusinessRuleException("dab_task_not_found", "One or more DAB tasks were not found.", StatusCodes.Status404NotFound);
                }

                var nonDabTasks = tasks
                    .Where(x => x.WorkflowVersion?.Steps.Any(step =>
                        step.ActionType.Equals("Dab", StringComparison.OrdinalIgnoreCase)) != true)
                    .Select(x => x.Id)
                    .ToList();
                if (nonDabTasks.Count > 0)
                {
                    throw new BusinessRuleException("dab_task_incompatible", "Every selected task must use a workflow containing a DAB step.", StatusCodes.Status409Conflict);
                }

                var alreadyAssigned = await dbContext.DabBatchTasks
                    .Where(x => taskIds.Contains(x.StainingTaskId) && x.DabBatch!.Status != DabBatchStatus.Cleaned)
                    .Select(x => x.StainingTaskId)
                    .ToListAsync(cancellationToken);
                if (alreadyAssigned.Count > 0)
                {
                    throw new BusinessRuleException("dab_task_already_assigned", "One or more tasks already belong to an uncleaned DAB batch.", StatusCodes.Status409Conflict);
                }

                var sourceIds = new[] { request.DabAReagentBottleId.Trim(), request.DabBReagentBottleId.Trim() };
                var bottles = await dbContext.ReagentBottles
                    .Where(x => sourceIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);
                if (bottles.Count != 2)
                {
                    throw new BusinessRuleException("dab_source_bottle_not_found", "DAB A or DAB B source bottle was not found.", StatusCodes.Status404NotFound);
                }

                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                if (bottles.Any(x => !x.Status.Equals("Available", StringComparison.OrdinalIgnoreCase)
                    || x.ExpirationDate < today))
                {
                    throw new BusinessRuleException("dab_source_bottle_unavailable", "DAB source bottles must be available and unexpired.", StatusCodes.Status409Conflict);
                }

                DabFormulaVolumes required;
                try
                {
                    required = DabFormula.CalculateRequired(taskIds.Count);
                }
                catch (OverflowException)
                {
                    throw new BusinessRuleException("dab_volume_invalid", "Calculated DAB volume exceeds the supported integer range.");
                }

                var dabABottle = bottles.Single(x => x.Id == sourceIds[0]);
                var dabBBottle = bottles.Single(x => x.Id == sourceIds[1]);

                var position = await SelectPositionAsync(request.PositionCode, cancellationToken);
                var now = DateTimeOffset.UtcNow;
                var batch = new DabBatch
                {
                    DabMixPosition = position,
                    DabMixPositionId = position.Id,
                    PositionCode = position.Code,
                    DabAReagentBottle = dabABottle,
                    DabAReagentBottleId = dabABottle.Id,
                    DabBReagentBottle = dabBBottle,
                    DabBReagentBottleId = dabBBottle.Id,
                    CreatedByUserId = actor.UserId,
                    Status = DabBatchStatus.PendingPreparation,
                    CleaningStatus = DabCleaningStatus.NotRequired,
                    SlideCount = taskIds.Count,
                    TotalRequiredVolumeUl = required.TotalVolumeUl,
                    CreatedAtUtc = now,
                    UpdatedAtUtc = now
                };
                await ReserveBottleSourceAsync(
                    batch,
                    "DabA",
                    dabABottle.ReagentCode,
                    dabABottle.Id,
                    required.DabAVolumeUl,
                    request.CommandId,
                    actor,
                    now,
                    cancellationToken);
                await ReserveBottleSourceAsync(
                    batch,
                    "DabB",
                    dabBBottle.ReagentCode,
                    dabBBottle.Id,
                    required.DabBVolumeUl,
                    request.CommandId,
                    actor,
                    now,
                    cancellationToken);
                batch.ReagentReservations.Add(new ReagentReservation
                {
                    ReagentCode = "WATER",
                    ReservationKind = ReagentReservationKind.DabBatch,
                    SourceRole = "Water",
                    Status = ReagentReservationStatus.Reserved,
                    CommandId = request.CommandId,
                    CreatedByUserId = actor.UserId,
                    RequiredVolumeUl = required.WaterVolumeUl,
                    ReservedVolumeUl = required.WaterVolumeUl,
                    CreatedAtUtc = now
                });
                foreach (var task in tasks.OrderBy(x => x.TaskCode))
                {
                    batch.Tasks.Add(new DabBatchTask
                    {
                        StainingTask = task,
                        StainingTaskId = task.Id,
                        RequiredVolumeUl = DabFormula.VolumePerSlideUl,
                        CreatedAtUtc = now
                    });
                }

                position.Status = DabMixPositionStatus.Occupied;
                position.ActiveDabBatchId = batch.Id;
                position.UpdatedAtUtc = now;
                dbContext.DabBatches.Add(batch);
                AddAudit(actor, request.CommandId, "dab.batch.create", batch, new
                {
                    batch.PositionCode,
                    taskIds,
                    batch.DabAReagentBottleId,
                    batch.DabBReagentBottleId,
                    batch.TotalRequiredVolumeUl,
                    reservations = batch.ReagentReservations.Select(x => new
                    {
                        x.SourceRole,
                        x.ReagentCode,
                        x.ReagentBottleId,
                        x.ReservedVolumeUl
                    }),
                    ratio = "1:1:18"
                }, now);

                return Result(batch, request.CommandId, "DAB batch created and mix position occupied.");
            },
            cancellationToken);
    }

    public Task<DabBatchResponse> StartPreparationAsync(
        string batchId,
        DabBatchCommandRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return MutateAsync(batchId, request.CommandId, "dab.batch.preparation.start", request, actor,
            (batch, now) =>
            {
                RequireStatus(batch, DabBatchStatus.PendingPreparation);
                batch.Status = DabBatchStatus.Preparing;
                batch.UpdatedAtUtc = now;
                AddAudit(actor, request.CommandId, "dab.batch.preparation.start", batch, new { batch.PositionCode }, now);
                return "DAB preparation started.";
            }, cancellationToken);
    }

    public Task<DabBatchResponse> CompletePreparationAsync(
        string batchId,
        CompleteDabPreparationRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return MutateAsync(batchId, request.CommandId, "dab.batch.preparation.complete", request, actor,
            (batch, now) =>
            {
                RequireStatus(batch, DabBatchStatus.Preparing);
                if (request.ActualPreparedVolumeUl != batch.TotalRequiredVolumeUl)
                {
                    throw new BusinessRuleException("dab_prepared_volume_mismatch", "Actual prepared volume must match the reserved total volume in DAB lifecycle phase 08.");
                }

                DabFormulaVolumes actual;
                try
                {
                    actual = DabFormula.Calculate(request.ActualPreparedVolumeUl);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new BusinessRuleException("dab_volume_invalid", "Actual prepared volume must be a positive multiple of 20 uL.");
                }

                RequireReservedSources(batch, actual);
                batch.ActualPreparedVolumeUl = actual.TotalVolumeUl;
                batch.DabAVolumeUl = actual.DabAVolumeUl;
                batch.DabBVolumeUl = actual.DabBVolumeUl;
                batch.WaterVolumeUl = actual.WaterVolumeUl;
                batch.UsedVolumeUl = 0;
                batch.RemainingVolumeUl = actual.TotalVolumeUl;
                batch.PreparedAtUtc = now;
                batch.ExpiresAtUtc = now.AddHours(DabFormula.ValidityHours);
                batch.Status = DabBatchStatus.Available;
                batch.UpdatedAtUtc = now;
                AddAudit(actor, request.CommandId, "dab.batch.preparation.complete", batch, new
                {
                    batch.ActualPreparedVolumeUl,
                    batch.DabAVolumeUl,
                    batch.DabBVolumeUl,
                    batch.WaterVolumeUl,
                    batch.PreparedAtUtc,
                    batch.ExpiresAtUtc
                }, now);
                return "DAB preparation completed and is available for three hours.";
            }, cancellationToken);
    }

    public Task<DabBatchResponse> ConsumeAsync(
        string batchId,
        ConsumeDabBatchRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return MutateAsync(batchId, request.CommandId, "dab.batch.consume", request, actor,
            (batch, now) =>
            {
                RequireStatus(batch, DabBatchStatus.Available);
                if (batch.ExpiresAtUtc is null || batch.ExpiresAtUtc <= now)
                {
                    throw new BusinessRuleException("dab_batch_expired", "DAB batch has expired and must be marked expired before cleaning.", StatusCodes.Status409Conflict);
                }

                if (request.VolumeUl <= 0 || request.VolumeUl > batch.RemainingVolumeUl)
                {
                    throw new BusinessRuleException("dab_usage_volume_invalid", "Usage volume must be positive and cannot exceed remaining volume.");
                }

                var taskId = string.IsNullOrWhiteSpace(request.StainingTaskId) ? null : request.StainingTaskId.Trim();
                if (taskId is not null && batch.Tasks.All(x => x.StainingTaskId != taskId))
                {
                    throw new BusinessRuleException("dab_task_not_in_batch", "Usage task does not belong to this DAB batch.", StatusCodes.Status409Conflict);
                }

                batch.UsedVolumeUl += request.VolumeUl;
                batch.RemainingVolumeUl -= request.VolumeUl;
                batch.UpdatedAtUtc = now;
                dbContext.DabBatchUsages.Add(new DabBatchUsage
                {
                    DabBatch = batch,
                    StainingTaskId = taskId,
                    CommandId = request.CommandId,
                    CreatedByUserId = actor.UserId,
                    VolumeUl = request.VolumeUl,
                    CreatedAtUtc = now
                });
                if (batch.RemainingVolumeUl == 0)
                {
                    batch.Status = DabBatchStatus.Depleted;
                    RequireCleaning(batch, now);
                }

                AddAudit(actor, request.CommandId, "dab.batch.consume", batch, new
                {
                    request.VolumeUl,
                    taskId,
                    batch.UsedVolumeUl,
                    batch.RemainingVolumeUl,
                    batch.Status
                }, now);
                return batch.Status == DabBatchStatus.Depleted
                    ? "DAB batch depleted; cleaning is required."
                    : "DAB usage recorded.";
            }, cancellationToken);
    }

    public Task<DabBatchResponse> MarkExpiredAsync(
        string batchId,
        DabBatchCommandRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return MutateAsync(batchId, request.CommandId, "dab.batch.expire", request, actor,
            (batch, now) =>
            {
                RequireStatus(batch, DabBatchStatus.Available);
                if (batch.ExpiresAtUtc is null || batch.ExpiresAtUtc > now)
                {
                    throw new BusinessRuleException("dab_batch_not_expired", "DAB batch has not reached its expiry time.", StatusCodes.Status409Conflict);
                }

                batch.Status = DabBatchStatus.Expired;
                batch.UpdatedAtUtc = now;
                RequireCleaning(batch, now);
                AddAudit(actor, request.CommandId, "dab.batch.expire", batch, new { batch.ExpiresAtUtc }, now);
                return "DAB batch marked expired; cleaning is required.";
            }, cancellationToken);
    }

    public Task<DabBatchResponse> FailAsync(
        string batchId,
        FailDabBatchRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return MutateAsync(batchId, request.CommandId, "dab.batch.fail", request, actor,
            (batch, now) =>
            {
                if (string.IsNullOrWhiteSpace(request.Reason))
                {
                    throw new BusinessRuleException("reason_required", "Failure reason is required.");
                }

                var previousStatus = batch.Status;
                if (previousStatus is not (DabBatchStatus.PendingPreparation or DabBatchStatus.Preparing or DabBatchStatus.Available))
                {
                    throw InvalidTransition(batch.Status, DabBatchStatus.Failed);
                }

                batch.Status = DabBatchStatus.Failed;
                batch.UpdatedAtUtc = now;
                RequireManualResolution(batch, now);
                MarkReservationsForFailure(batch, previousStatus, now);
                AddAudit(actor, request.CommandId, "dab.batch.fail", batch, new { reason = request.Reason.Trim() }, now);
                return "DAB batch marked failed; manual resolution is required before the mix position can be released.";
            }, cancellationToken);
    }

    public Task<DabBatchResponse> StartCleaningAsync(
        string batchId,
        DabBatchCommandRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return MutateAsync(batchId, request.CommandId, "dab.batch.cleaning.start", request, actor,
            (batch, now) =>
            {
                if (batch.Status is not (DabBatchStatus.Depleted or DabBatchStatus.Expired))
                {
                    throw InvalidTransition(batch.Status, DabBatchStatus.AwaitingCleaning);
                }

                batch.Status = DabBatchStatus.AwaitingCleaning;
                batch.CleaningStatus = DabCleaningStatus.Required;
                batch.UpdatedAtUtc = now;
                batch.DabMixPosition!.Status = DabMixPositionStatus.AwaitingCleaning;
                batch.DabMixPosition.UpdatedAtUtc = now;
                AddAudit(actor, request.CommandId, "dab.batch.cleaning.start", batch, new { batch.PositionCode }, now);
                return "DAB cleaning started.";
            }, cancellationToken);
    }

    public Task<DabBatchResponse> ConfirmCleaningAsync(
        string batchId,
        DabBatchCommandRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return MutateAsync(batchId, request.CommandId, "dab.batch.cleaning.confirm", request, actor,
            (batch, now) =>
            {
                RequireStatus(batch, DabBatchStatus.AwaitingCleaning);
                batch.Status = DabBatchStatus.Cleaned;
                batch.CleaningStatus = DabCleaningStatus.Confirmed;
                batch.CleaningConfirmedAtUtc = now;
                batch.UpdatedAtUtc = now;
                batch.DabMixPosition!.Status = DabMixPositionStatus.Available;
                batch.DabMixPosition.ActiveDabBatchId = null;
                batch.DabMixPosition.UpdatedAtUtc = now;
                AddAudit(actor, request.CommandId, "dab.batch.cleaning.confirm", batch, new { batch.PositionCode }, now);
                return "DAB cleaning confirmed and mix position released.";
            }, cancellationToken);
    }

    public async Task<DabExecutorMutationResult> CompletePreparationFromDeviceAsync(
        string batchId,
        MachineRun run,
        WorkflowStepExecution step,
        DeviceCommandExecution command,
        DeviceCommandResult deviceResult,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(run.Id)
            || string.IsNullOrWhiteSpace(step.Id)
            || string.IsNullOrWhiteSpace(command.Id)
            || command.MachineRunId != run.Id
            || command.WorkflowStepExecutionId != step.Id)
        {
            return DabExecutorMutationResult.Failure("dab_context_missing", "DAB preparation requires matching machine run, workflow step and device command context.");
        }

        if (!deviceResult.Ok || deviceResult.Status != DeviceCommandStatuses.Succeeded)
        {
            return DabExecutorMutationResult.Failure("dab_device_not_completed", "DAB source consumption is only allowed after the device reports successful completion.");
        }

        var batch = await BatchQuery(asTracking: true)
            .SingleOrDefaultAsync(x => x.Id == batchId, cancellationToken);
        if (batch is null)
        {
            return DabExecutorMutationResult.Failure("dab_batch_not_found", "DAB batch was not found.");
        }

        if (await dbContext.ReagentConsumptions.AnyAsync(x => x.DabBatchId == batch.Id && x.DeviceCommandExecutionId == command.Id, cancellationToken)
            || await dbContext.SystemLiquidUsages.AnyAsync(x => x.DabBatchId == batch.Id && x.DeviceCommandExecutionId == command.Id, cancellationToken))
        {
            return DabExecutorMutationResult.Success(batch, "DAB preparation result was already applied.");
        }

        if (batch.Status is not (DabBatchStatus.PendingPreparation or DabBatchStatus.Preparing))
        {
            return DabExecutorMutationResult.Failure("dab_status_transition_invalid", $"DAB batch cannot complete preparation from {batch.Status}.", batch);
        }

        DabFormulaVolumes actual;
        try
        {
            actual = DabFormula.Calculate(batch.TotalRequiredVolumeUl);
        }
        catch (ArgumentOutOfRangeException)
        {
            return DabExecutorMutationResult.Failure("dab_volume_invalid", $"DAB batch has invalid total volume {batch.TotalRequiredVolumeUl} uL.", batch);
        }

        var reserved = batch.ReagentReservations
            .Where(x => x.Status == ReagentReservationStatus.Reserved)
            .ToList();
        if (reserved.Where(x => x.SourceRole == "DabA").Sum(x => x.ReservedVolumeUl) != actual.DabAVolumeUl
            || reserved.Where(x => x.SourceRole == "DabB").Sum(x => x.ReservedVolumeUl) != actual.DabBVolumeUl
            || reserved.Where(x => x.SourceRole == "Water").Sum(x => x.ReservedVolumeUl) != actual.WaterVolumeUl)
        {
            return DabExecutorMutationResult.Failure("dab_source_reservation_mismatch", "DAB source reservations do not match the formula.", batch);
        }

        var bottleReservations = reserved.Where(x => x.SourceRole is "DabA" or "DabB").ToList();
        if (bottleReservations.Any(x => x.ReagentBottle is null || x.ReagentBottle.RemainingVolumeUl < x.ReservedVolumeUl))
        {
            return DabExecutorMutationResult.Failure("dab_source_volume_insufficient", "DAB source bottle volume is insufficient.", batch);
        }

        var now = DateTimeOffset.UtcNow;
        foreach (var reservation in bottleReservations)
        {
            var bottle = reservation.ReagentBottle!;
            bottle.RemainingVolumeUl -= reservation.ReservedVolumeUl;
            bottle.UpdatedAtUtc = now;
            reservation.Status = ReagentReservationStatus.Consumed;
            reservation.UpdatedAtUtc = now;
            dbContext.ReagentConsumptions.Add(new ReagentConsumption
            {
                MachineRunId = run.Id,
                WorkflowStepExecutionId = step.Id,
                DeviceCommandExecutionId = command.Id,
                DabBatchId = batch.Id,
                ReagentBottleId = bottle.Id,
                ReagentCode = reservation.ReagentCode,
                SourceRole = reservation.SourceRole,
                VolumeUl = reservation.ReservedVolumeUl,
                CreatedAtUtc = now
            });
            dbContext.DispenseExecutions.Add(new DispenseExecution
            {
                DeviceCommandExecutionId = command.Id,
                ReagentBottleId = bottle.Id,
                ReagentCode = reservation.ReagentCode,
                VolumeUl = reservation.ReservedVolumeUl,
                SourcePositionCode = reservation.SourceRole,
                TargetSlotCode = batch.PositionCode,
                Status = DeviceCommandStatus.Completed,
                CreatedAtUtc = now
            });
        }

        var waterReservation = reserved.Single(x => x.SourceRole == "Water");
        waterReservation.Status = ReagentReservationStatus.Consumed;
        waterReservation.UpdatedAtUtc = now;
        var waterConsumption = await fluidicsControlService.ConsumeSystemLiquidFromRunAsync(
            SystemLiquidSourceTypes.SystemWater,
            waterReservation.ReservedVolumeUl,
            run,
            step,
            command,
            cancellationToken);
        if (!waterConsumption.Ok)
        {
            return DabExecutorMutationResult.Failure(waterConsumption.ErrorCode ?? "system_water_unavailable", waterConsumption.Message, batch);
        }

        dbContext.SystemLiquidUsages.Add(new SystemLiquidUsage
        {
            MachineRunId = run.Id,
            WorkflowStepExecutionId = step.Id,
            DeviceCommandExecutionId = command.Id,
            DabBatchId = batch.Id,
            SourceType = SystemLiquidSourceTypes.SystemWater,
            VolumeUl = waterReservation.ReservedVolumeUl,
            LevelSnapshotJson = JsonSerializer.Serialize(waterConsumption.Data),
            CreatedAtUtc = now
        });

        batch.Status = DabBatchStatus.Available;
        batch.CleaningStatus = DabCleaningStatus.NotRequired;
        batch.ActualPreparedVolumeUl = actual.TotalVolumeUl;
        batch.DabAVolumeUl = actual.DabAVolumeUl;
        batch.DabBVolumeUl = actual.DabBVolumeUl;
        batch.WaterVolumeUl = actual.WaterVolumeUl;
        batch.UsedVolumeUl = 0;
        batch.RemainingVolumeUl = actual.TotalVolumeUl;
        batch.PreparedAtUtc = now;
        batch.ExpiresAtUtc = now.AddHours(DabFormula.ValidityHours);
        batch.UpdatedAtUtc = now;
        batch.DabMixPosition!.Status = DabMixPositionStatus.Occupied;
        batch.DabMixPosition.ActiveDabBatchId = batch.Id;
        batch.DabMixPosition.UpdatedAtUtc = now;
        dbContext.AuditLogs.Add(new AuditLog
        {
            Action = "run.dab_preparation_completed",
            EntityType = "DabBatch",
            EntityId = batch.Id,
            Message = JsonSerializer.Serialize(new
            {
                runId = run.Id,
                workflowStepExecutionId = step.Id,
                deviceCommandExecutionId = command.Id,
                batch.ActualPreparedVolumeUl,
                batch.DabAVolumeUl,
                batch.DabBVolumeUl,
                batch.WaterVolumeUl
            }),
            CreatedAtUtc = now
        });
        ValidateStateInvariants(batch);
        return DabExecutorMutationResult.Success(batch, "DAB preparation completed and reservations converted to consumption.");
    }

    public async Task<DabExecutorMutationResult> HandlePreparationNotCompletedFromDeviceAsync(
        MachineRun run,
        WorkflowStepExecution step,
        DeviceCommandResult deviceResult,
        bool deviceOutcomeUnknown,
        CancellationToken cancellationToken = default)
    {
        var stainingTaskId = step.WorkflowExecution?.SlideTask?.StainingTaskId;
        if (string.IsNullOrWhiteSpace(run.Id) || string.IsNullOrWhiteSpace(step.Id) || string.IsNullOrWhiteSpace(stainingTaskId))
        {
            return DabExecutorMutationResult.Failure("dab_context_missing", "DAB failure handling requires machine run and workflow step context.");
        }

        var candidates = await BatchQuery(asTracking: true)
            .Where(x => x.Tasks.Any(task => task.StainingTaskId == stainingTaskId)
                && (x.Status == DabBatchStatus.PendingPreparation || x.Status == DabBatchStatus.Preparing))
            .ToListAsync(cancellationToken);
        var batch = candidates.OrderBy(x => x.CreatedAtUtc).FirstOrDefault();
        if (batch is null)
        {
            return DabExecutorMutationResult.Failure("dab_batch_not_prepared", "No formal DAB batch is assigned to this DAB workflow step.");
        }

        var now = DateTimeOffset.UtcNow;
        if (deviceOutcomeUnknown)
        {
            batch.Status = DabBatchStatus.Unknown;
            batch.CleaningStatus = DabCleaningStatus.NeedsManualResolution;
            batch.UpdatedAtUtc = now;
            batch.DabMixPosition!.Status = DabMixPositionStatus.NeedsManualResolution;
            batch.DabMixPosition.UpdatedAtUtc = now;
            AddExecutorAudit(run.Id, step.Id, "run.dab_preparation_unknown", batch, deviceResult, now);
            ValidateStateInvariants(batch);
            return DabExecutorMutationResult.Failure("dab_preparation_unknown", deviceResult.Message, batch);
        }

        batch.Status = DabBatchStatus.Failed;
        batch.CleaningStatus = DabCleaningStatus.NeedsManualResolution;
        batch.UpdatedAtUtc = now;
        batch.DabMixPosition!.Status = DabMixPositionStatus.NeedsManualResolution;
        batch.DabMixPosition.UpdatedAtUtc = now;
        foreach (var reservation in batch.ReagentReservations.Where(x => x.Status == ReagentReservationStatus.Reserved))
        {
            reservation.Status = ReagentReservationStatus.Released;
            reservation.UpdatedAtUtc = now;
        }

        AddExecutorAudit(run.Id, step.Id, "run.dab_preparation_failed", batch, deviceResult, now);
        ValidateStateInvariants(batch);
        return DabExecutorMutationResult.Failure("dab_preparation_failed", deviceResult.Message, batch);
    }

    private Task<DabBatchResponse> MutateAsync<TRequest>(
        string batchId,
        string commandId,
        string operation,
        TRequest request,
        AuthenticatedUser actor,
        Func<DabBatch, DateTimeOffset, string> mutate,
        CancellationToken cancellationToken)
        where TRequest : class
    {
        return idempotencyService.RunAsync(
            commandId,
            operation,
            new { batchId, request },
            actor,
            async () =>
            {
                var batch = await BatchQuery(asTracking: true)
                    .SingleOrDefaultAsync(x => x.Id == batchId, cancellationToken);
                if (batch is null)
                {
                    throw new BusinessRuleException("dab_batch_not_found", "DAB batch was not found.", StatusCodes.Status404NotFound);
                }

                var message = mutate(batch, DateTimeOffset.UtcNow);
                return Result(batch, commandId, message);
            },
            cancellationToken);
    }

    public async Task<DabExpiryProcessingResult> ProcessExpirationsAsync(
        DateTimeOffset now,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var batches = (await BatchQuery(asTracking: true)
                .Where(x => x.Status == DabBatchStatus.Available || x.Status == DabBatchStatus.Expired)
                .ToListAsync(cancellationToken))
            .Where(x => x.Status == DabBatchStatus.Expired || (x.ExpiresAtUtc is not null && x.ExpiresAtUtc <= now))
            .OrderBy(x => x.ExpiresAtUtc)
            .ToList();

        var newlyExpired = 0;
        var plansCreated = 0;
        var replacementsCreated = 0;
        foreach (var batch in batches)
        {
            if (batch.Status == DabBatchStatus.Available)
            {
                batch.Status = DabBatchStatus.Expired;
                batch.UpdatedAtUtc = now;
                RequireCleaning(batch, now);
                AddSystemAudit("dab.batch.expired.automatic", batch.Id, new { batch.ExpiresAtUtc, processedAtUtc = now }, now);
                newlyExpired++;
            }

            var unfinishedSteps = (await dbContext.WorkflowStepExecutions
                    .Include(x => x.WorkflowExecution)
                    .ThenInclude(x => x!.SlideTask)
                    .Include(x => x.WorkflowExecution)
                    .ThenInclude(x => x!.MachineRun)
                    .Where(x => x.WorkflowExecution != null
                        && x.WorkflowExecution.SlideTask != null
                        && batch.Tasks.Select(task => task.StainingTaskId).Contains(x.WorkflowExecution.SlideTask.StainingTaskId)
                        && x.WorkflowExecution.MachineRun != null
                        && x.WorkflowExecution.MachineRun.Status != RuntimeLedgerStatus.Completed
                        && x.WorkflowExecution.MachineRun.Status != RuntimeLedgerStatus.Stopped
                        && (x.Status == RuntimeLedgerStatus.Pending
                            || x.Status == RuntimeLedgerStatus.Running
                            || x.Status == RuntimeLedgerStatus.Failed
                            || x.Status == RuntimeLedgerStatus.Unknown))
                    .ToListAsync(cancellationToken))
                .Where(IsDabWorkflowStep)
                .ToList();

            foreach (var runGroup in unfinishedSteps.GroupBy(x => x.WorkflowExecution!.MachineRunId))
            {
                var plan = await dbContext.DabRepreparationPlans
                    .Include(x => x.ExpiredDabBatch)
                    .SingleOrDefaultAsync(x => x.ExpiredDabBatchId == batch.Id && x.MachineRunId == runGroup.Key, cancellationToken);
                if (plan is null)
                {
                    plan = new DabRepreparationPlan
                    {
                        ExpiredDabBatch = batch,
                        ExpiredDabBatchId = batch.Id,
                        MachineRunId = runGroup.Key,
                        Status = DabRepreparationPlanStatus.AwaitingMixPosition,
                        Reason = "The assigned DAB batch expired while the run still had unfinished DAB operations.",
                        CreatedAtUtc = now,
                        UpdatedAtUtc = now
                    };
                    dbContext.DabRepreparationPlans.Add(plan);
                    AddRunAlarm(runGroup.Key, "dab_expired_repreparation_required", "Critical", "DAB 已到期，运行存在未完成的 DAB 操作，已创建重配计划。", now);
                    AddSystemAudit("run.dab_repreparation_planned", plan.Id, new { runId = runGroup.Key, expiredDabBatchId = batch.Id }, now, "DabRepreparationPlan");
                    plansCreated++;
                }

                if (plan.ReplacementDabBatchId is not null || plan.Status == DabRepreparationPlanStatus.NeedsManualResolution)
                {
                    continue;
                }

                var taskIds = runGroup
                    .Select(x => x.WorkflowExecution!.SlideTask!.StainingTaskId)
                    .Distinct(StringComparer.Ordinal)
                    .ToList();
                try
                {
                    var replacement = await CreateReplacementBatchAsync(batch, taskIds, plan, now, cancellationToken);
                    plan.ReplacementDabBatch = replacement;
                    plan.ReplacementDabBatchId = replacement.Id;
                    plan.Status = DabRepreparationPlanStatus.Planned;
                    plan.UpdatedAtUtc = now;
                    replacementsCreated++;
                }
                catch (BusinessRuleException exception) when (exception.Code == "dab_positions_unavailable")
                {
                    plan.Status = DabRepreparationPlanStatus.AwaitingMixPosition;
                    plan.UpdatedAtUtc = now;
                    AddRunAlarm(runGroup.Key, "dab_mix_area_cleaning_required", "Critical", "DAB 配液区需清洗：M1–M8 当前均不可用于重配。", now);
                }
                catch (BusinessRuleException exception)
                {
                    plan.Status = DabRepreparationPlanStatus.NeedsManualResolution;
                    plan.Reason = exception.Message;
                    plan.UpdatedAtUtc = now;
                    AddRunAlarm(runGroup.Key, exception.Code, "Critical", exception.Message, now);
                }
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return new DabExpiryProcessingResult(newlyExpired, plansCreated, replacementsCreated);
    }

    private async Task<DabBatch> CreateReplacementBatchAsync(
        DabBatch expiredBatch,
        IReadOnlyList<string> taskIds,
        DabRepreparationPlan plan,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        if (taskIds.Count == 0 || expiredBatch.DabAReagentBottleId is null || expiredBatch.DabBReagentBottleId is null)
        {
            throw new BusinessRuleException("dab_repreparation_source_invalid", "DAB re-preparation requires unfinished tasks and verified source bottles.", StatusCodes.Status409Conflict);
        }

        var position = await SelectPositionAsync(null, cancellationToken);
        var required = DabFormula.CalculateRequired(taskIds.Count);
        var commandId = $"dab-reprepare-{plan.Id}";
        var systemActor = new AuthenticatedUser(string.Empty, "system", "System", "system", ["admin"]);
        var batch = new DabBatch
        {
            DabMixPosition = position,
            DabMixPositionId = position.Id,
            PositionCode = position.Code,
            DabAReagentBottleId = expiredBatch.DabAReagentBottleId,
            DabBReagentBottleId = expiredBatch.DabBReagentBottleId,
            Status = DabBatchStatus.PendingPreparation,
            CleaningStatus = DabCleaningStatus.NotRequired,
            SlideCount = taskIds.Count,
            TotalRequiredVolumeUl = required.TotalVolumeUl,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };
        await ReserveBottleSourceAsync(batch, "DabA", expiredBatch.DabAReagentBottle!.ReagentCode, expiredBatch.DabAReagentBottleId, required.DabAVolumeUl, commandId, systemActor, now, cancellationToken);
        await ReserveBottleSourceAsync(batch, "DabB", expiredBatch.DabBReagentBottle!.ReagentCode, expiredBatch.DabBReagentBottleId, required.DabBVolumeUl, commandId, systemActor, now, cancellationToken);
        batch.ReagentReservations.Add(new ReagentReservation
        {
            ReagentCode = "WATER",
            ReservationKind = ReagentReservationKind.DabBatch,
            SourceRole = "Water",
            Status = ReagentReservationStatus.Reserved,
            CommandId = commandId,
            RequiredVolumeUl = required.WaterVolumeUl,
            ReservedVolumeUl = required.WaterVolumeUl,
            CreatedAtUtc = now
        });
        foreach (var taskId in taskIds)
        {
            batch.Tasks.Add(new DabBatchTask { StainingTaskId = taskId, RequiredVolumeUl = DabFormula.VolumePerSlideUl, CreatedAtUtc = now });
        }

        position.Status = DabMixPositionStatus.Occupied;
        position.ActiveDabBatchId = batch.Id;
        position.UpdatedAtUtc = now;
        dbContext.DabBatches.Add(batch);
        AddSystemAudit("run.dab_repreparation_batch_created", batch.Id, new { planId = plan.Id, expiredDabBatchId = expiredBatch.Id, runId = plan.MachineRunId, position = position.Code, taskIds }, now);
        ValidateStateInvariants(batch);
        return batch;
    }

    private static bool IsDabWorkflowStep(WorkflowStepExecution step) =>
        step.MajorStepCode.Contains("DAB", StringComparison.OrdinalIgnoreCase)
        || step.ActionType.Contains("DAB", StringComparison.OrdinalIgnoreCase)
        || string.Equals(step.ReagentCode, "DAB", StringComparison.OrdinalIgnoreCase);

    private void AddRunAlarm(string runId, string code, string severity, string message, DateTimeOffset now)
    {
        if (dbContext.Alarms.Local.Any(x => x.MachineRunId == runId && x.Code == code && x.Status == "Active")
            || dbContext.Alarms.Any(x => x.MachineRunId == runId && x.Code == code && x.Status == "Active"))
        {
            return;
        }

        dbContext.Alarms.Add(new Alarm { MachineRunId = runId, Code = code, Severity = severity, Message = message, Status = "Active", CreatedAtUtc = now });
    }

    private void AddSystemAudit(string action, string entityId, object details, DateTimeOffset now, string entityType = "DabBatch")
    {
        dbContext.AuditLogs.Add(new AuditLog { Action = action, EntityType = entityType, EntityId = entityId, Message = JsonSerializer.Serialize(details, JsonOptions), CreatedAtUtc = now });
    }

    private async Task<DabMixPosition> SelectPositionAsync(string? requestedCode, CancellationToken cancellationToken)
    {
        var positions = await dbContext.DabMixPositions
            .Where(x => x.PositionNo >= 1 && x.PositionNo <= 8)
            .OrderBy(x => x.PositionNo)
            .ToListAsync(cancellationToken);
        if (!string.IsNullOrWhiteSpace(requestedCode))
        {
            var code = requestedCode.Trim().ToUpperInvariant();
            var requested = positions.SingleOrDefault(x => x.Code == code);
            if (requested is null || !requested.IsEnabled || requested.Status == DabMixPositionStatus.Disabled)
            {
                throw new BusinessRuleException("dab_position_unavailable", "Requested DAB mix position is unavailable.", StatusCodes.Status409Conflict);
            }

            if (!await IsPositionFreeAsync(requested, cancellationToken))
            {
                throw new BusinessRuleException("dab_position_occupied", $"DAB mix position {code} is already occupied.", StatusCodes.Status409Conflict);
            }

            return requested;
        }

        foreach (var position in positions.Where(x => x.IsEnabled && x.Status == DabMixPositionStatus.Available && x.ActiveDabBatchId == null))
        {
            if (await IsPositionFreeAsync(position, cancellationToken))
            {
                return position;
            }
        }

        throw new BusinessRuleException("dab_positions_unavailable", "DAB 配液区需清洗：M1–M8 均不可分配。", StatusCodes.Status409Conflict);
    }

    private async Task<bool> IsPositionFreeAsync(DabMixPosition position, CancellationToken cancellationToken)
    {
        if (position.Status != DabMixPositionStatus.Available || position.ActiveDabBatchId is not null)
        {
            return false;
        }

        return !await dbContext.DabBatches.AnyAsync(
            x => x.DabMixPositionId == position.Id && x.Status != DabBatchStatus.Cleaned,
            cancellationToken);
    }

    private async Task ReserveBottleSourceAsync(
        DabBatch batch,
        string sourceRole,
        string reagentCode,
        string preferredBottleId,
        int requiredVolumeUl,
        string commandId,
        AuthenticatedUser actor,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        if (requiredVolumeUl <= 0)
        {
            throw new BusinessRuleException("dab_volume_invalid", "DAB source reservation volume must be positive.");
        }

        var today = DateOnly.FromDateTime(now.UtcDateTime);
        var candidates = (await dbContext.ReagentBottles
            .Where(x => x.ReagentCode == reagentCode
                && x.Status == "Available"
                && x.ExpirationDate >= today)
            .ToListAsync(cancellationToken))
            .OrderByDescending(x => x.Id == preferredBottleId)
            .ThenBy(x => x.ExpirationDate)
            .ThenBy(x => x.CreatedAtUtc)
            .ToList();
        if (candidates.All(x => x.Id != preferredBottleId))
        {
            throw new BusinessRuleException("dab_source_bottle_unavailable", "Requested DAB source bottle is unavailable or expired.", StatusCodes.Status409Conflict);
        }

        var candidateIds = candidates.Select(x => x.Id).ToList();
        var activeReservations = await dbContext.ReagentReservations
            .Where(x => x.ReagentBottleId != null
                && candidateIds.Contains(x.ReagentBottleId)
                && x.Status == ReagentReservationStatus.Reserved)
            .GroupBy(x => x.ReagentBottleId!)
            .Select(x => new { ReagentBottleId = x.Key, ReservedVolumeUl = x.Sum(y => y.ReservedVolumeUl) })
            .ToDictionaryAsync(x => x.ReagentBottleId, x => x.ReservedVolumeUl, cancellationToken);

        var remainingToReserve = requiredVolumeUl;
        foreach (var bottle in candidates)
        {
            var alreadyReserved = activeReservations.GetValueOrDefault(bottle.Id);
            var availableVolumeUl = bottle.RemainingVolumeUl - alreadyReserved;
            if (availableVolumeUl <= 0)
            {
                continue;
            }

            var reservedVolumeUl = Math.Min(remainingToReserve, availableVolumeUl);
            batch.ReagentReservations.Add(new ReagentReservation
            {
                ReagentBottleId = bottle.Id,
                ReagentCode = reagentCode,
                ReservationKind = ReagentReservationKind.DabBatch,
                SourceRole = sourceRole,
                Status = ReagentReservationStatus.Reserved,
                CommandId = commandId,
                CreatedByUserId = string.IsNullOrWhiteSpace(actor.UserId) ? null : actor.UserId,
                RequiredVolumeUl = reservedVolumeUl,
                ReservedVolumeUl = reservedVolumeUl,
                CreatedAtUtc = now
            });

            remainingToReserve -= reservedVolumeUl;
            if (remainingToReserve == 0)
            {
                return;
            }
        }

        throw new BusinessRuleException("dab_source_volume_insufficient", "DAB source bottle volume is insufficient after active reservations are considered.", StatusCodes.Status409Conflict);
    }

    private static void RequireReservedSources(DabBatch batch, DabFormulaVolumes actual)
    {
        var reservedByRole = batch.ReagentReservations
            .Where(x => x.Status == ReagentReservationStatus.Reserved)
            .GroupBy(x => x.SourceRole)
            .ToDictionary(x => x.Key, x => x.Sum(y => y.ReservedVolumeUl), StringComparer.OrdinalIgnoreCase);

        if (reservedByRole.GetValueOrDefault("DabA") < actual.DabAVolumeUl
            || reservedByRole.GetValueOrDefault("DabB") < actual.DabBVolumeUl
            || reservedByRole.GetValueOrDefault("Water") < actual.WaterVolumeUl)
        {
            throw new BusinessRuleException("dab_source_reservation_insufficient", "DAB source reservations are insufficient for the actual prepared formula.", StatusCodes.Status409Conflict);
        }
    }

    private static void MarkReservationsForFailure(DabBatch batch, string previousStatus, DateTimeOffset now)
    {
        var status = previousStatus == DabBatchStatus.PendingPreparation
            ? ReagentReservationStatus.Released
            : ReagentReservationStatus.NeedsManualResolution;
        foreach (var reservation in batch.ReagentReservations.Where(x => x.Status == ReagentReservationStatus.Reserved))
        {
            reservation.Status = status;
            reservation.UpdatedAtUtc = now;
        }
    }

    private IQueryable<DabBatch> BatchQuery(bool asTracking)
    {
        IQueryable<DabBatch> query = dbContext.DabBatches
            .Include(x => x.DabMixPosition)
            .Include(x => x.DabAReagentBottle)
            .Include(x => x.DabBReagentBottle)
            .Include(x => x.Tasks)
            .Include(x => x.ReagentReservations)
            .ThenInclude(x => x.ReagentBottle);
        return asTracking ? query : query.AsNoTracking();
    }

    private static void RequireStatus(DabBatch batch, string expected)
    {
        if (batch.Status != expected)
        {
            throw InvalidTransition(batch.Status, expected);
        }
    }

    private static BusinessRuleException InvalidTransition(string current, string requested)
    {
        return new BusinessRuleException(
            "dab_status_transition_invalid",
            $"DAB batch cannot transition from {current} to {requested}.",
            StatusCodes.Status409Conflict);
    }

    private static void RequireCleaning(DabBatch batch, DateTimeOffset now)
    {
        batch.CleaningStatus = DabCleaningStatus.Required;
        batch.DabMixPosition!.Status = DabMixPositionStatus.AwaitingCleaning;
        batch.DabMixPosition.UpdatedAtUtc = now;
    }

    private static void RequireManualResolution(DabBatch batch, DateTimeOffset now)
    {
        batch.CleaningStatus = DabCleaningStatus.NeedsManualResolution;
        batch.DabMixPosition!.Status = DabMixPositionStatus.NeedsManualResolution;
        batch.DabMixPosition.UpdatedAtUtc = now;
    }

    private void AddAudit(
        AuthenticatedUser actor,
        string commandId,
        string action,
        DabBatch batch,
        object details,
        DateTimeOffset now)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            ActorUserId = actor.UserId,
            Action = action,
            EntityType = "DabBatch",
            EntityId = batch.Id,
            Message = JsonSerializer.Serialize(new { commandId, details }, JsonOptions),
            CreatedAtUtc = now
        });
    }

    private void AddExecutorAudit(
        string runId,
        string stepId,
        string action,
        DabBatch batch,
        DeviceCommandResult deviceResult,
        DateTimeOffset now)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            Action = action,
            EntityType = "DabBatch",
            EntityId = batch.Id,
            Message = JsonSerializer.Serialize(new
            {
                runId,
                workflowStepExecutionId = stepId,
                deviceResult.Status,
                deviceResult.ErrorCode,
                deviceResult.Message
            }),
            CreatedAtUtc = now
        });
    }

    private static CommandExecutionResult<DabBatchResponse> Result(DabBatch batch, string commandId, string message)
    {
        ValidateStateInvariants(batch);
        return new CommandExecutionResult<DabBatchResponse>(
            ToResponse(batch, commandId, false, message),
            "DabBatch",
            batch.Id);
    }

    private static void ValidateStateInvariants(DabBatch batch)
    {
        var position = batch.DabMixPosition
            ?? throw new BusinessRuleException("dab_position_not_loaded", "DAB mix position state must be loaded before lifecycle mutation.");
        switch (batch.Status)
        {
            case DabBatchStatus.PendingPreparation:
            case DabBatchStatus.Preparing:
            case DabBatchStatus.Available:
                if (position.Status != DabMixPositionStatus.Occupied
                    || position.ActiveDabBatchId != batch.Id
                    || batch.CleaningStatus != DabCleaningStatus.NotRequired)
                {
                    throw new BusinessRuleException("dab_state_invariant_violation", "Usable or preparing DAB batches must occupy exactly one mix position and require no cleaning.", StatusCodes.Status409Conflict);
                }
                break;
            case DabBatchStatus.Depleted:
            case DabBatchStatus.Expired:
            case DabBatchStatus.AwaitingCleaning:
                if (position.Status != DabMixPositionStatus.AwaitingCleaning
                    || position.ActiveDabBatchId != batch.Id
                    || batch.CleaningStatus != DabCleaningStatus.Required)
                {
                    throw new BusinessRuleException("dab_state_invariant_violation", "Depleted or expired DAB batches must keep their position awaiting cleaning.", StatusCodes.Status409Conflict);
                }
                break;
            case DabBatchStatus.Failed:
            case DabBatchStatus.Unknown:
                if (position.Status != DabMixPositionStatus.NeedsManualResolution
                    || position.ActiveDabBatchId != batch.Id
                    || batch.CleaningStatus != DabCleaningStatus.NeedsManualResolution)
                {
                    throw new BusinessRuleException("dab_state_invariant_violation", "Failed or unknown DAB batches must keep their position locked for manual resolution.", StatusCodes.Status409Conflict);
                }
                break;
            case DabBatchStatus.Cleaned:
                if (position.Status != DabMixPositionStatus.Available
                    || position.ActiveDabBatchId is not null
                    || batch.CleaningStatus != DabCleaningStatus.Confirmed)
                {
                    throw new BusinessRuleException("dab_state_invariant_violation", "Cleaned DAB batches must release the mix position only after cleaning confirmation.", StatusCodes.Status409Conflict);
                }
                break;
            case DabBatchStatus.LegacyUnverified:
                break;
            default:
                throw new BusinessRuleException("dab_state_unknown", "DAB batch status is not recognized.", StatusCodes.Status409Conflict);
        }
    }

    private static DabBatchResponse ToResponse(DabBatch batch, string? commandId, bool replayed, string message)
    {
        return new DabBatchResponse(
            true,
            commandId,
            replayed,
            batch.Id,
            batch.DabMixPositionId,
            batch.PositionCode,
            batch.Status,
            batch.CleaningStatus,
            batch.DabAReagentBottleId,
            batch.DabAReagentBottle?.FullBarcode,
            batch.DabBReagentBottleId,
            batch.DabBReagentBottle?.FullBarcode,
            batch.Tasks.Select(x => x.StainingTaskId).OrderBy(x => x).ToList(),
            batch.SlideCount,
            batch.VolumePerSlideUl,
            batch.LineReserveVolumeUl,
            batch.DabARatioParts,
            batch.DabBRatioParts,
            batch.WaterRatioParts,
            batch.TotalRequiredVolumeUl,
            batch.ActualPreparedVolumeUl,
            batch.DabAVolumeUl,
            batch.DabBVolumeUl,
            batch.WaterVolumeUl,
            batch.UsedVolumeUl,
            batch.RemainingVolumeUl,
            batch.PreparedAtUtc,
            batch.ExpiresAtUtc,
            batch.CleaningConfirmedAtUtc,
            batch.CreatedAtUtc,
            batch.UpdatedAtUtc,
            batch.ReagentReservations
                .Select(x => new DabReservationResponse(
                    x.Id,
                    x.ReagentCode,
                    x.SourceRole,
                    x.Status,
                    x.ReagentBottleId,
                    x.ReagentBottle?.FullBarcode,
                    x.ReservedVolumeUl))
                .OrderBy(x => x.SourceRole)
                .ThenBy(x => x.ReagentBottleId)
                .ToList(),
            message);
    }
}

public sealed record DabExecutorMutationResult(
    bool Ok,
    string? ErrorCode,
    string Message,
    DabBatch? Batch)
{
    public static DabExecutorMutationResult Success(DabBatch batch, string message) =>
        new(true, null, message, batch);

    public static DabExecutorMutationResult Failure(string errorCode, string message, DabBatch? batch = null) =>
        new(false, errorCode, message, batch);
}

public sealed record DabExpiryProcessingResult(
    int NewlyExpiredCount,
    int RepreparationPlanCount,
    int ReplacementBatchCount);
