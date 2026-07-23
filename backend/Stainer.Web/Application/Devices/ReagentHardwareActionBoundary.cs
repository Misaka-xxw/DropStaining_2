namespace Stainer.Web.Application.Devices;

public interface IReagentHardwareActionClient
{
    Task<ReagentHardwareActionResult> ExecuteAsync(
        ReagentHardwareActionRequest request,
        CancellationToken cancellationToken = default);
}

public sealed record ReagentHardwareActionRequest(
    string Operation,
    string Axis,
    long XUm,
    long YUm,
    long SafeZUm,
    long ActionZUm,
    int? VolumeUl = null,
    long? LiquidDetectMaximumZUm = null);

public sealed record ReagentHardwareActionResult(
    bool Ok,
    string Status,
    string? ErrorCode,
    string Message,
    IReadOnlyList<string> CompletedSteps);

public static class ReagentHardwareActionOperations
{
    public const string Move = "Move";
    public const string Aspirate = "Aspirate";
    public const string Dispense = "Dispense";
    public const string LiquidDetect = "LiquidDetect";
}
