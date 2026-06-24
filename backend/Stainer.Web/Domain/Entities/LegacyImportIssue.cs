namespace Stainer.Web.Domain.Entities;

public sealed class LegacyImportIssue
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string LegacyImportRunId { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? RecordIdentifier { get; set; }
    public string? FieldName { get; set; }
    public string IssueType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? RawFragment { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public LegacyImportRun? LegacyImportRun { get; set; }
}
