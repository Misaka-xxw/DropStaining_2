using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Repositories;
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
                x.CoordinatePoints
                    .OrderBy(point => point.PointType)
                    .ThenBy(point => point.PointCode)
                    .Select(point => new CoordinatePointResponse(
                        point.Id,
                        point.PointCode,
                        point.PointType,
                        point.PresetXUm,
                        point.PresetYUm,
                        point.CalibratedXUm,
                        point.CalibratedYUm,
                        point.SafeZUm,
                        point.AspirateZUm,
                        point.DispenseZUm,
                        point.RequiresCalibration,
                        point.IsEnabled))
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
}
