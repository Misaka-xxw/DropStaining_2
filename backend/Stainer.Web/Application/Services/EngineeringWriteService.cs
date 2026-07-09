using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class EngineeringWriteService(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService,
    CoordinateProfileLifecycleService coordinateProfileLifecycleService)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task<EngineeringWriteResponse> CalibrateCoordinatePointAsync(
        CalibrateCoordinatePointRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return coordinateProfileLifecycleService.CalibratePointAsNewVersionAsync(request, actor, cancellationToken);
    }

    public Task<LiquidClassVersionMutationResponse> SaveLiquidClassAsync(
        SaveLiquidClassRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "engineering.liquid_class.save",
            request,
            actor,
            async () =>
            {
                RequireReason(request.Reason);
                var code = RequireValue(request.Code, "code");
                var liquidClass = await dbContext.LiquidClassProfiles
                    .Include(x => x.Versions)
                    .SingleOrDefaultAsync(x => x.Code == code, cancellationToken);
                if (liquidClass is null)
                {
                    liquidClass = new LiquidClassProfile
                    {
                        Code = code,
                        CreatedAtUtc = DateTimeOffset.UtcNow
                    };
                    dbContext.LiquidClassProfiles.Add(liquidClass);
                }

                LiquidClassVersion? source = null;
                var sourceVersionId = string.IsNullOrWhiteSpace(request.SourceVersionId)
                    ? liquidClass.EnabledVersionId
                    : request.SourceVersionId.Trim();
                if (!string.IsNullOrWhiteSpace(sourceVersionId))
                {
                    source = liquidClass.Versions.SingleOrDefault(x => x.Id == sourceVersionId)
                        ?? throw new BusinessRuleException("liquid_class_source_not_found", "Source Liquid Class version was not found for this profile.", StatusCodes.Status404NotFound);
                }

                var now = DateTimeOffset.UtcNow;
                var version = new LiquidClassVersion
                {
                    LiquidClassProfile = liquidClass,
                    LiquidClassProfileId = liquidClass.Id,
                    VersionNo = liquidClass.Versions.Count == 0 ? 1 : liquidClass.Versions.Max(x => x.VersionNo) + 1,
                    Name = RequireValue(request.Name, "name"),
                    Status = LiquidClassVersionStatus.Draft,
                    SourceVersionId = source?.Id,
                    ChangeReason = request.Reason.Trim(),
                    LiquidDetectionEnabled = request.LiquidDetectionEnabled ?? source?.LiquidDetectionEnabled ?? false,
                    LiquidDetectionSensitivityPercent = request.LiquidDetectionSensitivityPercent ?? source?.LiquidDetectionSensitivityPercent ?? 50,
                    LiquidDetectionSpeedUmPerSecond = request.LiquidDetectionSpeedUmPerSecond ?? source?.LiquidDetectionSpeedUmPerSecond ?? 1_000,
                    AspirateSpeedUlPerSecond = request.AspirateSpeedUlPerSecond ?? source?.AspirateSpeedUlPerSecond ?? liquidClass.AspirateSpeedUlPerSecond ?? 100,
                    AspirateDelayMs = request.AspirateDelayMs ?? source?.AspirateDelayMs ?? 0,
                    DispenseSpeedUlPerSecond = request.DispenseSpeedUlPerSecond ?? source?.DispenseSpeedUlPerSecond ?? liquidClass.DispenseSpeedUlPerSecond ?? 100,
                    DispenseDelayMs = request.DispenseDelayMs ?? source?.DispenseDelayMs ?? 0,
                    LeadingAirGapUl = request.LeadingAirGapUl ?? source?.LeadingAirGapUl ?? liquidClass.LeadingAirGapUl ?? 0,
                    TrailingAirGapUl = request.TrailingAirGapUl ?? source?.TrailingAirGapUl ?? liquidClass.TrailingAirGapUl ?? 0,
                    BlowoutVolumeUl = request.BlowoutVolumeUl ?? source?.BlowoutVolumeUl ?? 0,
                    BlowoutDelayMs = request.BlowoutDelayMs ?? source?.BlowoutDelayMs ?? 0,
                    VolumeAdjustmentUl = request.VolumeAdjustmentUl ?? request.ExcessVolumeUl ?? source?.VolumeAdjustmentUl ?? liquidClass.ExcessVolumeUl ?? 0,
                    PreWetCycles = request.PreWetCycles ?? source?.PreWetCycles ?? liquidClass.PreWetCycles ?? 0,
                    MixCycles = request.MixCycles ?? source?.MixCycles ?? liquidClass.MixCycles ?? 0,
                    LiquidFollowingDepthUm = request.LiquidFollowingDepthUm ?? source?.LiquidFollowingDepthUm ?? 0,
                    RetractSpeedUmPerSecond = request.RetractSpeedUmPerSecond ?? source?.RetractSpeedUmPerSecond ?? 0,
                    ConditioningVolumeUl = request.ConditioningVolumeUl ?? source?.ConditioningVolumeUl ?? 0,
                    BreakoffSpeedUlPerSecond = request.BreakoffSpeedUlPerSecond ?? source?.BreakoffSpeedUlPerSecond ?? 0,
                    PostDispenseAirGapUl = request.PostDispenseAirGapUl ?? source?.PostDispenseAirGapUl ?? 0,
                    CreatedByUserId = actor.UserId,
                    CreatedAtUtc = now
                };
                version.VersionLabel = string.IsNullOrWhiteSpace(request.VersionLabel) ? version.VersionNo.ToString() : request.VersionLabel.Trim();
                var validation = ValidateLiquidClass(version);
                version.ValidationRecords.Add(CreateValidation(version, LiquidClassValidationStage.Draft, validation, actor, now));
                AddDifferences(version, source, now);
                version.ChangeSummaryJson = JsonSerializer.Serialize(version.Differences.Select(x => new
                {
                    x.ParameterName,
                    x.PreviousValue,
                    x.NewValue,
                    x.Unit
                }), JsonOptions);
                liquidClass.Name = version.Name;
                liquidClass.UpdatedAtUtc = DateTimeOffset.UtcNow;
                liquidClass.Versions.Add(version);

                AddAudit(actor, "engineering.liquid_class.version.create", "LiquidClassVersion", version.Id, source is null ? null : ToVersionAudit(source), ToVersionAudit(version), request.Reason);
                return new CommandExecutionResult<LiquidClassVersionMutationResponse>(
                    ToMutation(request.CommandId, liquidClass, version, "Liquid Class draft created."),
                    "LiquidClassVersion",
                    version.Id);
            },
            cancellationToken);
    }

    public Task<LiquidClassVersionMutationResponse> PublishLiquidClassVersionAsync(
        string versionId,
        PublishLiquidClassVersionRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return ChangeLiquidClassStateAsync(versionId, request.CommandId, request.Reason, actor, LiquidClassVersionStatus.Published, cancellationToken);
    }

    public Task<LiquidClassVersionMutationResponse> EnableLiquidClassVersionAsync(
        string versionId,
        EnableLiquidClassVersionRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return ChangeLiquidClassStateAsync(versionId, request.CommandId, request.Reason, actor, LiquidClassVersionStatus.Enabled, cancellationToken);
    }

    private Task<LiquidClassVersionMutationResponse> ChangeLiquidClassStateAsync(
        string versionId,
        string commandId,
        string reason,
        AuthenticatedUser actor,
        string targetStatus,
        CancellationToken cancellationToken)
    {
        var operation = targetStatus == LiquidClassVersionStatus.Published
            ? "engineering.liquid_class.version.publish"
            : "engineering.liquid_class.version.enable";
        return idempotencyService.RunAsync(
            commandId,
            operation,
            new { versionId, reason },
            actor,
            async () =>
            {
                RequireReason(reason);
                var normalizedId = RequireValue(versionId, "versionId");
                var version = await dbContext.LiquidClassVersions
                    .Include(x => x.LiquidClassProfile)
                    .ThenInclude(x => x!.Versions)
                    .Include(x => x.ValidationRecords)
                    .SingleOrDefaultAsync(x => x.Id == normalizedId, cancellationToken)
                    ?? throw new BusinessRuleException("liquid_class_version_not_found", "Liquid Class version was not found.", StatusCodes.Status404NotFound);
                var profile = version.LiquidClassProfile!;
                var now = DateTimeOffset.UtcNow;
                var validation = ValidateLiquidClass(version);

                if (targetStatus == LiquidClassVersionStatus.Published)
                {
                    if (version.Status != LiquidClassVersionStatus.Draft)
                    {
                        throw new BusinessRuleException("liquid_class_version_not_draft", "Only a Draft Liquid Class version can be published.", StatusCodes.Status409Conflict);
                    }

                    version.Status = LiquidClassVersionStatus.Published;
                    version.PublishedAtUtc = now;
                    version.PublishedByUserId = actor.UserId;
                    version.ValidationRecords.Add(CreateValidation(version, LiquidClassValidationStage.Publish, validation, actor, now));
                }
                else
                {
                    if (version.Status != LiquidClassVersionStatus.Published)
                    {
                        throw new BusinessRuleException("liquid_class_version_not_published", "Only a Published Liquid Class version can be enabled.", StatusCodes.Status409Conflict);
                    }

                    var previouslyEnabled = profile.Versions.Where(x => x.Status == LiquidClassVersionStatus.Enabled && x.Id != version.Id).ToList();
                    foreach (var old in previouslyEnabled)
                    {
                        old.Status = LiquidClassVersionStatus.Published;
                    }
                    if (previouslyEnabled.Count > 0)
                    {
                        await dbContext.SaveChangesAsync(cancellationToken);
                    }

                    version.Status = LiquidClassVersionStatus.Enabled;
                    version.EnabledAtUtc = now;
                    version.EnabledByUserId = actor.UserId;
                    version.ValidationRecords.Add(CreateValidation(version, LiquidClassValidationStage.Enable, validation, actor, now));
                    profile.EnabledVersionId = version.Id;
                    profile.IsEnabled = true;
                    profile.Name = version.Name;
                    profile.AspirateSpeedUlPerSecond = version.AspirateSpeedUlPerSecond;
                    profile.DispenseSpeedUlPerSecond = version.DispenseSpeedUlPerSecond;
                    profile.LeadingAirGapUl = version.LeadingAirGapUl;
                    profile.TrailingAirGapUl = version.TrailingAirGapUl;
                    profile.ExcessVolumeUl = version.VolumeAdjustmentUl;
                    profile.PreWetCycles = version.PreWetCycles;
                    profile.MixCycles = version.MixCycles;
                    profile.UpdatedAtUtc = now;
                }

                AddAudit(actor, operation, "LiquidClassVersion", version.Id, null, ToVersionAudit(version), reason);
                return new CommandExecutionResult<LiquidClassVersionMutationResponse>(
                    ToMutation(commandId, profile, version, targetStatus == LiquidClassVersionStatus.Published ? "Liquid Class version published." : "Liquid Class version enabled."),
                    "LiquidClassVersion",
                    version.Id);
            },
            cancellationToken);
    }

    public Task<EngineeringWriteResponse> SaveDeviceProfileAsync(
        SaveDeviceProfileRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "engineering.device_profile.save",
            request,
            actor,
            async () =>
            {
                RequireReason(request.Reason);
                var code = RequireValue(request.Code, "code");
                var profile = await dbContext.DeviceProfiles.SingleOrDefaultAsync(x => x.Code == code, cancellationToken);
                object? before = null;
                if (profile is null)
                {
                    profile = new DeviceProfile
                    {
                        Code = code,
                        CreatedAtUtc = DateTimeOffset.UtcNow
                    };
                    dbContext.DeviceProfiles.Add(profile);
                }
                else
                {
                    before = new { profile.Code, profile.Name, profile.IsActive };
                }

                if (request.IsActive)
                {
                    var activeProfiles = await dbContext.DeviceProfiles.Where(x => x.IsActive && x.Code != code).ToListAsync(cancellationToken);
                    foreach (var activeProfile in activeProfiles)
                    {
                        activeProfile.IsActive = false;
                    }
                }

                profile.Name = RequireValue(request.Name, "name");
                profile.IsActive = request.IsActive;

                AddAudit(actor, "engineering.device_profile.save", "DeviceProfile", profile.Id, before, new
                {
                    profile.Code,
                    profile.Name,
                    profile.IsActive
                }, request.Reason);
                return new CommandExecutionResult<EngineeringWriteResponse>(
                    new EngineeringWriteResponse(true, request.CommandId, false, profile.Id, "Device profile saved."),
                    "DeviceProfile",
                    profile.Id);
            },
            cancellationToken);
    }

    private void AddAudit(AuthenticatedUser actor, string action, string entityType, string entityId, object? before, object after, string reason)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            ActorUserId = actor.UserId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Message = JsonSerializer.Serialize(new { before, after, reason = reason.Trim(), actor = actor.Username }, JsonOptions),
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
    }

    private static object ToLiquidClassAudit(LiquidClassProfile liquidClass)
    {
        return new
        {
            liquidClass.Code,
            liquidClass.Name,
            liquidClass.AspirateSpeedUlPerSecond,
            liquidClass.DispenseSpeedUlPerSecond,
            liquidClass.LeadingAirGapUl,
            liquidClass.TrailingAirGapUl,
            liquidClass.ExcessVolumeUl,
            liquidClass.PreWetCycles,
            liquidClass.MixCycles,
            liquidClass.IsEnabled
        };
    }

    private static LiquidClassVersionMutationResponse ToMutation(
        string commandId,
        LiquidClassProfile profile,
        LiquidClassVersion version,
        string message)
    {
        return new LiquidClassVersionMutationResponse(
            true,
            commandId,
            false,
            profile.Id,
            version.Id,
            version.VersionNo,
            version.VersionLabel,
            version.Status,
            version.Status == LiquidClassVersionStatus.Enabled && profile.EnabledVersionId == version.Id,
            message);
    }

    private static object ToVersionAudit(LiquidClassVersion version)
    {
        return new
        {
            version.LiquidClassProfileId,
            version.VersionNo,
            version.VersionLabel,
            version.Name,
            version.Status,
            version.SourceVersionId,
            version.LiquidDetectionEnabled,
            version.LiquidDetectionSensitivityPercent,
            version.LiquidDetectionSpeedUmPerSecond,
            version.AspirateSpeedUlPerSecond,
            version.AspirateDelayMs,
            version.DispenseSpeedUlPerSecond,
            version.DispenseDelayMs,
            version.LeadingAirGapUl,
            version.TrailingAirGapUl,
            version.BlowoutVolumeUl,
            version.BlowoutDelayMs,
            version.VolumeAdjustmentUl,
            version.PreWetCycles,
            version.MixCycles,
            version.LiquidFollowingDepthUm,
            version.RetractSpeedUmPerSecond,
            version.ConditioningVolumeUl,
            version.BreakoffSpeedUlPerSecond,
            version.PostDispenseAirGapUl
        };
    }

    private static IReadOnlyList<string> ValidateLiquidClass(LiquidClassVersion version)
    {
        var errors = new List<string>();
        CheckRange(errors, nameof(version.LiquidDetectionSensitivityPercent), version.LiquidDetectionSensitivityPercent, version.LiquidDetectionEnabled ? 1 : 0, 100, "%");
        CheckRange(errors, nameof(version.LiquidDetectionSpeedUmPerSecond), version.LiquidDetectionSpeedUmPerSecond, 1, 100_000, "um/s");
        CheckRange(errors, nameof(version.AspirateSpeedUlPerSecond), version.AspirateSpeedUlPerSecond, 1, 10_000, "uL/s");
        CheckRange(errors, nameof(version.AspirateDelayMs), version.AspirateDelayMs, 0, 60_000, "ms");
        CheckRange(errors, nameof(version.DispenseSpeedUlPerSecond), version.DispenseSpeedUlPerSecond, 1, 10_000, "uL/s");
        CheckRange(errors, nameof(version.DispenseDelayMs), version.DispenseDelayMs, 0, 60_000, "ms");
        CheckRange(errors, nameof(version.LeadingAirGapUl), version.LeadingAirGapUl, 0, 1_000, "uL");
        CheckRange(errors, nameof(version.TrailingAirGapUl), version.TrailingAirGapUl, 0, 1_000, "uL");
        CheckRange(errors, nameof(version.BlowoutVolumeUl), version.BlowoutVolumeUl, 0, 1_000, "uL");
        CheckRange(errors, nameof(version.BlowoutDelayMs), version.BlowoutDelayMs, 0, 60_000, "ms");
        CheckRange(errors, nameof(version.VolumeAdjustmentUl), version.VolumeAdjustmentUl, -1_000, 1_000, "uL");
        CheckRange(errors, nameof(version.PreWetCycles), version.PreWetCycles, 0, 20, "cycles");
        CheckRange(errors, nameof(version.MixCycles), version.MixCycles, 0, 20, "cycles");
        CheckRange(errors, nameof(version.LiquidFollowingDepthUm), version.LiquidFollowingDepthUm, 0, 50_000, "um");
        CheckRange(errors, nameof(version.RetractSpeedUmPerSecond), version.RetractSpeedUmPerSecond, 0, 100_000, "um/s");
        CheckRange(errors, nameof(version.ConditioningVolumeUl), version.ConditioningVolumeUl, 0, 1_000, "uL");
        CheckRange(errors, nameof(version.BreakoffSpeedUlPerSecond), version.BreakoffSpeedUlPerSecond, 0, 10_000, "uL/s");
        CheckRange(errors, nameof(version.PostDispenseAirGapUl), version.PostDispenseAirGapUl, 0, 1_000, "uL");
        if (errors.Count > 0)
        {
            throw new BusinessRuleException("liquid_class_validation_failed", string.Join(" ", errors), StatusCodes.Status400BadRequest);
        }

        return errors;
    }

    private static void CheckRange(List<string> errors, string name, int value, int minimum, int maximum, string unit)
    {
        if (value < minimum || value > maximum)
        {
            errors.Add($"{name} must be between {minimum} and {maximum} {unit}.");
        }
    }

    private static LiquidClassValidationRecord CreateValidation(
        LiquidClassVersion version,
        string stage,
        IReadOnlyList<string> errors,
        AuthenticatedUser actor,
        DateTimeOffset now)
    {
        return new LiquidClassValidationRecord
        {
            LiquidClassVersion = version,
            LiquidClassVersionId = version.Id,
            Stage = stage,
            IsValid = errors.Count == 0,
            ResultJson = JsonSerializer.Serialize(new { valid = errors.Count == 0, errors, units = "uL,uL/s,um/s,ms,%" }, JsonOptions),
            ValidatedByUserId = actor.UserId,
            CreatedAtUtc = now
        };
    }

    private static void AddDifferences(LiquidClassVersion version, LiquidClassVersion? source, DateTimeOffset now)
    {
        var before = source is null ? new Dictionary<string, (string? Value, string Unit)>() : ParameterValues(source);
        foreach (var pair in ParameterValues(version))
        {
            var previous = before.GetValueOrDefault(pair.Key);
            if (source is not null && previous.Value == pair.Value.Value)
            {
                continue;
            }

            version.Differences.Add(new LiquidClassVersionDifference
            {
                LiquidClassVersion = version,
                LiquidClassVersionId = version.Id,
                ParameterName = pair.Key,
                PreviousValue = previous.Value,
                NewValue = pair.Value.Value,
                Unit = pair.Value.Unit,
                CreatedAtUtc = now
            });
        }
    }

    private static Dictionary<string, (string? Value, string Unit)> ParameterValues(LiquidClassVersion version)
    {
        return new(StringComparer.Ordinal)
        {
            [nameof(version.Name)] = (version.Name, string.Empty),
            [nameof(version.LiquidDetectionEnabled)] = (version.LiquidDetectionEnabled.ToString(), "boolean"),
            [nameof(version.LiquidDetectionSensitivityPercent)] = (version.LiquidDetectionSensitivityPercent.ToString(), "%"),
            [nameof(version.LiquidDetectionSpeedUmPerSecond)] = (version.LiquidDetectionSpeedUmPerSecond.ToString(), "um/s"),
            [nameof(version.AspirateSpeedUlPerSecond)] = (version.AspirateSpeedUlPerSecond.ToString(), "uL/s"),
            [nameof(version.AspirateDelayMs)] = (version.AspirateDelayMs.ToString(), "ms"),
            [nameof(version.DispenseSpeedUlPerSecond)] = (version.DispenseSpeedUlPerSecond.ToString(), "uL/s"),
            [nameof(version.DispenseDelayMs)] = (version.DispenseDelayMs.ToString(), "ms"),
            [nameof(version.LeadingAirGapUl)] = (version.LeadingAirGapUl.ToString(), "uL"),
            [nameof(version.TrailingAirGapUl)] = (version.TrailingAirGapUl.ToString(), "uL"),
            [nameof(version.BlowoutVolumeUl)] = (version.BlowoutVolumeUl.ToString(), "uL"),
            [nameof(version.BlowoutDelayMs)] = (version.BlowoutDelayMs.ToString(), "ms"),
            [nameof(version.VolumeAdjustmentUl)] = (version.VolumeAdjustmentUl.ToString(), "uL"),
            [nameof(version.PreWetCycles)] = (version.PreWetCycles.ToString(), "cycles"),
            [nameof(version.MixCycles)] = (version.MixCycles.ToString(), "cycles"),
            [nameof(version.LiquidFollowingDepthUm)] = (version.LiquidFollowingDepthUm.ToString(), "um"),
            [nameof(version.RetractSpeedUmPerSecond)] = (version.RetractSpeedUmPerSecond.ToString(), "um/s"),
            [nameof(version.ConditioningVolumeUl)] = (version.ConditioningVolumeUl.ToString(), "uL"),
            [nameof(version.BreakoffSpeedUlPerSecond)] = (version.BreakoffSpeedUlPerSecond.ToString(), "uL/s"),
            [nameof(version.PostDispenseAirGapUl)] = (version.PostDispenseAirGapUl.ToString(), "uL")
        };
    }

    private static void RequireReason(string reason)
    {
        _ = RequireValue(reason, "reason");
    }

    private static string RequireValue(string value, string fieldName)
    {
        var normalized = value?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BusinessRuleException($"{fieldName}_required", $"{fieldName} is required.");
        }

        return normalized;
    }
}
