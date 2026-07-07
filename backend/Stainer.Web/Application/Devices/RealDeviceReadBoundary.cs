namespace Stainer.Web.Application.Devices;

public interface IDeviceByteTransport
{
    string Name { get; }

    bool IsConfigured { get; }

    Task<DeviceByteTransportResult> ExchangeAsync(
        DeviceByteTransportRequest request,
        CancellationToken cancellationToken = default);

    Task<DeviceByteTransportResult> ReceiveAsync(
        string endpoint,
        CancellationToken cancellationToken = default);
}

public sealed record DeviceByteTransportRequest(
    string Endpoint,
    string Operation,
    byte[] RequestBytes);

public sealed record DeviceByteTransportResult(
    string Status,
    IReadOnlyList<byte[]> ResponseChunks,
    string? ErrorCode = null,
    string? Message = null);

public static class DeviceByteTransportStatuses
{
    public const string Succeeded = "Succeeded";
    public const string TimedOut = "TimedOut";
    public const string Disconnected = "Disconnected";
    public const string Failed = "Failed";
}

public static class DeviceByteTransportEndpoints
{
    public const string MainController = "main-controller-v1.0.4";
    public const string Dcr55 = "dcr55-sample-scanner";
    public const string StandaloneCooling = "standalone-cooling-v1.0";
}

public interface IRealDeviceReadAdapter
{
    Task<RealDeviceReadResult<MainControllerWorkStatus>> ReadControllerWorkStatusAsync(CancellationToken cancellationToken = default);
    Task<RealDeviceReadResult<MainControllerNodeStatuses>> ReadControllerNodeStatusesAsync(CancellationToken cancellationToken = default);
    Task<RealDeviceReadResult<MainControllerRunTime>> ReadControllerRunTimeAsync(CancellationToken cancellationToken = default);
    Task<RealDeviceReadResult<MainControllerTemperatureBoard>> ReadTemperaturesAsync(byte boardId, CancellationToken cancellationToken = default);
    Task<RealDeviceReadResult<MainControllerTemperatureBoard>> ReadTargetTemperaturesAsync(byte boardId, CancellationToken cancellationToken = default);
    Task<RealDeviceReadResult<MainControllerSwitchBoard>> ReadTemperatureSwitchesAsync(byte boardId, CancellationToken cancellationToken = default);
    Task<RealDeviceReadResult<MainControllerOptocouplerStatus>> ReceiveLiquidLevelStatusAsync(CancellationToken cancellationToken = default);
    Task<RealDeviceReadResult<MainControllerPwmSpeeds>> ReadPwmSpeedsAsync(CancellationToken cancellationToken = default);
    Task<RealDeviceReadResult<MainControllerMixerValue>> ReadMixerOriginAsync(byte boardId, CancellationToken cancellationToken = default);
    Task<RealDeviceReadResult<MainControllerMixerValue>> ReadMixerRemainingCountAsync(byte boardId, CancellationToken cancellationToken = default);
    Task<RealDeviceReadResult<MainControllerQrScanStatus>> ReadQrScanStatusAsync(CancellationToken cancellationToken = default);
    Task<RealDeviceReadResult<MainControllerQrText>> ReadQrTextAsync(CancellationToken cancellationToken = default);
    Dcr55TriggerPreparation PrepareDcr55Trigger(Dcr55TriggerMode mode, byte[]? configuredTerminator);
    Task<RealDeviceReadResult<Dcr55ScanResult>> ReceiveDcr55ResultAsync(CancellationToken cancellationToken = default);
    Task<RealDeviceReadResult<StandaloneCoolingFrame>> ReadCoolingTemperatureAsync(CancellationToken cancellationToken = default);
}

public sealed record RealDeviceReadResult<T>(
    bool Ok,
    string Status,
    string? ErrorCode,
    string Message,
    T? Value,
    byte[] RequestBytes,
    byte[] ResponseBytes,
    IReadOnlyList<MainControllerAck> Acknowledgements,
    IReadOnlyList<MainControllerPutReport> PutReports);

public sealed record MainControllerPutReport(
    byte ParentClass,
    byte SubClass,
    object Value);

public sealed record Dcr55TriggerPreparation(
    bool Ok,
    string Status,
    string? ErrorCode,
    string Message,
    byte[] CommandBytes,
    bool Sent);
