namespace Stainer.Web.Domain.Entities;

// 串口连接配置（DB 持久化）。当前用于主控（main-controller）链路的 COM 参数，
// 字段口径对齐 MainControllerConnectionOptions（Dcr55DeviceBoundary.cs）。
// Parity/StopBits/Handshake 以 string 存储（与 ScannerProfile.TriggerMode 一致，免去 enum↔DB 映射）。
// 注意：仅持久化配置，不表示已打开真实串口；真实 Transport 仍 fail-closed。
public sealed class SerialConnectionProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DeviceKey { get; set; } = SerialConnectionDeviceKeys.MainController;
    public string? PortName { get; set; }
    public int? BaudRate { get; set; } = 115200;
    public int? DataBits { get; set; } = 8;
    public string Parity { get; set; } = SerialParityNames.None;
    public string StopBits { get; set; } = SerialStopBitsNames.One;
    public string Handshake { get; set; } = SerialHandshakeNames.None;
    public int? ReadTimeoutMilliseconds { get; set; } = 2000;
    public int? WriteTimeoutMilliseconds { get; set; } = 2000;
    public bool Enabled { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}

public static class SerialConnectionDeviceKeys
{
    public const string MainController = "main-controller";
}

// 与 MainControllerParity / MainControllerStopBits / MainControllerHandshake 枚举名对齐。
public static class SerialParityNames
{
    public const string None = "None";
    public const string Odd = "Odd";
    public const string Even = "Even";
    public const string Mark = "Mark";
    public const string Space = "Space";
}

public static class SerialStopBitsNames
{
    public const string One = "One";
    public const string OnePointFive = "OnePointFive";
    public const string Two = "Two";
}

public static class SerialHandshakeNames
{
    public const string None = "None";
}
