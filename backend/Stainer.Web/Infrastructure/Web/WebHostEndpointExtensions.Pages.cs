namespace Stainer.Web.Infrastructure.Web;

using Microsoft.Extensions.Hosting;

public static partial class WebHostEndpointExtensions
{
    private static readonly string[] PageRoutes =
    [
        "/",
        "/login",
        "/control-console",
        "/dashboard",
        "/samples",
        "/reagents",
        "/run",
        "/alerts",
        "/alarms",
        "/history",
        "/configure",
        "/engineer",
        "/admin",
        "/management"
    ];

    private static void MapPageEndpoints(WebApplication app, bool legacyRuntimeCompatibilityEnabled)
    {
        var pageRoutes = legacyRuntimeCompatibilityEnabled
            ? PageRoutes.Concat(["/mock-timeline"])
            : PageRoutes.Where(x => !x.Equals("/control-console", StringComparison.OrdinalIgnoreCase));

        foreach (var route in pageRoutes)
        {
            var capturedRoute = route;
            app.MapGet(capturedRoute, (LegacyUiPageRenderer renderer) => renderer.Render(capturedRoute));
        }
    }
}
