namespace Stainer.Web.Infrastructure.Web;

using Microsoft.Extensions.Hosting;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;
using Stainer.Web.Infrastructure.Health;

public static partial class WebHostEndpointExtensions
{
    private static void MapTraceabilityEndpoints(WebApplication app)
    {
        app.MapGet("/api/history/runs", async (HttpContext context, UserSessionService sessionService, TraceabilityQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.ListRunsAsync(context.Request.Query, cancellationToken));
            }));
        app.MapGet("/api/history/runs/{machineRunId}", async (HttpContext context, string machineRunId, UserSessionService sessionService, TraceabilityQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                var detail = await service.GetRunDetailAsync(machineRunId, cancellationToken);
                if (detail is null) return Results.NotFound();
                if (!string.Equals(actor.ActiveRole, "operator", StringComparison.OrdinalIgnoreCase))
                {
                    return Results.Ok(detail with { Alarms = detail.Alarms.Select(ToNonTechnicalAlarm).ToList() });
                }
                return Results.Ok(new
                {
                    detail.MachineRunId,
                    detail.RunCode,
                    detail.Status,
                    detail.CreatedAtUtc,
                    detail.StartedAtUtc,
                    detail.CompletedAtUtc,
                    detail.RequestedBy,
                    Channels = detail.ChannelBatches.Select(x => new
                    {
                        x.ChannelBatchId,
                        x.DrawerCode,
                        x.Status,
                        x.ExperimentType,
                        x.WorkflowCode,
                        x.WorkflowName,
                        x.WorkflowVersionLabel,
                        x.CreatedAtUtc,
                        x.StartedAtUtc,
                        x.CompletedAtUtc,
                        Slides = x.Slides.Select(s => new
                        {
                            s.SlideTaskId,
                            s.TaskCode,
                            s.SlotCode,
                            s.TaskType,
                            s.Status,
                            SampleCode = s.NormalizedSampleCode ?? s.RawSampleCode,
                            s.PrimaryAntibodyCode,
                            s.CreatedBy,
                            s.CreatedAtUtc
                        })
                    }),
                    Steps = detail.WorkflowExecutions.SelectMany(x => x.Steps).Select(x => new
                    {
                        x.StepNo,
                        x.StepName,
                        x.Status,
                        x.RedoCount,
                        x.StartedAtUtc,
                        x.CompletedAtUtc
                    }),
                    detail.ReagentConsumptions,
                    detail.DabUsages,
                    Alarms = detail.Alarms.Select(ToNonTechnicalAlarm)
                });
            }));
        app.MapGet("/api/history/reagent-consumptions", async (HttpContext context, UserSessionService sessionService, TraceabilityQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.ListReagentConsumptionsAsync(context.Request.Query, cancellationToken));
            }));
        app.MapGet("/api/alarms", async (HttpContext context, UserSessionService sessionService, TraceabilityQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                var result = await service.ListAlarmsAsync(context.Request.Query, cancellationToken);
                return Results.Ok(result with { Items = result.Items.Select(ToNonTechnicalAlarm).ToList() });
            }));
        app.MapPost("/api/alarms/{alarmId}/acknowledge", async (HttpContext context, string alarmId, AcknowledgeAlarmRequest request, UserSessionService sessionService, TraceabilityQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.AcknowledgeAlarmAsync(alarmId, request, actor, cancellationToken));
            }));
        app.MapGet("/api/audit/logs", async (HttpContext context, UserSessionService sessionService, TraceabilityQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireAnyRoleAsync(context, ["admin"], cancellationToken);
                return Results.Ok(await service.ListAuditLogsAsync(context.Request.Query, cancellationToken));
            }));
        app.MapGet("/api/history/export/runs", async (HttpContext context, UserSessionService sessionService, TraceabilityQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return ToCsvFile(await service.ExportRunsAsync(context.Request.Query, actor, cancellationToken));
            }));
        app.MapGet("/api/history/export/reagent-consumptions", async (HttpContext context, UserSessionService sessionService, TraceabilityQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return ToCsvFile(await service.ExportReagentConsumptionsAsync(context.Request.Query, actor, cancellationToken));
            }));
        app.MapGet("/api/alarms/export", async (HttpContext context, UserSessionService sessionService, TraceabilityQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return ToCsvFile(await service.ExportAlarmsAsync(context.Request.Query, actor, cancellationToken));
            }));
        app.MapGet("/api/audit/export", async (HttpContext context, UserSessionService sessionService, TraceabilityQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["admin"], cancellationToken);
                return ToCsvFile(await service.ExportAuditLogsAsync(context.Request.Query, actor, cancellationToken));
            }));
    }
}
