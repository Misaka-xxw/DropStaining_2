using System.Text;
using Stainer.Web.Application.Devices;

namespace Stainer.Web.Infrastructure.Devices;

public sealed class Dcr55RealAdapter(
    IDeviceByteTransport? transport,
    Dcr55ConnectionOptions configuration,
    TimeProvider? timeProvider = null) : IDcr55Adapter
{
    private readonly TimeProvider clock = timeProvider ?? TimeProvider.System;

    public async Task<Dcr55ScanResult> ReceiveScanAsync(CancellationToken cancellationToken = default)
    {
        var timestamp = clock.GetUtcNow();
        if (!configuration.IsConfigured || transport is not { IsConfigured: true })
        {
            return Dcr55Protocol.FromTransportStatus(Dcr55ScanStatus.NotConfigured, string.Empty, timestamp);
        }

        var receive = await transport.ReceiveAsync(DeviceByteTransportEndpoints.Dcr55, cancellationToken);
        var responseBytes = receive.ResponseChunks.SelectMany(chunk => chunk).ToArray();
        var rawText = Encoding.ASCII.GetString(responseBytes);

        if (string.Equals(receive.Status, DeviceByteTransportStatuses.TimedOut, StringComparison.OrdinalIgnoreCase))
        {
            return Dcr55Protocol.FromTransportStatus(Dcr55ScanStatus.Timeout, rawText, timestamp);
        }

        if (string.Equals(receive.Status, DeviceByteTransportStatuses.Disconnected, StringComparison.OrdinalIgnoreCase))
        {
            return Dcr55Protocol.FromTransportStatus(Dcr55ScanStatus.Disconnected, rawText, timestamp);
        }

        if (!string.Equals(receive.Status, DeviceByteTransportStatuses.Succeeded, StringComparison.OrdinalIgnoreCase)
            || responseBytes.Any(value => value > 0x7F))
        {
            return Dcr55Protocol.FromTransportStatus(Dcr55ScanStatus.InvalidResponse, rawText, timestamp);
        }

        return Dcr55Protocol.ParseBarcodeResult(rawText, timestamp);
    }
}
