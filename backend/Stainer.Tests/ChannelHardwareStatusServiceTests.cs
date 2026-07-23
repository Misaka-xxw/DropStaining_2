using Stainer.Web.Application.Devices;
using Stainer.Web.Application.Services;

namespace Stainer.Tests;

public sealed class ChannelHardwareStatusServiceTests
{
    [Fact]
    public async Task Read_maps_configured_nodes_without_touching_business_channel_state()
    {
        var nodes = Enumerable.Range(0, 64).Select(value => (byte)value).ToArray();
        var reader = new FakeReader(
            Success(new MainControllerWorkStatus(3)),
            Success(new MainControllerNodeStatuses(nodes)));
        var service = new ChannelHardwareStatusService(reader, EnabledOptions());

        var result = await service.ReadAsync();

        Assert.True(result.Ok);
        Assert.Equal((byte)3, result.ControllerWorkStatus);
        Assert.Collection(
            result.Channels,
            channel => Assert.Equal<byte>([0, 1], channel.RawNodeStatuses),
            channel => Assert.Equal<byte>([2], channel.RawNodeStatuses),
            channel => Assert.Equal<byte>([3, 4], channel.RawNodeStatuses),
            channel => Assert.Equal<byte>([5], channel.RawNodeStatuses));
        Assert.Equal(1, reader.WorkReadCount);
        Assert.Equal(1, reader.NodeReadCount);
    }

    [Fact]
    public async Task Read_is_fail_closed_when_mapping_is_disabled_or_invalid()
    {
        var reader = new FakeReader(
            Success(new MainControllerWorkStatus(1)),
            Success(new MainControllerNodeStatuses(new byte[64])));
        var disabled = new ChannelHardwareStatusService(reader, new ChannelHardwareStatusOptions());

        var disabledResult = await disabled.ReadAsync();

        Assert.False(disabledResult.Ok);
        Assert.Equal(DeviceCommandStatuses.NotConfigured, disabledResult.Status);
        Assert.Equal("channel_hardware_status_disabled", disabledResult.ErrorCode);
        Assert.Equal(0, reader.WorkReadCount);
        Assert.Equal(0, reader.NodeReadCount);

        var invalidOptions = EnabledOptions();
        invalidOptions.Mappings[0].NodeIndexes = [64];
        var invalid = await new ChannelHardwareStatusService(reader, invalidOptions).ReadAsync();
        Assert.False(invalid.Ok);
        Assert.Equal("channel_hardware_node_index_invalid", invalid.ErrorCode);
        Assert.Equal(0, reader.WorkReadCount);
    }

    [Fact]
    public async Task Read_propagates_device_failure_and_does_not_report_partial_channel_state()
    {
        var reader = new FakeReader(
            Success(new MainControllerWorkStatus(1)),
            Failure<MainControllerNodeStatuses>(DeviceCommandStatuses.TimedOut, "controller_timeout"));
        var service = new ChannelHardwareStatusService(reader, EnabledOptions());

        var result = await service.ReadAsync();

        Assert.False(result.Ok);
        Assert.Equal(DeviceCommandStatuses.TimedOut, result.Status);
        Assert.Equal("controller_timeout", result.ErrorCode);
        Assert.Equal((byte)1, result.ControllerWorkStatus);
        Assert.Empty(result.Channels);
    }

    private static ChannelHardwareStatusOptions EnabledOptions() => new()
    {
        Enabled = true,
        Mappings =
        [
            new() { ChannelCode = "A", NodeIndexes = [0, 1] },
            new() { ChannelCode = "B", NodeIndexes = [2] },
            new() { ChannelCode = "C", NodeIndexes = [3, 4] },
            new() { ChannelCode = "D", NodeIndexes = [5] }
        ]
    };

    private static RealDeviceReadResult<T> Success<T>(T value) =>
        new(true, DeviceCommandStatuses.Succeeded, null, "ok", value, [], [], [], []);

    private static RealDeviceReadResult<T> Failure<T>(string status, string errorCode) =>
        new(false, status, errorCode, "failed", default, [], [], [], []);

    private sealed class FakeReader(
        RealDeviceReadResult<MainControllerWorkStatus> work,
        RealDeviceReadResult<MainControllerNodeStatuses> nodes) : IChannelHardwareStatusDeviceReader
    {
        public int WorkReadCount { get; private set; }
        public int NodeReadCount { get; private set; }

        public Task<RealDeviceReadResult<MainControllerWorkStatus>> ReadControllerWorkStatusAsync(CancellationToken cancellationToken = default)
        {
            WorkReadCount++;
            return Task.FromResult(work);
        }

        public Task<RealDeviceReadResult<MainControllerNodeStatuses>> ReadControllerNodeStatusesAsync(CancellationToken cancellationToken = default)
        {
            NodeReadCount++;
            return Task.FromResult(nodes);
        }
    }
}
