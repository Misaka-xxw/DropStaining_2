using System.Text;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.Devices;
using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Tests;

public sealed class ScannerControlServiceTests
{
    private static readonly AuthenticatedUser Engineer = new(string.Empty, "engineer", "Engineer", "engineer", ["engineer"]);

    [Fact]
    public async Task Apply_roi_sends_documented_dcr55_roi_commands_and_records_audit()
    {
        var transport = new FakeDcr55ControlTransport();
        await using var context = await CreateServiceContextAsync(transport);

        var response = await context.Service.ApplyRoiAsync(
            context.Profile.Id,
            new ScannerRoiRequest("cmd-scanner-roi", 0, 0, 1280, 960, "apply documented ROI"),
            Engineer);

        Assert.True(response.Ok);
        Assert.Equal(DeviceByteTransportStatuses.Succeeded, response.Status);
        Assert.Equal(
            ["CDOPSRW1280\r", "CDOPSRH960\r", "CDOPSRL0\r", "CDOPSRT0\r"],
            transport.Requests.Select(x => x.CommandText).ToArray());
        Assert.Equal(
            ["roi.width", "roi.height", "roi.left", "roi.top"],
            transport.Requests.Select(x => x.Operation).ToArray());
        Assert.All(transport.Requests, x => Assert.Equal(DeviceByteTransportEndpoints.Dcr55, x.Endpoint));
        Assert.Contains(response.Steps, x => x.Operation == "roi.width" && x.CommandText == "CDOPSRW1280\r");
        Assert.True(await context.DbContext.AuditLogs.AnyAsync(x =>
            x.Action == "scanner_control.roi.apply"
            && x.EntityId == context.Profile.Id
            && x.Message.Contains("apply documented ROI")));
    }

    [Fact]
    public async Task Restart_and_calibration_light_use_documented_dcr55_commands()
    {
        var transport = new FakeDcr55ControlTransport();
        await using var context = await CreateServiceContextAsync(transport);

        var restart = await context.Service.RestartScannerAsync(
            context.Profile.Id,
            new ScannerRestartRequest("cmd-scanner-restart", "restart scanner"),
            Engineer);
        var enabled = await context.Service.EnableCalibrationLightAsync(
            context.Profile.Id,
            new ScannerCalibrationLightRequest("cmd-scanner-light-enable", "enable calibration light"),
            Engineer);
        var disabled = await context.Service.DisableCalibrationLightAsync(
            context.Profile.Id,
            new ScannerCalibrationLightRequest("cmd-scanner-light-disable", "disable calibration light"),
            Engineer);

        Assert.True(restart.Ok);
        Assert.True(enabled.Ok);
        Assert.True(disabled.Ok);
        Assert.Equal(
            ["RDCMXRB1\r", "RDCMXEV2,P11\r", "RDCMXEV2,P10\r"],
            transport.Requests.Select(x => x.CommandText).ToArray());
        Assert.Equal(3, await context.DbContext.AuditLogs.CountAsync(x => x.Action.StartsWith("scanner_control.")));
    }

    [Fact]
    public async Task Unconfigured_transport_returns_clear_error_without_sending_command()
    {
        var transport = new FakeDcr55ControlTransport(isConfigured: false);
        await using var context = await CreateServiceContextAsync(transport);

        var response = await context.Service.RestartScannerAsync(
            context.Profile.Id,
            new ScannerRestartRequest("cmd-scanner-offline", "offline should be explicit"),
            Engineer);

        Assert.False(response.Ok);
        Assert.Equal(DeviceByteTransportStatuses.NotConnected, response.Status);
        Assert.Equal("dcr55_control_transport_not_configured", response.ErrorCode);
        Assert.Empty(transport.Requests);
        Assert.True(await context.DbContext.AuditLogs.AnyAsync(x =>
            x.Action == "scanner_control.restart"
            && x.Message.Contains("dcr55_control_transport_not_configured")));
    }

    [Fact]
    public async Task Invalid_roi_is_rejected_before_transport_io()
    {
        var transport = new FakeDcr55ControlTransport();
        await using var context = await CreateServiceContextAsync(transport);

        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            context.Service.ApplyRoiAsync(
                context.Profile.Id,
                new ScannerRoiRequest("cmd-scanner-bad-roi", 0, 0, 0, 960, "invalid ROI"),
                Engineer));

        Assert.Equal("scanner_roi_size_invalid", exception.Code);
        Assert.Empty(transport.Requests);
    }

    private static async Task<ScannerControlTestContext> CreateServiceContextAsync(FakeDcr55ControlTransport transport)
    {
        var databasePath = Path.Combine(Path.GetTempPath(), "stainer-scanner-control-tests", Guid.NewGuid().ToString("N"), "stainer.db");
        Directory.CreateDirectory(Path.GetDirectoryName(databasePath)!);
        var options = new DbContextOptionsBuilder<StainerDbContext>()
            .UseSqlite($"Data Source={databasePath}")
            .Options;
        var dbContext = new StainerDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();
        var profile = new ScannerProfile
        {
            Name = "DCR55 sample scanner",
            ScannerType = ScannerTypes.Dcr55,
            Enabled = true,
            Port = "MOCK-COM",
            BaudRate = 115200,
            TimeoutMilliseconds = 1000,
            TriggerMode = ScannerTriggerModes.Software,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
        dbContext.ScannerProfiles.Add(profile);
        await dbContext.SaveChangesAsync();
        var service = new ScannerControlService(
            dbContext,
            new CommandIdempotencyService(dbContext),
            [transport]);
        return new ScannerControlTestContext(dbContext, service, profile);
    }

    private sealed record ScannerControlTestContext(
        StainerDbContext DbContext,
        ScannerControlService Service,
        ScannerProfile Profile) : IAsyncDisposable
    {
        public async ValueTask DisposeAsync()
        {
            await DbContext.DisposeAsync();
        }
    }

    private sealed class FakeDcr55ControlTransport(bool isConfigured = true) : IDeviceByteTransport
    {
        public string Name => "fake-dcr55-control";
        public bool IsConfigured { get; } = isConfigured;
        public List<RecordedRequest> Requests { get; } = [];

        public Task<DeviceByteTransportResult> ExchangeAsync(
            DeviceByteTransportRequest request,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Requests.Add(new RecordedRequest(
                request.Endpoint,
                request.Operation,
                Encoding.ASCII.GetString(request.RequestBytes)));
            return Task.FromResult(new DeviceByteTransportResult(
                DeviceByteTransportStatuses.Succeeded,
                [Encoding.ASCII.GetBytes("OK\r\n")]));
        }

        public Task<DeviceByteTransportResult> ReceiveAsync(
            string endpoint,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(new DeviceByteTransportResult(
                DeviceByteTransportStatuses.Failed,
                [],
                "fake_receive_not_supported",
                "Scanner control tests do not receive barcode data."));
    }

    private sealed record RecordedRequest(string Endpoint, string Operation, string CommandText);
}
