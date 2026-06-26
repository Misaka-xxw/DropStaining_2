using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Infrastructure.Health;

public sealed class DatabaseMaintenanceService(
    StainerDbContext dbContext,
    DatabaseHealthChecker healthChecker,
    CommandIdempotencyService idempotencyService,
    SafetyLogWriter safetyLogWriter,
    IConfiguration configuration,
    IHostEnvironment environment)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<DatabaseMaintenanceReportResponse> CheckAsync(CancellationToken cancellationToken = default)
    {
        var health = await healthChecker.CheckAsync(cancellationToken);
        var applied = (await dbContext.Database.GetAppliedMigrationsAsync(cancellationToken)).Count();
        var pending = (await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Count();
        var ok = health.ForeignKeysEnabled
            && string.Equals(health.JournalMode, "wal", StringComparison.OrdinalIgnoreCase)
            && health.BusyTimeoutMilliseconds >= DatabaseInitializer.MinimumBusyTimeoutMilliseconds
            && health.CanReadWrite
            && health.IntegrityOk
            && pending == 0;
        return new DatabaseMaintenanceReportResponse(
            ok,
            health.DatabasePath,
            health.SqliteVersion,
            health.ForeignKeysEnabled,
            health.JournalMode,
            health.BusyTimeoutMilliseconds,
            health.CanReadWrite,
            health.IntegrityOk,
            applied,
            pending,
            ok ? "Database health checks passed." : "Database health checks failed or migrations are pending.");
    }

    public async Task<DatabaseBackupResponse> BackupAsync(DatabaseBackupRequest request, AuthenticatedUser actor, CancellationToken cancellationToken = default)
    {
        var commandId = RequireCommandId(request.CommandId);
        const string operation = "database.backup";
        var requestHash = HashRequest(operation, request);
        var existing = await dbContext.CommandReceipts
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.CommandId == commandId, cancellationToken);
        if (existing is not null)
        {
            if (existing.Operation != operation || existing.RequestHash != requestHash)
            {
                throw new BusinessRuleException("command_conflict", "commandId already exists for a different request.", StatusCodes.Status409Conflict);
            }

            var existingResponse = JsonSerializer.Deserialize<DatabaseBackupResponse>(existing.ResponseJson, JsonOptions);
            if (existingResponse is null)
            {
                throw new BusinessRuleException("command_replay_failed", "Stored command response could not be replayed.", StatusCodes.Status409Conflict);
            }

            return existingResponse with { Replayed = true };
        }

        var before = await CheckAsync(cancellationToken);
        if (!before.IntegrityOk || !before.CanReadWrite)
        {
            await SaveBackupAlarmAsync(
                "database_backup_health_failed",
                "Database backup was blocked because health or integrity checks failed.",
                cancellationToken);
            throw new BusinessRuleException("database_health_failed", "Database backup requires passing health and integrity checks.", StatusCodes.Status409Conflict);
        }

        var backupDirectory = ResolveBackupDirectory(request.OutputDirectory);
        Directory.CreateDirectory(backupDirectory);
        var backupPath = Path.Combine(backupDirectory, $"stainer-backup-{DateTimeOffset.UtcNow:yyyyMMdd-HHmmss}Z-{Guid.NewGuid():N}.db");
        try
        {
            backupPath = await BackupSqliteAsync(before.DatabasePath, backupPath, cancellationToken);
        }
        catch (Exception ex) when (ex is SqliteException or IOException or UnauthorizedAccessException)
        {
            await SaveBackupAlarmAsync(
                "database_backup_failed",
                $"Database backup failed: {ex.Message}",
                cancellationToken);
            throw new BusinessRuleException("database_backup_failed", $"Database backup failed: {ex.Message}", StatusCodes.Status500InternalServerError);
        }

        var afterIntegrityOk = await CheckBackupIntegrityAsync(backupPath, cancellationToken);
        if (!afterIntegrityOk)
        {
            await SaveBackupAlarmAsync(
                "database_backup_integrity_failed",
                "Database backup was created but failed integrity check.",
                cancellationToken);
            throw new BusinessRuleException("database_backup_integrity_failed", "Database backup failed integrity check.", StatusCodes.Status500InternalServerError);
        }

        var response = new DatabaseBackupResponse(
            true,
            commandId,
            false,
            backupPath,
            before.IntegrityOk,
            afterIntegrityOk,
            DateTimeOffset.UtcNow,
            "Database backup completed.");

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        dbContext.CommandReceipts.Add(new CommandReceipt
        {
            CommandId = commandId,
            Operation = operation,
            RequestHash = requestHash,
            Status = "Completed",
            ResponseJson = JsonSerializer.Serialize(response, JsonOptions),
            ActorUserId = string.IsNullOrWhiteSpace(actor.UserId) ? null : actor.UserId,
            EntityType = "DatabaseBackup",
            EntityId = backupPath,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            CompletedAtUtc = DateTimeOffset.UtcNow
        });
        dbContext.AuditLogs.Add(new AuditLog
        {
            ActorUserId = string.IsNullOrWhiteSpace(actor.UserId) ? null : actor.UserId,
            Action = "database.backup",
            EntityType = "DatabaseBackup",
            EntityId = backupPath,
            Message = JsonSerializer.Serialize(new { commandId, backupPath, before.DatabasePath }, JsonOptions),
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        await safetyLogWriter.WriteAsync(
            "runtime",
            "Information",
            "Database backup completed.",
            new SafetyLogContext(CommandId: commandId, DeviceMode: "Mock", Actor: actor.Username, Source: "DatabaseMaintenanceService"),
            cancellationToken: cancellationToken);

        return response;
    }

    public Task<DatabaseRestoreResponse> RequestRestoreAsync(DatabaseRestoreRequest request, AuthenticatedUser actor, CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "database.restore_request",
            request,
            actor,
            async () =>
            {
                if (string.IsNullOrWhiteSpace(request.Reason))
                {
                    throw new BusinessRuleException("reason_required", "Database restore requires a reason.", StatusCodes.Status400BadRequest);
                }

                var backupPath = Path.GetFullPath(request.BackupPath);
                if (!File.Exists(backupPath))
                {
                    throw new BusinessRuleException("backup_not_found", "Backup file was not found.", StatusCodes.Status404NotFound);
                }

                var integrityOk = await CheckBackupIntegrityAsync(backupPath, cancellationToken);
                if (!integrityOk)
                {
                    throw new BusinessRuleException("backup_integrity_failed", "Backup integrity check failed.", StatusCodes.Status409Conflict);
                }

                dbContext.AuditLogs.Add(new AuditLog
                {
                    ActorUserId = string.IsNullOrWhiteSpace(actor.UserId) ? null : actor.UserId,
                    Action = "database.restore_requested",
                    EntityType = "DatabaseBackup",
                    EntityId = backupPath,
                    Message = JsonSerializer.Serialize(new { commandId = request.CommandId, backupPath, request.Reason, restartRequired = true }, JsonOptions),
                    CreatedAtUtc = DateTimeOffset.UtcNow
                });
                await safetyLogWriter.WriteAsync(
                    "runtime",
                    "Warning",
                    "Database restore requested. Stop the service and restore offline before restart.",
                    new SafetyLogContext(CommandId: request.CommandId, Actor: actor.Username, Source: "DatabaseMaintenanceService"),
                    cancellationToken: cancellationToken);
                return new CommandExecutionResult<DatabaseRestoreResponse>(
                    new DatabaseRestoreResponse(
                        true,
                        request.CommandId,
                        false,
                        backupPath,
                        true,
                        true,
                        "Restore request was audited. Stop the service and restore the verified backup offline before restart."),
                    "DatabaseBackup",
                    backupPath);
            },
            cancellationToken);
    }

    private string ResolveBackupDirectory(string? requested)
    {
        var configured = requested ?? configuration["Database:BackupDirectory"];
        if (!string.IsNullOrWhiteSpace(configured))
        {
            return Path.IsPathRooted(configured)
                ? configured
                : Path.GetFullPath(Path.Combine(environment.ContentRootPath, configured));
        }

        return Path.GetFullPath(Path.Combine(environment.ContentRootPath, "..", "..", "data", "backups"));
    }

    private async Task SaveBackupAlarmAsync(string code, string message, CancellationToken cancellationToken)
    {
        dbContext.Alarms.Add(new Alarm
        {
            Code = code,
            Severity = "Critical",
            Message = message,
            Status = "Active",
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task<string> BackupSqliteAsync(string databasePath, string backupPath, CancellationToken cancellationToken)
    {
        try
        {
            await BackupSqliteWithBackupApiAsync(databasePath, backupPath, cancellationToken);
            return backupPath;
        }
        catch (SqliteException)
        {
            var fallbackPath = TryCleanupPartialBackup(backupPath)
                ? backupPath
                : Path.Combine(
                    Path.GetDirectoryName(backupPath) ?? ".",
                    $"{Path.GetFileNameWithoutExtension(backupPath)}-vacuum-{Guid.NewGuid():N}.db");
            try
            {
                await BackupSqliteWithVacuumIntoAsync(databasePath, fallbackPath, cancellationToken);
                return fallbackPath;
            }
            catch (Exception ex) when (ex is SqliteException or IOException or UnauthorizedAccessException)
            {
                var copyPath = Path.Combine(
                    Path.GetDirectoryName(backupPath) ?? ".",
                    $"{Path.GetFileNameWithoutExtension(backupPath)}-copy-{Guid.NewGuid():N}.db");
                await CheckpointSqliteAsync(databasePath, cancellationToken);
                await using var source = new FileStream(databasePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 81920, useAsync: true);
                await using var destination = new FileStream(copyPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, useAsync: true);
                await source.CopyToAsync(destination, cancellationToken);
                return copyPath;
            }
        }
    }

    private static async Task BackupSqliteWithBackupApiAsync(string databasePath, string backupPath, CancellationToken cancellationToken)
    {
        await using var source = new SqliteConnection($"Data Source={databasePath}");
        await using var destination = new SqliteConnection($"Data Source={backupPath}");
        await source.OpenAsync(cancellationToken);
        await destination.OpenAsync(cancellationToken);
        source.BackupDatabase(destination);
    }

    private static async Task BackupSqliteWithVacuumIntoAsync(string databasePath, string backupPath, CancellationToken cancellationToken)
    {
        await using var source = new SqliteConnection($"Data Source={databasePath}");
        await source.OpenAsync(cancellationToken);
        await using (var timeout = source.CreateCommand())
        {
            timeout.CommandText = $"PRAGMA busy_timeout = {DatabaseInitializer.MinimumBusyTimeoutMilliseconds};";
            await timeout.ExecuteNonQueryAsync(cancellationToken);
        }

        await using var command = source.CreateCommand();
        command.CommandText = $"VACUUM INTO '{backupPath.Replace("'", "''")}';";
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task CheckpointSqliteAsync(string databasePath, CancellationToken cancellationToken)
    {
        await using var connection = new SqliteConnection($"Data Source={databasePath}");
        await connection.OpenAsync(cancellationToken);
        await using var timeout = connection.CreateCommand();
        timeout.CommandText = $"PRAGMA busy_timeout = {DatabaseInitializer.MinimumBusyTimeoutMilliseconds};";
        await timeout.ExecuteNonQueryAsync(cancellationToken);
        await using var checkpoint = connection.CreateCommand();
        checkpoint.CommandText = "PRAGMA wal_checkpoint(TRUNCATE);";
        await checkpoint.ExecuteNonQueryAsync(cancellationToken);
    }

    private static bool TryCleanupPartialBackup(string backupPath)
    {
        try
        {
            foreach (var path in new[] { backupPath, $"{backupPath}-journal", $"{backupPath}-wal", $"{backupPath}-shm" })
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            return true;
        }
        catch (IOException)
        {
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }

    private static async Task<bool> CheckBackupIntegrityAsync(string backupPath, CancellationToken cancellationToken)
    {
        await using var connection = new SqliteConnection($"Data Source={backupPath};Mode=ReadOnly");
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA integrity_check;";
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return string.Equals(Convert.ToString(result), "ok", StringComparison.OrdinalIgnoreCase);
    }

    private static string RequireCommandId(string commandId)
    {
        if (string.IsNullOrWhiteSpace(commandId))
        {
            throw new BusinessRuleException("command_id_required", "commandId is required.", StatusCodes.Status400BadRequest);
        }

        return commandId.Trim();
    }

    private static string HashRequest(string operation, object request)
    {
        var json = JsonSerializer.Serialize(new { operation, request }, JsonOptions);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(json));
        return Convert.ToHexString(bytes);
    }
}
