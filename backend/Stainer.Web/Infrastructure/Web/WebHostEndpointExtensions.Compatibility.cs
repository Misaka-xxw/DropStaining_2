namespace Stainer.Web.Infrastructure.Web;

using Microsoft.Extensions.Hosting;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;
using Stainer.Web.Infrastructure.Health;

public static partial class WebHostEndpointExtensions
{
    private static void MapCompatibilityEndpoints(WebApplication app, bool legacyRuntimeCompatibilityEnabled)
    {
        if (legacyRuntimeCompatibilityEnabled)
        {
            app.MapGet("/api/state", async (RuntimePageBridgeService bridge, CancellationToken cancellationToken) =>
                Results.Ok(await bridge.GetStateAsync(cancellationToken)));
        }

        app.MapPost("/api/system/initialize", async (HttpContext context, StartDeviceInitializationRequest request, UserSessionService sessionService, DeviceInitializationService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "engineer", "admin"], cancellationToken);
                return Results.Ok(await service.InitializeAsync(request, actor, cancellationToken));
            }));
        app.MapPost("/api/system/reset", () => Results.Json(
            new { code = "legacy_runtime_reset_disabled", message = "Legacy MockRuntimeStore reset is disabled." },
            statusCode: StatusCodes.Status410Gone));
        app.MapPost("/api/samples/scan", async (HttpContext context, int? count, UserSessionService sessionService, SampleScanWriteService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                var body = await ReadOptionalJsonAsync<MockSampleScanRequest>(context, cancellationToken);
                var request = body ?? new MockSampleScanRequest($"sample-scan-{Guid.NewGuid():N}", count ?? 8, "Mixed", null, null);
                return Results.Ok(await service.ScanAsync(request, actor, cancellationToken));
            }));
        app.MapPost("/api/reagents/scan", async (HttpContext context, UserSessionService sessionService, ReagentScannerMockService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                var request = await ReadOptionalJsonAsync<MockReagentScanRequest>(context, cancellationToken)
                    ?? new MockReagentScanRequest($"reagent-scan-{Guid.NewGuid():N}", "all", null, null, "Mixed", null, null);
                return Results.Ok(await service.ScanAsync(request, actor, cancellationToken));
            }));
        app.MapPost("/api/mock-demo-data/seed", async (HttpContext context, RunCommandRequest request, UserSessionService sessionService, MockDemoDataSeeder seeder, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                return Results.Ok(await seeder.SeedAsync(request.CommandId, actor, cancellationToken));
            }));
        app.MapPost("/api/mock-demo-data/reset", async (HttpContext context, ResetMockDemoDataRequest request, UserSessionService sessionService, MockDemoDataSeeder seeder, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                return Results.Ok(await seeder.ResetAsync(request, actor, cancellationToken));
            }));
        if (legacyRuntimeCompatibilityEnabled)
        {
            app.MapPost("/api/run/start", async (HttpContext context, RuntimePageBridgeService bridge, UserSessionService sessionService, CancellationToken cancellationToken) =>
                await ExecuteBusinessAsync(async () =>
                {
                    var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                    return Results.Ok(await bridge.RunActionAsync("start", actor, cancellationToken));
                }));
            app.MapPost("/api/run/pause", async (HttpContext context, RuntimePageBridgeService bridge, UserSessionService sessionService, CancellationToken cancellationToken) =>
                await ExecuteBusinessAsync(async () =>
                {
                    var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                    return Results.Ok(await bridge.RunActionAsync("pause", actor, cancellationToken));
                }));
            app.MapPost("/api/run/resume", async (HttpContext context, RuntimePageBridgeService bridge, UserSessionService sessionService, CancellationToken cancellationToken) =>
                await ExecuteBusinessAsync(async () =>
                {
                    var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                    return Results.Ok(await bridge.RunActionAsync("resume", actor, cancellationToken));
                }));
            app.MapPost("/api/run/stop", async (HttpContext context, RuntimePageBridgeService bridge, UserSessionService sessionService, CancellationToken cancellationToken) =>
                await ExecuteBusinessAsync(async () =>
                {
                    var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                    return Results.Ok(await bridge.RunActionAsync("stop", actor, cancellationToken));
                }));
        }
        if (app.Environment.IsDevelopment())
        {
            app.MapPost("/api/slides/configure", (MockRuntimeStore store, SlideConfigureRequest request) =>
            {
                try
                {
                    return Results.Ok(store.ConfigureSlide(request));
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.Json(new { detail = ex.Message }, statusCode: StatusCodes.Status404NotFound);
                }
            });
            app.MapPost("/api/run/add-slide", (MockRuntimeStore store) => Results.Ok(store.GetState()));
            app.MapPost("/api/engineer/command", (MockRuntimeStore store, EngineerCommandRequest request) => Results.Ok(store.EngineerCommand(request)));
        }
    }
}
