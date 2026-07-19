using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Tests;

public sealed class TraceabilityApiIntegrationTests
{
    [Fact]
    public async Task History_queries_filter_by_channel_slot_time_and_reagent_batch()
    {
        await using var factory = CreateFactory();
        var seeded = await SeedTraceabilityGraphAsync(factory);
        using var client = factory.CreateClient();
        await LoginAsync(client, "operator", "operator");

        var fromUtc = Uri.EscapeDataString(seeded.CreatedAtUtc.AddMinutes(-5).ToString("O"));
        var toUtc = Uri.EscapeDataString(seeded.CreatedAtUtc.AddMinutes(5).ToString("O"));
        var runs = await client.GetFromJsonAsync<TraceabilityListResponse<HistoryRunSummaryResponse>>(
            $"/api/history/runs?channel=A&slot=A-01&fromUtc={fromUtc}&toUtc={toUtc}&reagentBatchNo=LOT-TRACE-1");

        Assert.NotNull(runs);
        var run = Assert.Single(runs!.Items);
        Assert.Equal(seeded.MachineRunId, run.MachineRunId);
        Assert.Equal("A", run.Channels);
        Assert.Contains("Trace Workflow", run.WorkflowNames);
        Assert.Equal(1, run.SlideTaskCount);

        var consumptions = await client.GetFromJsonAsync<TraceabilityListResponse<HistoryReagentConsumptionResponse>>(
            "/api/history/reagent-consumptions?reagentBatchNo=LOT-TRACE-1&reagentCode=TRC");
        Assert.NotNull(consumptions);
        var consumption = Assert.Single(consumptions!.Items);
        Assert.Equal(seeded.MachineRunId, consumption.MachineRunId);
        Assert.Equal("LOT-TRACE-1", consumption.ProductionBatchNo);
        Assert.Equal("TRC", consumption.ReagentCode);

        var noMatch = await client.GetFromJsonAsync<TraceabilityListResponse<HistoryRunSummaryResponse>>(
            "/api/history/runs?channel=B&slot=A-01&reagentBatchNo=LOT-TRACE-1");
        Assert.NotNull(noMatch);
        Assert.Empty(noMatch!.Items);

        var operatorDetail = await client.GetFromJsonAsync<JsonElement>($"/api/history/runs/{seeded.MachineRunId}");
        Assert.Equal(seeded.MachineRunId, operatorDetail.GetProperty("machineRunId").GetString());
        Assert.Single(operatorDetail.GetProperty("channels").EnumerateArray());
        Assert.False(operatorDetail.TryGetProperty("deviceCommands", out _));
        Assert.False(operatorDetail.TryGetProperty("coordinateSnapshotJson", out _));
        Assert.False(operatorDetail.TryGetProperty("liquidClassSnapshotJson", out _));

        using var engineerClient = factory.CreateClient();
        await LoginAsync(engineerClient, "admin", "admin");
        var engineeringDetail = await engineerClient.GetFromJsonAsync<HistoryRunDetailResponse>($"/api/history/runs/{seeded.MachineRunId}");
        Assert.NotNull(engineeringDetail);
        Assert.Single(engineeringDetail!.ChannelBatches);
        Assert.Single(engineeringDetail.WorkflowExecutions);
        Assert.Single(engineeringDetail.DeviceCommands);
        Assert.Single(engineeringDetail.ReagentConsumptions);
        Assert.Single(engineeringDetail.DabUsages);
        Assert.Equal(2, engineeringDetail.Alarms.Count);
    }

    [Fact]
    public async Task Operator_alarm_surfaces_hide_technical_details_while_engineering_diagnostics_keep_them()
    {
        await using var factory = CreateFactory();
        var seeded = await SeedTraceabilityGraphAsync(factory);

        using var operatorClient = factory.CreateClient();
        await LoginAsync(operatorClient, "operator", "operator");

        var list = await operatorClient.GetFromJsonAsync<TraceabilityListResponse<TraceAlarmResponse>>(
            "/api/alarms?alarmCode=database_backup_degraded");
        var alarm = Assert.Single(list!.Items);
        Assert.Equal("数据库维护", alarm.Code);
        Assert.Equal("数据库备份已完成，临时文件清理待工程维护。", alarm.Message);
        AssertOperatorSafe(JsonSerializer.Serialize(alarm));

        var detail = await operatorClient.GetFromJsonAsync<JsonElement>($"/api/history/runs/{seeded.MachineRunId}");
        AssertOperatorSafe(detail.GetRawText());
        var detailAlarm = Assert.Single(detail.GetProperty("alarms").EnumerateArray(),
            x => x.GetProperty("alarmId").GetString() == seeded.CriticalAlarmId);
        Assert.Equal("数据库维护", detailAlarm.GetProperty("code").GetString());

        var snapshot = await operatorClient.GetFromJsonAsync<JsonElement>("/api/operator/snapshot");
        Assert.Contains(snapshot.GetProperty("alarms").EnumerateArray(),
            x => x.GetString() == "数据库备份已完成，临时文件清理待工程维护。");
        AssertOperatorSafe(snapshot.GetProperty("alarms").GetRawText());
        AssertOperatorSafe(snapshot.GetProperty("alarmDetails").GetRawText());
        AssertOperatorSafe(snapshot.GetProperty("recentEvents").GetRawText());

        var operatorRun = await operatorClient.GetFromJsonAsync<JsonElement>($"/api/runs/{seeded.MachineRunId}");
        AssertOperatorSafe(operatorRun.GetRawText());
        Assert.Equal(string.Empty, operatorRun.GetProperty("coordinateSnapshotJson").GetString());
        Assert.Equal(string.Empty, operatorRun.GetProperty("liquidClassSnapshotJson").GetString());

        var csvResponse = await operatorClient.GetAsync("/api/alarms/export?alarmCode=database_backup_degraded");
        Assert.Equal(HttpStatusCode.OK, csvResponse.StatusCode);
        var csv = await csvResponse.Content.ReadAsStringAsync();
        Assert.Contains("Category", csv.Split('\n')[0]);
        Assert.DoesNotContain(",Code,", csv.Split('\n')[0]);
        Assert.Contains("数据库备份已完成，临时文件清理待工程维护。", csv);
        AssertOperatorSafe(csv);

        using var engineerClient = factory.CreateClient();
        await LoginAsync(engineerClient, "admin", "admin");
        var engineeringRun = await engineerClient.GetFromJsonAsync<JsonElement>($"/api/runs/{seeded.MachineRunId}");
        AssertOperatorSafe(engineeringRun.GetProperty("alarms").GetRawText());
        var diagnostics = await engineerClient.GetFromJsonAsync<TraceabilityListResponse<EngineeringErrorCodeResponse>>(
            "/api/engineering/diagnostics/errors?code=database_backup_degraded");
        var engineeringAlarm = Assert.Single(diagnostics!.Items);
        Assert.Equal("database_backup_degraded", engineeringAlarm.Code);
        Assert.Contains("AttemptDirectory", engineeringAlarm.Message);
        Assert.Contains("SQLite", engineeringAlarm.Message);
        Assert.Contains("StateHash", engineeringAlarm.Message);
    }

    [Fact]
    public async Task Alarm_acknowledge_updates_database_actions_audit_and_enforces_permissions()
    {
        await using var factory = CreateFactory();
        var seeded = await SeedTraceabilityGraphAsync(factory);

        using var operatorClient = factory.CreateClient();
        await LoginAsync(operatorClient, "operator", "operator");
        var operatorAck = await operatorClient.PostAsJsonAsync($"/api/alarms/{seeded.CriticalAlarmId}/acknowledge", new
        {
            commandId = "cmd-trace-operator-critical-ack",
            reason = "operator should be forbidden"
        });
        Assert.Equal(HttpStatusCode.Forbidden, operatorAck.StatusCode);
        Assert.Equal("alarm_ack_forbidden", (await operatorAck.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("code").GetString());

        using var adminClient = factory.CreateClient();
        await LoginAsync(adminClient, "admin", "admin");
        var missingReason = await adminClient.PostAsJsonAsync($"/api/alarms/{seeded.CriticalAlarmId}/acknowledge", new
        {
            commandId = "cmd-trace-critical-missing-reason"
        });
        Assert.Equal(HttpStatusCode.BadRequest, missingReason.StatusCode);
        Assert.Equal("alarm_ack_reason_required", (await missingReason.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("code").GetString());

        var ack = await PostJsonAsync<AlarmMutationResponse>(adminClient, $"/api/alarms/{seeded.CriticalAlarmId}/acknowledge", new
        {
            commandId = "cmd-trace-critical-ack",
            reason = "Verified root cause during traceability test."
        });
        Assert.Equal("Acknowledged", ack.Status);
        Assert.False(ack.Replayed);

        var replay = await PostJsonAsync<AlarmMutationResponse>(adminClient, $"/api/alarms/{seeded.CriticalAlarmId}/acknowledge", new
        {
            commandId = "cmd-trace-critical-ack",
            reason = "Verified root cause during traceability test."
        });
        Assert.True(replay.Replayed);
        Assert.Equal(seeded.CriticalAlarmId, replay.AlarmId);

        await using var scope = factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
        var alarm = await dbContext.Alarms.SingleAsync(x => x.Id == seeded.CriticalAlarmId);
        Assert.Equal("Acknowledged", alarm.Status);
        Assert.True(await dbContext.AlarmActions.AnyAsync(x =>
            x.AlarmId == seeded.CriticalAlarmId
            && x.Action == "Acknowledged"
            && x.Message.Contains("Verified root cause")));
        Assert.True(await dbContext.AuditLogs.AnyAsync(x => x.Action == "alarm.acknowledge" && x.EntityId == seeded.CriticalAlarmId));
    }

    [Fact]
    public async Task Audit_queries_link_objects_and_csv_exports_are_filtered_and_scrub_sensitive_values()
    {
        await using var factory = CreateFactory();
        var seeded = await SeedTraceabilityGraphAsync(factory);
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin", "admin");

        var audit = await client.GetFromJsonAsync<TraceabilityListResponse<AuditLogResponse>>(
            $"/api/audit/logs?machineRunId={seeded.MachineRunId}&commandId=cmd-script-select&correlationId=corr-script-001");
        Assert.NotNull(audit);
        var log = Assert.Single(audit!.Items);
        Assert.Equal("channel.workflow.select", log.Action);
        Assert.Equal(seeded.MachineRunId, log.MachineRunId);
        Assert.Equal(seeded.StainingTaskId, log.TaskId);
        Assert.Equal("A", log.Channel);
        Assert.Equal("A-01", log.Slot);
        Assert.Equal("corr-script-001", log.CorrelationId);

        var reagentExport = await client.GetAsync("/api/history/export/reagent-consumptions?reagentBatchNo=LOT-TRACE-1");
        Assert.Equal(HttpStatusCode.OK, reagentExport.StatusCode);
        var reagentCsv = await reagentExport.Content.ReadAsStringAsync();
        Assert.Contains("LOT-TRACE-1", reagentCsv);
        Assert.Contains("TRC", reagentCsv);

        var auditExport = await client.GetAsync("/api/audit/export?commandId=cmd-sensitive");
        Assert.Equal(HttpStatusCode.OK, auditExport.StatusCode);
        var auditCsv = await auditExport.Content.ReadAsStringAsync();
        Assert.Contains("[redacted sensitive details]", auditCsv);
        Assert.DoesNotContain("SuperSecret123!", auditCsv);
        Assert.DoesNotContain("Bearer abc", auditCsv);

        await using var scope = factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
        Assert.True(await dbContext.AuditLogs.AnyAsync(x => x.Action == "export.csv" && x.EntityType == "TraceabilityExport"));
    }

    [Fact]
    public async Task Formal_history_data_survives_page_refresh_and_reads_from_database()
    {
        await using var factory = CreateFactory();
        var seeded = await SeedTraceabilityGraphAsync(factory);
        using var client = factory.CreateClient();
        await LoginAsync(client, "operator", "operator");

        // 旧 /history 页面已删除；本用例聚焦后端历史数据在重复读取间保持一致（来自正式数据库，非前端缓存）。

        var first = await client.GetFromJsonAsync<TraceabilityListResponse<HistoryRunSummaryResponse>>(
            $"/api/history/runs?machineRunId={seeded.MachineRunId}");
        var second = await client.GetFromJsonAsync<TraceabilityListResponse<HistoryRunSummaryResponse>>(
            $"/api/history/runs?machineRunId={seeded.MachineRunId}");

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.Equal(seeded.MachineRunId, Assert.Single(first!.Items).MachineRunId);
        Assert.Equal(seeded.MachineRunId, Assert.Single(second!.Items).MachineRunId);
    }

    private static WebApplicationFactory<Program> CreateFactory()
    {
        var databasePath = Path.Combine(TestPaths.TempRoot, "stainer-traceability-tests", Guid.NewGuid().ToString("N"), "stainer.db");
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.UseSetting("ConnectionStrings:StainerDatabase", $"Data Source={databasePath}");
                builder.ConfigureAppConfiguration((_, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:StainerDatabase"] = $"Data Source={databasePath}"
                    });
                });
            });
    }

    private static async Task LoginAsync(HttpClient client, string username, string role)
    {
        var response = await client.PostAsJsonAsync("/api/login", new
        {
            username,
            password = "123456",
            role
        });
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

    private static async Task<SeededTraceabilityGraph> SeedTraceabilityGraphAsync(WebApplicationFactory<Program> factory)
    {
        await using var scope = factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
        var operatorUser = await dbContext.Users.SingleAsync(x => x.Username == "operator");
        var adminUser = await dbContext.Users.SingleAsync(x => x.Username == "admin");
        var drawer = await dbContext.Drawers.SingleAsync(x => x.Code == "A");
        var slot = await dbContext.PhysicalSlots.SingleAsync(x => x.Code == "A-01");
        var dabPosition = await dbContext.DabMixPositions.FirstAsync();
        var createdAtUtc = DateTimeOffset.UtcNow.AddMinutes(-10);

        var reagentDefinition = new ReagentDefinition
        {
            ReagentCode = "TRC",
            Name = "Trace Reagent",
            ReagentType = "trace",
            CreatedAtUtc = createdAtUtc
        };
        var reagentBottle = new ReagentBottle
        {
            ReagentDefinition = reagentDefinition,
            FullBarcode = "TRC05020280101001",
            ReagentCode = "TRC",
            ProductionBatchNo = "LOT-TRACE-1",
            SerialNo = "S-TRACE-1",
            InitialVolumeUl = 5000,
            RemainingVolumeUl = 4800,
            ExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
            Status = "Valid",
            CreatedAtUtc = createdAtUtc
        };

        var workflowDefinition = new WorkflowDefinition
        {
            Code = "TRACE-IHC",
            Name = "Trace Workflow",
            WorkflowType = StainingTaskType.Ihc,
            Description = "Traceability integration workflow",
            CreatedAtUtc = createdAtUtc
        };
        var workflowVersion = new WorkflowVersion
        {
            WorkflowDefinition = workflowDefinition,
            VersionNo = 1,
            VersionLabel = "1.0",
            Status = WorkflowVersionStatus.Published,
            ChangeNote = "Traceability test publish.",
            PublishedAtUtc = createdAtUtc,
            CreatedAtUtc = createdAtUtc
        };
        workflowVersion.Steps.Add(new WorkflowStep
        {
            StepNo = 1,
            MajorStepCode = "TRACE_STEP",
            StepName = "Trace step",
            ActionType = "Dispense",
            ReagentCode = "TRC",
            VolumeUl = 100,
            DurationSeconds = 3,
            FailureStrategy = "Stop",
            CreatedAtUtc = createdAtUtc
        });
        workflowVersion.ReagentRequirements.Add(new WorkflowReagentRequirement
        {
            ReagentCode = "TRC",
            RequiredVolumeUl = 100,
            IsRequired = true,
            CreatedAtUtc = createdAtUtc
        });

        var stainingTask = new StainingTask
        {
            TaskCode = "TRACE-TASK-001",
            TaskType = StainingTaskType.Ihc,
            Status = StainingTaskStatus.Confirmed,
            PhysicalSlot = slot,
            WorkflowDefinition = workflowDefinition,
            WorkflowVersion = workflowVersion,
            WorkflowSnapshotJson = "{\"workflowCode\":\"TRACE-IHC\",\"workflowVersionId\":\"trace\"}",
            InputMode = "DirectPrimaryAntibody",
            RawSampleCode = "SAMPLE-TRACE-001",
            NormalizedSampleCode = "SAMPLE-TRACE-001",
            PrimaryAntibodyCode = "001",
            ConfirmedPrimaryAntibodyCode = "001",
            CompatibilityValidationStatus = "Compatible",
            CreatedByUser = operatorUser,
            CreatedAtUtc = createdAtUtc
        };
        var machineRun = new MachineRun
        {
            RunCode = "RUN-TRACE-001",
            Status = RuntimeLedgerStatus.Completed,
            RequestedByUser = operatorUser,
            CreatedAtUtc = createdAtUtc,
            StartedAtUtc = createdAtUtc.AddSeconds(5),
            CompletedAtUtc = createdAtUtc.AddMinutes(2)
        };
        var channelBatch = new ChannelBatch
        {
            MachineRun = machineRun,
            Drawer = drawer,
            DrawerCode = "A",
            Status = RuntimeLedgerStatus.Completed,
            ExperimentType = StainingTaskType.Ihc,
            SelectedWorkflowVersion = workflowVersion,
            WorkflowSnapshotJson = "{\"workflowCode\":\"TRACE-IHC\",\"workflowVersionId\":\"trace\"}",
            WorkflowSelectionStatus = WorkflowSelectionStatus.Locked,
            WorkflowSelectedAtUtc = createdAtUtc.AddSeconds(-5),
            WorkflowSelectedByUser = operatorUser,
            WorkflowLockedAtUtc = createdAtUtc.AddSeconds(5),
            CreatedAtUtc = createdAtUtc,
            StartedAtUtc = createdAtUtc.AddSeconds(5),
            CompletedAtUtc = createdAtUtc.AddMinutes(2)
        };
        var slideTask = new SlideTask
        {
            ChannelBatch = channelBatch,
            StainingTask = stainingTask,
            PhysicalSlot = slot,
            SlotCode = "A-01",
            TaskType = StainingTaskType.Ihc,
            Status = RuntimeLedgerStatus.Completed,
            CreatedAtUtc = createdAtUtc
        };
        var workflowExecution = new WorkflowExecution
        {
            MachineRun = machineRun,
            SlideTask = slideTask,
            WorkflowVersion = workflowVersion,
            Status = RuntimeLedgerStatus.Completed,
            CreatedAtUtc = createdAtUtc,
            StartedAtUtc = createdAtUtc.AddSeconds(5),
            CompletedAtUtc = createdAtUtc.AddMinutes(1)
        };
        var stepExecution = new WorkflowStepExecution
        {
            WorkflowExecution = workflowExecution,
            StepNo = 1,
            MajorStepCode = "TRACE_STEP",
            StepName = "Trace step",
            ActionType = "Dispense",
            ReagentCode = "TRC",
            VolumeUl = 100,
            Status = RuntimeLedgerStatus.Completed,
            RedoCount = 1,
            CreatedAtUtc = createdAtUtc,
            StartedAtUtc = createdAtUtc.AddSeconds(5),
            CompletedAtUtc = createdAtUtc.AddSeconds(10)
        };
        var deviceCommand = new DeviceCommandExecution
        {
            MachineRun = machineRun,
            WorkflowStepExecution = stepExecution,
            CommandType = "Dispense",
            Status = DeviceCommandStatus.Completed,
            CreatedAtUtc = createdAtUtc,
            CommandSentAtUtc = createdAtUtc.AddSeconds(6),
            AcknowledgedAtUtc = createdAtUtc.AddSeconds(7),
            CompletedAtUtc = createdAtUtc.AddSeconds(9)
        };
        var reagentConsumption = new ReagentConsumption
        {
            MachineRun = machineRun,
            WorkflowStepExecution = stepExecution,
            ReagentBottle = reagentBottle,
            ReagentCode = "TRC",
            VolumeUl = 100,
            CreatedAtUtc = createdAtUtc.AddSeconds(9)
        };
        var dabBatch = new DabBatch
        {
            DabMixPosition = dabPosition,
            PositionCode = dabPosition.Code,
            Status = RuntimeLedgerStatus.Available,
            RemainingVolumeUl = 1000,
            PreparedAtUtc = createdAtUtc,
            ExpiresAtUtc = createdAtUtc.AddHours(1),
            CreatedAtUtc = createdAtUtc
        };
        var dabUsage = new DabBatchUsage
        {
            DabBatch = dabBatch,
            MachineRun = machineRun,
            WorkflowStepExecution = stepExecution,
            VolumeUl = 50,
            CreatedAtUtc = createdAtUtc.AddSeconds(12)
        };
        var criticalAlarm = new Alarm
        {
            MachineRun = machineRun,
            Code = "database_backup_degraded",
            Severity = "Critical",
            Message = "Database backup completed. AttemptDirectory=%TEMP%\\stainer-backup-attempts\\attempt-1; FinalBackup=C:\\data\\stainer-backup.db; SQLite error: database is locked; StateHash=ABC123; RequestJson={raw-packet}.",
            Status = "Active",
            CreatedAtUtc = createdAtUtc.AddSeconds(30)
        };
        var warningAlarm = new Alarm
        {
            MachineRun = machineRun,
            Code = "TRACE_WARNING",
            Severity = "Warning",
            Message = "Trace warning alarm.",
            Status = "Resolved",
            CreatedAtUtc = createdAtUtc.AddSeconds(20),
            ClearedAtUtc = createdAtUtc.AddSeconds(40)
        };
        warningAlarm.Actions.Add(new AlarmAction
        {
            ActorUser = operatorUser,
            Action = "Resolved",
            Message = "Warning resolved.",
            CreatedAtUtc = createdAtUtc.AddSeconds(45)
        });

        dbContext.AddRange(
            reagentBottle,
            workflowVersion,
            stainingTask,
            machineRun,
            channelBatch,
            slideTask,
            workflowExecution,
            stepExecution,
            deviceCommand,
            reagentConsumption,
            dabBatch,
            dabUsage,
            criticalAlarm,
            warningAlarm);
        await dbContext.SaveChangesAsync();

        dbContext.AuditLogs.AddRange(
            new AuditLog
            {
                ActorUser = operatorUser,
                Action = "channel.workflow.select",
                EntityType = "ChannelBatch",
                EntityId = channelBatch.Id,
                Message = JsonSerializer.Serialize(new
                {
                    commandId = "cmd-script-select",
                    correlationId = "corr-script-001",
                    machineRunId = machineRun.Id,
                    taskId = stainingTask.Id,
                    drawerCode = "A",
                    slotCode = "A-01",
                    reason = "Initial script selection"
                }),
                CreatedAtUtc = createdAtUtc.AddSeconds(-5)
            },
            new AuditLog
            {
                ActorUser = adminUser,
                Action = "channel.workflow.change",
                EntityType = "ChannelBatch",
                EntityId = channelBatch.Id,
                Message = JsonSerializer.Serialize(new
                {
                    commandId = "cmd-script-change",
                    correlationId = "corr-script-change-001",
                    machineRunId = machineRun.Id,
                    taskId = stainingTask.Id,
                    drawerCode = "A",
                    slotCode = "A-01",
                    reason = "Pre-start change audit trail"
                }),
                CreatedAtUtc = createdAtUtc.AddSeconds(-2)
            },
            new AuditLog
            {
                ActorUser = adminUser,
                Action = "user.reset_password",
                EntityType = "User",
                EntityId = operatorUser.Id,
                Message = JsonSerializer.Serialize(new
                {
                    commandId = "cmd-sensitive",
                    password = "SuperSecret123!",
                    token = "Bearer abc"
                }),
                CreatedAtUtc = createdAtUtc.AddSeconds(1)
            });
        await dbContext.SaveChangesAsync();

        return new SeededTraceabilityGraph(
            machineRun.Id,
            stainingTask.Id,
            criticalAlarm.Id,
            createdAtUtc);
    }

    private sealed record SeededTraceabilityGraph(
        string MachineRunId,
        string StainingTaskId,
        string CriticalAlarmId,
        DateTimeOffset CreatedAtUtc);

    private static void AssertOperatorSafe(string value)
    {
        Assert.DoesNotContain("%TEMP%", value, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("stainer-backup.db", value, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("SQLite", value, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("AttemptDirectory", value, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("database_backup_degraded", value, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("raw-packet", value, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("StateHash", value, StringComparison.OrdinalIgnoreCase);
    }
}
