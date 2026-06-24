using Stainer.Web.Infrastructure.Data;
using Stainer.Web.Infrastructure.Health;

namespace Stainer.Tests;

public sealed class DatabaseInfrastructureTests
{
    [Fact]
    public async Task Initializer_enables_sqlite_pragmas_and_creates_parent_directory()
    {
        var databasePath = CreateTempDatabasePath();
        var connectionString = $"Data Source={databasePath}";

        await DatabaseInitializer.InitializeAsync(connectionString);
        var checker = new DatabaseHealthChecker(connectionString);
        var report = await checker.CheckAsync();

        Assert.True(File.Exists(databasePath));
        Assert.True(report.ForeignKeysEnabled);
        Assert.Equal("wal", report.JournalMode, ignoreCase: true);
        Assert.True(report.BusyTimeoutMilliseconds >= DatabaseInitializer.MinimumBusyTimeoutMilliseconds);
        Assert.True(report.CanReadWrite);
    }

    [Fact]
    public async Task Health_checker_reports_database_path_and_sqlite_version()
    {
        var databasePath = CreateTempDatabasePath();
        var connectionString = $"Data Source={databasePath}";
        var checker = new DatabaseHealthChecker(connectionString);

        var report = await checker.CheckAsync();

        Assert.Equal(Path.GetFullPath(databasePath), report.DatabasePath);
        Assert.False(string.IsNullOrWhiteSpace(report.SqliteVersion));
        Assert.True(report.CanReadWrite);
    }

    private static string CreateTempDatabasePath()
    {
        return Path.Combine(Path.GetTempPath(), "stainer-tests", Guid.NewGuid().ToString("N"), "stainer.db");
    }
}
