namespace Stainer.Web.Infrastructure.Web;

using Microsoft.Extensions.Hosting;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;
using Stainer.Web.Infrastructure.Health;

public static partial class WebHostEndpointExtensions
{
    private static void MapRunEndpoints(WebApplication app)
    {
        app.MapGet("/api/run/preflight", async (HttpContext context, UserSessionService sessionService, PreflightValidationService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.ValidateAsync(cancellationToken));
            }));
        app.MapPost("/api/runs", async (HttpContext context, CreateMachineRunRequest request, UserSessionService sessionService, MachineRunService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.CreateRunAsync(request, actor, cancellationToken));
            }));
        app.MapGet("/api/runs/current", async (HttpContext context, UserSessionService sessionService, MachineRunQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                var run = await service.GetCurrentAsync(cancellationToken);
                var nonTechnicalRun = run is null ? null : ToNonTechnicalRun(run);
                return run is null
                    ? Results.NotFound()
                    : Results.Ok(string.Equals(actor.ActiveRole, "operator", StringComparison.OrdinalIgnoreCase)
                        ? ToOperatorRun(nonTechnicalRun!)
                        : nonTechnicalRun!);
            }));
        app.MapGet("/api/runs/{id}", async (HttpContext context, string id, UserSessionService sessionService, MachineRunQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                var run = await service.GetAsync(id, cancellationToken);
                var nonTechnicalRun = run is null ? null : ToNonTechnicalRun(run);
                return run is null
                    ? Results.NotFound()
                    : Results.Ok(string.Equals(actor.ActiveRole, "operator", StringComparison.OrdinalIgnoreCase)
                        ? ToOperatorRun(nonTechnicalRun!)
                        : nonTechnicalRun!);
            }));
        app.MapPost("/api/runs/{id}/start", async (HttpContext context, string id, RunCommandRequest request, UserSessionService sessionService, RunControlService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.StartAsync(id, request, actor, cancellationToken));
            }));
        app.MapPost("/api/runs/{id}/pause", async (HttpContext context, string id, RunCommandRequest request, UserSessionService sessionService, RunControlService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.PauseAsync(id, request, actor, cancellationToken));
            }));
        app.MapPost("/api/runs/{id}/resume", async (HttpContext context, string id, RunCommandRequest request, UserSessionService sessionService, RunControlService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.ResumeAsync(id, request, actor, cancellationToken));
            }));
        app.MapPost("/api/runs/{id}/stop", async (HttpContext context, string id, RunCommandRequest request, UserSessionService sessionService, RunControlService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.StopAsync(id, request, actor, cancellationToken));
            }));
        app.MapPost("/api/runs/{id}/fault", async (HttpContext context, string id, InjectFaultRequest request, UserSessionService sessionService, RunControlService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["admin"], cancellationToken);
                return Results.Ok(await service.InjectFaultAsync(id, request, actor, cancellationToken));
            }));
        app.MapPost("/api/runs/{id}/redo-current-major-step", async (HttpContext context, string id, RedoMajorStepRequest request, UserSessionService sessionService, RunControlService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["admin"], cancellationToken);
                return Results.Ok(await service.RedoCurrentMajorStepAsync(id, request, actor, cancellationToken));
            }));
    }
}
