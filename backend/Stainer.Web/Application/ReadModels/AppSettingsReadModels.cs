namespace Stainer.Web.Application.ReadModels;

public sealed record AppSettingsResponse(
    string ScopeKey,
    string? DataInterface,
    string? HostAddress,
    int? HeartbeatSec,
    decimal? ReagentBottleCapacityMl,
    decimal? ReagentTargetTempC,
    decimal? WorkTargetTempC,
    decimal? NeedleGapMm,
    bool Enabled,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record AppSettingsMutationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string EntityType,
    string EntityId,
    string Message);
