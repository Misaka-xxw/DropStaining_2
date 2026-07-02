using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class LiquidClassSnapshotFactory(StainerDbContext dbContext)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<string> FreezeForWorkflowAsync(WorkflowVersion workflowVersion, CancellationToken cancellationToken = default)
    {
        var reagentCodes = GetRequiredReagentCodes(workflowVersion);

        if (reagentCodes.Count == 0)
        {
            return JsonSerializer.Serialize(new LiquidClassSnapshot(1, []), JsonOptions);
        }

        var definitions = await dbContext.ReagentDefinitions
            .AsNoTracking()
            .Include(x => x.LiquidClassProfile)
            .ThenInclude(x => x!.EnabledVersion)
            .Where(x => reagentCodes.Contains(x.ReagentCode))
            .ToListAsync(cancellationToken);
        var byCode = definitions.ToDictionary(x => x.ReagentCode, StringComparer.OrdinalIgnoreCase);
        var unresolved = reagentCodes.Where(code => !byCode.TryGetValue(code, out var definition)
            || definition.LiquidClassProfile is null
            || !definition.LiquidClassProfile.IsEnabled
            || definition.LiquidClassProfile.EnabledVersion is null
            || definition.LiquidClassProfile.EnabledVersion.Status != LiquidClassVersionStatus.Enabled).ToList();
        if (unresolved.Count > 0)
        {
            throw new BusinessRuleException(
                "liquid_class_resolution_required",
                $"Enabled Liquid Class versions are required for reagent(s): {string.Join(", ", unresolved)}.",
                StatusCodes.Status409Conflict);
        }

        var items = reagentCodes.Select(code =>
        {
            var definition = byCode[code];
            var profile = definition.LiquidClassProfile!;
            var version = profile.EnabledVersion!;
            return new LiquidClassSnapshotItem(
                code,
                profile.Id,
                profile.Code,
                version.Id,
                version.VersionNo,
                version.VersionLabel,
                CreateParameterSummary(version));
        }).ToList();
        return JsonSerializer.Serialize(new LiquidClassSnapshot(1, items), JsonOptions);
    }

    public async Task<string> FreezeCatalogAsync(CancellationToken cancellationToken = default)
    {
        var definitions = await dbContext.ReagentDefinitions
            .AsNoTracking()
            .Include(x => x.LiquidClassProfile)
            .ThenInclude(x => x!.EnabledVersion)
            .OrderBy(x => x.ReagentCode)
            .ToListAsync(cancellationToken);
        var items = definitions
            .Where(x => x.LiquidClassProfile is { IsEnabled: true, EnabledVersion: { Status: LiquidClassVersionStatus.Enabled } })
            .Select(definition =>
            {
                var profile = definition.LiquidClassProfile!;
                var version = profile.EnabledVersion!;
                return new LiquidClassSnapshotItem(
                    definition.ReagentCode,
                    profile.Id,
                    profile.Code,
                    version.Id,
                    version.VersionNo,
                    version.VersionLabel,
                    CreateParameterSummary(version));
            })
            .ToList();
        return JsonSerializer.Serialize(new LiquidClassSnapshot(1, items), JsonOptions);
    }

    public static string ValidateFrozenForWorkflow(WorkflowVersion workflowVersion, string frozenSnapshotJson)
    {
        var snapshot = Parse(frozenSnapshotJson);
        var available = snapshot.Items.Select(x => x.ReagentCode).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var unresolved = GetRequiredReagentCodes(workflowVersion).Where(x => !available.Contains(x)).ToList();
        if (unresolved.Count > 0)
        {
            throw new BusinessRuleException(
                "liquid_class_resolution_required",
                $"The ChannelBatch creation snapshot has no enabled Liquid Class version for reagent(s): {string.Join(", ", unresolved)}.",
                StatusCodes.Status409Conflict);
        }

        return JsonSerializer.Serialize(snapshot, JsonOptions);
    }

    public static string FreezeForRun(IEnumerable<ChannelBatch> batches)
    {
        var items = batches.OrderBy(x => x.DrawerCode).Select(batch => new RunLiquidClassBatchSnapshot(
            batch.Id,
            batch.DrawerCode,
            Parse(batch.LiquidClassSnapshotJson))).ToList();
        return JsonSerializer.Serialize(new RunLiquidClassSnapshot(1, items), JsonOptions);
    }

    public static LiquidClassSnapshotItem? FindForCommand(string snapshotJson, string? reagentCode)
    {
        if (string.IsNullOrWhiteSpace(reagentCode))
        {
            return null;
        }

        return Parse(snapshotJson).Items.SingleOrDefault(x => x.ReagentCode.Equals(reagentCode, StringComparison.OrdinalIgnoreCase));
    }

    public static LiquidClassSnapshot Parse(string snapshotJson)
    {
        try
        {
            return JsonSerializer.Deserialize<LiquidClassSnapshot>(snapshotJson, JsonOptions)
                ?? new LiquidClassSnapshot(1, []);
        }
        catch (JsonException)
        {
            return new LiquidClassSnapshot(1, []);
        }
    }

    public static LiquidClassParameterSummary CreateParameterSummary(LiquidClassVersion version)
    {
        return new LiquidClassParameterSummary(
            version.LiquidDetectionEnabled,
            version.LiquidDetectionSensitivityPercent,
            version.LiquidDetectionSpeedUmPerSecond,
            version.AspirateSpeedUlPerSecond,
            version.AspirateDelayMs,
            version.DispenseSpeedUlPerSecond,
            version.DispenseDelayMs,
            version.LeadingAirGapUl,
            version.TrailingAirGapUl,
            version.BlowoutVolumeUl,
            version.BlowoutDelayMs,
            version.VolumeAdjustmentUl,
            version.PreWetCycles,
            version.MixCycles,
            "uL",
            "uL/s",
            "um/s",
            "ms");
    }

    private static List<string> GetRequiredReagentCodes(WorkflowVersion workflowVersion)
    {
        return workflowVersion.Steps
            .Where(x => !string.IsNullOrWhiteSpace(x.ReagentCode) && ((x.VolumeUl ?? 0) > 0 || IsLiquidAction(x.ActionType)))
            .Select(x => x.ReagentCode!.Trim())
            .Concat(workflowVersion.ReagentRequirements.Where(x => x.IsRequired).Select(x => x.ReagentCode.Trim()))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static bool IsLiquidAction(string actionType)
    {
        var normalized = actionType.Replace("_", string.Empty, StringComparison.Ordinal).Replace("-", string.Empty, StringComparison.Ordinal);
        return normalized.Contains("aspirat", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("dispens", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("liquiddetect", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("blowout", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("pipett", StringComparison.OrdinalIgnoreCase);
    }
}

public sealed record LiquidClassSnapshot(int SchemaVersion, IReadOnlyList<LiquidClassSnapshotItem> Items);

public sealed record LiquidClassSnapshotItem(
    string ReagentCode,
    string LiquidClassProfileId,
    string LiquidClassCode,
    string LiquidClassVersionId,
    int VersionNo,
    string VersionLabel,
    LiquidClassParameterSummary Parameters);

public sealed record LiquidClassParameterSummary(
    bool LiquidDetectionEnabled,
    int LiquidDetectionSensitivityPercent,
    int LiquidDetectionSpeedUmPerSecond,
    int AspirateSpeedUlPerSecond,
    int AspirateDelayMs,
    int DispenseSpeedUlPerSecond,
    int DispenseDelayMs,
    int LeadingAirGapUl,
    int TrailingAirGapUl,
    int BlowoutVolumeUl,
    int BlowoutDelayMs,
    int VolumeAdjustmentUl,
    int PreWetCycles,
    int MixCycles,
    string VolumeUnit,
    string FlowRateUnit,
    string DetectionSpeedUnit,
    string DelayUnit);

public sealed record RunLiquidClassSnapshot(int SchemaVersion, IReadOnlyList<RunLiquidClassBatchSnapshot> Batches);

public sealed record RunLiquidClassBatchSnapshot(string ChannelBatchId, string DrawerCode, LiquidClassSnapshot Snapshot);
