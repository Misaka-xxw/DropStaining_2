using System.Text;

namespace Stainer.Web.Application.Devices;

public static class Dcr55Protocol
{
    public const string SingleTriggerCommandText = "RDCMXEV1,P11,P20";
    public const string StopTriggerCommandText = "RDCMXEV1,P10";
    public const string ContinuousTriggerCommandText = "RDCMXEV1,P11,P21";

    public static string GetCommandText(Dcr55TriggerMode mode) => mode switch
    {
        Dcr55TriggerMode.Single => SingleTriggerCommandText,
        Dcr55TriggerMode.Stop => StopTriggerCommandText,
        Dcr55TriggerMode.Continuous => ContinuousTriggerCommandText,
        _ => throw new ArgumentOutOfRangeException(nameof(mode))
    };

    public static byte[] EncodeCommand(Dcr55TriggerMode mode, byte[] configuredTerminator)
    {
        ArgumentNullException.ThrowIfNull(configuredTerminator);
        return Encoding.ASCII.GetBytes(GetCommandText(mode)).Concat(configuredTerminator).ToArray();
    }

    public static Dcr55ScanResult ParseBarcodeResult(string rawText, DateTimeOffset? timestamp = null)
    {
        ArgumentNullException.ThrowIfNull(rawText);
        var observedAt = timestamp ?? DateTimeOffset.UtcNow;
        if (rawText.Length == 0 || !rawText.EndsWith("\r\n", StringComparison.Ordinal))
        {
            return Invalid(rawText, observedAt);
        }

        var records = rawText
            .Split("\r\n", StringSplitOptions.None)
            .Where(value => value.Length > 0)
            .ToArray();
        if (records.Length != 1
            || string.IsNullOrWhiteSpace(records[0])
            || records[0].Any(char.IsControl))
        {
            return Invalid(rawText, observedAt);
        }

        return new Dcr55ScanResult(records[0], rawText, Dcr55ScanStatus.Success, observedAt);
    }

    public static Dcr55ScanResult FromTransportStatus(
        Dcr55ScanStatus status,
        string rawText,
        DateTimeOffset? timestamp = null)
    {
        if (status is Dcr55ScanStatus.Success)
        {
            throw new ArgumentOutOfRangeException(nameof(status), "A successful result must be produced by parsing barcode text.");
        }

        return new Dcr55ScanResult(null, rawText, status, timestamp ?? DateTimeOffset.UtcNow);
    }

    private static Dcr55ScanResult Invalid(string rawText, DateTimeOffset timestamp) =>
        new(null, rawText, Dcr55ScanStatus.InvalidResponse, timestamp);
}

public enum Dcr55TriggerMode
{
    Single,
    Stop,
    Continuous
}
