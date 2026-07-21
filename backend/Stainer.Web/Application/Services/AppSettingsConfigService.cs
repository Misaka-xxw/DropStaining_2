using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

// 应用运行参数配置服务：单行(ScopeKey=default) 读取 / upsert 持久化。
// 镜像 WashValveConfigService 的幂等 + 审计模式。仅持久化工程参数，不直接驱动真实硬件。
public sealed class AppSettingsConfigService(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<AppSettingsResponse> GetAsync(CancellationToken cancellationToken = default)
    {
        var profile = await dbContext.AppSettingsProfiles
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.ScopeKey == AppSettingsScopeKeys.Default, cancellationToken);
        return ToResponse(profile ?? CreateDefault());
    }

    public Task<AppSettingsMutationResponse> SaveAsync(
        SaveAppSettingsRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "app_settings.save",
            request,
            actor,
            async () =>
            {
                RequireReason(request.Reason);
                var dataInterface = NormalizeOptionalValue(request.DataInterface, "dataInterface", 64);
                var hostAddress = NormalizeOptionalValue(request.HostAddress, "hostAddress", 256);
                var heartbeatSec = ValidateIntRange(request.HeartbeatSec, "heartbeatSec", 1, 3600);
                var reagentBottleCapacityMl = ValidateDecimalRange(request.ReagentBottleCapacityMl, "reagentBottleCapacityMl", 0, 1_000_000m);
                var reagentTargetTempC = ValidateDecimalRange(request.ReagentTargetTempC, "reagentTargetTempC", 0, 200m);
                var workTargetTempC = ValidateDecimalRange(request.WorkTargetTempC, "workTargetTempC", 0, 200m);
                var needleGapMm = ValidateDecimalRange(request.NeedleGapMm, "needleGapMm", 0, 100m);

                var profile = await dbContext.AppSettingsProfiles
                    .SingleOrDefaultAsync(x => x.ScopeKey == AppSettingsScopeKeys.Default, cancellationToken);
                object? before = profile is null ? null : ToAudit(profile);
                if (profile is null)
                {
                    profile = new AppSettingsProfile
                    {
                        ScopeKey = AppSettingsScopeKeys.Default,
                        CreatedAtUtc = DateTimeOffset.UtcNow
                    };
                    dbContext.AppSettingsProfiles.Add(profile);
                }

                if (!string.IsNullOrWhiteSpace(request.DataInterface)) profile.DataInterface = dataInterface;
                if (!string.IsNullOrWhiteSpace(request.HostAddress)) profile.HostAddress = hostAddress;
                if (heartbeatSec.HasValue) profile.HeartbeatSec = heartbeatSec;
                if (reagentBottleCapacityMl.HasValue) profile.ReagentBottleCapacityMl = reagentBottleCapacityMl;
                if (reagentTargetTempC.HasValue) profile.ReagentTargetTempC = reagentTargetTempC;
                if (workTargetTempC.HasValue) profile.WorkTargetTempC = workTargetTempC;
                if (needleGapMm.HasValue) profile.NeedleGapMm = needleGapMm;
                profile.Enabled = true;
                profile.UpdatedAtUtc = DateTimeOffset.UtcNow;

                AddAudit(actor, "app_settings.save", "AppSettingsProfile", profile.Id, before, ToAudit(profile), request.Reason);

                return new CommandExecutionResult<AppSettingsMutationResponse>(
                    new AppSettingsMutationResponse(true, request.CommandId, false, "AppSettingsProfile", profile.Id, "App settings saved."),
                    "AppSettingsProfile",
                    profile.Id);
            },
            cancellationToken);
    }

    private static AppSettingsProfile CreateDefault() => new()
    {
        ScopeKey = AppSettingsScopeKeys.Default,
        Enabled = true
    };

    private static AppSettingsResponse ToResponse(AppSettingsProfile p) => new(
        p.ScopeKey, p.DataInterface, p.HostAddress, p.HeartbeatSec, p.ReagentBottleCapacityMl,
        p.ReagentTargetTempC, p.WorkTargetTempC, p.NeedleGapMm, p.Enabled, p.CreatedAtUtc, p.UpdatedAtUtc);

    private static object ToAudit(AppSettingsProfile p) => new
    {
        p.ScopeKey, p.DataInterface, p.HostAddress, p.HeartbeatSec,
        p.ReagentBottleCapacityMl, p.ReagentTargetTempC, p.WorkTargetTempC, p.NeedleGapMm, p.Enabled
    };

    private void AddAudit(AuthenticatedUser actor, string action, string entityType, string entityId, object? before, object after, string reason)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            ActorUserId = string.IsNullOrWhiteSpace(actor.UserId) ? null : actor.UserId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Message = JsonSerializer.Serialize(new { before, after, reason = reason.Trim(), actor = actor.Username }, JsonOptions),
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
    }

    private static int? ValidateIntRange(int? value, string fieldName, int minimum, int maximum)
    {
        if (!value.HasValue) return null;
        if (value.Value < minimum || value.Value > maximum)
        {
            throw new BusinessRuleException($"{ToCode(fieldName)}_invalid", $"{fieldName} must be between {minimum} and {maximum}.", StatusCodes.Status400BadRequest);
        }
        return value;
    }

    private static decimal? ValidateDecimalRange(decimal? value, string fieldName, decimal minimum, decimal maximum)
    {
        if (!value.HasValue) return null;
        if (value.Value < minimum || value.Value > maximum)
        {
            throw new BusinessRuleException($"{ToCode(fieldName)}_invalid", $"{fieldName} must be between {minimum} and {maximum}.", StatusCodes.Status400BadRequest);
        }
        return value;
    }

    private static string? NormalizeOptionalValue(string? value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var normalized = value.Trim();
        if (normalized.Length > maxLength)
        {
            throw new BusinessRuleException($"{ToCode(fieldName)}_too_long", $"{fieldName} must be at most {maxLength} characters.", StatusCodes.Status400BadRequest);
        }
        return normalized;
    }

    private static void RequireReason(string? reason)
    {
        var normalized = reason?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BusinessRuleException("reason_required", "reason is required.", StatusCodes.Status400BadRequest);
        }

        if (normalized.Length > 2000)
        {
            throw new BusinessRuleException("reason_too_long", "reason must be at most 2000 characters.", StatusCodes.Status400BadRequest);
        }
    }

    private static string ToCode(string fieldName)
    {
        return string.Concat(fieldName.Select((ch, index) =>
            char.IsUpper(ch) && index > 0 ? $"_{char.ToLowerInvariant(ch)}" : char.ToLowerInvariant(ch).ToString()));
    }
}
