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
    string Name,
    string RegionType,
    string ScannerProfileId,
    string ScanPathJson,
    string? CoordinateProfileId,
    string? CoordinateProfileVersionId,
    IReadOnlyList<string> CoordinatePointCodes,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record ScannerConfigurationMutationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string EntityType,
    string EntityId,
    string Message);
