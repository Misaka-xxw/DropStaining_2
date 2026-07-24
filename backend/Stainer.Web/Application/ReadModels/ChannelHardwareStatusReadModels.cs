namespace Stainer.Web.Application.ReadModels;

/// <summary>
/// 独立的真实硬件通道状态快照。
/// 不参与 ChannelBatch/运行页业务状态计算，也不覆盖前端通道状态。
/// </summary>
public sealed record ChannelHardwareStatusResponse(
    bool Ok,
    string Status,
    byte? ControllerWorkStatus,
    IReadOnlyList<ChannelHardwareStatusItemResponse> Channels,
    string? ErrorCode,
    string Message,
    DateTimeOffset ReadAtUtc);

public sealed record ChannelHardwareStatusItemResponse(
    string ChannelCode,
    IReadOnlyList<int> NodeIndexes,
    IReadOnlyList<byte> RawNodeStatuses);
