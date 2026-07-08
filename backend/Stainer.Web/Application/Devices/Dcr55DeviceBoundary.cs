namespace Stainer.Web.Application.Devices;

public interface IDcr55Adapter
{
    Task<Dcr55ScanResult> ReceiveScanAsync(CancellationToken cancellationToken = default);
}

public sealed record Dcr55ConnectionOptions
{
    public string? Port { get; init; }

    public int BaudRate { get; init; } = 115200;

    public int DataBits { get; init; } = 8;

    public Dcr55Parity Parity { get; init; } = Dcr55Parity.None;

    public Dcr55StopBits StopBits { get; init; } = Dcr55StopBits.One;

    public bool IsConfigured => !string.IsNullOrWhiteSpace(Port);
}

public enum Dcr55Parity
{
    None,
    Odd,
    Even,
    Mark,
    Space
}

public enum Dcr55StopBits
{
    One,
    OnePointFive,
    Two
}

public enum Dcr55ScanStatus
{
    Success,
    Timeout,
    Disconnected,
    InvalidResponse,
    NotConfigured
}

public sealed record Dcr55ScanResult(
    string? Barcode,
    string RawText,
    Dcr55ScanStatus Status,
    DateTimeOffset Timestamp);
