using Microsoft.EntityFrameworkCore;
using Stainer.LegacyImporter;
using Stainer.Web.Application.Services;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Tests;

public sealed class LegacyJsonImporterTests
{
    [Fact]
    public async Task Dry_run_reports_legacy_json_without_writing_database()
    {
        var sourceDirectory = CreateLegacyFixture();
        await using var dbContext = await CreateMigratedContextAsync();
        var reportPath = Path.Combine(Path.GetTempPath(), "stainer-import-tests", Guid.NewGuid().ToString("N"), "dry-run-report.json");
        var importer = new LegacyJsonImporter(dbContext, new ReagentBarcodeParser());

        var report = await importer.ImportAsync(new LegacyImportOptions
        {
            SourceDirectory = sourceDirectory,
            DryRun = true,
            Apply = false,
            ReportPath = reportPath
        });

        Assert.True(report.DryRun);
        Assert.True(File.Exists(reportPath));
        Assert.Equal(2, report.Statistics.Scanned["users"]);
        Assert.Equal(0, await dbContext.Users.CountAsync());
        Assert.Equal(0, await dbContext.LegacyImportRuns.CountAsync());
    }

    [Fact]
    public async Task Apply_imports_legacy_json_hashes_passwords_and_is_idempotent()
    {
        var sourceDirectory = CreateLegacyFixture();
        await using var dbContext = await CreateMigratedContextAsync();
        var importer = new LegacyJsonImporter(dbContext, new ReagentBarcodeParser());

        var firstReport = await importer.ImportAsync(new LegacyImportOptions
        {
            SourceDirectory = sourceDirectory,
            DryRun = false,
            Apply = true,
            ReportPath = Path.Combine(sourceDirectory, "first-report.json")
        });
        var secondReport = await importer.ImportAsync(new LegacyImportOptions
        {
            SourceDirectory = sourceDirectory,
            DryRun = false,
            Apply = true,
            ReportPath = Path.Combine(sourceDirectory, "second-report.json")
        });

        Assert.True(await dbContext.Users.CountAsync() >= 2);
        var importedUser = await dbContext.Users.SingleAsync(x => x.Username == "operator");
        Assert.NotEqual("123456", importedUser.PasswordHash);
        Assert.True(LegacyPasswordHasher.Verify("123456", importedUser.PasswordHash!));

        var importedWorkflowCodes = new[] { "HE", "IHC" };
        Assert.Equal(2, await dbContext.WorkflowDefinitions.CountAsync(x => importedWorkflowCodes.Contains(x.Code)));
        Assert.Equal(2, await dbContext.WorkflowVersions.CountAsync(x => importedWorkflowCodes.Contains(x.WorkflowDefinition!.Code)));
        Assert.Equal(3, await dbContext.WorkflowSteps.CountAsync(x => importedWorkflowCodes.Contains(x.WorkflowVersion!.WorkflowDefinition!.Code)));
        Assert.Equal(2, await dbContext.ReagentDefinitions.CountAsync(x => x.ReagentCode == "ABC" || x.ReagentCode == "XYZ"));
        Assert.Equal(2, await dbContext.ReagentBottles.CountAsync());
        Assert.Equal(2, await dbContext.ReagentRackPlacements.CountAsync());
        Assert.Single(await dbContext.LiquidClassProfiles.ToListAsync());
        Assert.Single(await dbContext.LegacyRuntimeSnapshots.ToListAsync());

        Assert.Equal(2, await dbContext.LegacyImportRuns.CountAsync());
        Assert.True(await dbContext.Users.CountAsync() >= 2);
        Assert.Contains(firstReport.Issues, x => x.IssueType == "RuntimeSnapshotOnly");
        Assert.Contains(secondReport.Statistics.Skipped.Keys, x => x.Contains(":exists", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Apply_reports_conflicts_and_does_not_overwrite_existing_data()
    {
        var sourceDirectory = CreateLegacyFixture();
        await using var dbContext = await CreateMigratedContextAsync();
        var importer = new LegacyJsonImporter(dbContext, new ReagentBarcodeParser());

        await importer.ImportAsync(new LegacyImportOptions
        {
            SourceDirectory = sourceDirectory,
            DryRun = false,
            Apply = true,
            ReportPath = Path.Combine(sourceDirectory, "initial-report.json")
        });

        var conflictingDirectory = CreateLegacyFixture(operatorDisplayName: "Changed Operator");
        var conflictReport = await importer.ImportAsync(new LegacyImportOptions
        {
            SourceDirectory = conflictingDirectory,
            DryRun = false,
            Apply = true,
            ReportPath = Path.Combine(conflictingDirectory, "conflict-report.json")
        });

        var user = await dbContext.Users.SingleAsync(x => x.Username == "operator");
        Assert.Equal("Operator", user.DisplayName);
        Assert.Contains(conflictReport.Issues, x => x.IssueType == "Conflict" && x.RecordIdentifier == "operator");
        Assert.True(await dbContext.LegacyImportIssues.AnyAsync(x => x.IssueType == "Conflict"));
    }

    private static string CreateLegacyFixture(string operatorDisplayName = "Operator")
    {
        var root = Path.Combine(Path.GetTempPath(), "stainer-legacy-fixtures", Guid.NewGuid().ToString("N"));
        var protocolDirectory = Path.Combine(root, "protocols");
        Directory.CreateDirectory(protocolDirectory);

        File.WriteAllText(Path.Combine(root, "users.json"), $$"""
        [
          { "username": "operator", "password": "123456", "role": "operator", "display_name": "{{operatorDisplayName}}", "enabled": true },
          { "username": "admin", "password": "abcdef", "role": "admin", "display_name": "Admin", "enabled": true }
        ]
        """);

        File.WriteAllText(Path.Combine(root, "liquid_classes.json"), """
        {
          "Thin": {
            "aspirate_speed": 40,
            "dispense_speed": 45,
            "leading_air_gap_ul": 2,
            "trailing_air_gap_ul": 2,
            "excess_volume_ul": 5
          }
        }
        """);

        File.WriteAllText(Path.Combine(root, "reagents.json"), """
        [
          {
            "position": "R1",
            "barcode": "ABC05020260101001",
            "name": "Alpha",
            "code": "ABC",
            "reagent_type": "common",
            "volume_ml": 5,
            "min_alarm_ml": 1,
            "available": true,
            "lot_no": "LOT-A",
            "expire_date": "2027-01-01"
          },
          {
            "position": "R2",
            "barcode": "XYZ99920260101002",
            "name": "Beta",
            "code": "XYZ",
            "reagent_type": "wash",
            "volume_ml": 99.9,
            "min_alarm_ml": 2,
            "available": true,
            "lot_no": "LOT-B",
            "expire_date": "2027-01-02"
          }
        ]
        """);

        File.WriteAllText(Path.Combine(root, "positions.json"), """
        {
          "reagents": {
            "R1": { "x": 0, "y": 0, "z_travel": 0, "z_start": 0, "z_end": 0, "z_dispense": 0 }
          },
          "slides": {
            "C1S1": { "x": 0, "y": 0, "z_travel": 0, "z_start": 0, "z_end": 0, "z_dispense": 0 }
          }
        }
        """);

        File.WriteAllText(Path.Combine(root, "runtime.json"), """
        {
          "run_id": "RUN-LEGACY-001",
          "status": "ready",
          "initialized": true,
          "channels": [],
          "reagents": [],
          "active_user": null,
          "alarms": [],
          "logs": []
        }
        """);

        File.WriteAllText(Path.Combine(protocolDirectory, "ihc.json"), """
        {
          "code": "IHC",
          "name": "IHC",
          "version": "0.1",
          "description": "Fixture IHC workflow",
          "default_temperature_c": 42.0,
          "steps": [
            { "id": 1, "name": "Manual", "step_type": "manual", "duration_s": 60, "machine_execute": false },
            { "id": 2, "name": "Dispense", "step_type": "dispense", "reagent_code": "ABC", "volume_ul": 100, "duration_s": 20, "temperature_c": 42, "mix_after": true }
          ]
        }
        """);

        File.WriteAllText(Path.Combine(protocolDirectory, "he.json"), """
        {
          "code": "HE",
          "name": "HE",
          "version": "0.1",
          "description": "Fixture HE workflow",
          "default_temperature_c": 42.0,
          "steps": [
            { "id": 1, "name": "Wash", "step_type": "wash", "reagent_code": "XYZ", "duration_s": 10, "temperature_c": 42, "mix_after": true, "channel_level": true }
          ]
        }
        """);

        return root;
    }

    private static async Task<StainerDbContext> CreateMigratedContextAsync()
    {
        var databasePath = Path.Combine(Path.GetTempPath(), "stainer-legacy-import-tests", Guid.NewGuid().ToString("N"), "stainer.db");
        var connectionString = $"Data Source={databasePath}";
        var options = new DbContextOptionsBuilder<StainerDbContext>()
            .UseSqlite(connectionString)
            .AddInterceptors(new SqlitePragmaConnectionInterceptor())
            .Options;
        var dbContext = new StainerDbContext(options);
        DatabaseInitializer.EnsureDatabaseDirectory(connectionString);
        await dbContext.Database.MigrateAsync();
        return dbContext;
    }
}
