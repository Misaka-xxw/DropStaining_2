using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.Devices;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

/// <summary>
/// 试剂位坐标/Z 配置到 SOCON 真实运动的独立工程入口。
/// 保存配置仍由 ReagentPositionConfigService 负责；本服务只做显式危险确认后的现场移动。
/// </summary>
public sealed class ReagentPositionHardwareService(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService,
    DeviceModeService deviceModeService,
    IReagentHardwareActionClient hardwareActionClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task<ReagentPositionHardwareActionResponse> MoveAsync(
        string rackCode,
        MoveReagentPositionHardwareRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        var normalizedRackCode = NormalizeRackCode(rackCode);
        return idempotencyService.RunAsync(
            request.CommandId,
            "reagent_position.hardware_move",
            new { rackCode = normalizedRackCode, request },
            actor,
            async () =>
            {
                if (deviceModeService.IsMock)
                {
                    throw new BusinessRuleException(
                        "reagent_position_hardware_real_required",
                        "Reagent position hardware movement requires Real device mode.",
                        StatusCodes.Status409Conflict);
                }

                var config = await dbContext.ReagentPositionConfigs
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.RackCode == normalizedRackCode && x.Enabled, cancellationToken)
                    ?? throw new BusinessRuleException(
                        "reagent_position_config_not_found",
                        "Enabled reagent position configuration was not found.",
                        StatusCodes.Status404NotFound);

                if (!config.CalibratedXMm.HasValue || !config.CalibratedYMm.HasValue || !config.SafeZMm.HasValue)
                {
                    throw new BusinessRuleException(
                        "reagent_position_coordinate_incomplete",
                        "Calibrated X/Y and safe Z are required for real movement.",
                        StatusCodes.Status409Conflict);
                }

                var targetZName = NormalizeTargetZ(request.TargetZ);
                var targetZ = ResolveTargetZ(config, targetZName)
                    ?? throw new BusinessRuleException(
                        "reagent_position_target_z_missing",
                        $"Configured {targetZName} Z value is required.",
                        StatusCodes.Status409Conflict);
                var needleCode = NormalizeNeedleCode(request.NeedleCode);
                var action = await hardwareActionClient.ExecuteAsync(
                    new ReagentHardwareActionRequest(
                        ReagentHardwareActionOperations.Move,
                        needleCode == NeedleCodes.Needle2 ? "z2" : "z1",
                        ToMicrometers(config.CalibratedXMm.Value),
                        ToMicrometers(config.CalibratedYMm.Value),
                        ToMicrometers(config.SafeZMm.Value),
                        ToMicrometers(targetZ)),
                    cancellationToken);
                if (!action.Ok)
                {
                    throw new BusinessRuleException(
                        action.ErrorCode ?? "reagent_position_hardware_move_failed",
                        action.Message,
                        StatusCodes.Status503ServiceUnavailable);
                }

                dbContext.AuditLogs.Add(new AuditLog
                {
                    ActorUserId = string.IsNullOrWhiteSpace(actor.UserId) ? null : actor.UserId,
                    Action = "reagent_position.hardware_move",
                    EntityType = "ReagentPositionConfig",
                    EntityId = config.Id,
                    Message = JsonSerializer.Serialize(new
                    {
                        rackCode = normalizedRackCode,
                        needleCode,
                        targetZ = targetZName,
                        reason = request.Reason.Trim(),
                        action.CompletedSteps
                    }, JsonOptions),
                    CreatedAtUtc = DateTimeOffset.UtcNow
                });

                return new CommandExecutionResult<ReagentPositionHardwareActionResponse>(
                    new ReagentPositionHardwareActionResponse(
                        true,
                        request.CommandId,
                        false,
                        normalizedRackCode,
                        needleCode,
                        targetZName,
                        action.CompletedSteps,
                        "Reagent position hardware movement completed."),
                    "ReagentPositionConfig",
                    config.Id);
            },
            cancellationToken);
    }

    private static string NormalizeRackCode(string? value)
    {
        var normalized = value?.Trim().ToUpperInvariant() ?? string.Empty;
        if (!System.Text.RegularExpressions.Regex.IsMatch(normalized, @"^R([1-9]|[1-3]\d|40)$"))
            throw new BusinessRuleException("rack_code_invalid", "rackCode must be R1-R40.", StatusCodes.Status400BadRequest);
        return normalized;
    }

    private static string NormalizeNeedleCode(string? value)
    {
        var normalized = value?.Trim() ?? string.Empty;
        if (string.Equals(normalized, NeedleCodes.Needle1, StringComparison.OrdinalIgnoreCase)) return NeedleCodes.Needle1;
        if (string.Equals(normalized, NeedleCodes.Needle2, StringComparison.OrdinalIgnoreCase)) return NeedleCodes.Needle2;
        throw new BusinessRuleException("needle_code_invalid", "needleCode must be Needle1 or Needle2.", StatusCodes.Status400BadRequest);
    }

    private static string NormalizeTargetZ(string? value)
    {
        var normalized = value?.Trim().ToLowerInvariant() ?? string.Empty;
        return normalized switch
        {
            "safe" => "safe",
            "liquid-detect" => "liquid-detect",
            "aspirate-end" => "aspirate-end",
            "dispense" => "dispense",
            _ => throw new BusinessRuleException(
                "target_z_invalid",
                "targetZ must be safe, liquid-detect, aspirate-end or dispense.",
                StatusCodes.Status400BadRequest)
        };
    }

    private static decimal? ResolveTargetZ(ReagentPositionConfig config, string targetZ) => targetZ switch
    {
        "safe" => config.SafeZMm,
        "liquid-detect" => config.LiquidDetectZMm,
        "aspirate-end" => config.AspirateEndZMm,
        "dispense" => config.DispenseZMm,
        _ => null
    };

    private static long ToMicrometers(decimal millimeters) =>
        checked((long)decimal.Round(millimeters * 1000m, 0, MidpointRounding.AwayFromZero));
}
