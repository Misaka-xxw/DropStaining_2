using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

/// <summary>
/// 试剂区坐标插值生成服务。
/// 根据一个试剂列的首尾锚点，按线性插值公式生成中间坐标点：
///   middle = start + ratio * (end - start)，其中 ratio = index / (slotCount - 1)。
/// 支持 X/Y/Z 三轴独立插值；缺失轴保持为 null。
/// 生成结果可作为 CoordinatePoint 保存到指定坐标档案版本（复用已有版本体系），
/// 不修改任何已有运动坐标执行逻辑，仅生成配置数据。
/// </summary>
public sealed class ReagentCoordinateGenerationService(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService)
{
    private const string GeneratedPointType = "ReagentSlot";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// 仅计算生成结果（不落库），用于预览。
    /// </summary>
    public async Task<ReagentCoordinateGenerationResultResponse> PreviewAsync(
        GenerateReagentCoordinatesRequest request,
        CancellationToken cancellationToken = default)
    {
        var anchor = await LoadAnchorAsync(request.AnchorId, cancellationToken);
        var versionId = await ResolveVersionIdAsync(anchor, request.CoordinateProfileVersionId, cancellationToken);
        var points = GeneratePoints(anchor, request.PointCodePrefix);

        return new ReagentCoordinateGenerationResultResponse(
            Ok: true,
            CommandId: request.CommandId,
            Replayed: false,
            AnchorId: anchor.Id,
            CoordinateProfileId: anchor.CoordinateProfileId,
            CoordinateProfileVersionId: versionId,
            ColumnNo: anchor.ColumnNo,
            ColumnCode: anchor.ColumnCode,
            SlotCount: anchor.SlotCount,
            Saved: false,
            Points: points,
            Message: "Coordinate preview generated.");
    }

    /// <summary>
    /// 生成并保存为 CoordinatePoint（幂等）。
    /// </summary>
    public Task<ReagentCoordinateGenerationResultResponse> GenerateAndSaveAsync(
        GenerateReagentCoordinatesRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "reagent_coordinate.generate",
            request,
            actor,
            async () =>
            {
                RequireReason(request.Reason);
                var anchor = await LoadAnchorAsync(request.AnchorId, cancellationToken);
                var versionId = await ResolveVersionIdAsync(anchor, request.CoordinateProfileVersionId, cancellationToken);

                if (string.IsNullOrWhiteSpace(versionId))
                {
                    throw new BusinessRuleException("coordinate_version_required", "coordinateProfileVersionId is required to save generated points.", StatusCodes.Status400BadRequest);
                }

                await EnsureVersionIsDraftAsync(versionId, cancellationToken);

                var points = GeneratePoints(anchor, request.PointCodePrefix);
                var savedPointIds = await SavePointsAsync(anchor, versionId, points, cancellationToken);

                var resultPoints = points
                    .Select((p, index) => p with { SavedPointId = savedPointIds.ElementAtOrDefault(index) })
                    .ToList();

                AddAudit(actor, "reagent_coordinate.generate", "ReagentCoordinateAnchor", anchor.Id, null, new
                {
                    anchor.CoordinateProfileId,
                    CoordinateProfileVersionId = versionId,
                    anchor.ColumnNo,
                    anchor.SlotCount,
                    pointCodes = resultPoints.Select(x => x.PointCode).ToArray(),
                    reason = request.Reason.Trim()
                }, request.Reason);

                var response = new ReagentCoordinateGenerationResultResponse(
                    Ok: true,
                    CommandId: request.CommandId,
                    Replayed: false,
                    AnchorId: anchor.Id,
                    CoordinateProfileId: anchor.CoordinateProfileId,
                    CoordinateProfileVersionId: versionId,
                    ColumnNo: anchor.ColumnNo,
                    ColumnCode: anchor.ColumnCode,
                    SlotCount: anchor.SlotCount,
                    Saved: true,
                    Points: resultPoints,
                    Message: $"{resultPoints.Count} coordinate points generated and saved.");

                return new CommandExecutionResult<ReagentCoordinateGenerationResultResponse>(
                    response,
                    "CoordinatePoint",
                    anchor.Id);
            },
            cancellationToken);
    }

    private async Task<ReagentCoordinateAnchor> LoadAnchorAsync(string anchorId, CancellationToken cancellationToken)
    {
        var normalizedId = RequireValue(anchorId, "anchorId", 36);
        return await dbContext.ReagentCoordinateAnchors
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == normalizedId, cancellationToken)
            ?? throw new BusinessRuleException("reagent_coordinate_anchor_not_found", "Reagent coordinate anchor was not found.", StatusCodes.Status404NotFound);
    }

    private async Task<string?> ResolveVersionIdAsync(ReagentCoordinateAnchor anchor, string? requestedVersionId, CancellationToken cancellationToken)
    {
        var versionId = NormalizeOptionalValue(requestedVersionId, "coordinateProfileVersionId", 36);
        if (string.IsNullOrWhiteSpace(versionId))
        {
            return anchor.CoordinateProfileVersionId;
        }

        var version = await dbContext.CoordinateProfileVersions
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == versionId, cancellationToken)
            ?? throw new BusinessRuleException("coordinate_profile_version_not_found", "Coordinate profile version was not found.", StatusCodes.Status404NotFound);

        if (version.CoordinateProfileId != anchor.CoordinateProfileId)
        {
            throw new BusinessRuleException("coordinate_reference_mismatch", "Coordinate profile version does not belong to the anchor's coordinate profile.", StatusCodes.Status400BadRequest);
        }

        return versionId;
    }

    private async Task EnsureVersionIsDraftAsync(string versionId, CancellationToken cancellationToken)
    {
        var status = await dbContext.CoordinateProfileVersions
            .AsNoTracking()
            .Where(x => x.Id == versionId)
            .Select(x => x.Status)
            .SingleOrDefaultAsync(cancellationToken);

        if (status != CoordinateProfileVersionStatus.Draft)
        {
            throw new BusinessRuleException("coordinate_version_not_draft", "Generated points can only be saved to a draft coordinate profile version.", StatusCodes.Status400BadRequest);
        }
    }

    private async Task<IReadOnlyList<string>> SavePointsAsync(
        ReagentCoordinateAnchor anchor,
        string versionId,
        IReadOnlyList<GeneratedCoordinatePointResponse> points,
        CancellationToken cancellationToken)
    {
        // 仅移除由本生成服务此前生成的同 PointCode 旧点（重入幂等），
        // 严格按 PointType == ReagentSlot 过滤，避免误删人工校准点或其他类型点。
        var pointCodes = points.Select(x => x.PointCode).ToList();
        var existing = await dbContext.CoordinatePoints
            .Where(x => x.CoordinateProfileVersionId == versionId
                && x.PointType == GeneratedPointType
                && pointCodes.Contains(x.PointCode))
            .ToListAsync(cancellationToken);
        if (existing.Count > 0)
        {
            dbContext.CoordinatePoints.RemoveRange(existing);
        }

        var ids = new List<string>();
        foreach (var point in points)
        {
            var entity = new CoordinatePoint
            {
                CoordinateProfileId = anchor.CoordinateProfileId,
                CoordinateProfileVersionId = versionId,
                PointCode = point.PointCode,
                PointType = GeneratedPointType,
                PresetXUm = ToLong(point.XUm),
                PresetYUm = ToLong(point.YUm),
                CalibratedZUm = ToLong(point.ZUm),
                RequiresCalibration = true,
                ValidationStatus = CoordinateTargetPointValidationStatus.Unverified,
                IsEnabled = true,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };
            dbContext.CoordinatePoints.Add(entity);
            ids.Add(entity.Id);
        }

        return ids;
    }

    private static IReadOnlyList<GeneratedCoordinatePointResponse> GeneratePoints(ReagentCoordinateAnchor anchor, string? prefix)
    {
        var slotCount = anchor.SlotCount;
        if (slotCount <= 0)
        {
            return [];
        }

        var codePrefix = string.IsNullOrWhiteSpace(prefix) ? BuildDefaultPrefix(anchor) : prefix.Trim();
        var points = new List<GeneratedCoordinatePointResponse>(slotCount);
        var denominator = slotCount == 1 ? 1 : slotCount - 1;

        for (var i = 0; i < slotCount; i++)
        {
            var ratio = slotCount == 1 ? 0d : (double)i / denominator;
            var rowNo = i + 1;
            var pointCode = $"{codePrefix}-C{anchor.ColumnNo:D2}-R{rowNo:D2}";

            double? x = Interpolate(anchor.StartXUm, anchor.EndXUm, ratio);
            double? y = Interpolate(anchor.StartYUm, anchor.EndYUm, ratio);
            double? z = Interpolate(anchor.StartZUm, anchor.EndZUm, ratio);

            points.Add(new GeneratedCoordinatePointResponse(i, rowNo, pointCode, ratio, x, y, z, null));
        }

        return points;
    }

    private static double? Interpolate(double? start, double? end, double ratio)
    {
        if (!start.HasValue || !end.HasValue)
        {
            return null;
        }

        return start.Value + ratio * (end.Value - start.Value);
    }

    private static long? ToLong(double? value)
    {
        if (!value.HasValue)
        {
            return null;
        }

        return (long)Math.Round(value.Value, MidpointRounding.AwayFromZero);
    }

    private static string BuildDefaultPrefix(ReagentCoordinateAnchor anchor)
    {
        return string.IsNullOrWhiteSpace(anchor.ColumnCode)
            ? $"REAG{anchor.ColumnNo:D2}"
            : anchor.ColumnCode!;
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