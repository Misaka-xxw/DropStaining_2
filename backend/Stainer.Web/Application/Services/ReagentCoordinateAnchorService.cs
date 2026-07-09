using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

/// <summary>
/// 试剂区坐标锚点配置服务：负责单个试剂列首尾锚点的查询、创建与修改。
/// 复用已有 CoordinateProfile / CoordinateProfileVersion 版本体系，保证可追踪、可回滚。
/// 不执行任何机械臂运动，仅生成/保存配置数据。
/// </summary>
public sealed class ReagentCoordinateAnchorService(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<IReadOnlyList<ReagentCoordinateAnchorResponse>> ListAnchorsAsync(
        string? coordinateProfileId,
        string? coordinateProfileVersionId,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.ReagentCoordinateAnchors.AsNoTracking();

        var normalizedProfileId = NormalizeOptionalValue(coordinateProfileId, "coordinateProfileId", 36);
        if (!string.IsNullOrWhiteSpace(normalizedProfileId))
        {
            query = query.Where(x => x.CoordinateProfileId == normalizedProfileId);
        }

        var normalizedVersionId = NormalizeOptionalValue(coordinateProfileVersionId, "coordinateProfileVersionId", 36);
        if (!string.IsNullOrWhiteSpace(normalizedVersionId))
        {
            query = query.Where(x => x.CoordinateProfileVersionId == normalizedVersionId);
        }

        var anchors = await query
            .OrderBy(x => x.ColumnNo)
            .ToListAsync(cancellationToken);

        return anchors.Select(ToAnchorResponse).ToList();
    }

    public async Task<ReagentCoordinateAnchorResponse?> GetAnchorAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var normalizedId = RequireValue(id, "id", 36);
        var anchor = await dbContext.ReagentCoordinateAnchors
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == normalizedId, cancellationToken);

        return anchor is null ? null : ToAnchorResponse(anchor);
    }

    public Task<ScannerConfigurationMutationResponse> CreateAnchorAsync(
        SaveReagentCoordinateAnchorRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "reagent_coordinate_anchor.create",
            request,
            actor,
            async () =>
            {
                RequireReason(request.Reason);
                var coordinateProfileId = RequireValue(request.CoordinateProfileId, "coordinateProfileId", 36);
                await EnsureCoordinateProfileExistsAsync(coordinateProfileId, cancellationToken);

                var coordinateProfileVersionId = NormalizeOptionalValue(request.CoordinateProfileVersionId, "coordinateProfileVersionId", 36);
                await EnsureCoordinateVersionBelongsToProfileAsync(coordinateProfileId, coordinateProfileVersionId, cancellationToken);

                var columnNo = ValidateColumnNo(request.ColumnNo);
                await EnsureColumnNoUniqueAsync(coordinateProfileId, coordinateProfileVersionId, columnNo, null, cancellationToken);

                ValidateAnchorCoordinates(request);

                var now = DateTimeOffset.UtcNow;
                var anchor = new ReagentCoordinateAnchor
                {
                    CoordinateProfileId = coordinateProfileId,
                    CoordinateProfileVersionId = coordinateProfileVersionId,
                    ColumnNo = columnNo,
                    ColumnCode = NormalizeOptionalValue(request.ColumnCode, "columnCode", 32),
                    SlotCount = ValidateSlotCount(request.SlotCount),
                    StartXUm = request.StartXUm,
                    StartYUm = request.StartYUm,
                    StartZUm = request.StartZUm,
                    EndXUm = request.EndXUm,
                    EndYUm = request.EndYUm,
                    EndZUm = request.EndZUm,
                    IsEnabled = request.IsEnabled,
                    CreatedAtUtc = now
                };

                dbContext.ReagentCoordinateAnchors.Add(anchor);
                AddAudit(actor, "reagent_coordinate_anchor.create", "ReagentCoordinateAnchor", anchor.Id, null, ToAnchorAudit(anchor), request.Reason);

                return new CommandExecutionResult<ScannerConfigurationMutationResponse>(
                    new ScannerConfigurationMutationResponse(true, request.CommandId, false, "ReagentCoordinateAnchor", anchor.Id, "Reagent coordinate anchor created."),
                    "ReagentCoordinateAnchor",
                    anchor.Id);
            },
            cancellationToken);
    }

    public Task<ScannerConfigurationMutationResponse> UpdateAnchorAsync(
        string id,
        SaveReagentCoordinateAnchorRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        var normalizedId = RequireValue(id, "id", 36);
        return idempotencyService.RunAsync(
            request.CommandId,
            "reagent_coordinate_anchor.update",
            new { id = normalizedId, request },
            actor,
            async () =>
            {
                RequireReason(request.Reason);
                var coordinateProfileId = RequireValue(request.CoordinateProfileId, "coordinateProfileId", 36);
                await EnsureCoordinateProfileExistsAsync(coordinateProfileId, cancellationToken);

                var coordinateProfileVersionId = NormalizeOptionalValue(request.CoordinateProfileVersionId, "coordinateProfileVersionId", 36);
                await EnsureCoordinateVersionBelongsToProfileAsync(coordinateProfileId, coordinateProfileVersionId, cancellationToken);

                var anchor = await dbContext.ReagentCoordinateAnchors
                    .SingleOrDefaultAsync(x => x.Id == normalizedId, cancellationToken)
                    ?? throw new BusinessRuleException("reagent_coordinate_anchor_not_found", "Reagent coordinate anchor was not found.", StatusCodes.Status404NotFound);

                var columnNo = ValidateColumnNo(request.ColumnNo);
                await EnsureColumnNoUniqueAsync(coordinateProfileId, coordinateProfileVersionId, columnNo, anchor.Id, cancellationToken);

                ValidateAnchorCoordinates(request);

                var before = ToAnchorAudit(anchor);
                anchor.CoordinateProfileId = coordinateProfileId;
                anchor.CoordinateProfileVersionId = coordinateProfileVersionId;
                anchor.ColumnNo = columnNo;
                anchor.ColumnCode = NormalizeOptionalValue(request.ColumnCode, "columnCode", 32);
                anchor.SlotCount = ValidateSlotCount(request.SlotCount);
                anchor.StartXUm = request.StartXUm;
                anchor.StartYUm = request.StartYUm;
                anchor.StartZUm = request.StartZUm;
                anchor.EndXUm = request.EndXUm;
                anchor.EndYUm = request.EndYUm;
                anchor.EndZUm = request.EndZUm;
                anchor.IsEnabled = request.IsEnabled;
                anchor.UpdatedAtUtc = DateTimeOffset.UtcNow;

                AddAudit(actor, "reagent_coordinate_anchor.update", "ReagentCoordinateAnchor", anchor.Id, before, ToAnchorAudit(anchor), request.Reason);

                return new CommandExecutionResult<ScannerConfigurationMutationResponse>(
                    new ScannerConfigurationMutationResponse(true, request.CommandId, false, "ReagentCoordinateAnchor", anchor.Id, "Reagent coordinate anchor updated."),
                    "ReagentCoordinateAnchor",
                    anchor.Id);
            },
            cancellationToken);
    }

    private async Task EnsureCoordinateProfileExistsAsync(string coordinateProfileId, CancellationToken cancellationToken)
    {
        var exists = await dbContext.CoordinateProfiles
            .AsNoTracking()
            .AnyAsync(x => x.Id == coordinateProfileId, cancellationToken);
        if (!exists)
        {
            throw new BusinessRuleException("coordinate_profile_not_found", "Coordinate profile was not found.", StatusCodes.Status404NotFound);
        }
    }

    private async Task EnsureCoordinateVersionBelongsToProfileAsync(
        string coordinateProfileId,
        string? coordinateProfileVersionId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(coordinateProfileVersionId))
        {
            return;
        }

        var version = await dbContext.CoordinateProfileVersions
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == coordinateProfileVersionId, cancellationToken)
            ?? throw new BusinessRuleException("coordinate_profile_version_not_found", "Coordinate profile version was not found.", StatusCodes.Status404NotFound);

        if (version.CoordinateProfileId != coordinateProfileId)
        {
            throw new BusinessRuleException("coordinate_reference_mismatch", "Coordinate profile version does not belong to the selected coordinate profile.", StatusCodes.Status400BadRequest);
        }
    }

    private async Task EnsureColumnNoUniqueAsync(
        string coordinateProfileId,
        string? coordinateProfileVersionId,
        int columnNo,
        string? excludeAnchorId,
        CancellationToken cancellationToken)
    {
        var duplicate = await dbContext.ReagentCoordinateAnchors
            .AsNoTracking()
            .Where(x => x.CoordinateProfileId == coordinateProfileId && x.ColumnNo == columnNo)
            .Where(x => excludeAnchorId == null || x.Id != excludeAnchorId)
            .AnyAsync(cancellationToken);
        if (duplicate)
        {
            throw new BusinessRuleException("reagent_column_no_duplicate", "columnNo already exists for this coordinate profile.", StatusCodes.Status409Conflict);
        }

        if (!string.IsNullOrWhiteSpace(coordinateProfileVersionId))
        {
            var duplicateInVersion = await dbContext.ReagentCoordinateAnchors
                .AsNoTracking()
                .Where(x => x.CoordinateProfileVersionId == coordinateProfileVersionId && x.ColumnNo == columnNo)
                .Where(x => excludeAnchorId == null || x.Id != excludeAnchorId)
                .AnyAsync(cancellationToken);
            if (duplicateInVersion)
            {
                throw new BusinessRuleException("reagent_column_no_duplicate", "columnNo already exists for this coordinate profile version.", StatusCodes.Status409Conflict);
            }
        }
    }

    private static void ValidateAnchorCoordinates(SaveReagentCoordinateAnchorRequest request)
    {
        // 起点与终点至少各有一对坐标，否则无法插值。
        var hasStart = request.StartXUm.HasValue || request.StartYUm.HasValue || request.StartZUm.HasValue;
        var hasEnd = request.EndXUm.HasValue || request.EndYUm.HasValue || request.EndZUm.HasValue;
        if (!hasStart || !hasEnd)
        {
            throw new BusinessRuleException("reagent_anchor_incomplete", "Both start and end coordinates require at least one axis.", StatusCodes.Status400BadRequest);
        }
    }

    private static int ValidateColumnNo(int columnNo)
    {
        if (columnNo < 1 || columnNo > 99)
        {
            throw new BusinessRuleException("column_no_invalid", "columnNo must be between 1 and 99.", StatusCodes.Status400BadRequest);
        }

        return columnNo;
    }

    private static int ValidateSlotCount(int slotCount)
    {
        if (slotCount < 1 || slotCount > 32)
        {
            throw new BusinessRuleException("slot_count_invalid", "slotCount must be between 1 and 32.", StatusCodes.Status400BadRequest);
        }

        return slotCount;
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

    private static ReagentCoordinateAnchorResponse ToAnchorResponse(ReagentCoordinateAnchor anchor)
    {
        return new ReagentCoordinateAnchorResponse(
            anchor.Id,
            anchor.CoordinateProfileId,
            anchor.CoordinateProfileVersionId,
            anchor.ColumnNo,
            anchor.ColumnCode,
            anchor.SlotCount,
            anchor.StartXUm,
            anchor.StartYUm,
            anchor.StartZUm,
            anchor.EndXUm,
            anchor.EndYUm,
            anchor.EndZUm,
            anchor.IsEnabled,
            anchor.CreatedAtUtc,
            anchor.UpdatedAtUtc);
    }

    private static object ToAnchorAudit(ReagentCoordinateAnchor anchor)
    {
        return new
        {
            anchor.CoordinateProfileId,
            anchor.CoordinateProfileVersionId,
            anchor.ColumnNo,
            anchor.ColumnCode,
            anchor.SlotCount,
            anchor.StartXUm,
            anchor.StartYUm,
            anchor.StartZUm,
            anchor.EndXUm,
            anchor.EndYUm,
            anchor.EndZUm,
            anchor.IsEnabled
        };
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