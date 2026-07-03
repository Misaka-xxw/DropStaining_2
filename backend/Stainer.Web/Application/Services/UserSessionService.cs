using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class UserSessionService(StainerDbContext dbContext, PasswordHashService passwordHashService, IHostEnvironment environment)
{
    public const string SessionCookieName = "stainer_session";
    private static readonly ConcurrentDictionary<string, AuthenticatedUser> Sessions = new(StringComparer.Ordinal);

    public async Task<LoginResponse> LoginAsync(
        string username,
        string password,
        string requestedRole,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(x => x.Username == username, cancellationToken);

        if (user is null || !user.IsEnabled || !passwordHashService.Verify(password, user.PasswordHash))
        {
            throw new BusinessRuleException("invalid_login", "Invalid username, password or role.", StatusCodes.Status401Unauthorized);
        }

        var roles = user.UserRoles
            .Select(x => x.Role?.Code)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!)
            .OrderBy(x => x)
            .ToList();

        if (!roles.Contains(requestedRole, StringComparer.OrdinalIgnoreCase))
        {
            throw new BusinessRuleException("invalid_role", "Invalid username, password or role.", StatusCodes.Status401Unauthorized);
        }

        var activeRole = roles.First(x => string.Equals(x, requestedRole, StringComparison.OrdinalIgnoreCase));
        var principal = new AuthenticatedUser(user.Id, user.Username, user.DisplayName, activeRole, roles);
        var sessionId = Guid.NewGuid().ToString("N");
        Sessions[sessionId] = principal;

        httpContext.Response.Cookies.Append(SessionCookieName, sessionId, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = false,
            Path = "/"
        });

        dbContext.AuditLogs.Add(new AuditLog
        {
            ActorUserId = user.Id,
            Action = "auth.login",
            EntityType = "User",
            EntityId = user.Id,
            Message = $"User login as {activeRole}",
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
        await dbContext.SaveChangesAsync(cancellationToken);

        var developmentOrTesting = environment.IsDevelopment() || environment.IsEnvironment("Testing");
        return new LoginResponse(true, ToCurrentUser(principal), developmentOrTesting ? "/control-console" : "/dashboard");
    }

    public async Task LogoutAsync(HttpContext httpContext, CancellationToken cancellationToken = default)
    {
        var current = await GetCurrentUserAsync(httpContext, cancellationToken);
        if (httpContext.Request.Cookies.TryGetValue(SessionCookieName, out var sessionId))
        {
            Sessions.TryRemove(sessionId, out _);
        }

        httpContext.Response.Cookies.Delete(SessionCookieName);

        if (current is not null)
        {
            dbContext.AuditLogs.Add(new AuditLog
            {
                ActorUserId = current.UserId,
                Action = "auth.logout",
                EntityType = "User",
                EntityId = current.UserId,
                Message = "User logout",
                CreatedAtUtc = DateTimeOffset.UtcNow
            });
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<AuthenticatedUser?> GetCurrentUserAsync(HttpContext httpContext, CancellationToken cancellationToken = default)
    {
        if (!httpContext.Request.Cookies.TryGetValue(SessionCookieName, out var sessionId)
            || !Sessions.TryGetValue(sessionId, out var principal))
        {
            return null;
        }

        var isEnabled = await dbContext.Users
            .AsNoTracking()
            .AnyAsync(x => x.Id == principal.UserId && x.IsEnabled, cancellationToken);
        return isEnabled ? principal : null;
    }

    public async Task<AuthenticatedUser> RequireRoleAsync(
        HttpContext httpContext,
        string requiredRole,
        CancellationToken cancellationToken = default)
    {
        return await RequireAnyRoleAsync(httpContext, [requiredRole], cancellationToken);
    }

    public async Task<AuthenticatedUser> RequireAnyRoleAsync(
        HttpContext httpContext,
        IReadOnlyList<string> requiredRoles,
        CancellationToken cancellationToken = default)
    {
        var user = await GetCurrentUserAsync(httpContext, cancellationToken);
        if (user is null)
        {
            throw new BusinessRuleException("authentication_required", "Login is required.", StatusCodes.Status401Unauthorized);
        }

        if (!requiredRoles.Any(user.HasRole))
        {
            throw new BusinessRuleException("forbidden", "Current user is not allowed to perform this operation.", StatusCodes.Status403Forbidden);
        }

        return user;
    }

    public static CurrentUserResponse ToCurrentUser(AuthenticatedUser user)
    {
        return new CurrentUserResponse(user.UserId, user.Username, user.DisplayName, user.ActiveRole, user.Roles);
    }
}
