namespace Stainer.Web.Application.ReadModels;

public sealed record PrecisionCalibrationResponse(
    string ScopeKey,
    double? MoveOffsetXMm,
    double? MoveOffsetYMm,
    double? DispenseTargetVolumeUl,
    double? DispenseMeasuredVolumeUl,
    double? DispenseCalibrationFactor,
    bool Enabled,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record PrecisionCalibrationMutationResponse(
    bool Ok,
    string CommandId,
    bool Replayed,
    string EntityType,
    string EntityId,
    string Message);
