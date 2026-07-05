namespace Stainer.Web.Infrastructure.Web;

using Microsoft.Extensions.Hosting;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;
using Stainer.Web.Infrastructure.Health;

public static partial class WebHostEndpointExtensions
{
    private static void MapDabEndpoints(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapGet("/api/dab", (MockRuntimeStore store, int? slideCount) => Results.Ok(store.GetDab(slideCount)));
        }
        app.MapGet("/api/dab/positions", async (HttpContext context, UserSessionService sessionService, DabLifecycleService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireAnyRoleAsync(context, ["operator", "engineer", "admin"], cancellationToken);
                return Results.Ok(await service.ListPositionsAsync(cancellationToken));
            }));
        app.MapGet("/api/dab/batches/{batchId}", async (HttpContext context, string batchId, UserSessionService sessionService, DabLifecycleService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireAnyRoleAsync(context, ["operator", "engineer", "admin"], cancellationToken);
                return Results.Ok(await service.GetBatchAsync(batchId, cancellationToken));
            }));
        app.MapPost("/api/dab/batches", async (HttpContext context, CreateDabBatchRequest request, UserSessionService sessionService, DabLifecycleService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                return Results.Ok(await service.CreateBatchAsync(request, actor, cancellationToken));
            }));
        app.MapPost("/api/dab/batches/{batchId}/preparation/start", async (HttpContext context, string batchId, DabBatchCommandRequest request, UserSessionService sessionService, DabLifecycleService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                return Results.Ok(await service.StartPreparationAsync(batchId, request, actor, cancellationToken));
            }));
        app.MapPost("/api/dab/batches/{batchId}/preparation/complete", () => Results.Json(
            new { code = "dab_preparation_requires_executor", message = "DAB preparation completion must be recorded by MachineExecutor after a completed device command." },
            statusCode: StatusCodes.Status410Gone));
        app.MapPost("/api/dab/batches/{batchId}/consume", () => Results.Json(
            new { code = "dab_consumption_requires_executor", message = "DAB consumption must be recorded by MachineExecutor with run and step context." },
            statusCode: StatusCodes.Status410Gone));
        app.MapPost("/api/dab/batches/{batchId}/expire", async (HttpContext context, string batchId, DabBatchCommandRequest request, UserSessionService sessionService, DabLifecycleService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                return Results.Ok(await service.MarkExpiredAsync(batchId, request, actor, cancellationToken));
            }));
        app.MapPost("/api/dab/batches/{batchId}/fail", async (HttpContext context, string batchId, FailDabBatchRequest request, UserSessionService sessionService, DabLifecycleService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                return Results.Ok(await service.FailAsync(batchId, request, actor, cancellationToken));
            }));
        app.MapPost("/api/dab/batches/{batchId}/cleaning/start", async (HttpContext context, string batchId, DabBatchCommandRequest request, UserSessionService sessionService, DabLifecycleService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                return Results.Ok(await service.StartCleaningAsync(batchId, request, actor, cancellationToken));
            }));
        app.MapPost("/api/dab/batches/{batchId}/cleaning/confirm", async (HttpContext context, string batchId, DabBatchCommandRequest request, UserSessionService sessionService, DabLifecycleService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                return Results.Ok(await service.ConfirmCleaningAsync(batchId, request, actor, cancellationToken));
            }));
        if (app.Environment.IsDevelopment())
        {
            app.MapGet("/api/logs", (MockRuntimeStore store) =>
            {
                var state = store.GetState();
                return Results.Ok(new { state.Logs, state.Alarms });
            });
        }
    }
}
