namespace Stainer.Web.Domain.Entities;

public sealed class LiquidClassProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? EnabledVersionId { get; set; }
    public int? AspirateSpeedUlPerSecond { get; set; }
    public int? DispenseSpeedUlPerSecond { get; set; }
    public int? LeadingAirGapUl { get; set; }
    public int? TrailingAirGapUl { get; set; }
    public int? ExcessVolumeUl { get; set; }
    public int? PreWetCycles { get; set; }
    public int? MixCycles { get; set; }
    public string LegacyParametersJson { get; set; } = "{}";
    public bool IsEnabled { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public LiquidClassVersion? EnabledVersion { get; set; }
    public ICollection<LiquidClassVersion> Versions { get; set; } = new List<LiquidClassVersion>();
    public ICollection<ReagentDefinition> ReagentDefinitions { get; } = new List<ReagentDefinition>();
}

public sealed class LiquidClassVersion
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string LiquidClassProfileId { get; set; } = string.Empty;
    public int VersionNo { get; set; }
    public string VersionLabel { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = LiquidClassVersionStatus.Draft;
    public string? SourceVersionId { get; set; }
    public string ChangeReason { get; set; } = string.Empty;
    public string ChangeSummaryJson { get; set; } = "{}";
    public bool LiquidDetectionEnabled { get; set; }
    public int LiquidDetectionSensitivityPercent { get; set; }
    public int LiquidDetectionSpeedUmPerSecond { get; set; }
    public int AspirateSpeedUlPerSecond { get; set; }
    public int AspirateDelayMs { get; set; }
    public int DispenseSpeedUlPerSecond { get; set; }
    public int DispenseDelayMs { get; set; }
    public int LeadingAirGapUl { get; set; }
    public int TrailingAirGapUl { get; set; }
    public int BlowoutVolumeUl { get; set; }
    public int BlowoutDelayMs { get; set; }
    public int VolumeAdjustmentUl { get; set; }
    public int PreWetCycles { get; set; }
    public int MixCycles { get; set; }
    public string? CreatedByUserId { get; set; }
    public string? PublishedByUserId { get; set; }
    public string? EnabledByUserId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? PublishedAtUtc { get; set; }
    public DateTimeOffset? EnabledAtUtc { get; set; }

    public LiquidClassProfile? LiquidClassProfile { get; set; }
    public LiquidClassVersion? SourceVersion { get; set; }
    public User? CreatedByUser { get; set; }
    public User? PublishedByUser { get; set; }
    public User? EnabledByUser { get; set; }
    public ICollection<LiquidClassVersion> DerivedVersions { get; set; } = new List<LiquidClassVersion>();
    public ICollection<LiquidClassVersionDifference> Differences { get; set; } = new List<LiquidClassVersionDifference>();
    public ICollection<LiquidClassValidationRecord> ValidationRecords { get; set; } = new List<LiquidClassValidationRecord>();
}

public sealed class LiquidClassVersionDifference
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string LiquidClassVersionId { get; set; } = string.Empty;
    public string ParameterName { get; set; } = string.Empty;
    public string? PreviousValue { get; set; }
    public string? NewValue { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public LiquidClassVersion? LiquidClassVersion { get; set; }
}

public sealed class LiquidClassValidationRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string LiquidClassVersionId { get; set; } = string.Empty;
    public string Stage { get; set; } = string.Empty;
    public bool IsValid { get; set; }
    public string ResultJson { get; set; } = "{}";
    public string? ValidatedByUserId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public LiquidClassVersion? LiquidClassVersion { get; set; }
    public User? ValidatedByUser { get; set; }
}

public static class LiquidClassVersionStatus
{
    public const string Draft = "Draft";
    public const string Published = "Published";
    public const string Enabled = "Enabled";
}

public static class LiquidClassValidationStage
{
    public const string Draft = "Draft";
    public const string Publish = "Publish";
    public const string Enable = "Enable";
}
