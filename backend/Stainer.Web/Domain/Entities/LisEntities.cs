namespace Stainer.Web.Domain.Entities;

public sealed class MockLisEntry
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string NormalizedCode { get; set; } = string.Empty;
    public string? PrimaryAntibodyCode { get; set; }
    public string Scenario { get; set; } = MockLisScenario.Candidate;
    public bool IsEnabled { get; set; } = true;
    public string MetadataJson { get; set; } = "{}";
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}

public sealed class LisQueryLog
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Source { get; set; } = "MockLIS";
    public string Status { get; set; } = LisQueryStatus.Running;
    public string RawCode { get; set; } = string.Empty;
    public string NormalizedCode { get; set; } = string.Empty;
    public string CandidatePrimaryAntibodyCodesJson { get; set; } = "[]";
    public string? SelectedPrimaryAntibodyCode { get; set; }
    public DateTimeOffset? SelectedAtUtc { get; set; }
    public string? SelectedByUserId { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string ExceptionJson { get; set; } = "{}";
    public DateTimeOffset StartedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public User? SelectedByUser { get; set; }
}

public sealed class MockDemoDataTag
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string DemoKey { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

public static class MockLisScenario
{
    public const string Candidate = "Candidate";
    public const string NoResult = "NoResult";
    public const string Timeout = "Timeout";
    public const string Exception = "Exception";
}

public static class LisQueryStatus
{
    public const string Running = "Running";
    public const string SingleCandidate = "SingleCandidate";
    public const string MultipleCandidates = "MultipleCandidates";
    public const string NoResult = "NoResult";
    public const string TimedOut = "TimedOut";
    public const string Failed = "Failed";
    public const string Selected = "Selected";
    public const string CompatibilityFailed = "CompatibilityFailed";
}
