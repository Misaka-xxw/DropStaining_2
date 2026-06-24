namespace Stainer.Web.Application.ReadModels;

public sealed record EngineeringLayoutResponse(
    IReadOnlyList<DrawerLayoutResponse> Drawers,
    IReadOnlyList<ReagentRackLayoutResponse> ReagentRackPositions,
    IReadOnlyList<NamedPositionResponse> DabMixPositions,
    IReadOnlyList<NamedPositionResponse> WashPositions);

public sealed record DrawerLayoutResponse(
    string Id,
    string Code,
    string Name,
    int SortOrder,
    int HeatBoardId,
    bool IsEnabled,
    IReadOnlyList<PhysicalSlotLayoutResponse> Slots);

public sealed record PhysicalSlotLayoutResponse(
    string Id,
    string Code,
    int SlotNo,
    int VerticalOrderFromBottom,
    int HeatPointId,
    bool IsEnabled);

public sealed record ReagentRackLayoutResponse(
    string Id,
    string Code,
    int PositionNo,
    int ColumnNo,
    int RowNo,
    int ScannerChannelNo,
    string ScannerChannelCode,
    bool IsEnabled);

public sealed record NamedPositionResponse(
    string Id,
    string Code,
    int? PositionNo,
    string? Type,
    bool IsEnabled);

public sealed record CoordinateProfileResponse(
    string Id,
    string Code,
    string Name,
    string Status,
    string OriginDefinition,
    bool IsActive,
    IReadOnlyList<CoordinatePointResponse> Points);

public sealed record CoordinatePointResponse(
    string Id,
    string PointCode,
    string PointType,
    long? PresetXUm,
    long? PresetYUm,
    long? CalibratedXUm,
    long? CalibratedYUm,
    long? SafeZUm,
    long? AspirateZUm,
    long? DispenseZUm,
    bool RequiresCalibration,
    bool IsEnabled);

public sealed record LiquidClassResponse(
    string Id,
    string Code,
    string Name,
    int? AspirateSpeedUlPerSecond,
    int? DispenseSpeedUlPerSecond,
    int? LeadingAirGapUl,
    int? TrailingAirGapUl,
    int? ExcessVolumeUl,
    int? PreWetCycles,
    int? MixCycles,
    bool IsEnabled);
