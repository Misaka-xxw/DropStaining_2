using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;
using Stainer.Web.Infrastructure;
using Stainer.Web.Infrastructure.Data;
using Stainer.Web.Infrastructure.Health;

var command = args.FirstOrDefault() ?? "help";
if (command is "help" or "-h" or "--help")
{
    PrintHelp();
    return 0;
}

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options => options.SingleLine = true);
builder.Logging.SetMinimumLevel(LogLevel.Warning);
var workspaceRoot = FindWorkspaceRoot(Directory.GetCurrentDirectory());
builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
    ["ConnectionStrings:StainerDatabase"] = $"Data Source={Path.Combine(workspaceRoot, "data", "stainer.db")}",
    ["MachineExecutor:LeasePath"] = Path.Combine(workspaceRoot, "data", "machine-executor.lock"),
    ["Safety:LogDirectory"] = Path.Combine(workspaceRoot, "data", "logs"),
    ["Database:BackupDirectory"] = Path.Combine(workspaceRoot, "data", "backups")
});
builder.Services.AddStainerInfrastructure(builder.Configuration, builder.Environment);
using var host = builder.Build();

await using var scope = host.Services.CreateAsyncScope();
var services = scope.ServiceProvider;
var dbContext = services.GetRequiredService<StainerDbContext>();
var actor = new AuthenticatedUser(string.Empty, "system", "System", "admin", ["admin", "engineer"]);

if (command == "backup-database")
{
    var outputDirectory = GetOption(args, "--output");
    var response = await services.GetRequiredService<DatabaseMaintenanceService>().BackupAsync(
        new DatabaseBackupRequest($"ops-backup-{Guid.NewGuid():N}", outputDirectory),
        actor);
    Console.WriteLine($"Backup: {response.BackupPath}");
    Console.WriteLine($"IntegrityBefore={response.IntegrityBeforeOk}; IntegrityAfter={response.IntegrityAfterOk}");
    return response.Ok ? 0 : 2;
}

await DatabaseInitializer.InitializeAsync(dbContext);
await dbContext.Database.MigrateAsync();
await services.GetRequiredService<ChannelBatchWorkflowBackfillService>().BackfillAsync();
await services.GetRequiredService<ReferenceDataSeeder>().SeedAsync();
await services.GetRequiredService<StartupRecoveryService>().RecoverAsync();

switch (command)
{
    case "seed-mock-demo-data":
    {
        var response = await services.GetRequiredService<MockDemoDataSeeder>()
            .SeedAsync($"ops-seed-mock-demo-{Guid.NewGuid():N}", actor);
        Console.WriteLine(response.Message);
        Console.WriteLine($"Created={response.CreatedCount}; Updated={response.UpdatedCount}; Skipped={response.SkippedCount}");
        return response.Ok ? 0 : 2;
    }
    case "reset-mock-demo-data":
    {
        var confirmation = GetOption(args, "--confirm") ?? string.Empty;
        var response = await services.GetRequiredService<MockDemoDataSeeder>()
            .ResetAsync(new ResetMockDemoDataRequest($"ops-reset-mock-demo-{Guid.NewGuid():N}", confirmation), actor);
        Console.WriteLine(response.Message);
        Console.WriteLine($"Deleted={response.DeletedCount}; Updated={response.UpdatedCount}; Skipped={response.SkippedCount}");
        return response.Ok ? 0 : 2;
    }
    case "verify-prehardware-readiness":
    {
        var readiness = await services.GetRequiredService<PreHardwareReadinessService>().VerifyAsync(createBackup: false);
        var backupDirectory = Path.Combine(Directory.GetCurrentDirectory(), "data", "backups", "prehardware");
        var backup = await services.GetRequiredService<DatabaseMaintenanceService>().BackupAsync(
            new DatabaseBackupRequest($"ops-readiness-backup-{Guid.NewGuid():N}", backupDirectory),
            actor);
        Console.WriteLine($"Pre-hardware readiness: {(readiness.Ok ? "PASS" : "FAIL")}");
        foreach (var check in readiness.Checks)
        {
            Console.WriteLine($"{(check.Ok ? "PASS" : "FAIL")} {check.Code}: {check.Message}");
        }

        Console.WriteLine($"{(backup.Ok ? "PASS" : "FAIL")} database_backup_success: {backup.BackupPath}; IntegrityBefore={backup.IntegrityBeforeOk}; IntegrityAfter={backup.IntegrityAfterOk}");

        if (readiness.BlockingReasons.Count > 0)
        {
            Console.WriteLine("Blocking reasons:");
            foreach (var reason in readiness.BlockingReasons)
            {
                Console.WriteLine($"- {reason}");
            }
        }

        return readiness.Ok && backup.Ok ? 0 : 2;
    }
    case "request-restore":
    {
        var backupPath = GetOption(args, "--backup");
        var reason = GetOption(args, "--reason") ?? "Operations restore request.";
        if (string.IsNullOrWhiteSpace(backupPath))
        {
            Console.Error.WriteLine("--backup is required.");
            return 2;
        }

        var response = await services.GetRequiredService<DatabaseMaintenanceService>().RequestRestoreAsync(
            new DatabaseRestoreRequest($"ops-restore-{Guid.NewGuid():N}", backupPath, reason),
            actor);
        Console.WriteLine(response.Message);
        Console.WriteLine($"Backup={response.BackupPath}; Integrity={response.IntegrityOk}; RestartRequired={response.RestartRequired}");
        return response.Ok ? 0 : 2;
    }
    default:
        Console.Error.WriteLine($"Unknown command: {command}");
        PrintHelp();
        return 2;
}

static string? GetOption(string[] args, string name)
{
    for (var i = 0; i < args.Length - 1; i++)
    {
        if (string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase))
        {
            return args[i + 1];
        }
    }

    return null;
}

static void PrintHelp()
{
    Console.WriteLine("Stainer operations");
    Console.WriteLine("Commands:");
    Console.WriteLine("  verify-prehardware-readiness");
    Console.WriteLine("  seed-mock-demo-data (Development + Device:Mode=Mock only)");
    Console.WriteLine("  reset-mock-demo-data --confirm \"RESET MOCK DEMO DATA\" (Development + Device:Mode=Mock only)");
    Console.WriteLine("  backup-database [--output <directory>]");
    Console.WriteLine("  request-restore --backup <path> [--reason <text>]");
}

static string FindWorkspaceRoot(string startDirectory)
{
    var directory = new DirectoryInfo(startDirectory);
    while (directory is not null)
    {
        if (File.Exists(Path.Combine(directory.FullName, "Stainer.sln")))
        {
            return directory.FullName;
        }

        directory = directory.Parent;
    }

    return startDirectory;
}
