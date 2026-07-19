using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stainer.Web.Application.Devices;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Services;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Tests;

public sealed class EngineeringPipettingApiTests
{
    [Fact]
    public async Task Engineering_pipetting_tests_require_session_and_record_mock_operations()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin", "admin");
        var setup = await LoadSetupAsync(factory);

        var catalog = await client.GetFromJsonAsync<EngineeringPipettingTestCatalogResponse>("/api/engineering/pipetting-tests/types");
        Assert.NotNull(catalog);
        Assert.Contains(catalog!.Operations, x => x.OperationType == PipettingOperationTypes.Aspirate && x.IsExecutable);

        var noSession = await client.PostAsJsonAsync("/api/engineering/pipetting-tests/aspirate", new
        {
            commandId = "cmd-eng-pipette-no-session",
            channel = "A",
            position = setup.PositionCode,
            volumeUl = 20,
            liquidClassVersionId = setup.LiquidClassVersionId,
            reason = "blocked without engineering session"
        });
        Assert.Equal(HttpStatusCode.Forbidden, noSession.StatusCode);

        await OpenEngineeringSessionAsync(client, "pipetting-api");

        var aspirate = await PostAsync<EngineeringPipettingTestResponse>(client, "/api/engineering/pipetting-tests/aspirate", new
        {
            commandId = "cmd-eng-pipette-aspirate",
            channel = "A",
            needleCode = "Needle1",
            position = setup.PositionCode,
            volumeUl = 50,
            liquidClassVersionId = setup.LiquidClassVersionId,
            reason = "manual aspirate engineering test",
            operationParameters = new
            {
                sourceType = NeedleLoadSourceTypes.SystemLiquid,
                systemLiquidSourceType = "SystemWater"
            }
        });
        Assert.True(aspirate.Ok, aspirate.Message);
        Assert.Equal(PipettingOperationTypes.Aspirate, aspirate.OperationType);

        var dispense = await PostAsync<EngineeringPipettingTestResponse>(client, "/api/engineering/pipetting-tests/dispense", new
        {
            commandId = "cmd-eng-pipette-dispense",
            channel = "A",
            needleCode = "Needle1",
            position = setup.PositionCode,
            volumeUl = 20,
            liquidClassVersionId = setup.LiquidClassVersionId,
            reason = "manual dispense engineering test"
        });
        Assert.True(dispense.Ok, dispense.Message);
        var needleVolume = Assert.IsType<JsonElement>(dispense.State["needleVolumeUl"]);
        Assert.Equal(30, needleVolume.GetInt32());

        var wash = await PostAsync<EngineeringPipettingTestResponse>(client, "/api/engineering/pipetting-tests/wash", new
        {
            commandId = "cmd-eng-pipette-wash",
            channel = "A",
            needleCode = "Needle1",
            position = setup.PositionCode,
            reason = "manual wash engineering test"
        });
        Assert.True(wash.Ok, wash.Message);
        Assert.Equal(PipettingOperationTypes.WashNeedle, wash.OperationType);

        await using var scope = factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
        var operations = await dbContext.PipettingOperations
            .Where(x => x.DeviceCommandExecutionId != null && x.DeviceCommandExecutionId.StartsWith("cmd-eng-pipette-"))
            .ToListAsync();
        operations = operations.OrderBy(x => x.CreatedAtUtc).ToList();
        Assert.Equal(3, operations.Count);
        Assert.All(operations, x =>
        {
            Assert.Equal("A", x.ChannelCode);
            Assert.Equal("Needle1", x.NeedleCode);
            Assert.Equal(setup.PositionCode, x.TargetPointCode);
            Assert.False(string.IsNullOrWhiteSpace(x.ActorUserId));
            Assert.Contains("operationType", x.ParametersJson);
        });
        Assert.Contains(operations, x => x.OperationType == PipettingOperationTypes.Aspirate && x.VolumeUl == 50 && x.Status == DeviceCommandStatus.Completed);
        Assert.Contains(operations, x => x.OperationType == PipettingOperationTypes.Dispense && x.VolumeUl == 20 && x.Status == DeviceCommandStatus.Completed);
        Assert.Contains(operations, x => x.OperationType == PipettingOperationTypes.WashNeedle && x.Status == DeviceCommandStatus.Completed);
        Assert.True(await dbContext.AuditLogs.AnyAsync(x => x.Action == "engineering.pipetting.aspirate"));
        Assert.Equal(0, (await dbContext.NeedleStates.SingleAsync(x => x.NeedleCode == NeedleCodes.Needle1)).VolumeUl);
    }

    [Fact]
    public async Task Engineering_pipetting_tests_fail_closed_in_real_mode()
    {
        await using var factory = CreateFactory(new Dictionary<string, string?>
        {
            ["Device:Mode"] = DeviceModes.Real,
            ["Device:HardwareAvailable"] = "true",
            ["Device:UseMockWhenHardwareUnavailable"] = "false",
            ["Device:DebugMode"] = "false"
        });
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin", "admin");
        await OpenEngineeringSessionAsync(client, "pipetting-real-blocked");
        var setup = await LoadSetupAsync(factory);

        var response = await client.PostAsJsonAsync("/api/engineering/pipetting-tests/liquid-detect", new
        {
            commandId = "cmd-eng-pipette-real-blocked",
            channel = "A",
            position = setup.PositionCode,
            reason = "real mode must fail closed"
        });
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

        await using var scope = factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
        Assert.False(await dbContext.PipettingOperations.AnyAsync(x => x.DeviceCommandExecutionId == "cmd-eng-pipette-real-blocked"));
    }

    private static async Task<(string PositionCode, string LiquidClassVersionId)> LoadSetupAsync(WebApplicationFactory<Program> factory)
    {
        await using var scope = factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
        var point = await dbContext.CoordinatePoints
            .Include(x => x.CoordinateProfileVersion)
            .Where(x => x.IsEnabled && x.CoordinateProfileVersion != null && x.CoordinateProfileVersion.IsActive)
            .OrderBy(x => x.PointCode)
            .FirstAsync();
        var liquidClassVersion = await dbContext.LiquidClassVersions
            .Include(x => x.LiquidClassProfile)
            .Where(x => x.LiquidClassProfile != null && x.LiquidClassProfile.EnabledVersionId == x.Id)
            .OrderBy(x => x.VersionNo)
            .FirstAsync();
        return (point.PointCode, liquidClassVersion.Id);
    }

    private static WebApplicationFactory<Program> CreateFactory(Dictionary<string, string?>? overrides = null)
    {
        var databasePath = Path.Combine(TestPaths.TempRoot, "stainer-engineering-pipetting-tests", Guid.NewGuid().ToString("N"), "stainer.db");
        var settings = new Dictionary<string, string?>
        {
            ["ConnectionStrings:StainerDatabase"] = $"Data Source={databasePath}",
            ["MachineExecutor:LeasePath"] = Path.Combine(Path.GetDirectoryName(databasePath)!, $"machine-executor-{Guid.NewGuid():N}.lock"),
            ["Safety:LogDirectory"] = Path.Combine(Path.GetDirectoryName(databasePath)!, "logs"),
            ["Device:Mode"] = DeviceModes.Mock,
            ["Device:StartupInitialization:Enabled"] = "false"
        };
        if (overrides is not null)
        {
            foreach (var pair in overrides)
            {
                settings[pair.Key] = pair.Value;
            }
        }

        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            foreach (var pair in settings)
            {
                builder.UseSetting(pair.Key, pair.Value);
            }

            builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(settings));
        });
    }

    private static async Task LoginAsync(HttpClient client, string username, string role)
    {
        var response = await client.PostAsJsonAsync("/api/login", new { username, password = "123456", role });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static async Task<EngineeringSessionResponse> OpenEngineeringSessionAsync(HttpClient client, string suffix)
    {
        return await PostAsync<EngineeringSessionResponse>(client, "/api/engineering/session", new
        {
            commandId = $"cmd-eng-session-{suffix}",
            password = "123456",
            reason = $"engineering pipetting test {suffix}",
            target = "engineering-pipetting"
        });
    }

    private static async Task<T> PostAsync<T>(HttpClient client, string url, object request)
    {
        var response = await client.PostAsJsonAsync(url, request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<T>();
        Assert.NotNull(body);
        return body!;
    }
}
