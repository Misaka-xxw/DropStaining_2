using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.Services;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.LegacyImporter;

public sealed class LegacyJsonImporter(StainerDbContext dbContext, IReagentBarcodeParser barcodeParser)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<LegacyImportReport> ImportAsync(LegacyImportOptions options, CancellationToken cancellationToken = default)
    {
        var sourceDirectory = Path.GetFullPath(options.SourceDirectory);
        var report = new LegacyImportReport
        {
            SourceDirectory = sourceDirectory,
            DryRun = options.DryRun
        };

        if (!Directory.Exists(sourceDirectory))
        {
            report.Result = LegacyImportResult.Failed;
            report.AddIssue(sourceDirectory, null, null, "SourceDirectoryMissing", "Source directory does not exist.", null);
            await WriteReportAsync(options, report, cancellationToken);
            return report;
        }

        var files = DiscoverFiles(sourceDirectory, report);
        foreach (var file in files)
        {
            var relativePath = Relative(sourceDirectory, file);
            report.Files.Add(relativePath);
            report.SourceFileHashes[relativePath] = ComputeSha256(file);
        }

        LegacyImportRun? importRun = null;
        if (options.Apply)
        {
            await DatabaseInitializer.InitializeAsync(dbContext, cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            await new ReferenceDataSeeder(dbContext).SeedAsync(cancellationToken);

            importRun = new LegacyImportRun
            {
                ImportedAtUtc = report.StartedAtUtc,
                SourcePath = sourceDirectory,
                IsDryRun = false
            };
            dbContext.LegacyImportRuns.Add(importRun);
        }

        await ImportUsersAsync(sourceDirectory, options, report, cancellationToken);
        await ImportLiquidClassesAsync(sourceDirectory, options, report, cancellationToken);
        await ImportProtocolsAsync(sourceDirectory, options, report, cancellationToken);
        await ImportReagentsAsync(sourceDirectory, options, report, cancellationToken);
        await ImportPositionsAsync(sourceDirectory, options, report, cancellationToken);
        await ImportRuntimeSnapshotAsync(sourceDirectory, options, report, importRun, cancellationToken);

        report.Result = report.Issues.Count == 0 ? LegacyImportResult.Completed : LegacyImportResult.CompletedWithIssues;
        report.CompletedAtUtc = DateTimeOffset.UtcNow;

        if (options.Apply && importRun is not null)
        {
            importRun.SourceHashJson = JsonSerializer.Serialize(report.SourceFileHashes, JsonOptions);
            importRun.Result = report.Result;
            importRun.StatisticsJson = JsonSerializer.Serialize(report.Statistics, JsonOptions);
            foreach (var issue in report.Issues)
            {
                importRun.Issues.Add(new LegacyImportIssue
                {
                    FilePath = issue.File,
                    RecordIdentifier = issue.RecordIdentifier,
                    FieldName = issue.Field,
                    IssueType = issue.IssueType,
                    Message = issue.Message,
                    RawFragment = issue.RawFragment,
                    CreatedAtUtc = DateTimeOffset.UtcNow
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        await WriteReportAsync(options, report, cancellationToken);

        if (options.Apply && importRun is not null)
        {
            importRun.ReportPath = report.ReportPath;
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return report;
    }

    private static IReadOnlyList<string> DiscoverFiles(string sourceDirectory, LegacyImportReport report)
    {
        var files = new List<string>();
        foreach (var relativePath in new[] { "users.json", "reagents.json", "liquid_classes.json", "positions.json", "runtime.json" })
        {
            var fullPath = Path.Combine(sourceDirectory, relativePath);
            if (File.Exists(fullPath))
            {
                files.Add(fullPath);
            }
            else
            {
                report.AddIssue(relativePath, null, null, "FileMissing", "Expected legacy JSON file was not found.", null);
            }
        }

        var protocolDirectory = Path.Combine(sourceDirectory, "protocols");
        if (Directory.Exists(protocolDirectory))
        {
            files.AddRange(Directory.GetFiles(protocolDirectory, "*.json").OrderBy(x => x, StringComparer.OrdinalIgnoreCase));
        }
        else
        {
            report.AddIssue("protocols", null, null, "DirectoryMissing", "Expected protocol directory was not found.", null);
        }

        return files;
    }

    private async Task ImportUsersAsync(string sourceDirectory, LegacyImportOptions options, LegacyImportReport report, CancellationToken cancellationToken)
    {
        var file = Path.Combine(sourceDirectory, "users.json");
        if (!File.Exists(file))
        {
            return;
        }

        using var document = await ReadJsonDocumentAsync(file, report, cancellationToken);
        if (document is null || document.RootElement.ValueKind != JsonValueKind.Array)
        {
            report.AddIssue(Relative(sourceDirectory, file), null, null, "UnexpectedShape", "users.json must be an array.", document?.RootElement.GetRawText());
            return;
        }

        var relativeFile = Relative(sourceDirectory, file);
        foreach (var userElement in document.RootElement.EnumerateArray())
        {
            report.Statistics.IncrementScanned("users");
            var username = userElement.GetStringOrNull("username");
            var password = userElement.GetStringOrNull("password");
            var roleCode = userElement.GetStringOrNull("role");
            var displayName = userElement.GetStringOrNull("display_name") ?? username ?? string.Empty;
            var enabled = userElement.GetBoolOrNull("enabled") ?? true;
            var recordId = username ?? "(missing username)";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(roleCode))
            {
                AddFailedIssue(report, relativeFile, recordId, null, "RequiredFieldMissing", "User username and role are required.", userElement);
                continue;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                AddFailedIssue(report, relativeFile, recordId, "password", "PasswordMissing", "Legacy user has no password to hash.", userElement);
                continue;
            }

            if (options.DryRun)
            {
                report.Statistics.IncrementSkipped("users:dry_run");
                continue;
            }

            var role = await dbContext.Roles.SingleOrDefaultAsync(x => x.Code == roleCode, cancellationToken);
            if (role is null)
            {
                role = new Role { Code = roleCode, Name = roleCode, CreatedAtUtc = DateTimeOffset.UtcNow };
                dbContext.Roles.Add(role);
                report.Statistics.IncrementImported("roles");
            }

            var user = await dbContext.Users
                .Include(x => x.UserRoles)
                .SingleOrDefaultAsync(x => x.Username == username, cancellationToken);
            if (user is null)
            {
                user = new User
                {
                    Username = username,
                    DisplayName = displayName,
                    IsEnabled = enabled,
                    PasswordHash = LegacyPasswordHasher.HashPassword(password),
                    PasswordHashAlgorithm = LegacyPasswordHasher.Algorithm,
                    PasswordUpdatedAtUtc = DateTimeOffset.UtcNow,
                    CreatedAtUtc = DateTimeOffset.UtcNow
                };
                user.UserRoles.Add(new UserRole { RoleId = role.Id, CreatedAtUtc = DateTimeOffset.UtcNow });
                dbContext.Users.Add(user);
                report.Statistics.IncrementImported("users");
                continue;
            }

            if (user.DisplayName != displayName || user.IsEnabled != enabled)
            {
                report.AddIssue(relativeFile, recordId, null, "Conflict", "Existing user with same username has different display name or enabled state; not overwritten.", Truncate(userElement.GetRawText()));
                report.Statistics.IncrementSkipped("users:conflict");
                continue;
            }

            if (!string.IsNullOrWhiteSpace(user.PasswordHash) && !LegacyPasswordHasher.Verify(password, user.PasswordHash))
            {
                report.AddIssue(relativeFile, recordId, "password", "Conflict", "Existing user password hash does not match legacy password; not overwritten.", Truncate(userElement.GetRawText()));
                report.Statistics.IncrementSkipped("users:conflict");
                continue;
            }

            if (user.UserRoles.All(x => x.RoleId != role.Id))
            {
                user.UserRoles.Add(new UserRole { RoleId = role.Id, CreatedAtUtc = DateTimeOffset.UtcNow });
                report.Statistics.IncrementImported("user_roles");
            }
            else
            {
                report.Statistics.IncrementSkipped("users:exists");
            }
        }

        if (options.Apply)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task ImportLiquidClassesAsync(string sourceDirectory, LegacyImportOptions options, LegacyImportReport report, CancellationToken cancellationToken)
    {
        var file = Path.Combine(sourceDirectory, "liquid_classes.json");
        if (!File.Exists(file))
        {
            return;
        }

        using var document = await ReadJsonDocumentAsync(file, report, cancellationToken);
        if (document is null || document.RootElement.ValueKind != JsonValueKind.Object)
        {
            report.AddIssue(Relative(sourceDirectory, file), null, null, "UnexpectedShape", "liquid_classes.json must be an object keyed by liquid class code.", document?.RootElement.GetRawText());
            return;
        }

        var relativeFile = Relative(sourceDirectory, file);
        foreach (var liquidClassProperty in document.RootElement.EnumerateObject())
        {
            report.Statistics.IncrementScanned("liquid_classes");
            var code = liquidClassProperty.Name;
            var value = liquidClassProperty.Value;
            var profile = new LiquidClassProfile
            {
                Code = code,
                Name = code,
                AspirateSpeedUlPerSecond = value.GetIntOrNull("aspirate_speed"),
                DispenseSpeedUlPerSecond = value.GetIntOrNull("dispense_speed"),
                LeadingAirGapUl = value.GetIntOrNull("leading_air_gap_ul"),
                TrailingAirGapUl = value.GetIntOrNull("trailing_air_gap_ul"),
                ExcessVolumeUl = value.GetIntOrNull("excess_volume_ul"),
                LegacyParametersJson = value.GetRawText(),
                CreatedAtUtc = DateTimeOffset.UtcNow
            };

            if (options.DryRun)
            {
                report.Statistics.IncrementSkipped("liquid_classes:dry_run");
                continue;
            }

            var existing = await dbContext.LiquidClassProfiles.SingleOrDefaultAsync(x => x.Code == code, cancellationToken);
            if (existing is null)
            {
                dbContext.LiquidClassProfiles.Add(profile);
                report.Statistics.IncrementImported("liquid_classes");
            }
            else if (!SameLiquidClass(existing, profile))
            {
                report.AddIssue(relativeFile, code, null, "Conflict", "Existing liquid class with same code has different content; not overwritten.", Truncate(value.GetRawText()));
                report.Statistics.IncrementSkipped("liquid_classes:conflict");
            }
            else
            {
                report.Statistics.IncrementSkipped("liquid_classes:exists");
            }
        }

        if (options.Apply)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task ImportProtocolsAsync(string sourceDirectory, LegacyImportOptions options, LegacyImportReport report, CancellationToken cancellationToken)
    {
        var protocolDirectory = Path.Combine(sourceDirectory, "protocols");
        if (!Directory.Exists(protocolDirectory))
        {
            return;
        }

        foreach (var file in Directory.GetFiles(protocolDirectory, "*.json").OrderBy(x => x, StringComparer.OrdinalIgnoreCase))
        {
            using var document = await ReadJsonDocumentAsync(file, report, cancellationToken);
            if (document is null || document.RootElement.ValueKind != JsonValueKind.Object)
            {
                report.AddIssue(Relative(sourceDirectory, file), null, null, "UnexpectedShape", "Protocol JSON must be an object.", document?.RootElement.GetRawText());
                continue;
            }

            await ImportProtocolAsync(sourceDirectory, file, document.RootElement, options, report, cancellationToken);
        }

        if (options.Apply)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task ImportProtocolAsync(
        string sourceDirectory,
        string file,
        JsonElement protocolElement,
        LegacyImportOptions options,
        LegacyImportReport report,
        CancellationToken cancellationToken)
    {
        report.Statistics.IncrementScanned("workflow_definitions");
        var relativeFile = Relative(sourceDirectory, file);
        var code = protocolElement.GetStringOrNull("code");
        var name = protocolElement.GetStringOrNull("name") ?? code ?? string.Empty;
        var description = protocolElement.GetStringOrNull("description") ?? string.Empty;
        var versionLabel = protocolElement.GetStringOrNull("version") ?? "1.0";

        if (string.IsNullOrWhiteSpace(code))
        {
            AddFailedIssue(report, relativeFile, null, "code", "RequiredFieldMissing", "Protocol code is required.", protocolElement);
            return;
        }

        if (options.DryRun)
        {
            if (protocolElement.TryGetProperty("steps", out var dryRunStepsElement) && dryRunStepsElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var stepElement in dryRunStepsElement.EnumerateArray())
                {
                    report.Statistics.IncrementScanned("workflow_steps");
                    var stepNo = stepElement.GetIntOrNull("id") ?? 0;
                    if (stepElement.GetIntOrNull("volume_ul") is null && stepElement.TryGetProperty("min_volume_ul", out _) && stepElement.TryGetProperty("max_volume_ul", out _))
                    {
                        report.AddIssue(relativeFile, $"{code}:{versionLabel}:{stepNo}", "volume_ul", "ManualConfirmationRequired", "Legacy step has min/max volume range but target model stores a single volume; range would be preserved in legacy_parameters_json on apply.", Truncate(stepElement.GetRawText()));
                    }
                }
            }

            report.Statistics.IncrementSkipped("workflow_definitions:dry_run");
            report.Statistics.IncrementSkipped("workflow_versions:dry_run");
            return;
        }

        var definition = await dbContext.WorkflowDefinitions
            .Include(x => x.Versions)
            .SingleOrDefaultAsync(x => x.Code == code, cancellationToken);
        if (definition is null)
        {
            definition = new WorkflowDefinition
            {
                Code = code,
                Name = name,
                WorkflowType = code,
                Description = description,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };
            dbContext.WorkflowDefinitions.Add(definition);
            report.Statistics.IncrementImported("workflow_definitions");
        }
        else if (definition.Name != name || definition.Description != description)
        {
            report.AddIssue(relativeFile, code, null, "Conflict", "Existing workflow definition with same code has different content; not overwritten.", Truncate(protocolElement.GetRawText()));
            report.Statistics.IncrementSkipped("workflow_definitions:conflict");
            return;
        }
        else
        {
            report.Statistics.IncrementSkipped("workflow_definitions:exists");
        }

        var existingVersion = await dbContext.WorkflowVersions
            .Include(x => x.Steps)
            .SingleOrDefaultAsync(x => x.WorkflowDefinitionId == definition.Id && x.VersionLabel == versionLabel, cancellationToken);
        if (existingVersion is not null)
        {
            report.Statistics.IncrementSkipped("workflow_versions:exists");
            return;
        }

        var version = new WorkflowVersion
        {
            WorkflowDefinition = definition,
            VersionLabel = versionLabel,
            VersionNo = ParseVersionNo(versionLabel),
            Status = WorkflowVersionStatus.Published,
            ChangeNote = $"Imported from legacy JSON file {relativeFile}.",
            PublishedAtUtc = DateTimeOffset.UtcNow,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        if (protocolElement.TryGetProperty("steps", out var stepsElement) && stepsElement.ValueKind == JsonValueKind.Array)
        {
            foreach (var stepElement in stepsElement.EnumerateArray())
            {
                report.Statistics.IncrementScanned("workflow_steps");
                var stepNo = stepElement.GetIntOrNull("id") ?? version.Steps.Count + 1;
                var volumeUl = stepElement.GetIntOrNull("volume_ul");
                if (volumeUl is null && stepElement.TryGetProperty("min_volume_ul", out _) && stepElement.TryGetProperty("max_volume_ul", out _))
                {
                    report.AddIssue(relativeFile, $"{code}:{versionLabel}:{stepNo}", "volume_ul", "ManualConfirmationRequired", "Legacy step has min/max volume range but target model stores a single volume; range preserved in legacy_parameters_json.", Truncate(stepElement.GetRawText()));
                }

                version.Steps.Add(new WorkflowStep
                {
                    StepNo = stepNo,
                    MajorStepCode = stepElement.GetStringOrNull("step_type") ?? $"STEP-{stepNo}",
                    StepName = stepElement.GetStringOrNull("name") ?? $"Step {stepNo}",
                    ActionType = stepElement.GetStringOrNull("step_type") ?? "unknown",
                    ReagentCode = stepElement.GetStringOrNull("reagent_code"),
                    VolumeUl = volumeUl,
                    DurationSeconds = stepElement.GetIntOrNull("duration_s"),
                    TargetTemperatureDeciC = stepElement.GetDeciCOrNull("temperature_c") ?? protocolElement.GetDeciCOrNull("default_temperature_c"),
                    MixParametersJson = JsonSerializer.Serialize(new { mixAfter = stepElement.GetBoolOrNull("mix_after") ?? false }, JsonOptions),
                    WashParametersJson = JsonSerializer.Serialize(new { channelLevel = stepElement.GetBoolOrNull("channel_level") ?? false }, JsonOptions),
                    LegacyParametersJson = stepElement.GetRawText(),
                    FailureStrategy = "Stop",
                    CreatedAtUtc = DateTimeOffset.UtcNow
                });
                report.Statistics.IncrementImported("workflow_steps");
            }
        }
        else
        {
            report.AddIssue(relativeFile, code, "steps", "RequiredFieldMissing", "Protocol has no steps array.", Truncate(protocolElement.GetRawText()));
        }

        dbContext.WorkflowVersions.Add(version);
        report.Statistics.IncrementImported("workflow_versions");
    }

    private async Task ImportReagentsAsync(string sourceDirectory, LegacyImportOptions options, LegacyImportReport report, CancellationToken cancellationToken)
    {
        var file = Path.Combine(sourceDirectory, "reagents.json");
        if (!File.Exists(file))
        {
            return;
        }

        using var document = await ReadJsonDocumentAsync(file, report, cancellationToken);
        if (document is null || document.RootElement.ValueKind != JsonValueKind.Array)
        {
            report.AddIssue(Relative(sourceDirectory, file), null, null, "UnexpectedShape", "reagents.json must be an array.", document?.RootElement.GetRawText());
            return;
        }

        ReagentScanSession? scanSession = null;
        var relativeFile = Relative(sourceDirectory, file);
        if (options.Apply)
        {
            var sourceHash = ComputeSha256(file);
            var sessionCode = $"LEGACY-REAGENTS-{sourceHash[..12]}";
            scanSession = await dbContext.ReagentScanSessions.SingleOrDefaultAsync(x => x.SessionCode == sessionCode, cancellationToken);
            if (scanSession is null)
            {
                scanSession = new ReagentScanSession
                {
                    SessionCode = sessionCode,
                    Status = "Imported",
                    StartedAtUtc = DateTimeOffset.UtcNow,
                    CompletedAtUtc = DateTimeOffset.UtcNow
                };
                dbContext.ReagentScanSessions.Add(scanSession);
                report.Statistics.IncrementImported("reagent_scan_sessions");
            }
            else
            {
                report.Statistics.IncrementSkipped("reagent_scan_sessions:exists");
            }
        }

        foreach (var reagentElement in document.RootElement.EnumerateArray())
        {
            report.Statistics.IncrementScanned("reagents");
            var code = reagentElement.GetStringOrNull("code");
            var name = reagentElement.GetStringOrNull("name") ?? code ?? string.Empty;
            var positionCode = reagentElement.GetStringOrNull("position");
            var barcode = reagentElement.GetStringOrNull("barcode");
            var recordId = $"{positionCode ?? "unknown"}:{code ?? "unknown"}";

            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(barcode))
            {
                AddFailedIssue(report, relativeFile, recordId, null, "RequiredFieldMissing", "Reagent code and barcode are required.", reagentElement);
                continue;
            }

            var parsed = barcodeParser.Parse(barcode);
            if (!parsed.IsValid)
            {
                report.AddIssue(relativeFile, recordId, "barcode", "InvalidBarcode", parsed.ValidationMessage, Truncate(reagentElement.GetRawText()));
            }
            else if (!string.Equals(parsed.ReagentCode, code, StringComparison.Ordinal))
            {
                report.AddIssue(relativeFile, recordId, "barcode", "BarcodeCodeMismatch", "Barcode reagent code segment differs from legacy reagent code field; legacy code field is used for catalog identity.", Truncate(reagentElement.GetRawText()));
            }

            if (options.DryRun)
            {
                report.Statistics.IncrementSkipped("reagents:dry_run");
                continue;
            }

            var definition = await EnsureReagentDefinitionAsync(relativeFile, reagentElement, code, name, report, cancellationToken);
            if (definition is null)
            {
                continue;
            }

            var volumeUl = reagentElement.GetMicrolitersFromMillilitersOrNull("volume_ml") ?? parsed.QuantityUl ?? 0;
            var expirationDate = ParseDateOnly(reagentElement.GetStringOrNull("expire_date"));
            if (expirationDate is null)
            {
                report.AddIssue(relativeFile, recordId, "expire_date", "InvalidDate", "Expiration date is missing or invalid; using DateOnly.MinValue in imported copy.", Truncate(reagentElement.GetRawText()));
            }

            var bottle = await dbContext.ReagentBottles.SingleOrDefaultAsync(x => x.FullBarcode == barcode, cancellationToken);
            if (bottle is null)
            {
                bottle = new ReagentBottle
                {
                    ReagentDefinitionId = definition.Id,
                    FullBarcode = barcode,
                    ReagentCode = code,
                    ProductionBatchNo = reagentElement.GetStringOrNull("lot_no") ?? parsed.ProductionBatchNo ?? string.Empty,
                    SerialNo = parsed.SerialNo ?? barcode,
                    InitialVolumeUl = volumeUl,
                    RemainingVolumeUl = volumeUl,
                    ExpirationDate = expirationDate ?? DateOnly.MinValue,
                    Status = reagentElement.GetBoolOrNull("available") == false ? "Unavailable" : "Available",
                    FirstScannedAtUtc = DateTimeOffset.UtcNow,
                    LastScannedAtUtc = DateTimeOffset.UtcNow,
                    CreatedAtUtc = DateTimeOffset.UtcNow
                };
                dbContext.ReagentBottles.Add(bottle);
                report.Statistics.IncrementImported("reagent_bottles");
            }
            else if (!SameBottle(bottle, code, volumeUl, expirationDate))
            {
                report.AddIssue(relativeFile, recordId, "barcode", "Conflict", "Existing reagent bottle with same full barcode has different content; not overwritten.", Truncate(reagentElement.GetRawText()));
                report.Statistics.IncrementSkipped("reagent_bottles:conflict");
                continue;
            }
            else
            {
                report.Statistics.IncrementSkipped("reagent_bottles:exists");
            }

            if (!string.IsNullOrWhiteSpace(positionCode))
            {
                await EnsurePlacementAsync(relativeFile, reagentElement, bottle, positionCode, scanSession, report, cancellationToken);
            }

            if (scanSession is not null && !string.IsNullOrWhiteSpace(positionCode))
            {
                await EnsureScanItemAsync(relativeFile, reagentElement, scanSession, positionCode, parsed, report, cancellationToken);
            }
        }

        if (options.Apply)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task<ReagentDefinition?> EnsureReagentDefinitionAsync(
        string relativeFile,
        JsonElement reagentElement,
        string code,
        string name,
        LegacyImportReport report,
        CancellationToken cancellationToken)
    {
        var definition = dbContext.ReagentDefinitions.Local.FirstOrDefault(x => x.ReagentCode == code)
            ?? await dbContext.ReagentDefinitions.SingleOrDefaultAsync(x => x.ReagentCode == code, cancellationToken);
        var reagentType = reagentElement.GetStringOrNull("reagent_type") ?? string.Empty;
        var minimumAlarmVolumeUl = reagentElement.GetMicrolitersFromMillilitersOrNull("min_alarm_ml");
        if (definition is null)
        {
            definition = new ReagentDefinition
            {
                ReagentCode = code,
                Name = name,
                ReagentType = reagentType,
                MinimumAlarmVolumeUl = minimumAlarmVolumeUl,
                LegacyMetadataJson = reagentElement.GetRawText(),
                IsEnabled = reagentElement.GetBoolOrNull("available") ?? true,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };
            dbContext.ReagentDefinitions.Add(definition);
            report.Statistics.IncrementImported("reagent_definitions");
            return definition;
        }

        if (definition.Name != name || definition.ReagentType != reagentType || definition.MinimumAlarmVolumeUl != minimumAlarmVolumeUl)
        {
            report.AddIssue(relativeFile, code, null, "Conflict", "Existing reagent definition with same code has different content; not overwritten.", Truncate(reagentElement.GetRawText()));
            report.Statistics.IncrementSkipped("reagent_definitions:conflict");
            return definition;
        }

        report.Statistics.IncrementSkipped("reagent_definitions:exists");
        return definition;
    }

    private async Task EnsurePlacementAsync(
        string relativeFile,
        JsonElement reagentElement,
        ReagentBottle bottle,
        string positionCode,
        ReagentScanSession? scanSession,
        LegacyImportReport report,
        CancellationToken cancellationToken)
    {
        var position = await dbContext.ReagentRackPositions.SingleOrDefaultAsync(x => x.Code == positionCode, cancellationToken);
        if (position is null)
        {
            report.AddIssue(relativeFile, positionCode, "position", "InvalidReference", "Reagent rack position does not exist.", Truncate(reagentElement.GetRawText()));
            report.Statistics.IncrementFailed("reagent_rack_placements");
            return;
        }

        var activePlacement = await dbContext.ReagentRackPlacements
            .SingleOrDefaultAsync(x => x.ReagentBottleId == bottle.Id && x.RemovedAtUtc == null, cancellationToken);
        if (activePlacement is not null && activePlacement.ReagentRackPositionId == position.Id)
        {
            report.Statistics.IncrementSkipped("reagent_rack_placements:exists");
            return;
        }

        var occupiedPlacement = await dbContext.ReagentRackPlacements
            .Include(x => x.ReagentBottle)
            .SingleOrDefaultAsync(x => x.ReagentRackPositionId == position.Id && x.RemovedAtUtc == null, cancellationToken);
        if (occupiedPlacement is not null && occupiedPlacement.ReagentBottleId != bottle.Id)
        {
            report.AddIssue(relativeFile, positionCode, "position", "Conflict", "Reagent rack position is already occupied by another active bottle; not overwritten.", Truncate(reagentElement.GetRawText()));
            report.Statistics.IncrementSkipped("reagent_rack_placements:conflict");
            return;
        }

        if (activePlacement is not null)
        {
            activePlacement.RemovedAtUtc = DateTimeOffset.UtcNow;
        }

        dbContext.ReagentRackPlacements.Add(new ReagentRackPlacement
        {
            ReagentBottleId = bottle.Id,
            ReagentRackPositionId = position.Id,
            ReagentScanSession = scanSession,
            PlacedAtUtc = DateTimeOffset.UtcNow,
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
        report.Statistics.IncrementImported("reagent_rack_placements");
    }

    private async Task EnsureScanItemAsync(
        string relativeFile,
        JsonElement reagentElement,
        ReagentScanSession scanSession,
        string positionCode,
        ReagentBarcodeParseResult parsed,
        LegacyImportReport report,
        CancellationToken cancellationToken)
    {
        var position = await dbContext.ReagentRackPositions.SingleOrDefaultAsync(x => x.Code == positionCode, cancellationToken);
        if (position is null)
        {
            return;
        }

        var existing = await dbContext.ReagentScanItems
            .SingleOrDefaultAsync(x => x.ReagentScanSessionId == scanSession.Id && x.ReagentRackPositionId == position.Id, cancellationToken);
        if (existing is not null)
        {
            report.Statistics.IncrementSkipped("reagent_scan_items:exists");
            return;
        }

        dbContext.ReagentScanItems.Add(new ReagentScanItem
        {
            ReagentScanSession = scanSession,
            ReagentRackPositionId = position.Id,
            ScannerChannelNo = position.ScannerChannelNo,
            ScannerChannelCode = position.ScannerChannelCode,
            LocatorCode = position.Code,
            ScanResult = parsed.IsValid ? ReagentScanResult.Valid : ReagentScanResult.Invalid,
            RawBarcode = reagentElement.GetStringOrNull("barcode"),
            ParsedReagentCode = parsed.ReagentCode,
            ParsedQuantityUl = parsed.QuantityUl,
            ParsedBatchNo = parsed.ProductionBatchNo,
            ParsedSerialNo = parsed.SerialNo,
            IsValidationPassed = parsed.IsValid,
            ValidationMessage = parsed.ValidationMessage,
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
        report.Statistics.IncrementImported("reagent_scan_items");
    }

    private async Task ImportPositionsAsync(string sourceDirectory, LegacyImportOptions options, LegacyImportReport report, CancellationToken cancellationToken)
    {
        var file = Path.Combine(sourceDirectory, "positions.json");
        if (!File.Exists(file))
        {
            return;
        }

        using var document = await ReadJsonDocumentAsync(file, report, cancellationToken);
        if (document is null || document.RootElement.ValueKind != JsonValueKind.Object)
        {
            report.AddIssue(Relative(sourceDirectory, file), null, null, "UnexpectedShape", "positions.json must be an object.", document?.RootElement.GetRawText());
            return;
        }

        var relativeFile = Relative(sourceDirectory, file);
        if (options.DryRun)
        {
            CountPositionPoints(document.RootElement, relativeFile, report, dryRun: true);
            return;
        }

        var sourceHash = ComputeSha256(file);
        var profileCode = $"LegacyJson-{sourceHash[..12]}";
        var profile = await dbContext.CoordinateProfiles.SingleOrDefaultAsync(x => x.Code == profileCode, cancellationToken);
        if (profile is null)
        {
            profile = new CoordinateProfile
            {
                Code = profileCode,
                Name = "Legacy JSON coordinate profile",
                Status = "Imported",
                OriginDefinition = "Imported from legacy positions.json; source coordinate units and origin require manual verification.",
                IsActive = false,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };
            dbContext.CoordinateProfiles.Add(profile);
            report.Statistics.IncrementImported("coordinate_profiles");
        }
        else
        {
            report.Statistics.IncrementSkipped("coordinate_profiles:exists");
        }

        foreach (var group in document.RootElement.EnumerateObject())
        {
            if (group.Value.ValueKind != JsonValueKind.Object)
            {
                report.AddIssue(relativeFile, group.Name, null, "UnexpectedShape", "Position group must be an object.", Truncate(group.Value.GetRawText()));
                continue;
            }

            foreach (var point in group.Value.EnumerateObject())
            {
                report.Statistics.IncrementScanned("coordinate_points");
                var pointCode = point.Name;
                var pointType = group.Name == "reagents" ? "ReagentRackPosition" : "LegacySlidePosition";
                var values = point.Value;
                var allZero = IsAllZeroCoordinate(values);
                if (allZero)
                {
                    report.AddIssue(relativeFile, pointCode, null, "PlaceholderCoordinate", "Legacy coordinate is all zero; imported as pending calibration with null X/Y/Z values.", Truncate(values.GetRawText()));
                }
                else
                {
                    report.AddIssue(relativeFile, pointCode, null, "ManualConfirmationRequired", "Legacy coordinate has non-zero values but source unit/origin is not verified; values are preserved in issue raw fragment and not activated.", Truncate(values.GetRawText()));
                }

                var exists = await dbContext.CoordinatePoints.AnyAsync(x => x.CoordinateProfileId == profile.Id && x.PointCode == pointCode, cancellationToken);
                if (exists)
                {
                    report.Statistics.IncrementSkipped("coordinate_points:exists");
                    continue;
                }

                dbContext.CoordinatePoints.Add(new CoordinatePoint
                {
                    CoordinateProfile = profile,
                    PointCode = pointCode,
                    PointType = pointType,
                    RequiresCalibration = true,
                    IsEnabled = false,
                    CreatedAtUtc = DateTimeOffset.UtcNow
                });
                report.Statistics.IncrementImported("coordinate_points");
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static void CountPositionPoints(JsonElement root, string relativeFile, LegacyImportReport report, bool dryRun)
    {
        foreach (var group in root.EnumerateObject())
        {
            if (group.Value.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            foreach (var _ in group.Value.EnumerateObject())
            {
                var point = _;
                report.Statistics.IncrementScanned("coordinate_points");
                if (IsAllZeroCoordinate(point.Value))
                {
                    report.AddIssue(relativeFile, point.Name, null, "PlaceholderCoordinate", "Legacy coordinate is all zero; apply would import it as pending calibration with null X/Y/Z values.", Truncate(point.Value.GetRawText()));
                }
                else
                {
                    report.AddIssue(relativeFile, point.Name, null, "ManualConfirmationRequired", "Legacy coordinate has non-zero values but source unit/origin is not verified; apply would not activate the value.", Truncate(point.Value.GetRawText()));
                }

                if (dryRun)
                {
                    report.Statistics.IncrementSkipped("coordinate_points:dry_run");
                }
            }
        }
    }

    private async Task ImportRuntimeSnapshotAsync(
        string sourceDirectory,
        LegacyImportOptions options,
        LegacyImportReport report,
        LegacyImportRun? importRun,
        CancellationToken cancellationToken)
    {
        var file = Path.Combine(sourceDirectory, "runtime.json");
        if (!File.Exists(file))
        {
            return;
        }

        using var document = await ReadJsonDocumentAsync(file, report, cancellationToken);
        if (document is null || document.RootElement.ValueKind != JsonValueKind.Object)
        {
            report.AddIssue(Relative(sourceDirectory, file), null, null, "UnexpectedShape", "runtime.json must be an object.", document?.RootElement.GetRawText());
            return;
        }

        var relativeFile = Relative(sourceDirectory, file);
        report.Statistics.IncrementScanned("runtime_snapshots");
        report.AddIssue(relativeFile, document.RootElement.GetStringOrNull("run_id"), null, "RuntimeSnapshotOnly", "runtime.json was treated only as a legacy state snapshot/report source, not as a new execution ledger.", Truncate(document.RootElement.GetRawText()));

        if (options.DryRun || importRun is null)
        {
            report.Statistics.IncrementSkipped("runtime_snapshots:dry_run");
            return;
        }

        var sourceHash = ComputeSha256(file);
        var exists = await dbContext.LegacyRuntimeSnapshots.AnyAsync(x => x.SourceFileHash == sourceHash, cancellationToken);
        if (exists)
        {
            report.Statistics.IncrementSkipped("runtime_snapshots:exists");
            return;
        }

        dbContext.LegacyRuntimeSnapshots.Add(new LegacyRuntimeSnapshot
        {
            LegacyImportRun = importRun,
            SourceFilePath = relativeFile,
            SourceFileHash = sourceHash,
            RunId = document.RootElement.GetStringOrNull("run_id"),
            Status = document.RootElement.GetStringOrNull("status"),
            SnapshotJson = document.RootElement.GetRawText(),
            CapturedAtUtc = DateTimeOffset.UtcNow
        });
        report.Statistics.IncrementImported("runtime_snapshots");
    }

    private static async Task<JsonDocument?> ReadJsonDocumentAsync(string file, LegacyImportReport report, CancellationToken cancellationToken)
    {
        try
        {
            await using var stream = File.OpenRead(file);
            return await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
        }
        catch (JsonException ex)
        {
            report.AddIssue(file, null, null, "InvalidJson", ex.Message, null);
            report.Statistics.IncrementFailed("json_files");
            return null;
        }
    }

    private static async Task WriteReportAsync(LegacyImportOptions options, LegacyImportReport report, CancellationToken cancellationToken)
    {
        var reportPath = options.ReportPath;
        if (string.IsNullOrWhiteSpace(reportPath))
        {
            var reportsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "data", "import-reports");
            Directory.CreateDirectory(reportsDirectory);
            reportPath = Path.Combine(reportsDirectory, $"legacy-import-{DateTimeOffset.UtcNow:yyyyMMdd-HHmmss}.json");
        }
        else
        {
            var parent = Path.GetDirectoryName(Path.GetFullPath(reportPath));
            if (!string.IsNullOrWhiteSpace(parent))
            {
                Directory.CreateDirectory(parent);
            }
        }

        report.ReportPath = Path.GetFullPath(reportPath);
        await File.WriteAllTextAsync(report.ReportPath, JsonSerializer.Serialize(report, JsonOptions), cancellationToken);
    }

    private static bool SameLiquidClass(LiquidClassProfile existing, LiquidClassProfile imported)
    {
        return existing.Name == imported.Name
            && existing.AspirateSpeedUlPerSecond == imported.AspirateSpeedUlPerSecond
            && existing.DispenseSpeedUlPerSecond == imported.DispenseSpeedUlPerSecond
            && existing.LeadingAirGapUl == imported.LeadingAirGapUl
            && existing.TrailingAirGapUl == imported.TrailingAirGapUl
            && existing.ExcessVolumeUl == imported.ExcessVolumeUl;
    }

    private static bool SameBottle(ReagentBottle bottle, string code, int volumeUl, DateOnly? expirationDate)
    {
        return bottle.ReagentCode == code
            && bottle.InitialVolumeUl == volumeUl
            && bottle.ExpirationDate == (expirationDate ?? DateOnly.MinValue);
    }

    private static bool IsAllZeroCoordinate(JsonElement element)
    {
        foreach (var propertyName in new[] { "x", "y", "z_travel", "z_start", "z_end", "z_dispense" })
        {
            var value = element.GetIntOrNull(propertyName);
            if (value is not 0)
            {
                return false;
            }
        }

        return true;
    }

    private static DateOnly? ParseDateOnly(string? value)
    {
        return DateOnly.TryParse(value, out var date) ? date : null;
    }

    private static int ParseVersionNo(string versionLabel)
    {
        var digits = new string(versionLabel.Where(char.IsDigit).ToArray());
        if (digits.Length == 0)
        {
            return 1;
        }

        return int.TryParse(digits, out var versionNo) ? versionNo : Math.Abs(versionLabel.GetHashCode());
    }

    private static string ComputeSha256(string file)
    {
        using var sha = SHA256.Create();
        using var stream = File.OpenRead(file);
        return Convert.ToHexString(sha.ComputeHash(stream));
    }

    private static void AddFailedIssue(LegacyImportReport report, string file, string? recordIdentifier, string? fieldName, string issueType, string message, JsonElement rawElement)
    {
        report.AddIssue(file, recordIdentifier, fieldName, issueType, message, Truncate(rawElement.GetRawText()));
        report.Statistics.IncrementFailed(issueType);
    }

    private static string Relative(string root, string file)
    {
        return Path.GetRelativePath(root, file).Replace('\\', '/');
    }

    private static string Truncate(string? value, int maxLength = 4000)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
        {
            return value ?? string.Empty;
        }

        return value[..maxLength];
    }
}
