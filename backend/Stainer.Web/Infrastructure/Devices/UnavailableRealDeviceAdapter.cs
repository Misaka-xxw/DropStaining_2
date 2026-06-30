using Stainer.Web.Application.Devices;
using Stainer.Web.Application.Services;

namespace Stainer.Web.Infrastructure.Devices;

public sealed class UnavailableRealDeviceAdapter : IDeviceAdapter
{
    private const string ErrorCode = "real_adapter_not_implemented";
    private const string Message = "Real device adapter is not implemented. No hardware command was sent.";

    public string Mode => DeviceModes.Real;

    public string Name => nameof(UnavailableRealDeviceAdapter);

    public Task<DeviceStatusSnapshot> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        return Task.FromResult(new DeviceStatusSnapshot(
            Mode,
            Name,
            false,
            0,
            now,
            [new DeviceModuleStatusSnapshot("real-adapter", DeviceConnectionStatuses.Disconnected, "Idle", null, null, ErrorCode, Message, now, 0)],
            []));
    }

    public Task<DeviceCommandResult> GetHealthAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> InitializeModuleAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> ScanSampleAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> ScanReagentAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> QueryLisAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> SetTemperatureAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> SetCoolingAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> RunPumpAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> ReadLiquidLevelsAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> MixAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> MoveRobotAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> OperateNeedlesAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> PipetteAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> WashNeedlesAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> PrepareDabAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);
    public Task<DeviceCommandResult> ExecuteWorkflowActionAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => RejectAsync(request);

    public async Task<DeviceFaultControlResult> ConfigureFaultAsync(DeviceFaultCommand command, CancellationToken cancellationToken = default)
    {
        return new DeviceFaultControlResult(false, Message, await GetStatusAsync(cancellationToken));
    }

    public async Task<DeviceFaultControlResult> ClearFaultsAsync(DeviceFaultClearCommand command, CancellationToken cancellationToken = default)
    {
        return new DeviceFaultControlResult(false, Message, await GetStatusAsync(cancellationToken));
    }

    private static Task<DeviceCommandResult> RejectAsync(DeviceOperationRequest request)
    {
        var now = DateTimeOffset.UtcNow;
        return Task.FromResult(new DeviceCommandResult(
            false,
            DeviceCommandStatuses.NotSupported,
            request.ModuleCode,
            request.Action,
            ErrorCode,
            Message,
            now,
            now,
            false,
            new Dictionary<string, object?>()));
    }
}
