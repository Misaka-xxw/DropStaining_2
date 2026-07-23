using System.Buffers.Binary;
using System.IO.Pipes;
using System.Text.Json;
using Stainer.Web.Application.Devices;
using Stainer.Web.Infrastructure.Devices;

namespace Stainer.Tests;

public sealed class SoconReagentHardwareActionClientTests
{
    [Fact]
    public async Task Aspirate_executes_safe_xyz_action_return_and_close_over_length_prefixed_pipe()
    {
        var pipeName = "Stainer.SoconBridge.Test." + Guid.NewGuid().ToString("N");
        var commands = new List<JsonElement>();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        var server = ServeSuccessAsync(pipeName, 8, commands, timeout.Token);
        var client = new SoconReagentHardwareActionClient(new SoconReagentHardwareOptions
        {
            Enabled = true,
            PipeName = pipeName,
            ConnectTimeoutMilliseconds = 2000,
            ResponseTimeoutMilliseconds = 2000,
            XYSpeedMmPerSecond = 40,
            ZSpeedMmPerSecond = 20
        });

        var result = await client.ExecuteAsync(
            new ReagentHardwareActionRequest(
                ReagentHardwareActionOperations.Aspirate,
                "z1",
                12_500,
                23_750,
                90_000,
                15_000,
                100),
            timeout.Token);
        await server;

        Assert.True(result.Ok, result.Message);
        Assert.Equal(
            [
                "OpenConfiguredReadOnlySession",
                "MoveConfiguredAxis",
                "MoveConfiguredAxis",
                "MoveConfiguredAxis",
                "MoveConfiguredAxis",
                "AspirateConfigured",
                "MoveConfiguredAxis",
                "CloseConfiguredReadOnlySession"
            ],
            commands.Select(Command).ToArray());
        Assert.Equal(90d, commands[1].GetProperty("positionMm").GetDouble());
        Assert.Equal(12.5d, commands[2].GetProperty("positionMm").GetDouble());
        Assert.Equal(23.75d, commands[3].GetProperty("positionMm").GetDouble());
        Assert.Equal(15d, commands[4].GetProperty("positionMm").GetDouble());
        Assert.Equal(100, commands[5].GetProperty("volumeUl").GetInt32());
        Assert.Equal("z1", commands[5].GetProperty("axis").GetString());
    }

    [Fact]
    public async Task Disabled_client_fails_before_pipe_connection()
    {
        var client = new SoconReagentHardwareActionClient(new SoconReagentHardwareOptions());

        var result = await client.ExecuteAsync(new ReagentHardwareActionRequest(
            ReagentHardwareActionOperations.Move,
            "z1",
            1,
            1,
            1,
            1));

        Assert.False(result.Ok);
        Assert.Equal(DeviceCommandStatuses.NotConfigured, result.Status);
        Assert.Equal("socon_reagent_actions_disabled", result.ErrorCode);
    }

    private static async Task ServeSuccessAsync(
        string pipeName,
        int requestCount,
        List<JsonElement> commands,
        CancellationToken cancellationToken)
    {
        for (var i = 0; i < requestCount; i++)
        {
            await using var server = new NamedPipeServerStream(
                pipeName,
                PipeDirection.InOut,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous);
            await server.WaitForConnectionAsync(cancellationToken);
            var prefix = new byte[4];
            await ReadExactAsync(server, prefix, cancellationToken);
            var length = BinaryPrimitives.ReadInt32LittleEndian(prefix);
            var payload = new byte[length];
            await ReadExactAsync(server, payload, cancellationToken);
            using var requestDocument = JsonDocument.Parse(payload);
            var request = requestDocument.RootElement.Clone();
            commands.Add(request);

            var response = JsonSerializer.SerializeToUtf8Bytes(new
            {
                requestId = request.GetProperty("requestId").GetString(),
                command = Command(request),
                success = true,
                bridgeStatus = "DeploymentValidated",
                message = "OK",
                details = new { },
                warnings = Array.Empty<string>()
            });
            BinaryPrimitives.WriteInt32LittleEndian(prefix, response.Length);
            await server.WriteAsync(prefix, cancellationToken);
            await server.WriteAsync(response, cancellationToken);
            await server.FlushAsync(cancellationToken);
        }
    }

    private static async Task ReadExactAsync(Stream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        var offset = 0;
        while (offset < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(offset), cancellationToken);
            if (read == 0) throw new EndOfStreamException();
            offset += read;
        }
    }

    private static string Command(JsonElement request) =>
        request.GetProperty("command").GetString() ?? string.Empty;
}
