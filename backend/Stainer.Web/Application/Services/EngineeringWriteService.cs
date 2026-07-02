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

    public Task<EngineeringWriteResponse> SaveLiquidClassAsync(
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
                var liquidClass = await dbContext.LiquidClassProfiles.SingleOrDefaultAsync(x => x.Code == code, cancellationToken);
                object? before = null;
                if (liquidClass is null)
                {
                    liquidClass = new LiquidClassProfile
                    {
                        Code = code,
                        CreatedAtUtc = DateTimeOffset.UtcNow
                    };
                    dbContext.LiquidClassProfiles.Add(liquidClass);
                }
                else
                {
                    before = ToLiquidClassAudit(liquidClass);
                }

                liquidClass.Name = RequireValue(request.Name, "name");
                liquidClass.AspirateSpeedUlPerSecond = request.AspirateSpeedUlPerSecond;
                liquidClass.DispenseSpeedUlPerSecond = request.DispenseSpeedUlPerSecond;
                liquidClass.LeadingAirGapUl = request.LeadingAirGapUl;
                liquidClass.TrailingAirGapUl = request.TrailingAirGapUl;
                liquidClass.ExcessVolumeUl = request.ExcessVolumeUl;
                liquidClass.PreWetCycles = request.PreWetCycles;
                liquidClass.MixCycles = request.MixCycles;
                liquidClass.IsEnabled = request.IsEnabled;
                liquidClass.UpdatedAtUtc = DateTimeOffset.UtcNow;

                AddAudit(actor, "engineering.liquid_class.save", "LiquidClassProfile", liquidClass.Id, before, ToLiquidClassAudit(liquidClass), request.Reason);
                return new CommandExecutionResult<EngineeringWriteResponse>(
                    new EngineeringWriteResponse(true, request.CommandId, false, liquidClass.Id, "Liquid class saved."),
                    "LiquidClassProfile",
                    liquidClass.Id);
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
