using System.Buffers.Binary;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using Stainer.Web.Application.Devices;

namespace Stainer.Web.Infrastructure.Devices;

public sealed class SoconReagentHardwareOptions
{
    public bool Enabled { get; set; }
    public string PipeName { get; set; } = "Stainer.SoconBridge";
    public int ConnectTimeoutMilliseconds { get; set; } = 2_000;
    public int ResponseTimeoutMilliseconds { get; set; } = 30_000;
    public double XYSpeedMmPerSecond { get; set; } = 50;
    public double ZSpeedMmPerSecond { get; set; } = 30;
}

/// <summary>
/// .NET 9 主进程到 x86 SOCON Bridge 的窄动作客户端。
/// 只发送逻辑轴和工程量；COM、NodeID、SDK 路径均由 Bridge 本地配置解析。
/// </summary>
public sealed class SoconReagentHardwareActionClient(SoconReagentHardwareOptions options) : IReagentHardwareActionClient
{
    private const int MaximumPayloadBytes = 64 * 1024;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly SemaphoreSlim gate = new(1, 1);

    public async Task<ReagentHardwareActionResult> ExecuteAsync(
        ReagentHardwareActionRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationError = Validate(request);
        if (validationError is not null)
        {
            return Failed(DeviceCommandStatuses.NotConfigured, validationError, "SOCON reagent hardware action is not configured.");
        }

        await gate.WaitAsync(cancellationToken);
        var completed = new List<string>();
        var sessionOpened = false;
        var sessionClosed = false;
        try
        {
            var open = await SendAsync("OpenConfiguredReadOnlySession", null, cancellationToken);
            if (!open.Success) return FromBridge(open, completed);
            sessionOpened = true;
            completed.Add("OpenSession");

            var steps = BuildSteps(request);
            foreach (var step in steps)
            {
                var response = await SendAsync(step.Command, step.Payload, cancellationToken);
                if (!response.Success)
                {
                    await TryStopAsync(request.Axis, cancellationToken);
                    return FromBridge(response, completed);
                }
                completed.Add(step.Name);
            }

            var close = await SendAsync("CloseConfiguredReadOnlySession", null, cancellationToken);
            if (!close.Success)
            {
                return FromBridge(close, completed);
            }
            sessionClosed = true;
            completed.Add("CloseSession");
            return new ReagentHardwareActionResult(
                true,
                DeviceCommandStatuses.Succeeded,
                null,
                "SOCON reagent hardware action completed.",
                completed);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (TimeoutException)
        {
            return Failed(DeviceCommandStatuses.TimedOut, "socon_bridge_timeout", "SOCON Bridge timed out.", completed);
        }
        catch (IOException)
        {
            return Failed(DeviceCommandStatuses.Offline, "socon_bridge_io_failed", "SOCON Bridge is unavailable.", completed);
        }
        catch (Exception)
        {
            return Failed(DeviceCommandStatuses.Failed, "socon_bridge_failed", "SOCON Bridge action failed.", completed);
        }
        finally
        {
            if (sessionOpened && !sessionClosed)
            {
                try
                {
                    var close = await SendAsync("CloseConfiguredReadOnlySession", null, CancellationToken.None);
                    if (close.Success) completed.Add("CloseSession");
                }
                catch
                {
                    // Preserve the original action failure; Bridge remains fail-closed.
                }
            }
            gate.Release();
        }
    }

    private string? Validate(ReagentHardwareActionRequest request)
    {
        if (!options.Enabled) return "socon_reagent_actions_disabled";
        if (string.IsNullOrWhiteSpace(options.PipeName)
            || options.ConnectTimeoutMilliseconds <= 0
            || options.ResponseTimeoutMilliseconds <= 0
            || options.XYSpeedMmPerSecond <= 0
            || options.ZSpeedMmPerSecond <= 0)
        {
            return "socon_reagent_options_invalid";
        }
        if (request.Axis is not ("z1" or "z2")) return "socon_reagent_axis_invalid";
        if (request.Operation is not (
            ReagentHardwareActionOperations.Move
            or ReagentHardwareActionOperations.Aspirate
            or ReagentHardwareActionOperations.Dispense
            or ReagentHardwareActionOperations.LiquidDetect))
        {
            return "socon_reagent_operation_invalid";
        }
        if (request.VolumeUl is <= 0
            && request.Operation is ReagentHardwareActionOperations.Aspirate or ReagentHardwareActionOperations.Dispense)
        {
            return "socon_reagent_volume_invalid";
        }
        if (request.Operation == ReagentHardwareActionOperations.LiquidDetect
            && (!request.LiquidDetectMaximumZUm.HasValue || request.ActionZUm > request.LiquidDetectMaximumZUm.Value))
        {
            return "socon_reagent_liquid_detect_range_invalid";
        }
        return null;
    }

    private IReadOnlyList<SoconStep> BuildSteps(ReagentHardwareActionRequest request)
    {
        var steps = new List<SoconStep>
        {
            Move("MoveSafeZ", request.Axis, request.SafeZUm, options.ZSpeedMmPerSecond),
            Move("MoveX", "x", request.XUm, options.XYSpeedMmPerSecond),
            Move("MoveY", "y", request.YUm, options.XYSpeedMmPerSecond)
        };

        if (request.Operation == ReagentHardwareActionOperations.LiquidDetect)
        {
            steps.Add(new SoconStep(
                "LiquidDetect",
                "DetectLiquidConfigured",
                new
                {
                    axis = request.Axis,
                    startMm = ToMillimeters(request.ActionZUm),
                    maximumMm = ToMillimeters(request.LiquidDetectMaximumZUm!.Value)
                }));
        }
        else
        {
            steps.Add(Move("MoveActionZ", request.Axis, request.ActionZUm, options.ZSpeedMmPerSecond));
            if (request.Operation == ReagentHardwareActionOperations.Aspirate)
            {
                steps.Add(new SoconStep("Aspirate", "AspirateConfigured", new { axis = request.Axis, volumeUl = request.VolumeUl }));
            }
            else if (request.Operation == ReagentHardwareActionOperations.Dispense)
            {
                steps.Add(new SoconStep("Dispense", "DispenseConfigured", new { axis = request.Axis, volumeUl = request.VolumeUl }));
            }
        }

        if (request.Operation != ReagentHardwareActionOperations.Move)
        {
            steps.Add(Move("ReturnSafeZ", request.Axis, request.SafeZUm, options.ZSpeedMmPerSecond));
        }
        return steps;
    }

    private static SoconStep Move(string name, string axis, long positionUm, double speed) =>
        new(name, "MoveConfiguredAxis", new { axis, positionMm = ToMillimeters(positionUm), speedMmPerSecond = speed });

    private async Task TryStopAsync(string axis, CancellationToken cancellationToken)
    {
        try
        {
            await SendAsync("StopConfiguredAxis", new { axis }, cancellationToken);
        }
        catch
        {
            // The original failure remains authoritative.
        }
    }

    private async Task<BridgeReply> SendAsync(string command, object? payload, CancellationToken cancellationToken)
    {
        using var pipe = new NamedPipeClientStream(".", options.PipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
        using var connectCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        connectCts.CancelAfter(options.ConnectTimeoutMilliseconds);
        await pipe.ConnectAsync(connectCts.Token);

        var requestId = Guid.NewGuid().ToString("N");
        var request = MergeRequest(requestId, command, payload);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(request, JsonOptions);
        if (bytes.Length > MaximumPayloadBytes) throw new InvalidOperationException("SOCON Bridge request is too large.");

        var prefix = new byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(prefix, bytes.Length);
        await pipe.WriteAsync(prefix, cancellationToken);
        await pipe.WriteAsync(bytes, cancellationToken);
        await pipe.FlushAsync(cancellationToken);

        using var responseCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        responseCts.CancelAfter(options.ResponseTimeoutMilliseconds);
        await ReadExactAsync(pipe, prefix, responseCts.Token);
        var length = BinaryPrimitives.ReadInt32LittleEndian(prefix);
        if (length <= 0 || length > MaximumPayloadBytes) throw new IOException("SOCON Bridge response length is invalid.");
        var responseBytes = new byte[length];
        await ReadExactAsync(pipe, responseBytes, responseCts.Token);
        var reply = JsonSerializer.Deserialize<BridgeReply>(responseBytes, JsonOptions)
            ?? throw new IOException("SOCON Bridge response is invalid.");
        if (!string.Equals(reply.RequestId, requestId, StringComparison.Ordinal))
            throw new IOException("SOCON Bridge response correlation failed.");
        return reply;
    }

    private static Dictionary<string, object?> MergeRequest(string requestId, string command, object? payload)
    {
        var result = new Dictionary<string, object?>
        {
            ["requestId"] = requestId,
            ["command"] = command
        };
        if (payload is null) return result;
        foreach (var property in JsonSerializer.SerializeToElement(payload, JsonOptions).EnumerateObject())
            result[property.Name] = property.Value.Clone();
        return result;
    }

    private static async Task ReadExactAsync(Stream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        var offset = 0;
        while (offset < buffer.Length)
        {
            var count = await stream.ReadAsync(buffer.AsMemory(offset), cancellationToken);
            if (count == 0) throw new IOException("SOCON Bridge closed the pipe.");
            offset += count;
        }
    }

    private static double ToMillimeters(long micrometers) => micrometers / 1000d;

    private static ReagentHardwareActionResult FromBridge(BridgeReply reply, IReadOnlyList<string> completed) =>
        Failed(
            DeviceCommandStatuses.Failed,
            reply.Details?.BlockReason ?? "socon_bridge_action_rejected",
            string.IsNullOrWhiteSpace(reply.Message) ? "SOCON Bridge rejected the action." : reply.Message,
            completed);

    private static ReagentHardwareActionResult Failed(
        string status,
        string errorCode,
        string message,
        IReadOnlyList<string>? completed = null) =>
        new(false, status, errorCode, message, completed ?? []);

    private sealed record SoconStep(string Name, string Command, object Payload);
    private sealed record BridgeReply(string RequestId, bool Success, string Message, BridgeReplyDetails? Details);
    private sealed record BridgeReplyDetails(string? BlockReason);
}
