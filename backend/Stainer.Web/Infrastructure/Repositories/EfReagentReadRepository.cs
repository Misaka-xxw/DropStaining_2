using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Repositories;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Infrastructure.Repositories;

public sealed class EfReagentReadRepository(StainerDbContext dbContext) : IReagentReadRepository
{
    public async Task<IReadOnlyList<ReagentCatalogItemResponse>> ListCatalogAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.ReagentDefinitions
            .AsNoTracking()
            .Include(x => x.LiquidClassProfile)
            .OrderBy(x => x.ReagentCode)
            .Select(x => new ReagentCatalogItemResponse(
                x.Id,
                x.ReagentCode,
                x.ReagentCode,
                x.Name,
                x.ReagentType,
                x.LiquidClassProfile == null ? null : x.LiquidClassProfile.Code,
                x.LiquidClassProfile == null ? null : x.LiquidClassProfile.Name,
                x.MinimumAlarmVolumeUl,
                x.IsEnabled))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ReagentRackPositionResponse>> ListRackAsync(CancellationToken cancellationToken = default)
    {
        var positions = await dbContext.ReagentRackPositions
            .AsNoTracking()
            .OrderBy(x => x.PositionNo)
            .ToListAsync(cancellationToken);

        var activePlacements = await dbContext.ReagentRackPlacements
            .AsNoTracking()
            .Where(x => x.RemovedAtUtc == null)
            .Include(x => x.ReagentBottle)
            .ThenInclude(x => x!.ReagentDefinition)
            .ToListAsync(cancellationToken);

        var placementByPosition = activePlacements
            .Where(x => x.ReagentBottle is not null)
            .ToDictionary(x => x.ReagentRackPositionId, x => x);

        return positions
            .Select(position =>
            {
                placementByPosition.TryGetValue(position.Id, out var placement);
                var bottle = placement?.ReagentBottle;
                return new ReagentRackPositionResponse(
                    position.Id,
                    position.Code,
                    position.Code,
                    position.PositionNo,
                    position.ColumnNo,
                    position.RowNo,
                    position.ScannerChannelNo,
                    position.ScannerChannelCode,
                    position.IsEnabled,
                    bottle is null
                        ? null
                        : new ReagentRackBottleResponse(
                            bottle.Id,
                            bottle.FullBarcode,
                            bottle.ReagentCode,
                            bottle.ReagentDefinition?.Name ?? bottle.ReagentCode,
                            bottle.ReagentDefinition?.ReagentType ?? string.Empty,
                            bottle.RemainingVolumeUl,
                            bottle.ExpirationDate,
                            bottle.Status,
                            bottle.ProductionBatchNo,
                            bottle.SerialNo));
            })
            .ToList();
    }
}
