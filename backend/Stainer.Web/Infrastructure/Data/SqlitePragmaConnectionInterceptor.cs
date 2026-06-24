using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Stainer.Web.Infrastructure.Data;

public sealed class SqlitePragmaConnectionInterceptor : DbConnectionInterceptor
{
    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        ApplyPragmas(connection);
    }

    public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        await ApplyPragmasAsync(connection, cancellationToken);
    }

    private static void ApplyPragmas(DbConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = $"PRAGMA foreign_keys = ON; PRAGMA journal_mode = WAL; PRAGMA busy_timeout = {DatabaseInitializer.MinimumBusyTimeoutMilliseconds};";
        command.ExecuteNonQuery();
    }

    private static async Task ApplyPragmasAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = $"PRAGMA foreign_keys = ON; PRAGMA journal_mode = WAL; PRAGMA busy_timeout = {DatabaseInitializer.MinimumBusyTimeoutMilliseconds};";
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
