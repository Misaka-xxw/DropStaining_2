using System.Text;
using Stainer.Web.Application.Devices;
using Stainer.Web.Infrastructure.Devices;

namespace Stainer.Tests;

public sealed class Dcr55RealAdapterTests
{
    private static readonly DateTimeOffset ObservedAt = new(2026, 7, 8, 1, 2, 3, TimeSpan.Zero);

    [Fact]
    public void Connection_options_model_confirmed_serial_settings_without_opening_a_port()
    {
        var options = new Dcr55ConnectionOptions { Port = "OFFLINE-FAKE" };

        Assert.True(options.IsConfigured);
        Assert.Equal("OFFLINE-FAKE", options.Port);
        Assert.Equal(115200, options.BaudRate);
        Assert.Equal(8, options.DataBits);
        Assert.Equal(Dcr55Parity.None, options.Parity);
        Assert.Equal(Dcr55StopBits.One, options.StopBits);
    }

    [Fact]
    public async Task Normal_crlf_response_returns_structured_success_result()
    {
        var fake = FakeDcr55Transport.Success("ABC123\r\n");
        var adapter = CreateAdapter(fake);

        var result = await adapter.ReceiveScanAsync();

        Assert.Equal("ABC123", result.Barcode);
        Assert.Equal("ABC123\r\n", result.RawText);
        Assert.Equal(Dcr55ScanStatus.Success, result.Status);
        Assert.Equal(ObservedAt, result.Timestamp);
        Assert.Equal(DeviceByteTransportEndpoints.Dcr55, fake.LastReceiveEndpoint);
        Assert.Equal(1, fake.ReceiveCount);
    }

    [Theory]
    [InlineData("\r\nABC123\r\n")]
    [InlineData("ABC123\r\n\r\n")]
    [InlineData("\r\n\r\nABC123\r\n\r\n")]
    public async Task Extra_crlf_records_are_ignored(string rawText)
    {
        var result = await CreateAdapter(FakeDcr55Transport.Success(rawText)).ReceiveScanAsync();

        Assert.Equal(Dcr55ScanStatus.Success, result.Status);
        Assert.Equal("ABC123", result.Barcode);
        Assert.Equal(rawText, result.RawText);
    }

    [Theory]
    [InlineData("")]
    [InlineData("\r\n")]
    [InlineData("\r\n\r\n")]
    public async Task Empty_response_is_invalid(string rawText)
    {
        var result = await CreateAdapter(FakeDcr55Transport.Success(rawText)).ReceiveScanAsync();

        Assert.Equal(Dcr55ScanStatus.InvalidResponse, result.Status);
        Assert.Null(result.Barcode);
        Assert.Equal(rawText, result.RawText);
    }

    [Theory]
    [InlineData("ABC123")]
    [InlineData("ABC123\n")]
    [InlineData("ABC\u0001\r\n")]
    [InlineData("ABC123\r\nDEF456\r\n")]
    public async Task Malformed_or_ambiguous_response_is_invalid(string rawText)
    {
        var result = await CreateAdapter(FakeDcr55Transport.Success(rawText)).ReceiveScanAsync();

        Assert.Equal(Dcr55ScanStatus.InvalidResponse, result.Status);
        Assert.Null(result.Barcode);
    }

    [Fact]
    public async Task Timeout_transport_result_maps_to_timeout_status()
    {
        var fake = new FakeDcr55Transport(new DeviceByteTransportResult(
            DeviceByteTransportStatuses.TimedOut,
            [],
            "offline_timeout",
            "No offline data was queued."));

        var result = await CreateAdapter(fake).ReceiveScanAsync();

        Assert.Equal(Dcr55ScanStatus.Timeout, result.Status);
        Assert.Null(result.Barcode);
    }

    [Fact]
    public async Task Disconnected_transport_result_maps_to_disconnected_status()
    {
        var fake = new FakeDcr55Transport(new DeviceByteTransportResult(
            DeviceByteTransportStatuses.Disconnected,
            [],
            "offline_disconnected",
            "The fake endpoint is disconnected."));

        var result = await CreateAdapter(fake).ReceiveScanAsync();

        Assert.Equal(Dcr55ScanStatus.Disconnected, result.Status);
        Assert.Null(result.Barcode);
    }

    [Fact]
    public async Task Missing_configuration_fails_closed_without_transport_io()
    {
        var fake = FakeDcr55Transport.Success("SHOULD-NOT-BE-READ\r\n");
        var adapter = new Dcr55RealAdapter(fake, new Dcr55ConnectionOptions(), new FixedTimeProvider(ObservedAt));

        var result = await adapter.ReceiveScanAsync();

        Assert.Equal(Dcr55ScanStatus.NotConfigured, result.Status);
        Assert.Null(result.Barcode);
        Assert.Equal(0, fake.ReceiveCount);
    }

    [Fact]
    public async Task Unconfigured_transport_fails_closed_without_transport_io()
    {
        var fake = FakeDcr55Transport.Success("SHOULD-NOT-BE-READ\r\n", isConfigured: false);

        var result = await CreateAdapter(fake).ReceiveScanAsync();

        Assert.Equal(Dcr55ScanStatus.NotConfigured, result.Status);
        Assert.Equal(0, fake.ReceiveCount);
    }

    private static Dcr55RealAdapter CreateAdapter(FakeDcr55Transport fake) =>
        new(fake, new Dcr55ConnectionOptions { Port = "OFFLINE-FAKE" }, new FixedTimeProvider(ObservedAt));

    private sealed class FakeDcr55Transport(DeviceByteTransportResult result, bool isConfigured = true) : IDeviceByteTransport
    {
        public string Name => nameof(FakeDcr55Transport);
        public bool IsConfigured { get; } = isConfigured;
        public int ReceiveCount { get; private set; }
        public string? LastReceiveEndpoint { get; private set; }

        public static FakeDcr55Transport Success(string rawText, bool isConfigured = true) =>
            new(
                new DeviceByteTransportResult(
                    DeviceByteTransportStatuses.Succeeded,
                    [Encoding.ASCII.GetBytes(rawText)]),
                isConfigured);

        public Task<DeviceByteTransportResult> ExchangeAsync(
            DeviceByteTransportRequest request,
            CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("DCR55 offline receive must never send or exchange command bytes.");

        public Task<DeviceByteTransportResult> ReceiveAsync(
            string endpoint,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ReceiveCount++;
            LastReceiveEndpoint = endpoint;
            return Task.FromResult(result);
        }
    }

    private sealed class FixedTimeProvider(DateTimeOffset value) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => value;
    }
}
