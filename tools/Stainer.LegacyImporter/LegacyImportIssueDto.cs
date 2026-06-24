namespace Stainer.LegacyImporter;

public sealed record LegacyImportIssueDto(
    string File,
    string? RecordIdentifier,
    string? Field,
    string IssueType,
    string Message,
    string? RawFragment);
