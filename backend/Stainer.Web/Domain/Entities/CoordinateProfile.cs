namespace Stainer.Web.Domain.Entities;

public sealed class CoordinateProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string OriginDefinition { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? ActiveVersionId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<CoordinatePoint> CoordinatePoints { get; set; } = new List<CoordinatePoint>();
    public ICollection<CoordinateProfileVersion> Versions { get; set; } = new List<CoordinateProfileVersion>();
    public CoordinateProfileVersion? ActiveVersion { get; set; }
}

public sealed class CoordinateProfileVersion
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CoordinateProfileId { get; set; } = string.Empty;
    public int VersionNo { get; set; }
    public string VersionLabel { get; set; } = string.Empty;
    public string Status { get; set; } = CoordinateProfileVersionStatus.Draft;
    public bool IsActive { get; set; }
    public string UsageScope { get; set; } = CoordinateVersionUsageScope.MockOnly;
    public string VerificationStatus { get; set; } = CoordinateVersionVerificationStatus.Unverified;
    public string? SourceVersionId { get; set; }
    public string ChangeReason { get; set; } = string.Empty;
    public string ChangeSummaryJson { get; set; } = "{}";
    public string ValidationResultJson { get; set; } = "{}";
    public string? CreatedByUserId { get; set; }
    public string? PublishedByUserId { get; set; }
    public string? ActivatedByUserId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? PublishedAtUtc { get; set; }
    public DateTimeOffset? ActivatedAtUtc { get; set; }
    public DateTimeOffset? RetiredAtUtc { get; set; }

    public CoordinateProfile? CoordinateProfile { get; set; }
    public CoordinateProfileVersion? SourceVersion { get; set; }
    public User? CreatedByUser { get; set; }
    public User? PublishedByUser { get; set; }
    public User? ActivatedByUser { get; set; }
    public ICollection<CoordinateProfileVersion> DerivedVersions { get; set; } = new List<CoordinateProfileVersion>();
    public ICollection<CoordinatePoint> TargetPoints { get; set; } = new List<CoordinatePoint>();
}

public static class CoordinateProfileStatus
{
    public const string Enabled = "Enabled";
    public const string Disabled = "Disabled";
    public const string NeedsManualResolution = "NeedsManualResolution";
}

public static class CoordinateProfileVersionStatus
{
    public const string Draft = "Draft";
    public const string Published = "Published";
    public const string Active = "Active";
    public const string Retired = "Retired";
    public const string Invalid = "Invalid";
    public const string NeedsManualResolution = "NeedsManualResolution";
}

public static class CoordinateVersionUsageScope
{
    public const string MockOnly = "MockOnly";
    public const string RealEligible = "RealEligible";
}

public static class CoordinateVersionVerificationStatus
{
    public const string Unverified = "Unverified";
    public const string EngineerVerified = "EngineerVerified";
}
