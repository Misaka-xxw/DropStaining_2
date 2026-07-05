namespace Stainer.Web.Infrastructure.Web;

using Microsoft.Extensions.Hosting;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;
using Stainer.Web.Infrastructure.Health;

public static partial class WebHostEndpointExtensions
{
    private static void MapTaskAndReagentEndpoints(WebApplication app)
    {
        app.MapPost("/api/tasks/he", async (HttpContext context, CreateHeTaskRequest request, UserSessionService sessionService, TaskCreationService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.CreateHeTaskAsync(request, actor, cancellationToken));
            }));
        app.MapPost("/api/tasks/ihc", async (HttpContext context, CreateIhcTaskRequest request, UserSessionService sessionService, TaskCreationService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.CreateIhcTaskAsync(request, actor, cancellationToken));
            }));
        app.MapPost("/api/lis/mock-query", async (HttpContext context, MockLisQueryRequest request, UserSessionService sessionService, MockLisQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.QueryAsync(request, actor, cancellationToken));
            }));
        app.MapPost("/api/reagents/scan-confirm", async (HttpContext context, ConfirmReagentScanRequest request, UserSessionService sessionService, ReagentScanWriteService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.ConfirmScanAsync(request, actor, cancellationToken));
            }));
        app.MapPost("/api/reagents/scan-sessions/start", async (HttpContext context, StartReagentScanSessionRequest request, UserSessionService sessionService, ReagentScanWriteService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.StartSessionAsync(request, actor, cancellationToken));
            }));
        app.MapPost("/api/reagents/scan-sessions/{scanSessionId}/complete", async (HttpContext context, string scanSessionId, CompleteReagentScanSessionRequest request, UserSessionService sessionService, ReagentScanWriteService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.CompleteSessionAsync(scanSessionId, request, actor, cancellationToken));
            }));
    }
}
