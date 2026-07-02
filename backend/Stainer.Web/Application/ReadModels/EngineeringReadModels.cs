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
    string? ActiveVersionId,
    IReadOnlyList<CoordinateProfileVersionResponse> Versions,
    IReadOnlyList<CoordinatePointResponse> Points);

public sealed record CoordinateProfileVersionResponse(
    string Id,
    string CoordinateProfileId,
    int VersionNo,
    string VersionLabel,
    string Status,
    bool IsActive,
    string UsageScope,
    string VerificationStatus,
    string? SourceVersionId,
    string ChangeReason,
    string ChangeSummaryJson,
    string ValidationResultJson,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? PublishedAtUtc,
    DateTimeOffset? ActivatedAtUtc,
    IReadOnlyList<CoordinatePointResponse> TargetPoints);

public sealed record CoordinatePointResponse(
    string Id,
    string PointCode,
    string PointType,
    long? PresetXUm,
    long? PresetYUm,
    long? CalibratedXUm,
    long? CalibratedYUm,
    long? CalibratedZUm,
    long? SafeZUm,
    long? AspirateZUm,
    long? DispenseZUm,
    long? ActionOffsetXUm,
    long? ActionOffsetYUm,
    long? ActionOffsetZUm,
    string ValidationStatus,
    string ValidationMessage,
    bool RequiresCalibration,
    bool IsEnabled);

public sealed record CoordinateProfileVersionMutationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string CoordinateProfileId,
    string CoordinateProfileVersionId,
    int VersionNo,
    string VersionLabel,
    string Status,
    bool IsActive,
    string UsageScope,
    string VerificationStatus,
    string Message);

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
