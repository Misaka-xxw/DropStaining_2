using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Stainer.Web.Application.Devices;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class DeviceControlService(
    IDeviceAdapter deviceAdapter,
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService,
    IRuntimeEventPublisher eventPublisher,
    SafetyLogWriter safetyLogWriter)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task<DeviceStatusSnapshot> GetStateAsync(CancellationToken cancellationToken = default)
    {
        return deviceAdapter.GetStatusAsync(cancellationToken);
    }

    public Task<DeviceFaultMutationResponse> ConfigureFaultAsync(
        ConfigureMockDeviceFaultRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "device.mock_fault.configure",
            request,
            actor,
            async () =>
            {
                EnsureMockAdapter();
                var moduleCode = RequireModule(request.ModuleCode);
                var faultType = RequireFaultType(request.FaultType);
                var reason = RequireReason(request.Reason);
                var result = await deviceAdapter.ConfigureFaultAsync(
                    new DeviceFaultCommand(
                        moduleCode,
                        faultType,
                        request.ErrorCode?.Trim(),
                        request.Message?.Trim(),
                        reason,
                        request.CommandId,
                        actor.UserId,
                        actor.Username),
                    cancellationToken);

                dbContext.AuditLogs.Add(new AuditLog
                {
                    ActorUserId = actor.UserId,
                    Action = "device.mock_fault.configured",
                    EntityType = "DeviceModule",
                    EntityId = moduleCode,
                    Message = JsonSerializer.Serialize(new
                    {
                        request.CommandId,
                        moduleCode,
                        faultType,
                        request.ErrorCode,
                        request.Message,
                        reason,
                        stateVersion = result.State.Version
                    }, JsonOptions),
                    CreatedAtUtc = DateTimeOffset.UtcNow
                });
                await safetyLogWriter.WriteAsync(
                    "device",
                    "Warning",
                    result.Message,
                    new SafetyLogContext(
                        CorrelationId: request.CommandId,
                        CommandId: request.CommandId,
                        DeviceMode: deviceAdapter.Mode,
                        Actor: actor.Username,
                        Source: nameof(DeviceControlService)),
                    cancellationToken: cancellationToken);
                PublishState(result.State, request.CommandId, "faultConfigured");
                return new CommandExecutionResult<DeviceFaultMutationResponse>(
                    new DeviceFaultMutationResponse(result.Ok, request.CommandId, false, result.Message, result.State),
                    "DeviceModule",
                    moduleCode);
            },
            cancellationToken);
    }

    public Task<DeviceFaultMutationResponse> ClearFaultsAsync(
        ClearMockDeviceFaultRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "device.mock_fault.clear",
            request,
            actor,
            async () =>
            {
                EnsureMockAdapter();
                var moduleCode = string.IsNullOrWhiteSpace(request.ModuleCode) ? null : RequireModule(request.ModuleCode);
                var reason = RequireReason(request.Reason);
                var result = await deviceAdapter.ClearFaultsAsync(
                    new DeviceFaultClearCommand(moduleCode, reason, request.CommandId, actor.UserId, actor.Username),
                    cancellationToken);

                dbContext.AuditLogs.Add(new AuditLog
                {
                    ActorUserId = actor.UserId,
                    Action = "device.mock_fault.cleared",
                    EntityType = "DeviceModule",
                    EntityId = moduleCode ?? "all",
                    Message = JsonSerializer.Serialize(new
                    {
                        request.CommandId,
                        moduleCode,
                        reason,
                        stateVersion = result.State.Version
                    }, JsonOptions),
                    CreatedAtUtc = DateTimeOffset.UtcNow
                });
                await safetyLogWriter.WriteAsync(
                    "device",
                    "Information",
                    result.Message,
                    new SafetyLogContext(
                        CorrelationId: request.CommandId,
                        CommandId: request.CommandId,
                        DeviceMode: deviceAdapter.Mode,
                        Actor: actor.Username,
                        Source: nameof(DeviceControlService)),
                    cancellationToken: cancellationToken);
                PublishState(result.State, request.CommandId, "faultCleared");
                return new CommandExecutionResult<DeviceFaultMutationResponse>(
                    new DeviceFaultMutationResponse(result.Ok, request.CommandId, false, result.Message, result.State),
                    "DeviceModule",
                    moduleCode ?? "all");
            },
            cancellationToken);
    }

    private void EnsureMockAdapter()
    {
        if (!string.Equals(deviceAdapter.Mode, DeviceModes.Mock, StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessRuleException(
                "mock_fault_not_available",
                "Mock fault control is available only when DeviceMode=Mock.",
                StatusCodes.Status409Conflict);
        }
    }

    private static string RequireModule(string value)
    {
        var normalized = value?.Trim() ?? string.Empty;
        if (!DeviceModules.All.Contains(normalized, StringComparer.OrdinalIgnoreCase))
        {
            throw new BusinessRuleException("device_module_invalid", "Unknown device module.", StatusCodes.Status400BadRequest);
        }

        return DeviceModules.All.First(x => string.Equals(x, normalized, StringComparison.OrdinalIgnoreCase));
    }

    private static string RequireFaultType(string value)
    {
        var normalized = value?.Trim() ?? string.Empty;
        if (!DeviceFaultTypes.All.Contains(normalized))
        {
            throw new BusinessRuleException("mock_fault_type_invalid", "Unknown Mock fault type.", StatusCodes.Status400BadRequest);
        }

        return DeviceFaultTypes.All.First(x => string.Equals(x, normalized, StringComparison.OrdinalIgnoreCase));
    }

    private static string RequireReason(string value)
    {
        var reason = value?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new BusinessRuleException("reason_required", "A reason is required.", StatusCodes.Status400BadRequest);
        }

        return reason;
    }

    private void PublishState(DeviceStatusSnapshot state, string commandId, string changeType)
    {
        eventPublisher.Publish(MachineEventMessage.Create(
            MachineEventTypes.DeviceStateChanged,
            null,
            "DeviceState",
            deviceAdapter.Name,
            null,
            new Dictionary<string, object?>
            {
                ["commandId"] = commandId,
                ["changeType"] = changeType,
                ["mode"] = state.Mode,
                ["adapterName"] = state.AdapterName,
                ["ready"] = state.Ready,
                ["version"] = state.Version
            }));
    }
}
