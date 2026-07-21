namespace Stainer.Web.Infrastructure.Web;

using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;

public static partial class WebHostEndpointExtensions
{
    // 设置模块「运行/通讯/设备参数」单行配置 → 后端数据库。admin-only。
    // PUT 手动 ReadFromJsonAsync（规避全简单属性 record 的 body 推断报错）。
    private static void MapAppSettingsEndpoints(WebApplication app)
    {
        app.MapGet("/api/engineering/app-settings", async (HttpContext context, UserSessionService sessionService, AppSettingsConfigService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireAnyRoleAsync(context, ["admin"], cancellationToken);
                return Results.Ok(await service.GetAsync(cancellationToken));
            }));

        app.MapPut("/api/engineering/app-settings", async (HttpContext context, UserSessionService sessionService, AppSettingsConfigService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["admin"], cancellationToken);
                var request = await context.Request.ReadFromJsonAsync<SaveAppSettingsRequest>(cancellationToken);
                if(request is null)
                {
                    return Results.BadRequest(new { code = "request_body_required", detail = "Request body is required." });
                }

                return Results.Ok(await service.SaveAsync(request, actor, cancellationToken));
            }));
    }
}
