namespace Stainer.Web.Infrastructure.Web;

using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;

public static partial class WebHostEndpointExtensions
{
    // 调试模块“精度校正” → 后端数据库。admin-only，对齐 /api/engineering/serial-config 的鉴权口径。
    // PUT 请求体手动 ReadFromJsonAsync 读取，不在 handler 上声明复杂 body 参数，
    // 以规避 minimal API 对该请求类型（全简单属性 record）的 body 推断报错（与 Serial 一致）。
    private static void MapPrecisionCalibrationEndpoints(WebApplication app)
    {
        app.MapGet("/api/engineering/precision-calibration/{scopeKey}", async (HttpContext context, string scopeKey, UserSessionService sessionService, PrecisionCalibrationConfigService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireAnyRoleAsync(context, ["admin"], cancellationToken);
                return Results.Ok(await service.GetAsync(scopeKey, cancellationToken));
            }));

        app.MapPut("/api/engineering/precision-calibration/{scopeKey}", async (HttpContext context, string scopeKey, UserSessionService sessionService, PrecisionCalibrationConfigService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["admin"], cancellationToken);
                var request = await context.Request.ReadFromJsonAsync<SavePrecisionCalibrationRequest>(cancellationToken);
                if(request is null)
                {
                    return Results.BadRequest(new { code = "request_body_required", detail = "Request body is required." });
                }

                // scopeKey 来自路由（权威），request 仅含可写的校正参数。
                return Results.Ok(await service.SaveAsync(scopeKey, request, actor, cancellationToken));
            }));
    }
}
