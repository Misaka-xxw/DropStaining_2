namespace Stainer.LegacyImporter;

public sealed class LegacyImportReport
{
    public DateTimeOffset StartedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public string SourceDirectory { get; set; } = string.Empty;
    public bool DryRun { get; set; }
    public string Result { get; set; } = LegacyImportResult.Completed;
    public string? ReportPath { get; set; }
    public List<string> Files { get; } = new();
    public Dictionary<string, string> SourceFileHashes { get; } = new(StringComparer.OrdinalIgnoreCase);
    public LegacyImportStatistics Statistics { get; } = new();
    public List<LegacyImportIssueDto> Issues { get; } = new();

    public void AddIssue(string file, string? recordIdentifier, string? fieldName, string issueType, string message, string? rawFragment)
    {
        Issues.Add(new LegacyImportIssueDto(file, recordIdentifier, fieldName, issueType, message, rawFragment));
        Statistics.Increment("issues", issueType);
    }
}
