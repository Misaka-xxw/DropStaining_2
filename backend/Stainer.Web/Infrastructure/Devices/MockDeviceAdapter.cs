using System.Text.Json;
using Stainer.Web.Application.Devices;
using Stainer.Web.Application.Services;
using Stainer.Web.Domain.Entities;

namespace Stainer.Web.Infrastructure.Devices;

public sealed class MockDeviceAdapter(MockDeviceStateStore stateStore, IServiceScopeFactory? scopeFactory = null) : IDeviceAdapter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly TimeSpan CommandDelay = TimeSpan.FromMilliseconds(5);

    public string Mode => DeviceModes.Mock;

    public string Name => nameof(MockDeviceAdapter);

    public Task<DeviceStatusSnapshot> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(stateStore.Snapshot());
    }

    public Task<DeviceCommandResult> GetHealthAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default)
    {
        var snapshot = stateStore.Snapshot();
        var now = DateTimeOffset.UtcNow;
        return Task.FromResult(new DeviceCommandResult(
            snapshot.Ready,
            snapshot.Ready ? DeviceCommandStatuses.Succeeded : DeviceCommandStatuses.Failed,
            request.ModuleCode,
            request.Action,
            snapshot.Ready ? null : "device_not_ready",
            snapshot.Ready ? "Mock device health is ready." : "Mock device initialization is incomplete.",
            now,
            now,
            true,
            new Dictionary<string, object?> { ["ready"] = snapshot.Ready, ["version"] = snapshot.Version }));
    }

    public Task<DeviceCommandResult> InitializeModuleAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default)
    {
        if (request.ModuleCode is DeviceModules.Temperature or DeviceModules.Cooling)
        {
            if (request.Parameters.TryGetValue("thermalStateValidated", out var validated) && Convert.ToBoolean(validated))
            {
                return ExecuteAsync(request, request.Parameters, cancellationToken);
            }

            return ExecuteThermalAsync(request, async service => await service.InitializeModuleAsync(request.ModuleCode, cancellationToken), cancellationToken);
        }

        return ExecuteAsync(request, InitializationData(request.ModuleCode), cancellationToken);
    }

    public Task<DeviceCommandResult> ScanSampleAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(request, new Dictionary<string, object?> { ["scanSource"] = "Mock", ["rawCode"] = request.Parameters.GetValueOrDefault("rawCode") }, cancellationToken);

    public Task<DeviceCommandResult> ScanReagentAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(request, new Dictionary<string, object?> { ["scanSource"] = "Mock", ["rawBarcode"] = request.Parameters.GetValueOrDefault("rawBarcode") }, cancellationToken);

    public Task<DeviceCommandResult> QueryLisAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(request, new Dictionary<string, object?> { ["source"] = "MockLIS", ["readOnly"] = true }, cancellationToken);

    public Task<DeviceCommandResult> SetTemperatureAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) =>
        ExecuteThermalAsync(
            request,
            service => service.SetPointFromDeviceAsync(
                Convert.ToString(request.Parameters.GetValueOrDefault("drawerCode")) ?? string.Empty,
                Convert.ToInt32(request.Parameters.GetValueOrDefault("slotNo") ?? 0),
                Convert.ToInt32(request.Parameters.GetValueOrDefault("targetTemperatureDeciC") ?? 420),
                cancellationToken),
            cancellationToken);

    public Task<DeviceCommandResult> SetCoolingAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) =>
        ExecuteThermalAsync(
            request,
            service => service.SetCoolingFromDeviceAsync(
                Convert.ToInt32(request.Parameters.GetValueOrDefault("targetTemperatureDeciC") ?? 80),
                Convert.ToBoolean(request.Parameters.GetValueOrDefault("isEnabled") ?? true),
                cancellationToken),
            cancellationToken);

    public Task<DeviceCommandResult> RunPumpAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => ExecuteAsync(request, null, cancellationToken);

    public Task<DeviceCommandResult> ReadLiquidLevelsAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(request, InitializationData(DeviceModules.LiquidLevel), cancellationToken);

    public Task<DeviceCommandResult> MixAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => ExecuteAsync(request, null, cancellationToken);

    public Task<DeviceCommandResult> MoveRobotAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => ExecuteAsync(request, null, cancellationToken);

    public Task<DeviceCommandResult> OperateNeedlesAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => ExecuteAsync(request, null, cancellationToken);

    public Task<DeviceCommandResult> PipetteAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => ExecuteAsync(request, null, cancellationToken);

    public Task<DeviceCommandResult> WashNeedlesAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => ExecuteAsync(request, null, cancellationToken);

    public Task<DeviceCommandResult> PrepareDabAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(request, new Dictionary<string, object?>
        {
            ["source"] = "MockDabModule",
            ["waterSourceType"] = "SystemWater",
            ["waterLevelSnapshot"] = new Dictionary<string, object?>
            {
                ["sourceType"] = "SystemWater",
                ["levelBeforeUl"] = 1_000_000,
                ["levelAfterUl"] = 999_280,
                ["unit"] = "uL"
            }
        }, cancellationToken);

    public Task<DeviceCommandResult> ExecuteWorkflowActionAsync(DeviceOperationRequest request, CancellationToken cancellationToken = default) => ExecuteAsync(request, null, cancellationToken);

    public Task<DeviceFaultControlResult> ConfigureFaultAsync(DeviceFaultCommand command, CancellationToken cancellationToken = default)
    {
        var state = stateStore.ConfigureFault(command);
        return Task.FromResult(new DeviceFaultControlResult(true, $"Mock fault {command.FaultType} configured for {command.ModuleCode}.", state));
    }

    public Task<DeviceFaultControlResult> ClearFaultsAsync(DeviceFaultClearCommand command, CancellationToken cancellationToken = default)
    {
        var state = stateStore.ClearFaults(command);
        return Task.FromResult(new DeviceFaultControlResult(true, "Mock fault plan cleared and module availability restored.", state));
    }

    private async Task<DeviceCommandResult> ExecuteAsync(
        DeviceOperationRequest request,
        IReadOnlyDictionary<string, object?>? successData,
        CancellationToken cancellationToken)
    {
        var startedAtUtc = DateTimeOffset.UtcNow;
        var targetJson = JsonSerializer.Serialize(request.Parameters, JsonOptions);
        stateStore.BeginOperation(request.ModuleCode, request.Action, targetJson);
        var fault = stateStore.ConsumeFault(request.ModuleCode);
        if (fault is not null)
        {
            var status = fault.FaultType switch
            {
                DeviceFaultTypes.TimeoutNextCommand => DeviceCommandStatuses.TimedOut,
                DeviceFaultTypes.ReturnUnknown => DeviceCommandStatuses.Unknown,
                _ => DeviceCommandStatuses.Failed
            };
            var connection = fault.FaultType == DeviceFaultTypes.Disconnect
                ? DeviceConnectionStatuses.Disconnected
                : DeviceConnectionStatuses.Faulted;
            var completedAtUtc = DateTimeOffset.UtcNow;
            stateStore.CompleteOperation(request.ModuleCode, connection, null, fault.ErrorCode ?? "mock_fault", fault.Message);
            return new DeviceCommandResult(
                false,
                status,
                request.ModuleCode,
                request.Action,
                fault.ErrorCode ?? "mock_fault",
                fault.Message,
                startedAtUtc,
                completedAtUtc,
                status is not DeviceCommandStatuses.TimedOut and not DeviceCommandStatuses.Unknown,
                new Dictionary<string, object?> { ["faultPlanId"] = fault.Id, ["faultType"] = fault.FaultType });
        }

        await Task.Delay(CommandDelay, cancellationToken);
        var data = successData ?? new Dictionary<string, object?>();
        var currentJson = JsonSerializer.Serialize(data, JsonOptions);
        stateStore.CompleteOperation(request.ModuleCode, DeviceConnectionStatuses.Connected, currentJson, null, null);
        return new DeviceCommandResult(
            true,
            DeviceCommandStatuses.Succeeded,
            request.ModuleCode,
            request.Action,
            null,
            $"Mock {request.ModuleCode}/{request.Action} completed.",
            startedAtUtc,
            DateTimeOffset.UtcNow,
            true,
            data);
    }

    private async Task<DeviceCommandResult> ExecuteThermalAsync(
        DeviceOperationRequest request,
        Func<ThermalControlService, Task<ThermalDeviceResult>> execute,
        CancellationToken cancellationToken)
    {
        var startedAtUtc = DateTimeOffset.UtcNow;
        var targetJson = JsonSerializer.Serialize(request.Parameters, JsonOptions);
        stateStore.BeginOperation(request.ModuleCode, request.Action, targetJson);
        var genericFault = stateStore.ConsumeFault(request.ModuleCode);
        if (genericFault is not null)
        {
            var status = genericFault.FaultType switch
            {
                DeviceFaultTypes.TimeoutNextCommand => DeviceCommandStatuses.TimedOut,
                DeviceFaultTypes.ReturnUnknown => DeviceCommandStatuses.Unknown,
                _ => DeviceCommandStatuses.Failed
            };
            stateStore.CompleteOperation(
                request.ModuleCode,
                genericFault.FaultType == DeviceFaultTypes.Disconnect ? DeviceConnectionStatuses.Disconnected : DeviceConnectionStatuses.Faulted,
                null,
                genericFault.ErrorCode ?? "mock_fault",
                genericFault.Message);
            return new DeviceCommandResult(false, status, request.ModuleCode, request.Action, genericFault.ErrorCode ?? "mock_fault", genericFault.Message, startedAtUtc, DateTimeOffset.UtcNow, status is not DeviceCommandStatuses.TimedOut and not DeviceCommandStatuses.Unknown, new Dictionary<string, object?>());
        }

        if (scopeFactory is null)
        {
            stateStore.CompleteOperation(request.ModuleCode, DeviceConnectionStatuses.Faulted, null, "thermal_service_unavailable", "Thermal service is unavailable.");
            return new DeviceCommandResult(false, DeviceCommandStatuses.NotSupported, request.ModuleCode, request.Action, "thermal_service_unavailable", "Thermal service is unavailable.", startedAtUtc, DateTimeOffset.UtcNow, true, new Dictionary<string, object?>());
        }

        await using var scope = scopeFactory.CreateAsyncScope();
        var result = await execute(scope.ServiceProvider.GetRequiredService<ThermalControlService>());
        var currentJson = JsonSerializer.Serialize(result.Data, JsonOptions);
        stateStore.CompleteOperation(
            request.ModuleCode,
            result.Ok ? DeviceConnectionStatuses.Connected : result.ErrorCode == ThermalFaultTypes.Disconnected ? DeviceConnectionStatuses.Disconnected : DeviceConnectionStatuses.Faulted,
            currentJson,
            result.ErrorCode,
            result.Ok ? null : result.Message);
        return new DeviceCommandResult(
            result.Ok,
            result.Status,
            request.ModuleCode,
            request.Action,
            result.ErrorCode,
            result.Message,
            startedAtUtc,
            DateTimeOffset.UtcNow,
            result.Status is not DeviceCommandStatuses.TimedOut and not DeviceCommandStatuses.Unknown,
            result.Data);
    }

    private static IReadOnlyDictionary<string, object?> InitializationData(string moduleCode)
    {
        return moduleCode switch
        {
            DeviceModules.Controller => new Dictionary<string, object?> { ["connected"] = true },
            DeviceModules.SampleScanner or DeviceModules.ReagentScanner => new Dictionary<string, object?> { ["connected"] = true, ["online"] = true },
            DeviceModules.RobotArm => new Dictionary<string, object?> { ["connected"] = true, ["homed"] = true },
            DeviceModules.LiquidLevel => new Dictionary<string, object?>
            {
                ["pureWaterOk"] = true,
                ["pbsOk"] = true,
                ["wasteTankFull"] = false,
                ["toxicTankFull"] = false
            },
            DeviceModules.NeedleWash => new Dictionary<string, object?> { ["prepared"] = true },
            _ => new Dictionary<string, object?> { ["connected"] = true }
        };
    }
}
