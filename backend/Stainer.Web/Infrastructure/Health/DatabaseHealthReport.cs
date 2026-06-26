namespace Stainer.Web.Infrastructure.Health;

public sealed record DatabaseHealthReport(
    string DatabasePath,
    string SqliteVersion,
    bool ForeignKeysEnabled,
    string JournalMode,
    int BusyTimeoutMilliseconds,
    bool CanReadWrite,
    bool IntegrityOk = false);
