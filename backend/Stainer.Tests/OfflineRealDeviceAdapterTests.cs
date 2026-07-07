using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stainer.Web.Application.Devices;
using Stainer.Web.Application.Services;
using Stainer.Web.Infrastructure.Devices;

namespace Stainer.Tests;

public sealed class OfflineRealDeviceAdapterTests
{
    [Fact]
    public async Task Main_controller_read_boundary_sends_confirmed_frames_and_parses_all_supported_models()
    {
        var fake = new InMemoryFakeDeviceByteTransport();
        var adapter = new UnavailableRealDeviceAdapter(fake);
        var nodes = Enumerable.Repeat((byte)1, 64).ToArray();
        nodes[7] = 2;

        fake.EnqueueExchange(MainControllerProtocol.BuildWorkStatusRequest(), Response(0x01, 0x08, [0x01]));
        fake.EnqueueExchange(MainControllerProtocol.BuildNodeStatusRequest(), Response(0x01, 0x09, nodes));
        fake.EnqueueExchange(MainControllerProtocol.BuildRunTimeRequest(), Response(0x01, 0x05, [1, 2, 3, 4]));
        fake.EnqueueExchange(
            MainControllerProtocol.BuildBoardTemperaturesRequest(2),
            Response(0x04, 0x09, [2, 0xFB, 0xFF, 5, 0, 42, 0, 80, 0]));
        fake.EnqueueExchange(
            MainControllerProtocol.BuildBoardTargetTemperaturesRequest(2),
            Response(0x04, 0x0A, [2, 1, 0, 2, 0, 3, 0, 4, 0]));
        fake.EnqueueExchange(
            MainControllerProtocol.BuildBoardSwitchStatesRequest(2),
            Response(0x04, 0x0B, [2, 1, 0, 0, 0, 1, 0, 0, 0]));
        fake.EnqueueReceive(
            DeviceByteTransportEndpoints.MainController,
            Request(0x05, 0x04, [3, 1, 0]));
        fake.EnqueueExchange(
            MainControllerProtocol.BuildPwmSpeedsRequest(),
            Response(0x07, 0x06, [1, 0, 2, 0, 3, 0, 4, 0]));
        fake.EnqueueExchange(MainControllerProtocol.BuildMixerOriginRequest(1), Response(0x0A, 0x02, [1, 0, 0]));
        fake.EnqueueExchange(MainControllerProtocol.BuildMixerRemainingCountRequest(1), Response(0x0A, 0x03, [1, 9, 0]));
        fake.EnqueueExchange(MainControllerProtocol.BuildQrScanStatusRequest(), Response(0x08, 0x06, [1, 0]));
        fake.EnqueueExchange(
            MainControllerProtocol.BuildQrTextRequest(),
            Response(0x08, 0x01, Encoding.ASCII.GetBytes("ch1\r\nABC\r\n")));

        var work = await adapter.ReadControllerWorkStatusAsync();
        var nodeStatuses = await adapter.ReadControllerNodeStatusesAsync();
        var runTime = await adapter.ReadControllerRunTimeAsync();
        var temperatures = await adapter.ReadTemperaturesAsync(2);
        var targetTemperatures = await adapter.ReadTargetTemperaturesAsync(2);
        var switches = await adapter.ReadTemperatureSwitchesAsync(2);
        var liquid = await adapter.ReceiveLiquidLevelStatusAsync();
        var pwm = await adapter.ReadPwmSpeedsAsync();
        var origin = await adapter.ReadMixerOriginAsync(1);
        var remaining = await adapter.ReadMixerRemainingCountAsync(1);
        var qrStatus = await adapter.ReadQrScanStatusAsync();
        var qrText = await adapter.ReadQrTextAsync();

        Assert.Equal(1, work.Value!.Value);
        Assert.Equal(2, nodeStatuses.Value!.Values[7]);
        Assert.Equal<byte>([1, 2, 3, 4], runTime.Value!.RawValue);
        Assert.Equal<short>([-5, 5, 42, 80], temperatures.Value!.ValuesCelsius);
        Assert.Equal<short>([1, 2, 3, 4], targetTemperatures.Value!.ValuesCelsius);
        Assert.Equal<ushort>([1, 0, 1, 0], switches.Value!.Values);
        Assert.Equal(1, liquid.Value!.Value);
        Assert.Equal<ushort>([1, 2, 3, 4], pwm.Value!.ValuesRpm);
        Assert.Equal(0, origin.Value!.Value);
        Assert.Equal(9, remaining.Value!.Value);
        Assert.Equal(1, qrStatus.Value!.Value);
        Assert.Equal("ch1\r\nABC\r\n", qrText.Value!.Text);

        Assert.Equal(11, fake.ExchangeRequests.Count);
        Assert.Single(fake.ReceiveEndpoints);
        Assert.All(fake.ExchangeRequests, request =>
            Assert.Equal(DeviceByteTransportEndpoints.MainController, request.Endpoint));
    }

    [Fact]
    public async Task Main_controller_boundary_handles_ack_put_partial_sticky_crc_timeout_and_disconnect()
    {
        var fake = new InMemoryFakeDeviceByteTransport();
        var adapter = new UnavailableRealDeviceAdapter(fake);
        var ack = Response(0x01, 0x04, [0x01]);
        var put = Request(0x08, 0x03, Encoding.ASCII.GetBytes("ch1\r\nABC\r\n"));
        var response = Response(0x01, 0x08, [0x01]);
        var sticky = ack.Concat(put).Concat(response).ToArray();
        fake.EnqueueExchange(
            MainControllerProtocol.BuildWorkStatusRequest(),
            sticky[..5],
            sticky[5..17],
            sticky[17..]);

        var combined = await adapter.ReadControllerWorkStatusAsync();
        Assert.True(combined.Ok, combined.Message);
        Assert.Single(combined.Acknowledgements);
        Assert.True(combined.Acknowledgements[0].Succeeded);
        var putReport = Assert.Single(combined.PutReports);
        var qr = Assert.IsType<MainControllerQrText>(putReport.Value);
        Assert.Equal("ch1\r\nABC\r\n", qr.Text);

        var badCrc = Response(0x01, 0x08, [0x01]);
        badCrc[^3] ^= 0x01;
        fake.EnqueueExchange(MainControllerProtocol.BuildWorkStatusRequest(), badCrc);
        var crc = await adapter.ReadControllerWorkStatusAsync();
        Assert.False(crc.Ok);
        Assert.Contains(nameof(IceImmunoProtocolError.CrcMismatch), crc.ErrorCode);

        fake.EnqueueExchangeResult(
            MainControllerProtocol.BuildWorkStatusRequest(),
            new DeviceByteTransportResult(DeviceByteTransportStatuses.TimedOut, [], "controller_timeout", "Timed out."));
        var timeout = await adapter.ReadControllerWorkStatusAsync();
        Assert.Equal(DeviceCommandStatuses.TimedOut, timeout.Status);

        fake.EnqueueExchangeResult(
            MainControllerProtocol.BuildWorkStatusRequest(),
            new DeviceByteTransportResult(DeviceByteTransportStatuses.Disconnected, [], "controller_disconnected", "Disconnected."));
        var disconnected = await adapter.ReadControllerWorkStatusAsync();
        Assert.Equal(DeviceCommandStatuses.Offline, disconnected.Status);
    }

    [Fact]
    public async Task Dcr55_boundary_prepares_only_explicit_terminators_and_receives_single_multiple_and_timeout()
    {
        var fake = new InMemoryFakeDeviceByteTransport();
        var adapter = new UnavailableRealDeviceAdapter(fake);

        var missing = adapter.PrepareDcr55Trigger(Dcr55TriggerMode.Single, null);
        Assert.False(missing.Ok);
        Assert.Equal(DeviceCommandStatuses.NotConfigured, missing.Status);
        Assert.Empty(missing.CommandBytes);
        Assert.False(adapter.PrepareDcr55Trigger(Dcr55TriggerMode.Single, []).Ok);

        var prepared = adapter.PrepareDcr55Trigger(Dcr55TriggerMode.Single, [0x03]);
        Assert.True(prepared.Ok);
        Assert.False(prepared.Sent);
        Assert.Equal(
            Encoding.ASCII.GetBytes(Dcr55Protocol.SingleTriggerCommandText).Append((byte)0x03),
            prepared.CommandBytes);
        Assert.Empty(fake.ExchangeRequests);

        fake.EnqueueReceive(DeviceByteTransportEndpoints.Dcr55, Encoding.ASCII.GetBytes("SAMPLE-001\r\n"));
        fake.EnqueueReceive(
            DeviceByteTransportEndpoints.Dcr55,
            Encoding.ASCII.GetBytes("SAMPLE-002\r\n"),
            Encoding.ASCII.GetBytes("SAMPLE-003\r\n"));
        fake.EnqueueReceiveResult(
            DeviceByteTransportEndpoints.Dcr55,
            new DeviceByteTransportResult(DeviceByteTransportStatuses.TimedOut, [], "dcr55_timeout", "No barcode."));

        var single = await adapter.ReceiveDcr55ResultAsync();
        Assert.Equal(["SAMPLE-001"], single.Value!.Barcodes);
        var multiple = await adapter.ReceiveDcr55ResultAsync();
        Assert.Equal(["SAMPLE-002", "SAMPLE-003"], multiple.Value!.Barcodes);
        var timeout = await adapter.ReceiveDcr55ResultAsync();
        Assert.False(timeout.Ok);
        Assert.Equal(DeviceCommandStatuses.TimedOut, timeout.Status);
        Assert.Equal(Dcr55ScanOutcome.NoBarcodeTimeout, timeout.Value!.Outcome);
    }

    [Fact]
    public async Task Cooling_boundary_reads_temperature_and_fails_closed_for_invalid_frame_and_timeout()
    {
        var fake = new InMemoryFakeDeviceByteTransport();
        var adapter = new UnavailableRealDeviceAdapter(fake);
        var request = StandaloneCoolingProtocol.BuildReadTemperatureFrame();
        fake.EnqueueExchange(request, [0xFF, 0x00], [0x05, 0xFA]);
        fake.EnqueueExchange(request, [0xFF, 0x00, 0x05, 0xFB]);
        fake.EnqueueExchangeResult(
            request,
            new DeviceByteTransportResult(DeviceByteTransportStatuses.TimedOut, [], "cooling_timeout", "Timed out."));

        var temperature = await adapter.ReadCoolingTemperatureAsync();
        Assert.True(temperature.Ok, temperature.Message);
        Assert.Equal<byte?>(5, temperature.Value!.TemperatureCelsius);

        var invalid = await adapter.ReadCoolingTemperatureAsync();
        Assert.False(invalid.Ok);
        Assert.Equal("cooling_invalid_response", invalid.ErrorCode);

        var timeout = await adapter.ReadCoolingTemperatureAsync();
        Assert.False(timeout.Ok);
        Assert.Equal(DeviceCommandStatuses.TimedOut, timeout.Status);
    }

    [Fact]
    public async Task Real_adapter_without_transport_is_not_configured_and_all_formal_control_paths_reject_without_io()
    {
        var unavailable = new UnavailableRealDeviceAdapter();
        var status = await unavailable.GetStatusAsync();
        Assert.False(status.Ready);
        Assert.Equal(DeviceConnectionStatuses.NotConfigured, Assert.Single(status.Modules).ConnectionStatus);
        var read = await unavailable.ReadControllerWorkStatusAsync();
        Assert.False(read.Ok);
        Assert.Equal(DeviceCommandStatuses.NotConfigured, read.Status);

        var fake = new InMemoryFakeDeviceByteTransport();
        IDeviceAdapter adapter = new UnavailableRealDeviceAdapter(fake);
        var requests = new[]
        {
            RequestFor(DeviceModules.Controller, "reset"),
            RequestFor(DeviceModules.Temperature, "set-target"),
            RequestFor(DeviceModules.Temperature, "set-switch"),
            RequestFor(DeviceModules.Pump, "write-pwm"),
            RequestFor(DeviceModules.Mixer, "start"),
            RequestFor(DeviceModules.LiquidLevel, "write-io"),
            RequestFor(DeviceModules.Pump, "drain"),
            RequestFor(DeviceModules.Pump, "detox"),
            RequestFor(DeviceModules.ReagentScanner, ReagentQrCommands.StartScan),
            RequestFor(DeviceModules.Cooling, "set-target"),
            RequestFor(DeviceModules.Cooling, "start"),
            RequestFor(DeviceModules.Cooling, "stop"),
            RequestFor(DeviceModules.SampleScanner, "trigger"),
            RequestFor(DeviceModules.Workflow, "execute")
        };

        var results = new[]
        {
            await adapter.InitializeModuleAsync(requests[0]),
            await adapter.SetTemperatureAsync(requests[1]),
            await adapter.SetTemperatureAsync(requests[2]),
            await adapter.RunPumpAsync(requests[3]),
            await adapter.MixAsync(requests[4]),
            await adapter.ReadLiquidLevelsAsync(requests[5]),
            await adapter.RunPumpAsync(requests[6]),
            await adapter.RunPumpAsync(requests[7]),
            await adapter.ScanReagentAsync(requests[8]),
            await adapter.SetCoolingAsync(requests[9]),
            await adapter.SetCoolingAsync(requests[10]),
            await adapter.SetCoolingAsync(requests[11]),
            await adapter.ScanSampleAsync(requests[12]),
            await adapter.ExecuteWorkflowActionAsync(requests[13])
        };

        Assert.All(results, result =>
        {
            Assert.False(result.Ok);
            Assert.Equal(DeviceCommandStatuses.NotSupported, result.Status);
            Assert.False(result.Acknowledged);
        });
        Assert.Empty(fake.ExchangeRequests);
        Assert.Empty(fake.ReceiveEndpoints);
    }

    [Fact]
    public async Task Real_mode_di_reuses_one_adapter_and_only_test_injected_fake_enables_offline_reads()
    {
        var root = Path.Combine(Path.GetTempPath(), "stainer-real-boundary", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        var fake = new InMemoryFakeDeviceByteTransport();
        fake.EnqueueExchange(MainControllerProtocol.BuildWorkStatusRequest(), Response(0x01, 0x08, [0x01]));

        await using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.UseSetting("Device:Mode", DeviceModes.Real);
            builder.UseSetting("Device:HardwareAvailable", "true");
            builder.UseSetting("Device:UseMockWhenHardwareUnavailable", "false");
            builder.UseSetting("Device:StartupInitialization:Enabled", "false");
            builder.UseSetting("ConnectionStrings:StainerDatabase", $"Data Source={Path.Combine(root, "stainer.db")}");
            builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Device:Mode"] = DeviceModes.Real,
                ["Device:HardwareAvailable"] = "true",
                ["Device:UseMockWhenHardwareUnavailable"] = "false",
                ["Device:StartupInitialization:Enabled"] = "false",
                ["ConnectionStrings:StainerDatabase"] = $"Data Source={Path.Combine(root, "stainer.db")}",
                ["MachineExecutor:LeasePath"] = Path.Combine(root, "machine-executor.lock"),
                ["Safety:LogDirectory"] = Path.Combine(root, "logs")
            }));
            builder.ConfigureServices(services => services.AddSingleton<IDeviceByteTransport>(fake));
        });

        var formal = factory.Services.GetRequiredService<IDeviceAdapter>();
        var reads = factory.Services.GetRequiredService<IRealDeviceReadAdapter>();
        Assert.IsType<UnavailableRealDeviceAdapter>(formal);
        Assert.Same(formal, reads);
        var status = await formal.GetStatusAsync();
        Assert.False(status.Ready);
        Assert.Equal(DeviceConnectionStatuses.Offline, Assert.Single(status.Modules).ConnectionStatus);
        Assert.True((await reads.ReadControllerWorkStatusAsync()).Ok);
    }

    private static DeviceOperationRequest RequestFor(string moduleCode, string action) =>
        new(
            new DeviceCommandContext($"cmd-{moduleCode}-{action}", null, "test", nameof(OfflineRealDeviceAdapterTests)),
            moduleCode,
            action,
            new Dictionary<string, object?>());

    private static byte[] Response(byte parentClass, byte subClass, byte[] payload) =>
        IceImmunoSerialProtocol.EncodeFrame(parentClass, subClass, IceImmunoSerialProtocol.ResponseType, payload);

    private static byte[] Request(byte parentClass, byte subClass, byte[] payload) =>
        IceImmunoSerialProtocol.EncodeFrame(parentClass, subClass, IceImmunoSerialProtocol.RequestType, payload);

    private sealed class InMemoryFakeDeviceByteTransport : IDeviceByteTransport
    {
        private readonly Queue<ExchangeScript> exchangeScripts = new();
        private readonly Queue<ReceiveScript> receiveScripts = new();

        public string Name => nameof(InMemoryFakeDeviceByteTransport);
        public bool IsConfigured => true;
        public List<DeviceByteTransportRequest> ExchangeRequests { get; } = [];
        public List<string> ReceiveEndpoints { get; } = [];

        public void EnqueueExchange(byte[] expectedRequest, params byte[][] chunks) =>
            EnqueueExchangeResult(
                expectedRequest,
                new DeviceByteTransportResult(DeviceByteTransportStatuses.Succeeded, chunks));

        public void EnqueueExchangeResult(byte[] expectedRequest, DeviceByteTransportResult result) =>
            exchangeScripts.Enqueue(new ExchangeScript(expectedRequest, result));

        public void EnqueueReceive(string endpoint, params byte[][] chunks) =>
            EnqueueReceiveResult(endpoint, new DeviceByteTransportResult(DeviceByteTransportStatuses.Succeeded, chunks));

        public void EnqueueReceiveResult(string endpoint, DeviceByteTransportResult result) =>
            receiveScripts.Enqueue(new ReceiveScript(endpoint, result));

        public Task<DeviceByteTransportResult> ExchangeAsync(
            DeviceByteTransportRequest request,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (exchangeScripts.Count == 0)
            {
                throw new InvalidOperationException($"Unexpected transport exchange for {request.Operation}.");
            }

            var script = exchangeScripts.Dequeue();
            if (!request.RequestBytes.SequenceEqual(script.ExpectedRequest))
            {
                throw new InvalidOperationException(
                    $"Unexpected request bytes. Expected {Convert.ToHexString(script.ExpectedRequest)}, actual {Convert.ToHexString(request.RequestBytes)}.");
            }

            ExchangeRequests.Add(request);
            return Task.FromResult(script.Result);
        }

        public Task<DeviceByteTransportResult> ReceiveAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (receiveScripts.Count == 0)
            {
                throw new InvalidOperationException($"Unexpected transport receive for {endpoint}.");
            }

            var script = receiveScripts.Dequeue();
            if (!string.Equals(endpoint, script.Endpoint, StringComparison.Ordinal))
            {
                throw new InvalidOperationException($"Unexpected receive endpoint {endpoint}; expected {script.Endpoint}.");
            }

            ReceiveEndpoints.Add(endpoint);
            return Task.FromResult(script.Result);
        }

        private sealed record ExchangeScript(byte[] ExpectedRequest, DeviceByteTransportResult Result);
        private sealed record ReceiveScript(string Endpoint, DeviceByteTransportResult Result);
    }
}
