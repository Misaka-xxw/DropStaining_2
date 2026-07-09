using System.Text.Json;

namespace Stainer.Web.Application.Requests;

public sealed record ScannerDeviceParametersRequest(
    int? RoiX = null,
    int? RoiY = null,
    int? RoiWidth = null,
    int? RoiHeight = null,
    bool? CheckLightEnabled = null,
    JsonElement? SpecialParameters = null);

public sealed record SaveScannerProfileRequest(
    string CommandId,
    string Name,
    string ScannerType,
    bool Enabled,
    string? Port,
    int? BaudRate,
    int? TimeoutMilliseconds,
    string TriggerMode,
    ScannerDeviceParametersRequest? DeviceParameters,
    string Reason);

public sealed record SaveScannerRegionRequest(
    string CommandId,
    string Name,
    string RegionType,
    string ScannerProfileId,
    JsonElement? ScanPath,
    string? CoordinateProfileId,
    string? CoordinateProfileVersionId,
    IReadOnlyList<string>? CoordinatePointCodes,
    string Reason);
