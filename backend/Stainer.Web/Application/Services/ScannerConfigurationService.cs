using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class ScannerConfigurationService(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<IReadOnlyList<ScannerProfileResponse>> ListProfilesAsync(CancellationToken cancellationToken = default)
    {
        var profiles = await dbContext.ScannerProfiles
            .AsNoTracking()
            .Include(x => x.Regions)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return profiles.Select(ToProfileResponse).ToList();
    }

    public async Task<ScannerProfileResponse?> GetProfileAsync(string id, CancellationToken cancellationToken = default)
    {
        var normalizedId = NormalizeId(id);
        var profile = await dbContext.ScannerProfiles
            .AsNoTracking()
            .Include(x => x.Regions)
            .SingleOrDefaultAsync(x => x.Id == normalizedId, cancellationToken);

        return profile is null ? null : ToProfileResponse(profile);
    }

    public Task<ScannerConfigurationMutationResponse> CreateProfileAsync(
        SaveScannerProfileRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "scanner_profile.create",
            request,
            actor,
            () =>
            {
                RequireReason(request.Reason);
                var now = DateTimeOffset.UtcNow;
                var profile = new ScannerProfile
                {
                    Name = RequireValue(request.Name, "name", 128),
                    ScannerType = RequireValue(request.ScannerType, "scannerType", 64),
                    Enabled = request.Enabled,
                    Port = NormalizeOptionalValue(request.Port, "port", 128),
                    BaudRate = ValidatePositive(request.BaudRate, "baudRate", 1_000_000),
                    TimeoutMilliseconds = ValidatePositive(request.TimeoutMilliseconds, "timeoutMilliseconds", 600_000),
                    TriggerMode = RequireValue(request.TriggerMode, "triggerMode", 64),
                    CreatedAtUtc = now
                };
                ApplyDeviceParameters(profile, request.DeviceParameters);

                dbContext.ScannerProfiles.Add(profile);
                AddAudit(actor, "scanner_profile.create", "ScannerProfile", profile.Id, null, ToProfileAudit(profile), request.Reason);

                return Task.FromResult(new CommandExecutionResult<ScannerConfigurationMutationResponse>(
                    new ScannerConfigurationMutationResponse(true, request.CommandId, false, "ScannerProfile", profile.Id, "Scanner profile created."),
                    "ScannerProfile",
                    profile.Id));
            },
            cancellationToken);
    }

    public Task<ScannerConfigurationMutationResponse> UpdateProfileAsync(
        string id,
        SaveScannerProfileRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        var normalizedId = NormalizeId(id);
        return idempotencyService.RunAsync(
            request.CommandId,
            "scanner_profile.update",
            new { id = normalizedId, request },
            actor,
            async () =>
            {
                RequireReason(request.Reason);
                var profile = await dbContext.ScannerProfiles
                    .SingleOrDefaultAsync(x => x.Id == normalizedId, cancellationToken)
                    ?? throw new BusinessRuleException("scanner_profile_not_found", "Scanner profile was not found.", StatusCodes.Status404NotFound);

                var before = ToProfileAudit(profile);
                profile.Name = RequireValue(request.Name, "name", 128);
                profile.ScannerType = RequireValue(request.ScannerType, "scannerType", 64);
                profile.Enabled = request.Enabled;
                profile.Port = NormalizeOptionalValue(request.Port, "port", 128);
                profile.BaudRate = ValidatePositive(request.BaudRate, "baudRate", 1_000_000);
                profile.TimeoutMilliseconds = ValidatePositive(request.TimeoutMilliseconds, "timeoutMilliseconds", 600_000);
                profile.TriggerMode = RequireValue(request.TriggerMode, "triggerMode", 64);
                profile.UpdatedAtUtc = DateTimeOffset.UtcNow;
                ApplyDeviceParameters(profile, request.DeviceParameters);

                AddAudit(actor, "scanner_profile.update", "ScannerProfile", profile.Id, before, ToProfileAudit(profile), request.Reason);

                return new CommandExecutionResult<ScannerConfigurationMutationResponse>(
                    new ScannerConfigurationMutationResponse(true, request.CommandId, false, "ScannerProfile", profile.Id, "Scanner profile updated."),
                    "ScannerProfile",
                    profile.Id);
            },
            cancellationToken);
    }

    public async Task<IReadOnlyList<ScannerRegionResponse>> ListRegionsAsync(
        string? scannerProfileId,
        string? regionType,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.ScannerRegions.AsNoTracking();
        var normalizedProfileId = NormalizeOptionalValue(scannerProfileId, "scannerProfileId", 36);
        if (!string.IsNullOrWhiteSpace(normalizedProfileId))
        {
            query = query.Where(x => x.ScannerProfileId == normalizedProfileId);
        }

        var normalizedRegionType = NormalizeOptionalValue(regionType, "regionType", 64);
        if (!string.IsNullOrWhiteSpace(normalizedRegionType))
        {
            query = query.Where(x => x.RegionType == normalizedRegionType);
        }

        var regions = await query
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return regions.Select(ToRegionResponse).ToList();
    }

    public Task<ScannerConfigurationMutationResponse> CreateRegionAsync(
        SaveScannerRegionRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "scanner_region.create",
            request,
            actor,
            async () =>
            {
                RequireReason(request.Reason);
                var scannerProfileId = RequireValue(request.ScannerProfileId, "scannerProfileId", 36);
                var profileExists = await dbContext.ScannerProfiles
                    .AsNoTracking()
                    .AnyAsync(x => x.Id == scannerProfileId, cancellationToken);
                if (!profileExists)
                {
                    throw new BusinessRuleException("scanner_profile_not_found", "Scanner profile was not found.", StatusCodes.Status404NotFound);
                }

                var coordinateProfileId = NormalizeOptionalValue(request.CoordinateProfileId, "coordinateProfileId", 36);
                var coordinateProfileVersionId = NormalizeOptionalValue(request.CoordinateProfileVersionId, "coordinateProfileVersionId", 36);
                await ValidateCoordinateReferenceAsync(coordinateProfileId, coordinateProfileVersionId, cancellationToken);

                var now = DateTimeOffset.UtcNow;
                var region = new ScannerRegion
                {
                    Name = RequireValue(request.Name, "name", 128),
                    RegionType = RequireValue(request.RegionType, "regionType", 64),
                    ScannerProfileId = scannerProfileId,
                    ScanPathJson = NormalizeJson(request.ScanPath, "scanPath", "[]", 40000, allowArray: true),
                    CoordinateProfileId = coordinateProfileId,
                    CoordinateProfileVersionId = coordinateProfileVersionId,
                    CoordinatePointCodesJson = JsonSerializer.Serialize(NormalizeCoordinatePointCodes(request.CoordinatePointCodes), JsonOptions),
                    CreatedAtUtc = now
                };

                dbContext.ScannerRegions.Add(region);
                AddAudit(actor, "scanner_region.create", "ScannerRegion", region.Id, null, ToRegionAudit(region), request.Reason);

                return new CommandExecutionResult<ScannerConfigurationMutationResponse>(
                    new ScannerConfigurationMutationResponse(true, request.CommandId, false, "ScannerRegion", region.Id, "Scanner region created."),
                    "ScannerRegion",
                    region.Id);
            },
            cancellationToken);
    }

    private async Task ValidateCoordinateReferenceAsync(
        string? coordinateProfileId,
        string? coordinateProfileVersionId,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(coordinateProfileId))
        {
            var profileExists = await dbContext.CoordinateProfiles
                .AsNoTracking()
                .AnyAsync(x => x.Id == coordinateProfileId, cancellationToken);
            if (!profileExists)
            {
                throw new BusinessRuleException("coordinate_profile_not_found", "Coordinate profile was not found.", StatusCodes.Status404NotFound);
            }
        }

        if (string.IsNullOrWhiteSpace(coordinateProfileVersionId))
        {
            return;
        }

        var version = await dbContext.CoordinateProfileVersions
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == coordinateProfileVersionId, cancellationToken)
            ?? throw new BusinessRuleException("coordinate_profile_version_not_found", "Coordinate profile version was not found.", StatusCodes.Status404NotFound);
        if (!string.IsNullOrWhiteSpace(coordinateProfileId)
            && version.CoordinateProfileId != coordinateProfileId)
        {
            throw new BusinessRuleException("coordinate_reference_mismatch", "Coordinate profile version does not belong to the selected coordinate profile.", StatusCodes.Status400BadRequest);
        }
    }

    private static void ApplyDeviceParameters(ScannerProfile profile, ScannerDeviceParametersRequest? parameters)
    {
        ValidateRoi(parameters);
        profile.RoiX = parameters?.RoiX;
        profile.RoiY = parameters?.RoiY;
        profile.RoiWidth = parameters?.RoiWidth;
        profile.RoiHeight = parameters?.RoiHeight;
        profile.CheckLightEnabled = parameters?.CheckLightEnabled;
        profile.SpecialParametersJson = NormalizeJson(parameters?.SpecialParameters, "specialParameters", "{}", 40000, allowArray: false);
    }

    private static void ValidateRoi(ScannerDeviceParametersRequest? parameters)
    {
        if (parameters is null)
        {
            return;
        }

        var values = new[] { parameters.RoiX, parameters.RoiY, parameters.RoiWidth, parameters.RoiHeight };
        var hasAny = values.Any(x => x.HasValue);
        if (!hasAny)
        {
            return;
        }

        if (values.Any(x => !x.HasValue))
        {
            throw new BusinessRuleException("scanner_roi_incomplete", "ROI requires roiX, roiY, roiWidth and roiHeight.", StatusCodes.Status400BadRequest);
        }

        if (parameters.RoiX < 0 || parameters.RoiY < 0 || parameters.RoiWidth <= 0 || parameters.RoiHeight <= 0)
        {
            throw new BusinessRuleException("scanner_roi_invalid", "ROI position must be non-negative and ROI size must be positive.", StatusCodes.Status400BadRequest);
        }
    }

    private static string NormalizeJson(JsonElement? value, string fieldName, string defaultJson, int maxLength, bool allowArray)
    {
        if (!value.HasValue || value.Value.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            return defaultJson;
        }

        var kind = value.Value.ValueKind;
        var valid = kind == JsonValueKind.Object || (allowArray && kind == JsonValueKind.Array);
        if (!valid)
        {
            var expected = allowArray ? "a JSON object or array" : "a JSON object";
            throw new BusinessRuleException($"{ToCode(fieldName)}_invalid", $"{fieldName} must be {expected}.", StatusCodes.Status400BadRequest);
        }

        var json = value.Value.GetRawText();
        if (json.Length > maxLength)
        {
            throw new BusinessRuleException($"{ToCode(fieldName)}_too_large", $"{fieldName} is too large.", StatusCodes.Status400BadRequest);
        }

        return json;
    }

    private static IReadOnlyList<string> NormalizeCoordinatePointCodes(IReadOnlyList<string>? pointCodes)
    {
        if (pointCodes is null || pointCodes.Count == 0)
        {
            return [];
        }

        return pointCodes
            .Select(x => RequireValue(x, "coordinatePointCode", 128))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static IReadOnlyList<string> ReadCoordinatePointCodes(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<IReadOnlyList<string>>(json, JsonOptions) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
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

    private static ScannerProfileResponse ToProfileResponse(ScannerProfile profile)
    {
        return new ScannerProfileResponse(
            profile.Id,
            profile.Name,
            profile.ScannerType,
            profile.Enabled,
            profile.Port,
            profile.BaudRate,
            profile.TimeoutMilliseconds,
            profile.TriggerMode,
            new ScannerDeviceParametersResponse(
                profile.RoiX,
                profile.RoiY,
                profile.RoiWidth,
                profile.RoiHeight,
                profile.CheckLightEnabled,
                profile.SpecialParametersJson),
            profile.Regions.OrderBy(x => x.Name).Select(ToRegionResponse).ToList(),
            profile.CreatedAtUtc,
            profile.UpdatedAtUtc);
    }

    private static ScannerRegionResponse ToRegionResponse(ScannerRegion region)
    {
        return new ScannerRegionResponse(
            region.Id,
            region.Name,
            region.RegionType,
            region.ScannerProfileId,
            region.ScanPathJson,
            region.CoordinateProfileId,
            region.CoordinateProfileVersionId,
            ReadCoordinatePointCodes(region.CoordinatePointCodesJson),
            region.CreatedAtUtc,
            region.UpdatedAtUtc);
    }

    private static object ToProfileAudit(ScannerProfile profile)
    {
        return new
        {
            profile.Name,
            profile.ScannerType,
            profile.Enabled,
            profile.Port,
            profile.BaudRate,
            profile.TimeoutMilliseconds,
            profile.TriggerMode,
            profile.RoiX,
            profile.RoiY,
            profile.RoiWidth,
            profile.RoiHeight,
            profile.CheckLightEnabled,
            profile.SpecialParametersJson
        };
    }

    private static object ToRegionAudit(ScannerRegion region)
    {
        return new
        {
            region.Name,
            region.RegionType,
            region.ScannerProfileId,
            region.ScanPathJson,
            region.CoordinateProfileId,
            region.CoordinateProfileVersionId,
            region.CoordinatePointCodesJson
        };
    }

    private static int? ValidatePositive(int? value, string fieldName, int maximum)
    {
        if (!value.HasValue)
        {
            return null;
        }

        if (value <= 0 || value > maximum)
        {
            throw new BusinessRuleException($"{ToCode(fieldName)}_invalid", $"{fieldName} must be between 1 and {maximum}.", StatusCodes.Status400BadRequest);
        }

        return value.Value;
    }

    private static string RequireValue(string? value, string fieldName, int maxLength)
    {
        var normalized = NormalizeOptionalValue(value, fieldName, maxLength);
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BusinessRuleException($"{ToCode(fieldName)}_required", $"{fieldName} is required.", StatusCodes.Status400BadRequest);
        }

        return normalized;
    }

    private static string? NormalizeOptionalValue(string? value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value.Trim();
        if (normalized.Length > maxLength)
        {
            throw new BusinessRuleException($"{ToCode(fieldName)}_too_long", $"{fieldName} must be at most {maxLength} characters.", StatusCodes.Status400BadRequest);
        }

        return normalized;
    }

    private static string NormalizeId(string id)
    {
        return RequireValue(id, "id", 36);
    }

    private static void RequireReason(string? reason)
    {
        _ = RequireValue(reason, "reason", 2000);
    }

    private static string ToCode(string fieldName)
    {
        return string.Concat(fieldName.Select((ch, index) =>
            char.IsUpper(ch) && index > 0 ? $"_{char.ToLowerInvariant(ch)}" : char.ToLowerInvariant(ch).ToString()));
    }
}
