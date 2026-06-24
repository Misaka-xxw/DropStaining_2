using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Stainer.Web.Infrastructure.Data;

public static class DatabaseInitializer
{
    public const int MinimumBusyTimeoutMilliseconds = 5000;

    public static void EnsureDatabaseDirectory(string connectionString)
    {
        var databasePath = DatabasePathResolver.GetDatabasePath(connectionString);
        var parentDirectory = Path.GetDirectoryName(databasePath);
        if (!string.IsNullOrWhiteSpace(parentDirectory))
        {
            Directory.CreateDirectory(parentDirectory);
        }
    }

    public static async Task InitializeAsync(StainerDbContext dbContext, CancellationToken cancellationToken = default)
    {
        EnsureDatabaseDirectory(dbContext.Database.GetConnectionString() ?? throw new InvalidOperationException("Database connection string is missing."));
        await dbContext.Database.OpenConnectionAsync(cancellationToken);
        try
        {
            await dbContext.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = ON;", cancellationToken);
            await dbContext.Database.ExecuteSqlRawAsync("PRAGMA journal_mode = WAL;", cancellationToken);
            await dbContext.Database.ExecuteSqlRawAsync($"PRAGMA busy_timeout = {MinimumBusyTimeoutMilliseconds};", cancellationToken);
        }
        finally
        {
            await dbContext.Database.CloseConnectionAsync();
        }
    }

    public static async Task InitializeAsync(string connectionString, CancellationToken cancellationToken = default)
    {
        EnsureDatabaseDirectory(connectionString);
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await ExecuteNonQueryAsync(connection, "PRAGMA foreign_keys = ON;", cancellationToken);
        await ExecuteNonQueryAsync(connection, "PRAGMA journal_mode = WAL;", cancellationToken);
        await ExecuteNonQueryAsync(connection, $"PRAGMA busy_timeout = {MinimumBusyTimeoutMilliseconds};", cancellationToken);
    }

    private static async Task ExecuteNonQueryAsync(SqliteConnection connection, string sql, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
