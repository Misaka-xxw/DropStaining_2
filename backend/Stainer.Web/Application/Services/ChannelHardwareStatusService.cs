using Stainer.Web.Application.Devices;
using Stainer.Web.Application.ReadModels;

namespace Stainer.Web.Application.Services;

public sealed class ChannelHardwareStatusOptions
{
    public bool Enabled { get; set; }
    public List<ChannelHardwareStatusMapping> Mappings { get; set; } = [];
}

public sealed class ChannelHardwareStatusMapping
{
    public string ChannelCode { get; set; } = string.Empty;
    public List<int> NodeIndexes { get; set; } = [];
}

/// <summary>
/// 只读连接主控工作状态和节点状态，并按部署配置映射到 A-D 通道。
/// 该服务不依赖数据库、ChannelBatch、运行页或 SignalR，因而不会改变现有前后端业务状态链路。
/// </summary>
public sealed class ChannelHardwareStatusService(
    IChannelHardwareStatusDeviceReader deviceReader,
    ChannelHardwareStatusOptions options)
{
    private static readonly string[] RequiredChannelCodes = ["A", "B", "C", "D"];
    private const int NodeStatusCount = 64;

    public async Task<ChannelHardwareStatusResponse> ReadAsync(CancellationToken cancellationToken = default)
    {
        var readAt = DateTimeOffset.UtcNow;
        var validationError = ValidateMappings(options);
        if (validationError is not null)
        {
            return Failed(
                DeviceCommandStatuses.NotConfigured,
                validationError,
                "Hardware channel status mapping is not configured.",
                readAt);
        }

        var work = await deviceReader.ReadControllerWorkStatusAsync(cancellationToken);
        if (!work.Ok || work.Value is null)
        {
            return Failed(work.Status, work.ErrorCode, work.Message, readAt);
        }

        var nodes = await deviceReader.ReadControllerNodeStatusesAsync(cancellationToken);
        if (!nodes.Ok || nodes.Value is null)
        {
            return Failed(nodes.Status, nodes.ErrorCode, nodes.Message, readAt, work.Value.Value);
        }

        if (nodes.Value.Values.Length != NodeStatusCount)
        {
            return Failed(
                DeviceCommandStatuses.Failed,
                "channel_hardware_node_count_invalid",
                $"Expected {NodeStatusCount} node statuses.",
                readAt,
                work.Value.Value);
        }

        var mappings = options.Mappings.ToDictionary(
            mapping => NormalizeChannelCode(mapping.ChannelCode),
            StringComparer.Ordinal);
        var channels = RequiredChannelCodes
            .Select(code =>
            {
                var indexes = mappings[code].NodeIndexes.ToArray();
                return new ChannelHardwareStatusItemResponse(
                    code,
                    indexes,
                    indexes.Select(index => nodes.Value.Values[index]).ToArray());
            })
            .ToArray();

        return new ChannelHardwareStatusResponse(
            true,
            DeviceCommandStatuses.Succeeded,
            work.Value.Value,
            channels,
            null,
            "Hardware channel statuses read from main controller.",
            readAt);
    }

    private static string? ValidateMappings(ChannelHardwareStatusOptions value)
    {
        if (!value.Enabled)
        {
            return "channel_hardware_status_disabled";
        }

        if (value.Mappings is null || value.Mappings.Count != RequiredChannelCodes.Length)
        {
            return "channel_hardware_mapping_incomplete";
        }

        var normalized = value.Mappings
            .Select(mapping => NormalizeChannelCode(mapping.ChannelCode))
            .ToArray();
        if (normalized.Distinct(StringComparer.Ordinal).Count() != RequiredChannelCodes.Length
            || RequiredChannelCodes.Any(code => !normalized.Contains(code, StringComparer.Ordinal)))
        {
            return "channel_hardware_mapping_invalid";
        }

        foreach (var mapping in value.Mappings)
        {
            if (mapping.NodeIndexes is null
                || mapping.NodeIndexes.Count == 0
                || mapping.NodeIndexes.Distinct().Count() != mapping.NodeIndexes.Count
                || mapping.NodeIndexes.Any(index => index < 0 || index >= NodeStatusCount))
            {
                return "channel_hardware_node_index_invalid";
            }
        }

        return null;
    }

    private static string NormalizeChannelCode(string? value) =>
        value?.Trim().ToUpperInvariant() ?? string.Empty;

    private static ChannelHardwareStatusResponse Failed(
        string status,
        string? errorCode,
        string message,
        DateTimeOffset readAt,
        byte? controllerWorkStatus = null) =>
        new(
            false,
            status,
            controllerWorkStatus,
            [],
            errorCode,
            string.IsNullOrWhiteSpace(message) ? "Hardware channel status read failed." : message,
            readAt);
}
