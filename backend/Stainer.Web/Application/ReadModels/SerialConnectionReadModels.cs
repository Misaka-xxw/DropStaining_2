namespace Stainer.Web.Application.ReadModels;

public sealed record SerialConnectionResponse(
    string DeviceKey,
    string? PortName,
    int? BaudRate,
    int? DataBits,
    string Parity,
    string StopBits,
    string Handshake,
    int? ReadTimeoutMilliseconds,
    int? WriteTimeoutMilliseconds,
    bool Enabled,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record SerialConnectionMutationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string EntityType,
    string EntityId,
    string Message);
