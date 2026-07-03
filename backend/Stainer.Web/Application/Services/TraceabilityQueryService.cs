using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class TraceabilityQueryService(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService,
    IRuntimeEventPublisher eventPublisher)
{
    private const int MaxPageSize = 200;
    private const int MaxExportRows = 5000;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<TraceabilityListResponse<HistoryRunSummaryResponse>> ListRunsAsync(IQueryCollection query, CancellationToken cancellationToken = default)
    {
        var filters = TraceFilters.From(query);
        var runQuery = ApplyRunFilters(dbContext.MachineRuns.AsNoTracking(), filters);
        var candidates = await runQuery
            .Select(x => new TraceCandidate(x.Id, x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
        var paged = PageCandidates(candidates, filters, descending: true);
        var runs = await LoadRunSummaries(paged.Ids, cancellationToken);
        return new TraceabilityListResponse<HistoryRunSummaryResponse>(paged.TotalCount, filters.Page, filters.PageSize, runs);
    }

    public async Task<HistoryRunDetailResponse?> GetRunDetailAsync(string machineRunId, CancellationToken cancellationToken = default)
    {
        var run = await dbContext.MachineRuns
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.RequestedByUser)
            .Include(x => x.ChannelBatches)
                .ThenInclude(x => x.SelectedWorkflowVersion)
                    .ThenInclude(x => x!.WorkflowDefinition)
            .Include(x => x.ChannelBatches)
                .ThenInclude(x => x.SlideTasks)
                    .ThenInclude(x => x.StainingTask)
                        .ThenInclude(x => x!.CreatedByUser)
            .Include(x => x.WorkflowExecutions)
                .ThenInclude(x => x.WorkflowVersion)
                    .ThenInclude(x => x!.WorkflowDefinition)
            .Include(x => x.WorkflowExecutions)
                .ThenInclude(x => x.StepExecutions)
            .SingleOrDefaultAsync(x => x.Id == machineRunId, cancellationToken);
        if (run is null)
        {
            return null;
        }

        var deviceCommands = (await dbContext.DeviceCommandExecutions
            .AsNoTracking()
            .Where(x => x.MachineRunId == machineRunId)
            .Select(x => new HistoryDeviceCommandResponse(
                x.Id,
                x.MachineRunId,
                x.WorkflowStepExecutionId,
                x.CommandType,
                x.Status,
                x.LiquidClassVersionId,
                x.LiquidClassVersionNo,
                x.LiquidClassSelectionStatus,
                x.LiquidClassParametersJson,
                x.CreatedAtUtc,
                x.CommandSentAtUtc,
                x.AcknowledgedAtUtc,
                x.CompletedAtUtc))
            .ToListAsync(cancellationToken))
            .OrderBy(x => x.CreatedAtUtc)
            .ToList();

        var reagentConsumptions = (await QueryReagentConsumptions(TraceFilters.ForRun(machineRunId))
            .ToListAsync(cancellationToken))
            .OrderBy(x => x.CreatedAtUtc)
            .Take(MaxExportRows)
            .ToList();

        var dabUsages = (await dbContext.DabBatchUsages
            .AsNoTracking()
            .Include(x => x.DabBatch)
            .Where(x => x.MachineRunId == machineRunId)
            .Select(x => new HistoryDabUsageResponse(
                x.Id,
                x.DabBatchId,
                x.MachineRunId,
                x.WorkflowStepExecutionId,
                x.DabBatch!.PositionCode,
                x.VolumeUl,
                x.CreatedAtUtc,
                x.DabBatch.PreparedAtUtc,
                x.DabBatch.ExpiresAtUtc))
            .ToListAsync(cancellationToken))
            .OrderBy(x => x.CreatedAtUtc)
            .ToList();

        var alarms = await ListAlarmItemsAsync(TraceFilters.ForRun(machineRunId), cancellationToken);

        return new HistoryRunDetailResponse(
            run.Id,
            run.RunCode,
            run.Status,
            run.CreatedAtUtc,
            run.StartedAtUtc,
            run.CompletedAtUtc,
            run.RequestedByUser?.DisplayName ?? run.RequestedByUser?.Username,
            run.CoordinateProfileVersionId,
            run.CoordinateSnapshotJson,
            run.LiquidClassSelectionStatus,
            run.LiquidClassSnapshotJson,
            run.ChannelBatches
                .OrderBy(x => x.DrawerCode)
                .Select(ToHistoryChannelBatch)
                .ToList(),
            run.WorkflowExecutions
                .OrderBy(x => x.SlideTaskId)
                .Select(ToHistoryWorkflowExecution)
                .ToList(),
            deviceCommands,
            reagentConsumptions,
            dabUsages,
            alarms);
    }

    public async Task<TraceabilityListResponse<HistoryReagentConsumptionResponse>> ListReagentConsumptionsAsync(IQueryCollection query, CancellationToken cancellationToken = default)
    {
        var filters = TraceFilters.From(query);
        var rows = await QueryReagentConsumptions(filters).ToListAsync(cancellationToken);
        var filtered = rows
            .Where(x => InRange(x.CreatedAtUtc, filters))
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToList();
        var totalCount = filtered.Count;
        var items = filtered
            .Skip((filters.Page - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .ToList();
        return new TraceabilityListResponse<HistoryReagentConsumptionResponse>(totalCount, filters.Page, filters.PageSize, items);
    }

    public async Task<TraceabilityListResponse<TraceAlarmResponse>> ListAlarmsAsync(IQueryCollection query, CancellationToken cancellationToken = default)
    {
        var filters = TraceFilters.From(query);
        var alarmQuery = ApplyAlarmFilters(dbContext.Alarms.AsNoTracking(), filters);
        var candidates = await alarmQuery
            .Select(x => new TraceCandidate(x.Id, x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
        var paged = PageCandidates(candidates, filters, descending: true);
        var items = await LoadAlarmItemsAsync(paged.Ids, cancellationToken);
        return new TraceabilityListResponse<TraceAlarmResponse>(paged.TotalCount, filters.Page, filters.PageSize, items);
    }

    public async Task<TraceabilityListResponse<AuditLogResponse>> ListAuditLogsAsync(IQueryCollection query, CancellationToken cancellationToken = default)
    {
        var filters = TraceFilters.From(query);
        var auditQuery = ApplyAuditFilters(dbContext.AuditLogs.AsNoTracking().Include(x => x.ActorUser), filters);
        var candidates = await auditQuery
            .Select(x => new TraceCandidate(x.Id, x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
        var paged = PageCandidates(candidates, filters, descending: true);
        var items = await LoadAuditLogItemsAsync(paged.Ids, cancellationToken);
        return new TraceabilityListResponse<AuditLogResponse>(paged.TotalCount, filters.Page, filters.PageSize, items);
    }

    public Task<AlarmMutationResponse> AcknowledgeAlarmAsync(string alarmId, AcknowledgeAlarmRequest request, AuthenticatedUser actor, CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "alarm.acknowledge",
            new { alarmId, request },
            actor,
            async () =>
            {
                var alarm = await dbContext.Alarms
                    .Include(x => x.Actions)
                    .SingleOrDefaultAsync(x => x.Id == alarmId, cancellationToken)
                    ?? throw new BusinessRuleException("alarm_not_found", "Alarm was not found.", StatusCodes.Status404NotFound);
                if (alarm.Status != "Active")
                {
                    throw new BusinessRuleException("alarm_not_active", "Only active alarms can be acknowledged.", StatusCodes.Status409Conflict);
                }

                var highSeverity = IsHighSeverity(alarm.Severity);
                if (highSeverity && !(actor.HasRole("admin") || actor.HasRole("engineer")))
                {
                    throw new BusinessRuleException("alarm_ack_forbidden", "Current user is not allowed to acknowledge this alarm.", StatusCodes.Status403Forbidden);
                }

                var reason = request.Reason?.Trim() ?? string.Empty;
                if (highSeverity && string.IsNullOrWhiteSpace(reason))
                {
                    throw new BusinessRuleException("alarm_ack_reason_required", "Acknowledging Error or Critical alarms requires a reason.", StatusCodes.Status400BadRequest);
                }

                alarm.Status = "Acknowledged";
                alarm.Actions.Add(new AlarmAction
                {
                    ActorUserId = actor.UserId,
                    Action = "Acknowledged",
                    Message = reason,
                    CreatedAtUtc = DateTimeOffset.UtcNow
                });
                dbContext.AuditLogs.Add(new AuditLog
                {
                    ActorUserId = actor.UserId,
                    Action = "alarm.acknowledge",
                    EntityType = "Alarm",
                    EntityId = alarm.Id,
                    Message = JsonSerializer.Serialize(new { commandId = request.CommandId, reason, alarm.Code, alarm.Severity, alarm.MachineRunId }, JsonOptions),
                    CreatedAtUtc = DateTimeOffset.UtcNow
                });

                eventPublisher.Publish(MachineEventMessage.Create(
                    MachineEventTypes.AlarmAcknowledged,
                    alarm.MachineRunId,
                    "Alarm",
                    alarm.Id,
                    null,
                    new Dictionary<string, object?>
                    {
                        ["alarmId"] = alarm.Id,
                        ["code"] = alarm.Code,
                        ["severity"] = alarm.Severity,
                        ["status"] = alarm.Status,
                        ["reason"] = reason
                    }));

                var response = new AlarmMutationResponse(true, request.CommandId, false, alarm.Id, alarm.Status, "Alarm acknowledged.");
                return new CommandExecutionResult<AlarmMutationResponse>(response, "Alarm", alarm.Id);
            },
            cancellationToken);
    }

    public async Task<CsvExportResult> ExportRunsAsync(IQueryCollection query, AuthenticatedUser actor, CancellationToken cancellationToken = default)
    {
        var filters = TraceFilters.From(query) with { Page = 1, PageSize = MaxExportRows };
        var candidates = await ApplyRunFilters(dbContext.MachineRuns.AsNoTracking(), filters)
            .Select(x => new TraceCandidate(x.Id, x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
        var paged = PageCandidates(candidates, filters, descending: true);
        var rows = await LoadRunSummaries(paged.Ids, cancellationToken);
        var csv = BuildCsv(
            ["RunId", "RunCode", "Status", "CreatedAtUtc", "StartedAtUtc", "CompletedAtUtc", "RequestedBy", "Channels", "WorkflowNames", "Slides", "Alarms"],
            rows.Select(x => new[]
            {
                x.MachineRunId, x.RunCode, x.Status, Format(x.CreatedAtUtc), Format(x.StartedAtUtc), Format(x.CompletedAtUtc),
                x.RequestedBy ?? string.Empty, x.Channels, x.WorkflowNames, x.SlideTaskCount.ToString(), x.AlarmCount.ToString()
            }));
        await AddExportAuditAsync(actor, "history_runs", rows.Count, filters, cancellationToken);
        return CsvExport("history-runs", csv, rows.Count);
    }

    public async Task<CsvExportResult> ExportReagentConsumptionsAsync(IQueryCollection query, AuthenticatedUser actor, CancellationToken cancellationToken = default)
    {
        var filters = TraceFilters.From(query) with { Page = 1, PageSize = MaxExportRows };
        var rows = (await QueryReagentConsumptions(filters)
            .ToListAsync(cancellationToken))
            .Where(x => InRange(x.CreatedAtUtc, filters))
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(MaxExportRows)
            .ToList();
        var csv = BuildCsv(
            ["ConsumptionId", "RunId", "StepExecutionId", "ReagentCode", "ReagentName", "BatchNo", "SerialNo", "VolumeUl", "CreatedAtUtc"],
            rows.Select(x => new[]
            {
                x.ReagentConsumptionId, x.MachineRunId, x.WorkflowStepExecutionId, x.ReagentCode, x.ReagentName ?? string.Empty,
                x.ProductionBatchNo ?? string.Empty, x.SerialNo ?? string.Empty, x.VolumeUl.ToString(), Format(x.CreatedAtUtc)
            }));
        await AddExportAuditAsync(actor, "reagent_consumptions", rows.Count, filters, cancellationToken);
        return CsvExport("reagent-consumptions", csv, rows.Count);
    }

    public async Task<CsvExportResult> ExportAlarmsAsync(IQueryCollection query, AuthenticatedUser actor, CancellationToken cancellationToken = default)
    {
        var filters = TraceFilters.From(query) with { Page = 1, PageSize = MaxExportRows };
        var candidates = await ApplyAlarmFilters(dbContext.Alarms.AsNoTracking(), filters)
            .Select(x => new TraceCandidate(x.Id, x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
        var paged = PageCandidates(candidates, filters, descending: true);
        var rows = await LoadAlarmItemsAsync(paged.Ids, cancellationToken);
        var csv = BuildCsv(
            ["AlarmId", "RunId", "Category", "Severity", "Status", "Summary", "SourceChannels", "AckBy", "AckAtUtc", "CreatedAtUtc"],
            rows.Select(x => new[]
            {
                x.AlarmId, x.MachineRunId ?? string.Empty, OperatorAlarmPresentation.Category(x.Code), x.Severity, x.Status,
                OperatorAlarmPresentation.Summary(x.Code, x.Severity), x.SourceChannels ?? string.Empty,
                x.AckBy ?? string.Empty, Format(x.AckAtUtc), Format(x.CreatedAtUtc)
            }));
        await AddExportAuditAsync(actor, "alarms", rows.Count, filters, cancellationToken);
        return CsvExport("alarms", csv, rows.Count);
    }

    public async Task<CsvExportResult> ExportAuditLogsAsync(IQueryCollection query, AuthenticatedUser actor, CancellationToken cancellationToken = default)
    {
        var filters = TraceFilters.From(query) with { Page = 1, PageSize = MaxExportRows };
        var candidates = await ApplyAuditFilters(dbContext.AuditLogs.AsNoTracking().Include(x => x.ActorUser), filters)
            .Select(x => new TraceCandidate(x.Id, x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
        var paged = PageCandidates(candidates, filters, descending: true);
        var rows = await LoadAuditLogItemsAsync(paged.Ids, cancellationToken);
        var csv = BuildCsv(
            ["AuditLogId", "Actor", "Action", "EntityType", "EntityId", "Reason", "CommandId", "CorrelationId", "MachineRunId", "TaskId", "Channel", "Slot", "Summary", "CreatedAtUtc"],
            rows.Select(x => new[]
            {
                x.AuditLogId, x.Actor ?? string.Empty, x.Action, x.EntityType, x.EntityId ?? string.Empty, x.Reason ?? string.Empty,
                x.CommandId ?? string.Empty, x.CorrelationId ?? string.Empty, x.MachineRunId ?? string.Empty, x.TaskId ?? string.Empty,
                x.Channel ?? string.Empty, x.Slot ?? string.Empty, x.Summary, Format(x.CreatedAtUtc)
            }));
        await AddExportAuditAsync(actor, "audit", rows.Count, filters, cancellationToken);
        return CsvExport("audit", csv, rows.Count);
    }

    private async Task<IReadOnlyList<HistoryRunSummaryResponse>> LoadRunSummaries(IReadOnlyList<string> ids, CancellationToken cancellationToken)
    {
        if (ids.Count == 0)
        {
            return [];
        }

        var runs = await dbContext.MachineRuns
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.RequestedByUser)
            .Include(x => x.ChannelBatches)
                .ThenInclude(x => x.SelectedWorkflowVersion)
                    .ThenInclude(x => x!.WorkflowDefinition)
            .Include(x => x.ChannelBatches)
                .ThenInclude(x => x.SlideTasks)
                    .ThenInclude(x => x.StainingTask)
            .Include(x => x.WorkflowExecutions)
                .ThenInclude(x => x.WorkflowVersion)
                    .ThenInclude(x => x!.WorkflowDefinition)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var order = ids.Select((id, index) => new { id, index }).ToDictionary(x => x.id, x => x.index, StringComparer.Ordinal);
        var alarmCounts = await dbContext.Alarms
            .AsNoTracking()
            .Where(x => x.MachineRunId != null && ids.Contains(x.MachineRunId))
            .GroupBy(x => x.MachineRunId!)
            .Select(x => new { MachineRunId = x.Key, Count = x.Count() })
            .ToDictionaryAsync(x => x.MachineRunId, x => x.Count, cancellationToken);
        return runs
            .OrderBy(x => order[x.Id])
            .Select(run =>
            {
                var workflows = run.ChannelBatches
                    .Select(x => x.SelectedWorkflowVersion)
                    .Concat(run.WorkflowExecutions.Select(x => x.WorkflowVersion))
                    .Where(x => x?.WorkflowDefinition is not null)
                    .Select(x => $"{x!.WorkflowDefinition!.Name} v{x.VersionLabel}")
                    .Distinct(StringComparer.Ordinal)
                    .ToList();
                return new HistoryRunSummaryResponse(
                    run.Id,
                    run.RunCode,
                    run.Status,
                    run.CreatedAtUtc,
                    run.StartedAtUtc,
                    run.CompletedAtUtc,
                    run.RequestedByUser?.DisplayName ?? run.RequestedByUser?.Username,
                    run.CoordinateProfileVersionId,
                    run.ChannelBatches.Count,
                    run.ChannelBatches.SelectMany(x => x.SlideTasks).Count(),
                    alarmCounts.GetValueOrDefault(run.Id),
                    string.Join("/", run.ChannelBatches.Select(x => x.DrawerCode).Distinct().Order()),
                    string.Join("; ", workflows));
            })
            .ToList();
    }

    private IQueryable<MachineRun> ApplyRunFilters(IQueryable<MachineRun> query, TraceFilters filters)
    {
        if (!string.IsNullOrWhiteSpace(filters.MachineRunId)) query = query.Where(x => x.Id == filters.MachineRunId);
        if (!string.IsNullOrWhiteSpace(filters.Status)) query = query.Where(x => x.Status == filters.Status);
        if (!string.IsNullOrWhiteSpace(filters.Channel)) query = query.Where(x => x.ChannelBatches.Any(c => c.DrawerCode == filters.Channel));
        if (!string.IsNullOrWhiteSpace(filters.Slot)) query = query.Where(x => x.ChannelBatches.Any(c => c.SlideTasks.Any(s => s.SlotCode == filters.Slot)));
        if (!string.IsNullOrWhiteSpace(filters.ExperimentType)) query = query.Where(x => x.ChannelBatches.Any(c => c.ExperimentType == filters.ExperimentType || c.SlideTasks.Any(s => s.TaskType == filters.ExperimentType)));
        if (!string.IsNullOrWhiteSpace(filters.PrimaryAntibodyCode)) query = query.Where(x => x.ChannelBatches.Any(c => c.SlideTasks.Any(s => s.StainingTask != null && (s.StainingTask.PrimaryAntibodyCode == filters.PrimaryAntibodyCode || s.StainingTask.ConfirmedPrimaryAntibodyCode == filters.PrimaryAntibodyCode))));
        if (!string.IsNullOrWhiteSpace(filters.SampleCode)) query = query.Where(x => x.ChannelBatches.Any(c => c.SlideTasks.Any(s => s.StainingTask != null && (s.StainingTask.TaskCode.Contains(filters.SampleCode) || (s.StainingTask.RawSampleCode != null && s.StainingTask.RawSampleCode.Contains(filters.SampleCode)) || (s.StainingTask.NormalizedSampleCode != null && s.StainingTask.NormalizedSampleCode.Contains(filters.SampleCode)) || (s.StainingTask.RawCode != null && s.StainingTask.RawCode.Contains(filters.SampleCode))))));
        if (!string.IsNullOrWhiteSpace(filters.Workflow)) query = query.Where(x => x.ChannelBatches.Any(c => c.SelectedWorkflowVersion != null && c.SelectedWorkflowVersion.WorkflowDefinition != null && (c.SelectedWorkflowVersion.WorkflowDefinition.Name.Contains(filters.Workflow) || c.SelectedWorkflowVersion.WorkflowDefinition.Code.Contains(filters.Workflow) || c.SelectedWorkflowVersion.VersionLabel.Contains(filters.Workflow))) || x.WorkflowExecutions.Any(w => w.WorkflowVersion != null && w.WorkflowVersion.WorkflowDefinition != null && (w.WorkflowVersion.WorkflowDefinition.Name.Contains(filters.Workflow) || w.WorkflowVersion.WorkflowDefinition.Code.Contains(filters.Workflow) || w.WorkflowVersion.VersionLabel.Contains(filters.Workflow))));
        if (!string.IsNullOrWhiteSpace(filters.ReagentCode)) query = query.Where(x => dbContext.ReagentConsumptions.Any(c => c.MachineRunId == x.Id && c.ReagentCode == filters.ReagentCode));
        if (!string.IsNullOrWhiteSpace(filters.ReagentBatchNo)) query = query.Where(x => dbContext.ReagentConsumptions.Any(c => c.MachineRunId == x.Id && c.ReagentBottle != null && c.ReagentBottle.ProductionBatchNo == filters.ReagentBatchNo));
        if (!string.IsNullOrWhiteSpace(filters.Operator)) query = query.Where(x => (x.RequestedByUser != null && (x.RequestedByUser.Username.Contains(filters.Operator) || x.RequestedByUser.DisplayName.Contains(filters.Operator))) || x.ChannelBatches.Any(c => c.SlideTasks.Any(s => s.StainingTask != null && s.StainingTask.CreatedByUser != null && (s.StainingTask.CreatedByUser.Username.Contains(filters.Operator) || s.StainingTask.CreatedByUser.DisplayName.Contains(filters.Operator)))));
        return query;
    }

    private IQueryable<HistoryReagentConsumptionResponse> QueryReagentConsumptions(TraceFilters filters)
    {
        var matchingRuns = ApplyRunFilters(dbContext.MachineRuns.AsNoTracking(), filters).Select(x => x.Id);
        var query = dbContext.ReagentConsumptions
            .AsNoTracking()
            .Include(x => x.ReagentBottle)
                .ThenInclude(x => x!.ReagentDefinition)
            .Where(x => matchingRuns.Contains(x.MachineRunId));
        if (!string.IsNullOrWhiteSpace(filters.ReagentCode)) query = query.Where(x => x.ReagentCode == filters.ReagentCode);
        if (!string.IsNullOrWhiteSpace(filters.ReagentBatchNo)) query = query.Where(x => x.ReagentBottle != null && x.ReagentBottle.ProductionBatchNo == filters.ReagentBatchNo);
        return query.Select(x => new HistoryReagentConsumptionResponse(
            x.Id,
            x.MachineRunId,
            x.WorkflowStepExecutionId,
            x.ReagentBottleId,
            x.ReagentCode,
            x.ReagentBottle != null && x.ReagentBottle.ReagentDefinition != null ? x.ReagentBottle.ReagentDefinition.Name : null,
            x.ReagentBottle != null ? x.ReagentBottle.ProductionBatchNo : null,
            x.ReagentBottle != null ? x.ReagentBottle.SerialNo : null,
            x.ReagentBottle != null ? x.ReagentBottle.FullBarcode : null,
            x.VolumeUl,
            x.CreatedAtUtc));
    }

    private IQueryable<Alarm> ApplyAlarmFilters(IQueryable<Alarm> query, TraceFilters filters)
    {
        if (!string.IsNullOrWhiteSpace(filters.MachineRunId)) query = query.Where(x => x.MachineRunId == filters.MachineRunId);
        if (!string.IsNullOrWhiteSpace(filters.AlarmStatus)) query = query.Where(x => x.Status == filters.AlarmStatus);
        if (!string.IsNullOrWhiteSpace(filters.Severity)) query = query.Where(x => x.Severity == filters.Severity);
        if (!string.IsNullOrWhiteSpace(filters.AlarmCode)) query = query.Where(x => x.Code.Contains(filters.AlarmCode));
        if (!string.IsNullOrWhiteSpace(filters.Channel)) query = query.Where(x => x.MachineRun != null && x.MachineRun.ChannelBatches.Any(c => c.DrawerCode == filters.Channel));
        if (!string.IsNullOrWhiteSpace(filters.Operator)) query = query.Where(x => x.Actions.Any(a => a.ActorUser != null && (a.ActorUser.Username.Contains(filters.Operator) || a.ActorUser.DisplayName.Contains(filters.Operator))));
        return query;
    }

    private async Task<IReadOnlyList<TraceAlarmResponse>> ListAlarmItemsAsync(TraceFilters filters, CancellationToken cancellationToken)
    {
        var ids = await ApplyAlarmFilters(dbContext.Alarms.AsNoTracking(), filters)
            .Select(x => new TraceCandidate(x.Id, x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
        return await LoadAlarmItemsAsync(PageCandidates(ids, filters, descending: true).Ids, cancellationToken);
    }

    private async Task<IReadOnlyList<TraceAlarmResponse>> LoadAlarmItemsAsync(IReadOnlyList<string> ids, CancellationToken cancellationToken)
    {
        if (ids.Count == 0)
        {
            return [];
        }

        var alarms = await dbContext.Alarms
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.MachineRun)
                .ThenInclude(x => x!.ChannelBatches)
            .Include(x => x.Actions)
                .ThenInclude(x => x.ActorUser)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);
        var order = ids.Select((id, index) => new { id, index }).ToDictionary(x => x.id, x => x.index, StringComparer.Ordinal);
        return alarms.OrderBy(x => order[x.Id]).Select(ToTraceAlarm).ToList();
    }

    private IQueryable<AuditLog> ApplyAuditFilters(IQueryable<AuditLog> query, TraceFilters filters)
    {
        if (!string.IsNullOrWhiteSpace(filters.User)) query = query.Where(x => x.ActorUser != null && (x.ActorUser.Username.Contains(filters.User) || x.ActorUser.DisplayName.Contains(filters.User)));
        if (!string.IsNullOrWhiteSpace(filters.Action)) query = query.Where(x => x.Action.Contains(filters.Action));
        if (!string.IsNullOrWhiteSpace(filters.EntityType)) query = query.Where(x => x.EntityType.Contains(filters.EntityType));
        if (!string.IsNullOrWhiteSpace(filters.MachineRunId)) query = query.Where(x => x.EntityId == filters.MachineRunId || x.Message.Contains(filters.MachineRunId));
        if (!string.IsNullOrWhiteSpace(filters.TaskId)) query = query.Where(x => x.EntityId == filters.TaskId || x.Message.Contains(filters.TaskId));
        if (!string.IsNullOrWhiteSpace(filters.CommandId)) query = query.Where(x => x.Message.Contains(filters.CommandId));
        if (!string.IsNullOrWhiteSpace(filters.CorrelationId)) query = query.Where(x => x.Message.Contains(filters.CorrelationId));
        if (!string.IsNullOrWhiteSpace(filters.Channel)) query = query.Where(x => x.Message.Contains(filters.Channel) || x.Message.Contains($"\"drawerCode\":\"{filters.Channel}\""));
        if (!string.IsNullOrWhiteSpace(filters.Slot)) query = query.Where(x => x.Message.Contains(filters.Slot));
        return query;
    }

    private async Task<IReadOnlyList<AuditLogResponse>> LoadAuditLogItemsAsync(IReadOnlyList<string> ids, CancellationToken cancellationToken)
    {
        if (ids.Count == 0)
        {
            return [];
        }

        var logs = await dbContext.AuditLogs
            .AsNoTracking()
            .Include(x => x.ActorUser)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);
        var order = ids.Select((id, index) => new { id, index }).ToDictionary(x => x.id, x => x.index, StringComparer.Ordinal);
        return logs.OrderBy(x => order[x.Id]).Select(ToAuditLogResponse).ToList();
    }

    private static TracePage PageCandidates(IReadOnlyList<TraceCandidate> candidates, TraceFilters filters, bool descending)
    {
        var filtered = candidates
            .Where(x => InRange(x.CreatedAtUtc, filters));
        filtered = descending
            ? filtered.OrderByDescending(x => x.CreatedAtUtc)
            : filtered.OrderBy(x => x.CreatedAtUtc);
        var list = filtered.ToList();
        var ids = list
            .Skip((filters.Page - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .Select(x => x.Id)
            .ToList();
        return new TracePage(list.Count, ids);
    }

    private static bool InRange(DateTimeOffset createdAtUtc, TraceFilters filters)
    {
        return (filters.FromUtc is null || createdAtUtc >= filters.FromUtc)
            && (filters.ToUtc is null || createdAtUtc <= filters.ToUtc);
    }

    private static HistoryChannelBatchResponse ToHistoryChannelBatch(ChannelBatch batch)
    {
        var version = batch.SelectedWorkflowVersion;
        return new HistoryChannelBatchResponse(
            batch.Id,
            batch.DrawerCode,
            batch.Status,
            batch.ExperimentType,
            batch.SelectedWorkflowVersionId,
            version?.WorkflowDefinition?.Code,
            version?.WorkflowDefinition?.Name,
            version?.VersionLabel,
            batch.CoordinateProfileVersionId,
            batch.CoordinateSelectionStatus,
            batch.CoordinateSnapshotJson,
            batch.LiquidClassSelectionStatus,
            batch.LiquidClassSnapshotJson,
            batch.WorkflowSelectionStatus,
            batch.CreatedAtUtc,
            batch.StartedAtUtc,
            batch.CompletedAtUtc,
            batch.WorkflowSnapshotJson,
            batch.SlideTasks.OrderBy(x => x.SlotCode).Select(ToHistorySlideTask).ToList());
    }

    private static HistorySlideTaskResponse ToHistorySlideTask(SlideTask slide)
    {
        var task = slide.StainingTask;
        return new HistorySlideTaskResponse(
            slide.Id,
            slide.StainingTaskId,
            task?.TaskCode ?? slide.StainingTaskId,
            slide.SlotCode,
            slide.TaskType,
            slide.Status,
            task?.RawSampleCode ?? task?.RawCode,
            task?.NormalizedSampleCode ?? task?.NormalizedCode,
            task?.ConfirmedPrimaryAntibodyCode ?? task?.PrimaryAntibodyCode,
            task?.CreatedByUser?.DisplayName ?? task?.CreatedByUser?.Username,
            slide.CreatedAtUtc,
            task?.WorkflowSnapshotJson ?? "{}");
    }

    private static HistoryWorkflowExecutionResponse ToHistoryWorkflowExecution(WorkflowExecution execution)
    {
        return new HistoryWorkflowExecutionResponse(
            execution.Id,
            execution.SlideTaskId,
            execution.WorkflowVersionId,
            execution.WorkflowVersion?.WorkflowDefinition?.Code,
            execution.WorkflowVersion?.WorkflowDefinition?.Name,
            execution.WorkflowVersion?.VersionLabel,
            execution.Status,
            execution.CreatedAtUtc,
            execution.StartedAtUtc,
            execution.CompletedAtUtc,
            execution.StepExecutions.OrderBy(x => x.StepNo).Select(x => new HistoryWorkflowStepExecutionResponse(
                x.Id,
                x.StepNo,
                x.MajorStepCode,
                x.StepName,
                x.ActionType,
                x.ReagentCode,
                x.VolumeUl,
                x.TargetTemperatureDeciC,
                x.Status,
                x.RedoCount,
                x.CreatedAtUtc,
                x.StartedAtUtc,
                x.CompletedAtUtc)).ToList());
    }

    private static TraceAlarmResponse ToTraceAlarm(Alarm alarm)
    {
        var ack = alarm.Actions
            .Where(x => x.Action == "Acknowledged")
            .OrderByDescending(x => x.CreatedAtUtc)
            .FirstOrDefault();
        return new TraceAlarmResponse(
            alarm.Id,
            alarm.MachineRunId,
            alarm.Code,
            alarm.Severity,
            alarm.Status,
            alarm.Message,
            alarm.CreatedAtUtc,
            alarm.ClearedAtUtc,
            alarm.MachineRun is null ? null : string.Join("/", alarm.MachineRun.ChannelBatches.Select(x => x.DrawerCode).Distinct().Order()),
            ack?.ActorUser?.DisplayName ?? ack?.ActorUser?.Username,
            ack?.CreatedAtUtc,
            ack?.Message,
            alarm.Actions.OrderBy(x => x.CreatedAtUtc).Select(x => new TraceAlarmActionResponse(
                x.Id,
                x.Action,
                x.Message,
                x.ActorUser?.DisplayName ?? x.ActorUser?.Username,
                x.CreatedAtUtc)).ToList());
    }

    private static AuditLogResponse ToAuditLogResponse(AuditLog log)
    {
        var message = ScrubSensitive(log.Message);
        return new AuditLogResponse(
            log.Id,
            log.ActorUserId,
            log.ActorUser?.DisplayName ?? log.ActorUser?.Username,
            log.Action,
            log.EntityType,
            log.EntityId,
            message,
            BuildSummary(message),
            ExtractJsonValue(message, "reason"),
            ExtractJsonValue(message, "commandId"),
            ExtractJsonValue(message, "correlationId"),
            log.EntityType == "MachineRun" ? log.EntityId : ExtractJsonValue(message, "machineRunId") ?? ExtractJsonValue(message, "runId"),
            log.EntityType is "StainingTask" or "SlideTask" ? log.EntityId : ExtractJsonValue(message, "taskId") ?? ExtractJsonValue(message, "slideTaskId"),
            ExtractJsonValue(message, "drawerCode") ?? ExtractJsonValue(message, "channel") ?? ExtractJsonValue(message, "drawer"),
            ExtractJsonValue(message, "slotCode") ?? ExtractJsonValue(message, "slot"),
            log.CreatedAtUtc);
    }

    private async Task AddExportAuditAsync(AuthenticatedUser actor, string exportType, int rowCount, TraceFilters filters, CancellationToken cancellationToken)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            ActorUserId = actor.UserId,
            Action = "export.csv",
            EntityType = "TraceabilityExport",
            EntityId = exportType,
            Message = JsonSerializer.Serialize(new
            {
                exportType,
                rowCount,
                filters = filters.ToAuditObject()
            }, JsonOptions),
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static CsvExportResult CsvExport(string exportType, string csv, int rowCount)
    {
        return new CsvExportResult(
            $"{exportType}-{DateTimeOffset.UtcNow:yyyyMMdd-HHmmss}Z.csv",
            "text/csv; charset=utf-8",
            Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv)).ToArray(),
            rowCount);
    }

    private static string BuildCsv(IReadOnlyList<string> headers, IEnumerable<IReadOnlyList<string>> rows)
    {
        var builder = new StringBuilder();
        builder.AppendLine(string.Join(",", headers.Select(EscapeCsv)));
        foreach (var row in rows)
        {
            builder.AppendLine(string.Join(",", row.Select(x => EscapeCsv(ScrubSensitive(x)))));
        }

        return builder.ToString();
    }

    private static string EscapeCsv(string? value)
    {
        var text = value ?? string.Empty;
        return text.Contains('"') || text.Contains(',') || text.Contains('\r') || text.Contains('\n')
            ? "\"" + text.Replace("\"", "\"\"") + "\""
            : text;
    }

    private static string Format(DateTimeOffset? value)
    {
        return value?.UtcDateTime.ToString("O") ?? string.Empty;
    }

    private static string ScrubSensitive(string? value)
    {
        var text = value ?? string.Empty;
        if (text.Contains("password", StringComparison.OrdinalIgnoreCase)
            || text.Contains("token", StringComparison.OrdinalIgnoreCase)
            || text.Contains("connectionString", StringComparison.OrdinalIgnoreCase)
            || text.Contains("connection_string", StringComparison.OrdinalIgnoreCase))
        {
            return "[redacted sensitive details]";
        }

        return text;
    }

    private static string BuildSummary(string message)
    {
        var summary = message.Length > 280 ? message[..280] + "..." : message;
        return string.IsNullOrWhiteSpace(summary) ? "--" : summary;
    }

    private static string? ExtractJsonValue(string? json, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(json) || !json.TrimStart().StartsWith('{'))
        {
            return null;
        }

        try
        {
            using var document = JsonDocument.Parse(json);
            return FindJsonValue(document.RootElement, propertyName);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static string? FindJsonValue(JsonElement element, string propertyName)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals(propertyName))
                {
                    return property.Value.ValueKind switch
                    {
                        JsonValueKind.String => property.Value.GetString(),
                        JsonValueKind.Number => property.Value.ToString(),
                        JsonValueKind.True => "true",
                        JsonValueKind.False => "false",
                        _ => property.Value.GetRawText()
                    };
                }

                var nested = FindJsonValue(property.Value, propertyName);
                if (!string.IsNullOrWhiteSpace(nested))
                {
                    return nested;
                }
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
            {
                var nested = FindJsonValue(item, propertyName);
                if (!string.IsNullOrWhiteSpace(nested))
                {
                    return nested;
                }
            }
        }

        return null;
    }

    private static bool IsHighSeverity(string severity)
    {
        return severity is "Error" or "Critical";
    }

    private sealed record TraceCandidate(string Id, DateTimeOffset CreatedAtUtc);

    private sealed record TracePage(int TotalCount, IReadOnlyList<string> Ids);

    private sealed record TraceFilters(
        DateTimeOffset? FromUtc,
        DateTimeOffset? ToUtc,
        string? Status,
        string? AlarmStatus,
        string? Severity,
        string? AlarmCode,
        string? Channel,
        string? Slot,
        string? ExperimentType,
        string? Workflow,
        string? SampleCode,
        string? PrimaryAntibodyCode,
        string? ReagentCode,
        string? ReagentBatchNo,
        string? Operator,
        string? User,
        string? Action,
        string? EntityType,
        string? MachineRunId,
        string? TaskId,
        string? CommandId,
        string? CorrelationId,
        int Page,
        int PageSize)
    {
        public static TraceFilters From(IQueryCollection query)
        {
            var page = Math.Max(1, ParseInt(query, "page") ?? 1);
            var pageSize = Math.Clamp(ParseInt(query, "pageSize") ?? 50, 1, MaxPageSize);
            return new TraceFilters(
                ParseDate(query, "fromUtc") ?? ParseDate(query, "from"),
                ParseDate(query, "toUtc") ?? ParseDate(query, "to"),
                Text(query, "status"),
                Text(query, "alarmStatus") ?? Text(query, "status"),
                Text(query, "severity"),
                Text(query, "alarmCode") ?? Text(query, "code"),
                NormalizeUpper(Text(query, "channel")),
                NormalizeUpper(Text(query, "slot")),
                NormalizeUpper(Text(query, "experimentType")),
                Text(query, "workflow"),
                Text(query, "sampleCode"),
                NormalizeUpper(Text(query, "primaryAntibodyCode")),
                NormalizeUpper(Text(query, "reagentCode")),
                Text(query, "reagentBatchNo") ?? Text(query, "batchNo"),
                Text(query, "operator"),
                Text(query, "user"),
                Text(query, "action"),
                Text(query, "entityType"),
                Text(query, "machineRunId"),
                Text(query, "taskId"),
                Text(query, "commandId"),
                Text(query, "correlationId"),
                page,
                pageSize);
        }

        public static TraceFilters ForRun(string machineRunId)
        {
            return From(new QueryCollection(new Dictionary<string, StringValues>
            {
                ["machineRunId"] = machineRunId,
                ["pageSize"] = MaxExportRows.ToString()
            }));
        }

        public object ToAuditObject()
        {
            return new
            {
                FromUtc,
                ToUtc,
                Status,
                AlarmStatus,
                Severity,
                AlarmCode,
                Channel,
                Slot,
                ExperimentType,
                Workflow,
                SampleCode,
                PrimaryAntibodyCode,
                ReagentCode,
                ReagentBatchNo,
                Operator,
                User,
                Action,
                EntityType,
                MachineRunId,
                TaskId,
                CommandId,
                CorrelationId
            };
        }

        private static string? Text(IQueryCollection query, string key)
        {
            return query.TryGetValue(key, out var value) && !StringValues.IsNullOrEmpty(value)
                ? value.ToString().Trim()
                : null;
        }

        private static string? NormalizeUpper(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant();
        }

        private static DateTimeOffset? ParseDate(IQueryCollection query, string key)
        {
            var value = Text(query, key);
            return DateTimeOffset.TryParse(value, out var parsed) ? parsed : null;
        }

        private static int? ParseInt(IQueryCollection query, string key)
        {
            var value = Text(query, key);
            return int.TryParse(value, out var parsed) ? parsed : null;
        }
    }
}

public sealed record CsvExportResult(
    string FileName,
    string ContentType,
    byte[] Content,
    int RowCount);
