namespace Stainer.Web.Domain.Entities;

public sealed class LegacyRuntimeSnapshot
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string LegacyImportRunId { get; set; } = string.Empty;
    public string SourceFilePath { get; set; } = string.Empty;
    public string SourceFileHash { get; set; } = string.Empty;
    public string? RunId { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset CapturedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public string SnapshotJson { get; set; } = "{}";

    public LegacyImportRun? LegacyImportRun { get; set; }
}
