using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Repositories;
using Stainer.Web.Domain.Entities;
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

        var positionIds = positions.Select(x => x.Id).ToHashSet(StringComparer.Ordinal);
        var latestScanItems = await dbContext.ReagentScanItems
            .AsNoTracking()
            .Include(x => x.ReagentScanSession)
            .Where(x => positionIds.Contains(x.ReagentRackPositionId))
            .ToListAsync(cancellationToken);

        var latestScanByPosition = latestScanItems
            .GroupBy(x => x.ReagentRackPositionId)
            .ToDictionary(
                x => x.Key,
                x => x.OrderByDescending(item => item.CreatedAtUtc).First(),
                StringComparer.Ordinal);

        var placementByPosition = activePlacements
            .Where(x => x.ReagentBottle is not null)
            .ToDictionary(x => x.ReagentRackPositionId, x => x);

        return positions
            .Select(position =>
            {
                placementByPosition.TryGetValue(position.Id, out var placement);
                var bottle = placement?.ReagentBottle;
                latestScanByPosition.TryGetValue(position.Id, out var latestScan);
                var scanState = latestScan?.ScanResult
                    ?? (bottle is null ? "UNSCANNED" : ReagentScanResult.Valid);
                var rawBarcode = latestScan?.RawBarcode ?? bottle?.FullBarcode;
                var lastScannedAtUtc = latestScan?.CreatedAtUtc
                    ?? bottle?.LastScannedAtUtc
                    ?? placement?.PlacedAtUtc;
                var validationMessage = latestScan?.ValidationMessage
                    ?? (bottle is null ? "No scan result recorded." : "Current placement has no scan item record.");
                var isValidationPassed = latestScan?.IsValidationPassed ?? bottle is not null;
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
                    scanState,
                    latestScan?.ReagentScanSessionId,
                    latestScan?.ReagentScanSession?.SessionCode,
                    latestScan?.ReagentScanSession?.Status,
                    lastScannedAtUtc,
                    rawBarcode,
                    ToBarcodeSummary(rawBarcode),
                    latestScan?.ParsedReagentCode ?? bottle?.ReagentCode,
                    validationMessage,
                    isValidationPassed,
                    bottle is null
                        ? null
                        : new ReagentRackBottleResponse(
                            bottle.Id,
                            bottle.FullBarcode,
                            ToBarcodeSummary(bottle.FullBarcode) ?? bottle.FullBarcode,
                            bottle.ReagentCode,
                            bottle.ReagentDefinition?.Name ?? bottle.ReagentCode,
                            bottle.ReagentDefinition?.ReagentType ?? string.Empty,
                            bottle.RemainingVolumeUl,
                            bottle.ExpirationDate,
                            bottle.Status,
                            bottle.ProductionBatchNo,
                            bottle.SerialNo,
                            bottle.FirstScannedAtUtc,
                            bottle.LastScannedAtUtc));
            })
            .ToList();
    }

    public async Task<ReagentScanSessionOverviewResponse> GetScanSessionOverviewAsync(CancellationToken cancellationToken = default)
    {
        var totalPositionCount = await dbContext.ReagentRackPositions
            .AsNoTracking()
            .CountAsync(cancellationToken);
        var sessions = await dbContext.ReagentScanSessions
            .AsNoTracking()
            .Include(x => x.CreatedByUser)
            .Include(x => x.Items)
            .ToListAsync(cancellationToken);

        var activeSession = sessions
            .Where(x => x.CompletedAtUtc is null && x.Status.Equals("Active", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.StartedAtUtc)
            .FirstOrDefault();
        var latestCompletedSession = sessions
            .Where(x => x.CompletedAtUtc is not null || x.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.CompletedAtUtc ?? x.StartedAtUtc)
            .FirstOrDefault();

        return new ReagentScanSessionOverviewResponse(
            activeSession is null ? null : ToSessionSummary(activeSession, totalPositionCount),
            latestCompletedSession is null ? null : ToSessionSummary(latestCompletedSession, totalPositionCount));
    }

    private static string? ToBarcodeSummary(string? barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode))
        {
            return null;
        }

        var value = barcode.Trim();
        return value.Length <= 10
            ? value
            : $"{value[..4]}...{value[^4..]}";
    }

    private static ReagentScanSessionSummaryResponse ToSessionSummary(ReagentScanSession session, int totalPositionCount)
    {
        var items = session.Items.ToList();
        var validCount = items.Count(x => x.ScanResult.Equals(ReagentScanResult.Valid, StringComparison.OrdinalIgnoreCase));
        var invalidCount = items.Count(x => x.ScanResult.Equals(ReagentScanResult.Invalid, StringComparison.OrdinalIgnoreCase));
        var emptyCount = items.Count(x => x.ScanResult.Equals(ReagentScanResult.Empty, StringComparison.OrdinalIgnoreCase));
        var scannedCount = items.Count;
        var scannedPositionCount = items
            .Select(x => x.ReagentRackPositionId)
            .Distinct(StringComparer.Ordinal)
            .Count();
        var unscannedCount = Math.Max(0, totalPositionCount - scannedPositionCount);
        var completed = session.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase) || session.CompletedAtUtc is not null;
        var hasWarning = completed && unscannedCount > 0;
        var message = hasWarning
            ? $"Reagent scan session completed with {unscannedCount} unscanned position(s)."
            : completed
                ? "Reagent scan session completed."
                : "Reagent scan session is active.";

        return new ReagentScanSessionSummaryResponse(
            session.Id,
            session.SessionCode,
            session.Status,
            session.StartedAtUtc,
            session.CompletedAtUtc,
            session.CreatedByUserId,
            session.CreatedByUser?.DisplayName,
            scannedCount,
            validCount,
            invalidCount,
            emptyCount,
            unscannedCount,
            totalPositionCount,
            hasWarning,
            message);
    }
}
