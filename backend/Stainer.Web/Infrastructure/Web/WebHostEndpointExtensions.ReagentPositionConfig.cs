namespace Stainer.Web.Infrastructure.Web;

using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;

public static partial class WebHostEndpointExtensions
{
    // 试剂位对象配置（坐标修正 + 孔位Z高度）→ 后端数据库。admin-only，简单 GET/PUT。
    private static void MapReagentPositionConfigEndpoints(WebApplication app)
    {
        app.MapGet("/api/engineering/reagent-position-config/{rackCode}", async (HttpContext context, string rackCode, UserSessionService sessionService, ReagentPositionConfigService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireAnyRoleAsync(context, ["admin"], cancellationToken);
                return Results.Ok(await service.GetAsync(rackCode, cancellationToken));
            }));

        app.MapPut("/api/engineering/reagent-position-config/{rackCode}", async (HttpContext context, string rackCode, UserSessionService sessionService, ReagentPositionConfigService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["admin"], cancellationToken);
                var request = await context.Request.ReadFromJsonAsync<SaveReagentPositionConfigRequest>(cancellationToken);
                if(request is null) return Results.BadRequest(new { code = "request_body_required", detail = "Request body is required." });
                return Results.Ok(await service.SaveAsync(rackCode, request, actor, cancellationToken));
            }));

        app.MapPost("/api/engineering/reagent-position-config/{rackCode}/move-test", async (HttpContext context, string rackCode, MoveReagentPositionHardwareRequest request, UserSessionService sessionService, EngineeringSessionService engineeringSessionService, ReagentPositionHardwareService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["admin"], cancellationToken);
                await engineeringSessionService.RequireWriteSessionAsync(
                    actor,
                    request.CommandId,
                    request.Reason,
                    request.Target ?? $"reagent-position:{rackCode}:{request.TargetZ}",
                    request.DangerousOperationConfirmed,
                    cancellationToken);
                return Results.Ok(await service.MoveAsync(rackCode, request, actor, cancellationToken));
            }));
    }
}
