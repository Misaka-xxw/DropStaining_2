namespace Stainer.Web.Domain.Entities;

public sealed class LegacyImportRun
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTimeOffset ImportedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public string SourcePath { get; set; } = string.Empty;
    public string SourceHashJson { get; set; } = "{}";
    public bool IsDryRun { get; set; }
    public string Result { get; set; } = string.Empty;
    public string StatisticsJson { get; set; } = "{}";
    public string? ReportPath { get; set; }

    public ICollection<LegacyImportIssue> Issues { get; } = new List<LegacyImportIssue>();
    public ICollection<LegacyRuntimeSnapshot> RuntimeSnapshots { get; } = new List<LegacyRuntimeSnapshot>();
}
