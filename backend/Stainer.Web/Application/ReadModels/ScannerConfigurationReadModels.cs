namespace Stainer.Web.Application.ReadModels;

public sealed record ScannerProfileResponse(
    string Id,
    string Name,
    string ScannerType,
    bool Enabled,
    string? Port,
    int? BaudRate,
    int? TimeoutMilliseconds,
    string TriggerMode,
    ScannerDeviceParametersResponse DeviceParameters,
    IReadOnlyList<ScannerRegionResponse> Regions,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record ScannerDeviceParametersResponse(
    int? RoiX,
    int? RoiY,
    int? RoiWidth,
    int? RoiHeight,
    bool? CheckLightEnabled,
    string SpecialParametersJson);

public sealed record ScannerRegionResponse(
    string Id,
    int RegionNo,
    string Name,
    string RegionType,
    string ScannerProfileId,
    int ScanOrder,
    string ScanPathJson,
    string? CoordinateProfileId,
    string? CoordinateProfileVersionId,
    IReadOnlyList<string> CoordinatePointCodes,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record ReagentCoordinateAnchorResponse(
    string Id,
    string CoordinateProfileId,
    string? CoordinateProfileVersionId,
    int ColumnNo,
    string? ColumnCode,
    int SlotCount,
    double? StartXUm,
    double? StartYUm,
    double? StartZUm,
    double? EndXUm,
    double? EndYUm,
    double? EndZUm,
    bool IsEnabled,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record GeneratedCoordinatePointResponse(
    int Index,
    int RowNo,
    string PointCode,
    double Ratio,
    double? XUm,
    double? YUm,
    double? ZUm,
    string? SavedPointId);

public sealed record ReagentCoordinateGenerationResultResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string AnchorId,
    string CoordinateProfileId,
    string? CoordinateProfileVersionId,
    int ColumnNo,
    string? ColumnCode,
    int SlotCount,
    bool Saved,
    IReadOnlyList<GeneratedCoordinatePointResponse> Points,
    string Message);

public sealed record ScannerConfigurationMutationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string EntityType,
    string EntityId,
    string Message);

public sealed record ScannerControlResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string ScannerProfileId,
    string Operation,
    string Status,
    string? ErrorCode,
    string Message,
    string TransportName,
    IReadOnlyList<ScannerControlStepResponse> Steps);

public sealed record ScannerControlStepResponse(
    string Operation,
    string CommandText,
    string Status,
    string? ErrorCode,
    string? Message,
    string ResponseText,
    string ResponseHex);
