namespace Stainer.Web.Infrastructure.Web;

using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;

public static partial class WebHostEndpointExtensions
{
    private static void MapScannerConfigurationEndpoints(WebApplication app)
    {
        app.MapGet("/api/scanners", async (HttpContext context, UserSessionService sessionService, ScannerConfigurationService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                return Results.Ok(await service.ListProfilesAsync(cancellationToken));
            }));

        app.MapGet("/api/scanners/{id}", async (HttpContext context, string id, UserSessionService sessionService, ScannerConfigurationService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                var profile = await service.GetProfileAsync(id, cancellationToken);
                return profile is null ? Results.NotFound() : Results.Ok(profile);
            }));

        app.MapPost("/api/scanners", async (HttpContext context, SaveScannerProfileRequest request, UserSessionService sessionService, ScannerConfigurationService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                return Results.Ok(await service.CreateProfileAsync(request, actor, cancellationToken));
            }));

        app.MapPut("/api/scanners/{id}", async (HttpContext context, string id, SaveScannerProfileRequest request, UserSessionService sessionService, ScannerConfigurationService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                return Results.Ok(await service.UpdateProfileAsync(id, request, actor, cancellationToken));
            }));

        app.MapGet("/api/scanner-regions", async (HttpContext context, string? scannerProfileId, string? regionType, UserSessionService sessionService, ScannerConfigurationService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                return Results.Ok(await service.ListRegionsAsync(scannerProfileId, regionType, cancellationToken));
            }));

        app.MapPost("/api/scanner-regions", async (HttpContext context, SaveScannerRegionRequest request, UserSessionService sessionService, ScannerConfigurationService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["engineer", "admin"], cancellationToken);
                return Results.Ok(await service.CreateRegionAsync(request, actor, cancellationToken));
            }));
    }
}
