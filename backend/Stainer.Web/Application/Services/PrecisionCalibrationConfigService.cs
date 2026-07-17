using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

// 精度校正配置服务：按 scopeKey（move|dispense）读取 / upsert 持久化。
// 镜像 SerialConnectionConfigService 的幂等 + 审计模式。仅持久化校正配置，不触发真实动作。
public sealed class PrecisionCalibrationConfigService(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly string[] AllowedScopes = { PrecisionCalibrationScopeKeys.Move, PrecisionCalibrationScopeKeys.Dispense };

    public async Task<PrecisionCalibrationResponse> GetAsync(string scopeKey, CancellationToken cancellationToken = default)
    {
        var key = NormalizeScopeKey(scopeKey);
        var profile = await dbContext.PrecisionCalibrationProfiles
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.ScopeKey == key, cancellationToken);
        return ToResponse(profile ?? CreateDefault(key));
    }

    public Task<PrecisionCalibrationMutationResponse> SaveAsync(
        string scopeKey,
        SavePrecisionCalibrationRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        scopeKey = NormalizeScopeKey(scopeKey);
        return idempotencyService.RunAsync(
            request.CommandId,
            "precision_calibration.save",
            new { scopeKey, request },
            actor,
            async () =>
            {
                RequireReason(request.Reason);
                var moveOffsetX = ValidateDoubleRange(request.MoveOffsetXMm, "moveOffsetXMm", -1000d, 1000d);
                var moveOffsetY = ValidateDoubleRange(request.MoveOffsetYMm, "moveOffsetYMm", -1000d, 1000d);
                var dispenseTarget = ValidateDoubleRange(request.DispenseTargetVolumeUl, "dispenseTargetVolumeUl", 0d, 100000d, requirePositive: true);
                var dispenseMeasured = ValidateDoubleRange(request.DispenseMeasuredVolumeUl, "dispenseMeasuredVolumeUl", 0d, 100000d);

                var profile = await dbContext.PrecisionCalibrationProfiles
                    .SingleOrDefaultAsync(x => x.ScopeKey == scopeKey, cancellationToken);
                object? before = profile is null ? null : ToAudit(profile);
                if (profile is null)
                {
                    profile = new PrecisionCalibrationProfile
                    {
                        ScopeKey = scopeKey,
                        CreatedAtUtc = DateTimeOffset.UtcNow
                    };
                    dbContext.PrecisionCalibrationProfiles.Add(profile);
                }

                if (moveOffsetX.HasValue) profile.MoveOffsetXMm = moveOffsetX;
                if (moveOffsetY.HasValue) profile.MoveOffsetYMm = moveOffsetY;
                if (dispenseTarget.HasValue) profile.DispenseTargetVolumeUl = dispenseTarget;
                if (dispenseMeasured.HasValue) profile.DispenseMeasuredVolumeUl = dispenseMeasured;

                // 加样校正因子：仅在目标体积 > 0 且有实测值时派生，避免除零。
                profile.DispenseCalibrationFactor = (scopeKey == PrecisionCalibrationScopeKeys.Dispense
                    && profile.DispenseTargetVolumeUl is double target && target > 0d
                    && profile.DispenseMeasuredVolumeUl is double measured)
                        ? Math.Round(measured / target, 6)
                        : profile.DispenseCalibrationFactor;

                profile.Enabled = true;
                profile.UpdatedAtUtc = DateTimeOffset.UtcNow;

                AddAudit(actor, "precision_calibration.save", "PrecisionCalibrationProfile", profile.Id, before, ToAudit(profile), request.Reason);

                return new CommandExecutionResult<PrecisionCalibrationMutationResponse>(
                    new PrecisionCalibrationMutationResponse(true, request.CommandId, false, "PrecisionCalibrationProfile", profile.Id, "Precision calibration config saved."),
                    "PrecisionCalibrationProfile",
                    profile.Id);
            },
            cancellationToken);
    }

    private static PrecisionCalibrationProfile CreateDefault(string scopeKey)
    {
        var isDispense = scopeKey == PrecisionCalibrationScopeKeys.Dispense;
        return new PrecisionCalibrationProfile
        {
            ScopeKey = scopeKey,
            MoveOffsetXMm = isDispense ? null : 0d,
            MoveOffsetYMm = isDispense ? null : 0d,
            DispenseTargetVolumeUl = isDispense ? 100d : null,
            DispenseMeasuredVolumeUl = null,
            DispenseCalibrationFactor = null,
            Enabled = true
        };
    }

    private static PrecisionCalibrationResponse ToResponse(PrecisionCalibrationProfile p) => new(
        p.ScopeKey, p.MoveOffsetXMm, p.MoveOffsetYMm, p.DispenseTargetVolumeUl, p.DispenseMeasuredVolumeUl,
        p.DispenseCalibrationFactor, p.Enabled, p.CreatedAtUtc, p.UpdatedAtUtc);

    private static object ToAudit(PrecisionCalibrationProfile p) => new
    {
        p.ScopeKey, p.MoveOffsetXMm, p.MoveOffsetYMm, p.DispenseTargetVolumeUl, p.DispenseMeasuredVolumeUl,
        p.DispenseCalibrationFactor, p.Enabled
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

    private static string NormalizeScopeKey(string? scopeKey)
    {
        var normalized = scopeKey?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalized))
        {
            return PrecisionCalibrationScopeKeys.Move;
        }

        return Array.Exists(AllowedScopes, x => string.Equals(x, normalized, StringComparison.OrdinalIgnoreCase))
            ? AllowedScopes.First(x => string.Equals(x, normalized, StringComparison.OrdinalIgnoreCase))
            : throw new BusinessRuleException("scope_key_invalid", $"scopeKey must be one of: {string.Join(", ", AllowedScopes)}.", StatusCodes.Status400BadRequest);
    }

    private static double? ValidateDoubleRange(double? value, string fieldName, double minimum, double maximum, bool requirePositive = false)
    {
        if (!value.HasValue)
        {
            return null;
        }

        var v = value.Value;
        if (requirePositive ? v <= 0d : v < minimum)
        {
            throw new BusinessRuleException($"{ToCode(fieldName)}_invalid", $"{fieldName} must be greater than {minimum}.", StatusCodes.Status400BadRequest);
        }

        if (v > maximum)
        {
            throw new BusinessRuleException($"{ToCode(fieldName)}_invalid", $"{fieldName} must be at most {maximum}.", StatusCodes.Status400BadRequest);
        }

        return v;
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
