using Microsoft.Data.Sqlite;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Infrastructure.Health;

public sealed class DatabaseHealthChecker(string connectionString)
{
    public async Task<DatabaseHealthReport> CheckAsync(CancellationToken cancellationToken = default)
    {
        await DatabaseInitializer.InitializeAsync(connectionString, cancellationToken);

        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        var sqliteVersion = await ExecuteScalarAsync<string>(connection, "SELECT sqlite_version();", cancellationToken);
        var foreignKeys = await ExecuteScalarAsync<long>(connection, "PRAGMA foreign_keys;", cancellationToken);
        var journalMode = await ExecuteScalarAsync<string>(connection, "PRAGMA journal_mode;", cancellationToken);
        var busyTimeout = await ExecuteScalarAsync<long>(connection, "PRAGMA busy_timeout;", cancellationToken);
        var canReadWrite = await CanReadWriteAsync(connection, cancellationToken);

        return new DatabaseHealthReport(
            DatabasePathResolver.GetDatabasePath(connectionString),
            sqliteVersion,
            foreignKeys == 1,
            journalMode,
            checked((int)busyTimeout),
            canReadWrite);
    }

    private static async Task<bool> CanReadWriteAsync(SqliteConnection connection, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = "CREATE TEMP TABLE IF NOT EXISTS health_check (id INTEGER PRIMARY KEY, value TEXT); INSERT INTO health_check (value) VALUES ('ok'); SELECT COUNT(*) FROM health_check;";
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(result) > 0;
    }

    private static async Task<T> ExecuteScalarAsync<T>(SqliteConnection connection, string sql, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        var value = await command.ExecuteScalarAsync(cancellationToken);
        if (value is null || value is DBNull)
        {
            throw new InvalidOperationException($"SQLite health query returned no value: {sql}");
        }

        return (T)Convert.ChangeType(value, typeof(T));
    }
}
