using Stainer.Web.Application.Services;

namespace Stainer.Web.Application.ReadModels;

/// <summary>
/// 单项预检结果。status 只有 Passed 算通过；Failed/Unavailable/Running 都必须阻止启动。
/// </summary>
public sealed record PrecheckCheckResponse(
    string CheckId,
    string Label,
    string Category,
    string Status,
    bool Blocking,
    string? Code,
    string Message,
    DateTimeOffset CheckedAtUtc,
    IReadOnlyDictionary<string, object?> Data);

/// <summary>
/// 完整预检报告。ok=所有 blocking 检查均为 Passed；checks 始终包含全部已执行项。
/// </summary>
public sealed record PrecheckReportResponse(
    bool Ok,
    string ReportId,
    string CommandId,
    IReadOnlyList<PrecheckCheckResponse> Checks,
    DateTimeOffset GeneratedAtUtc,
    string RunMode = RuntimeModes.Twin,
    string DeviceMode = DeviceModes.Mock);

public static class RuntimeModes
{
    public const string Twin = "Twin";
    public const string Debug = "Debug";
    public const string Production = "Production";

    public static string Normalize(string? value, string deviceMode)
    {
        if (string.Equals(value, Debug, StringComparison.OrdinalIgnoreCase)) return Debug;
        if (string.Equals(value, Production, StringComparison.OrdinalIgnoreCase)) return Production;
        return string.Equals(deviceMode, DeviceModes.Mock, StringComparison.OrdinalIgnoreCase) ? Twin : Production;
    }

    public static bool RequiresRealDevice(string value) => value is Debug or Production;
}

public static class PrecheckStatuses
{
    public const string Passed = "Passed";
    public const string Failed = "Failed";
    public const string Unavailable = "Unavailable";
    public const string Running = "Running";

    public static bool IsPassing(string? status) =>
        string.Equals(status, Passed, StringComparison.OrdinalIgnoreCase);
}

public static class PrecheckCategories
{
    public const string Device = "Device";
    public const string Motion = "Motion";
    public const string Thermal = "Thermal";
    public const string Scanner = "Scanner";
    public const string Fluidics = "Fluidics";
}
