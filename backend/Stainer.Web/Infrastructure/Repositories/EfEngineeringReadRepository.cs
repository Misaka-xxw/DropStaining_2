using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Repositories;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Infrastructure.Repositories;

public sealed class EfEngineeringReadRepository(StainerDbContext dbContext) : IEngineeringReadRepository
{
    public async Task<EngineeringLayoutResponse> GetLayoutAsync(CancellationToken cancellationToken = default)
    {
        var drawers = await dbContext.Drawers
            .AsNoTracking()
            .Include(x => x.PhysicalSlots)
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        var reagentPositions = await dbContext.ReagentRackPositions
            .AsNoTracking()
            .OrderBy(x => x.PositionNo)
            .Select(x => new ReagentRackLayoutResponse(
                x.Id,
                x.Code,
                x.PositionNo,
                x.ColumnNo,
                x.RowNo,
                x.ScannerChannelNo,
                x.ScannerChannelCode,
                x.IsEnabled))
            .ToListAsync(cancellationToken);

        var dabPositions = await dbContext.DabMixPositions
            .AsNoTracking()
            .OrderBy(x => x.PositionNo)
            .Select(x => new NamedPositionResponse(x.Id, x.Code, x.PositionNo, null, x.IsEnabled))
            .ToListAsync(cancellationToken);

        var washPositions = await dbContext.WashPositions
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .Select(x => new NamedPositionResponse(x.Id, x.Code, null, x.WashType, x.IsEnabled))
            .ToListAsync(cancellationToken);

        return new EngineeringLayoutResponse(
            drawers
                .Select(x => new DrawerLayoutResponse(
                    x.Id,
                    x.Code,
                    x.Name,
                    x.SortOrder,
                    x.HeatBoardId,
                    x.IsEnabled,
                    x.PhysicalSlots
                        .OrderBy(slot => slot.SlotNo)
                        .Select(slot => new PhysicalSlotLayoutResponse(
                            slot.Id,
                            slot.Code,
                            slot.SlotNo,
                            slot.VerticalOrderFromBottom,
                            slot.HeatPointId,
                            slot.IsEnabled))
                        .ToList()))
                .ToList(),
            reagentPositions,
            dabPositions,
            washPositions);
    }

    public async Task<IReadOnlyList<CoordinateProfileResponse>> ListCoordinateProfilesAsync(CancellationToken cancellationToken = default)
    {
        var profiles = await dbContext.CoordinateProfiles
            .AsNoTracking()
            .Include(x => x.CoordinatePoints)
            .Include(x => x.Versions)
            .ThenInclude(x => x.TargetPoints)
            .OrderByDescending(x => x.IsActive)
            .ThenBy(x => x.Code)
            .ToListAsync(cancellationToken);

        return profiles
            .Select(x => new CoordinateProfileResponse(
                x.Id,
                x.Code,
                x.Name,
                x.Status,
                x.OriginDefinition,
                x.IsActive,
                x.ActiveVersionId,
                x.Versions
                    .OrderByDescending(version => version.IsActive)
                    .ThenByDescending(version => version.VersionNo)
                    .Select(ToVersionResponse)
                    .ToList(),
                x.CoordinatePoints
                    .Where(point => x.ActiveVersionId == null || point.CoordinateProfileVersionId == x.ActiveVersionId)
                    .OrderBy(point => point.PointType)
                    .ThenBy(point => point.PointCode)
                    .Select(ToPointResponse)
                    .ToList()))
            .ToList();
    }

    public async Task<IReadOnlyList<LiquidClassResponse>> ListLiquidClassesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.LiquidClassProfiles
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .Select(x => new LiquidClassResponse(
                x.Id,
                x.Code,
                x.Name,
                x.AspirateSpeedUlPerSecond,
                x.DispenseSpeedUlPerSecond,
                x.LeadingAirGapUl,
                x.TrailingAirGapUl,
                x.ExcessVolumeUl,
                x.PreWetCycles,
                x.MixCycles,
                x.IsEnabled))
            .ToListAsync(cancellationToken);
    }

    private static CoordinateProfileVersionResponse ToVersionResponse(CoordinateProfileVersion version)
    {
        return new CoordinateProfileVersionResponse(
            version.Id,
            version.CoordinateProfileId,
            version.VersionNo,
            version.VersionLabel,
            version.Status,
            version.IsActive,
            version.UsageScope,
            version.VerificationStatus,
            version.SourceVersionId,
            version.ChangeReason,
            version.ChangeSummaryJson,
            version.ValidationResultJson,
            version.CreatedAtUtc,
            version.PublishedAtUtc,
            version.ActivatedAtUtc,
            version.TargetPoints
                .OrderBy(point => point.PointType)
                .ThenBy(point => point.PointCode)
                .Select(ToPointResponse)
                .ToList());
    }

    private static CoordinatePointResponse ToPointResponse(CoordinatePoint point)
    {
        return new CoordinatePointResponse(
            point.Id,
            point.PointCode,
            point.PointType,
            point.PresetXUm,
            point.PresetYUm,
            point.CalibratedXUm,
            point.CalibratedYUm,
            point.CalibratedZUm,
            point.SafeZUm,
            point.AspirateZUm,
            point.DispenseZUm,
            point.ActionOffsetXUm,
            point.ActionOffsetYUm,
            point.ActionOffsetZUm,
            point.ValidationStatus,
            point.ValidationMessage,
            point.RequiresCalibration,
            point.IsEnabled);
    }
}
