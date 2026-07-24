namespace Stainer.Web.Application.ReadModels;

public sealed record ReagentPositionConfigResponse(
    string RackCode,
    decimal? CalibratedXMm,
    decimal? CalibratedYMm,
    decimal? SafeZMm,
    decimal? LiquidDetectZMm,
    decimal? AspirateEndZMm,
    decimal? DispenseZMm,
    int? RoiLeft,
    int? RoiTop,
    int? RoiWidth,
    int? RoiHeight,
    int? PipetteVolumeUl,
    string? PipetteNeedleCode,
    string? PipetteLiquidClassCode,
    bool Enabled,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record ReagentPositionConfigMutationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string EntityType,
    string EntityId,
    string Message);

public sealed record ReagentPositionHardwareActionResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string RackCode,
    string NeedleCode,
    string TargetZ,
    IReadOnlyList<string> CompletedSteps,
    string Message);
