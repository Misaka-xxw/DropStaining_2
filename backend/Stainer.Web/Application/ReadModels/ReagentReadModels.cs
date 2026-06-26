namespace Stainer.Web.Application.ReadModels;

public sealed record ReagentCatalogItemResponse(
    string Id,
    string ReagentCode,
    string Code,
    string Name,
    string ReagentType,
    string? LiquidClassCode,
    string? LiquidClassName,
    int? MinimumAlarmVolumeUl,
    bool IsEnabled);

public sealed record ReagentRackPositionResponse(
    string Id,
    string Position,
    string Code,
    int PositionNo,
    int ColumnNo,
    int RowNo,
    int ScannerChannelNo,
    string ScannerChannelCode,
    bool IsEnabled,
    string ScanState,
    string? LastScanSessionId,
    string? LastScanSessionCode,
    string? LastScanSessionStatus,
    DateTimeOffset? LastScannedAtUtc,
    string? RawBarcode,
    string? BarcodeSummary,
    string? ParsedReagentCode,
    string? ValidationMessage,
    bool IsValidationPassed,
    ReagentRackBottleResponse? Bottle);

public sealed record ReagentRackBottleResponse(
    string Id,
    string FullBarcode,
    string BarcodeSummary,
    string ReagentCode,
    string Name,
    string ReagentType,
    int RemainingVolumeUl,
    DateOnly ExpirationDate,
    string Status,
    string? LotNo,
    string SerialNo,
    DateTimeOffset? FirstScannedAtUtc,
    DateTimeOffset? LastScannedAtUtc);

public sealed record ReagentScanSessionOverviewResponse(
    ReagentScanSessionSummaryResponse? ActiveSession,
    ReagentScanSessionSummaryResponse? LatestCompletedSession);

public sealed record ReagentScanSessionSummaryResponse(
    string? ScanSessionId,
    string? SessionCode,
    string Status,
    DateTimeOffset? StartedAtUtc,
    DateTimeOffset? CompletedAtUtc,
    string? CreatedByUserId,
    string? CreatedByDisplayName,
    int ScannedCount,
    int ValidCount,
    int InvalidCount,
    int EmptyCount,
    int UnscannedCount,
    int TotalPositionCount,
    bool HasWarning,
    string Message);
