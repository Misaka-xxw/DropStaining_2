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
    ReagentRackBottleResponse? Bottle);

public sealed record ReagentRackBottleResponse(
    string Id,
    string FullBarcode,
    string ReagentCode,
    string Name,
    string ReagentType,
    int RemainingVolumeUl,
    DateOnly ExpirationDate,
    string Status,
    string? LotNo,
    string SerialNo);
