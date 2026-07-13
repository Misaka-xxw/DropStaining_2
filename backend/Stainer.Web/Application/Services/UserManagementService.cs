using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class UserManagementService(
    StainerDbContext dbContext,
    PasswordHashService passwordHashService,
    CommandIdempotencyService idempotencyService)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task<UserMutationResponse> CreateUserAsync(
        CreateUserRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "user.create",
            request,
            actor,
            async () =>
            {
                var username = NormalizeRequired(request.Username, "username");
                if (await dbContext.Users.AnyAsync(x => x.Username == username, cancellationToken))
                {
                    throw new BusinessRuleException("username_exists", "Username already exists.", StatusCodes.Status409Conflict);
                }

                var roles = await LoadRolesAsync(request.Roles, cancellationToken);
                var user = new User
                {
                    Username = username,
                    DisplayName = NormalizeRequired(request.DisplayName, "displayName"),
                    PasswordHash = passwordHashService.Hash(request.Password),
                    PasswordHashAlgorithm = "PBKDF2-SHA256",
                    PasswordUpdatedAtUtc = DateTimeOffset.UtcNow,
                    CreatedAtUtc = DateTimeOffset.UtcNow
                };
                foreach (var role in roles)
                {
                    user.UserRoles.Add(new UserRole { RoleId = role.Id, CreatedAtUtc = DateTimeOffset.UtcNow });
                }

                dbContext.Users.Add(user);
                AddAudit(actor, "user.create", user.Id, new { after = ToAuditUser(user, roles.Select(x => x.Code)) });
                return new CommandExecutionResult<UserMutationResponse>(
                    ToResponse(request.CommandId, false, user, roles.Select(x => x.Code), "User created."),
                    "User",
                    user.Id);
            },
            cancellationToken);
    }

    public Task<UserMutationResponse> UpdateDisplayNameAsync(
        string userId,
        UpdateUserDisplayNameRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "user.update_display_name",
            new { userId, request },
            actor,
            async () =>
            {
                var user = await LoadUserAsync(userId, cancellationToken);
                var before = user.DisplayName;
                user.DisplayName = NormalizeRequired(request.DisplayName, "displayName");
                AddAudit(actor, "user.update_display_name", user.Id, new { before, after = user.DisplayName });
                return new CommandExecutionResult<UserMutationResponse>(
                    ToResponse(request.CommandId, false, user, GetRoleCodes(user), "Display name updated."),
                    "User",
                    user.Id);
            },
            cancellationToken);
    }

    public Task<UserMutationResponse> SetEnabledAsync(
        string userId,
        SetUserEnabledRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "user.set_enabled",
            new { userId, request },
            actor,
            async () =>
            {
                var user = await LoadUserAsync(userId, cancellationToken);
                var before = user.IsEnabled;
                user.IsEnabled = request.Enabled;
                AddAudit(actor, "user.set_enabled", user.Id, new { before, after = user.IsEnabled });
                return new CommandExecutionResult<UserMutationResponse>(
                    ToResponse(request.CommandId, false, user, GetRoleCodes(user), request.Enabled ? "User enabled." : "User disabled."),
                    "User",
                    user.Id);
            },
            cancellationToken);
    }

    public Task<UserMutationResponse> ResetPasswordAsync(
        string userId,
        ResetUserPasswordRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "user.reset_password",
            new { userId, request },
            actor,
            async () =>
            {
                var user = await LoadUserAsync(userId, cancellationToken);
                user.PasswordHash = passwordHashService.Hash(request.NewPassword);
                user.PasswordHashAlgorithm = "PBKDF2-SHA256";
                user.PasswordUpdatedAtUtc = DateTimeOffset.UtcNow;
                AddAudit(actor, "user.reset_password", user.Id, new { resetAtUtc = user.PasswordUpdatedAtUtc });
                return new CommandExecutionResult<UserMutationResponse>(
                    ToResponse(request.CommandId, false, user, GetRoleCodes(user), "Password reset."),
                    "User",
                    user.Id);
            },
            cancellationToken);
    }

    public Task<UserMutationResponse> SetRolesAsync(
        string userId,
        SetUserRolesRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "user.set_roles",
            new { userId, request },
            actor,
            async () =>
            {
                var user = await LoadUserAsync(userId, cancellationToken);
                var before = GetRoleCodes(user);
                var roles = await LoadRolesAsync(request.Roles, cancellationToken);
                user.UserRoles.Clear();
                foreach (var role in roles)
                {
                    user.UserRoles.Add(new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id,
                        CreatedAtUtc = DateTimeOffset.UtcNow
                    });
                }

                var after = roles.Select(x => x.Code).OrderBy(x => x).ToList();
                AddAudit(actor, "user.set_roles", user.Id, new { before, after });
                return new CommandExecutionResult<UserMutationResponse>(
                    ToResponse(request.CommandId, false, user, after, "Roles updated."),
                    "User",
                    user.Id);
            },
            cancellationToken);
    }

    public Task<CommandResponse> DeleteUserAsync(
        string userId,
        string commandId,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            commandId,
            "user.delete",
            new { userId, commandId },
            actor,
            async () =>
            {
                var user = await LoadUserAsync(userId, cancellationToken);
                // 仅当该用户作为 Actor 留下过“业务动作”审计时才阻止删除。
                // 登录/退出（auth.login / auth.logout）属于会话事件、不算业务动作，需排除，
                // 否则用户只要登录过一次就不能删除；账号本身的创建/改名等审计（由管理员发起）也不计入。
                var hasOperationalAudit = await dbContext.AuditLogs.AnyAsync(
                    x => x.ActorUserId == userId && !x.Action.StartsWith("auth."),
                    cancellationToken);
                if (hasOperationalAudit)
                {
                    throw new BusinessRuleException("user_has_audit_logs", "该用户存在操作审计记录（曾执行过业务动作），不能删除。", StatusCodes.Status409Conflict);
                }

                dbContext.Users.Remove(user);
                AddAudit(actor, "user.delete", user.Id, new { user.Username, user.DisplayName });
                return new CommandExecutionResult<CommandResponse>(
                    new CommandResponse(true, commandId, false, "User deleted."),
                    "User",
                    user.Id);
            },
            cancellationToken);
    }

    private async Task<User> LoadUserAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
        if (user is null)
        {
            throw new BusinessRuleException("user_not_found", "User was not found.", StatusCodes.Status404NotFound);
        }

        return user;
    }

    private async Task<IReadOnlyList<Role>> LoadRolesAsync(IReadOnlyList<string> roleCodes, CancellationToken cancellationToken)
    {
        var requested = roleCodes
            .Select(x => NormalizeRequired(x, "role"))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x)
            .ToList();
        if (requested.Count == 0)
        {
            throw new BusinessRuleException("role_required", "At least one role is required.");
        }

        var roles = await dbContext.Roles
            .Where(x => requested.Contains(x.Code))
            .OrderBy(x => x.Code)
            .ToListAsync(cancellationToken);
        if (roles.Count != requested.Count)
        {
            throw new BusinessRuleException("role_not_found", "One or more roles were not found.", StatusCodes.Status404NotFound);
        }

        return roles;
    }

    private static string NormalizeRequired(string value, string fieldName)
    {
        var normalized = value?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BusinessRuleException($"{fieldName}_required", $"{fieldName} is required.");
        }

        return normalized;
    }

    private void AddAudit(AuthenticatedUser actor, string action, string entityId, object details)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            ActorUserId = actor.UserId,
            Action = action,
            EntityType = "User",
            EntityId = entityId,
            Message = JsonSerializer.Serialize(details, JsonOptions),
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
    }

    private static IReadOnlyList<string> GetRoleCodes(User user)
    {
        return user.UserRoles
            .Select(x => x.Role?.Code)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!)
            .OrderBy(x => x)
            .ToList();
    }

    private static object ToAuditUser(User user, IEnumerable<string> roles)
    {
        return new
        {
            user.Username,
            user.DisplayName,
            user.IsEnabled,
            roles = roles.OrderBy(x => x).ToArray()
        };
    }

    private static UserMutationResponse ToResponse(
        string commandId,
        bool replayed,
        User user,
        IEnumerable<string> roles,
        string message)
    {
        return new UserMutationResponse(
            true,
            commandId,
            replayed,
            user.Id,
            user.Username,
            user.DisplayName,
            user.IsEnabled,
            roles.OrderBy(x => x).ToList(),
            message);
    }
}
