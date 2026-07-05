namespace Stainer.Web.Infrastructure.Web;

using Microsoft.Extensions.Hosting;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;
using Stainer.Web.Infrastructure.Health;

public static partial class WebHostEndpointExtensions
{
    private static void MapIdentityEndpoints(WebApplication app)
    {
        app.MapGet("/api/operator/snapshot", async (HttpContext context, UserSessionService sessionService, OperatorSnapshotQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "engineer", "admin"], cancellationToken);
                return Results.Ok(await service.GetAsync(actor, cancellationToken));
            }));
        app.MapGet("/api/current-user", async (HttpContext context, UserSessionService sessionService, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var user = await sessionService.GetCurrentUserAsync(context, cancellationToken);
                return user is null
                    ? Results.Json(new { code = "authentication_required", detail = "Login is required." }, statusCode: StatusCodes.Status401Unauthorized)
                    : Results.Ok(UserSessionService.ToCurrentUser(user));
            }));
        app.MapGet("/api/users", async (HttpContext context, UserSessionService sessionService, UserQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.ListUsersAsync(cancellationToken));
            }));
        app.MapGet("/api/roles", async (HttpContext context, UserSessionService sessionService, UserQueryService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                _ = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.ListRolesAsync(cancellationToken));
            }));

        app.MapPost("/api/login", async (HttpContext context, UserSessionService sessionService, LoginRequest request, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var response = await sessionService.LoginAsync(request.Username, request.Password, request.Role, context, cancellationToken);
                return Results.Ok(response);
            }));

        app.MapPost("/api/logout", async (HttpContext context, UserSessionService sessionService, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                await sessionService.LogoutAsync(context, cancellationToken);
                return Results.Ok(new { ok = true });
            }));

        app.MapPost("/api/users", async (HttpContext context, CreateUserRequest request, UserSessionService sessionService, UserManagementService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.CreateUserAsync(request, actor, cancellationToken));
            }));
        app.MapPut("/api/users/{id}/display-name", async (HttpContext context, string id, UpdateUserDisplayNameRequest request, UserSessionService sessionService, UserManagementService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.UpdateDisplayNameAsync(id, request, actor, cancellationToken));
            }));
        app.MapPut("/api/users/{id}/enabled", async (HttpContext context, string id, SetUserEnabledRequest request, UserSessionService sessionService, UserManagementService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.SetEnabledAsync(id, request, actor, cancellationToken));
            }));
        app.MapPut("/api/users/{id}/password", async (HttpContext context, string id, ResetUserPasswordRequest request, UserSessionService sessionService, UserManagementService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.ResetPasswordAsync(id, request, actor, cancellationToken));
            }));
        app.MapPut("/api/users/{id}/roles", async (HttpContext context, string id, SetUserRolesRequest request, UserSessionService sessionService, UserManagementService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.SetRolesAsync(id, request, actor, cancellationToken));
            }));
        app.MapDelete("/api/users/{id}", async (HttpContext context, string id, string commandId, UserSessionService sessionService, UserManagementService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.DeleteUserAsync(id, commandId, actor, cancellationToken));
            }));
    }
}
