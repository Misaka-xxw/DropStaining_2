using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class MockDemoDataSeeder(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService,
    DeviceModeService deviceModeService,
    IHostEnvironment environment)
{
    public const string DemoKey = "mock-demo";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task<MockDemoDataResponse> SeedAsync(
        string commandId,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            commandId,
            "mock_demo.seed",
            new { commandId },
            actor,
            async () =>
            {
                EnsureAllowed();
                var counts = new MutationCounts();
                var now = DateTimeOffset.UtcNow;

                await EnsureReagentDefinitionsAsync(now, counts, cancellationToken);
                var (heVersion, ihcVersion) = await EnsureWorkflowsAsync(now, counts, cancellationToken);
                await EnsurePrimaryAntibodyMappingsAsync(ihcVersion, now, counts, cancellationToken);
                await EnsureMockLisEntriesAsync(now, counts, cancellationToken);
                await EnsureDemoReagentScenariosAsync(now, counts, cancellationToken);
                await EnsureDemoRackAsync(now, counts, cancellationToken);

                AddAudit(actor, "mock_demo.seed", "MockDemoData", DemoKey, new
                {
                    commandId,
                    heWorkflowVersionId = heVersion.Id,
                    ihcWorkflowVersionId = ihcVersion.Id,
                    counts.Created,
                    counts.Updated,
                    counts.Skipped
                });

                return new CommandExecutionResult<MockDemoDataResponse>(
                    new MockDemoDataResponse(true, commandId, false, counts.Created, counts.Updated, 0, counts.Skipped, "Mock demo data seeded."),
                    "MockDemoData",
                    DemoKey);
            },
            cancellationToken);
    }

    public Task<MockDemoDataResponse> ResetAsync(
        ResetMockDemoDataRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "mock_demo.reset",
            request,
            actor,
            async () =>
            {
                EnsureAllowed();
                if (!string.Equals(request.Confirmation?.Trim(), "RESET MOCK DEMO DATA", StringComparison.Ordinal))
                {
                    throw new BusinessRuleException("mock_demo_reset_confirmation_required", "Confirmation must be exactly: RESET MOCK DEMO DATA.", StatusCodes.Status400BadRequest);
                }

                var counts = await ResetTaggedDataAsync(cancellationToken);
                AddAudit(actor, "mock_demo.reset", "MockDemoData", DemoKey, new
                {
                    request.CommandId,
                    request.Confirmation,
                    counts.Deleted,
                    counts.Updated,
                    counts.Skipped
                });

                return new CommandExecutionResult<MockDemoDataResponse>(
                    new MockDemoDataResponse(true, request.CommandId, false, 0, counts.Updated, counts.Deleted, counts.Skipped, "Mock demo data reset completed."),
                    "MockDemoData",
                    DemoKey);
            },
            cancellationToken);
    }

    private void EnsureAllowed()
    {
        if (!environment.IsDevelopment())
        {
            throw new BusinessRuleException("mock_demo_environment_required", "Mock demo data commands are allowed only in Development environment.", StatusCodes.Status409Conflict);
        }

        if (!deviceModeService.IsMock)
        {
            throw new BusinessRuleException("mock_demo_mode_required", "Mock demo data commands are allowed only when DeviceMode=Mock.", StatusCodes.Status409Conflict);
        }
    }

    private async Task EnsureReagentDefinitionsAsync(DateTimeOffset now, MutationCounts counts, CancellationToken cancellationToken)
    {
        var defaultLiquidClass = await LoadDefaultLiquidClassProfileAsync(cancellationToken);
        foreach (var reagent in DemoReagents())
        {
            var existing = await dbContext.ReagentDefinitions.SingleOrDefaultAsync(x => x.ReagentCode == reagent.Code, cancellationToken);
            if (existing is null)
            {
                existing = new ReagentDefinition
                {
                    ReagentCode = reagent.Code,
                    Name = reagent.Name,
                    ReagentType = reagent.Type,
                    LiquidClassProfileId = defaultLiquidClass?.Id,
                    MinimumAlarmVolumeUl = reagent.MinimumAlarmVolumeUl,
                    LegacyMetadataJson = JsonSerializer.Serialize(new { demo = DemoKey }, JsonOptions),
                    IsEnabled = true,
                    CreatedAtUtc = now
                };
                dbContext.ReagentDefinitions.Add(existing);
                counts.Created++;
                await EnsureTagAsync(nameof(ReagentDefinition), existing.Id, counts, cancellationToken);
            }
            else if (existing.LiquidClassProfileId is null && defaultLiquidClass is not null)
            {
                existing.LiquidClassProfileId = defaultLiquidClass.Id;
                existing.UpdatedAtUtc = now;
                counts.Updated++;
            }
        }
    }

    private async Task<LiquidClassProfile?> LoadDefaultLiquidClassProfileAsync(CancellationToken cancellationToken)
    {
        return await dbContext.LiquidClassProfiles
            .Where(x => x.IsEnabled && x.EnabledVersionId != null)
            .OrderByDescending(x => x.Code == "FactoryGeneral-v1")
            .ThenBy(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<(WorkflowVersion HeVersion, WorkflowVersion IhcVersion)> EnsureWorkflowsAsync(
        DateTimeOffset now,
        MutationCounts counts,
        CancellationToken cancellationToken)
    {
        var he = await LoadRequiredPublishedWorkflowAsync(ReferenceDataSeeder.DefaultHeWorkflowCode, cancellationToken);
        var ihc = await LoadRequiredPublishedWorkflowAsync(ReferenceDataSeeder.DefaultIhcWorkflowCode, cancellationToken);

        return (he, ihc);
    }

    private async Task<WorkflowVersion> LoadRequiredPublishedWorkflowAsync(string workflowCode, CancellationToken cancellationToken)
    {
        return await dbContext.WorkflowVersions
            .Include(x => x.WorkflowDefinition)
            .Where(x => x.WorkflowDefinition != null
                && x.WorkflowDefinition.Code == workflowCode
                && x.Status == WorkflowVersionStatus.Published)
            .OrderByDescending(x => x.DefaultExperimentType != null)
            .ThenByDescending(x => x.VersionNo)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new BusinessRuleException("default_workflow_required", $"Published workflow {workflowCode} is required.", StatusCodes.Status409Conflict);
    }

    private async Task<WorkflowVersion> EnsureWorkflowAsync(
        string code,
        string name,
        string workflowType,
        IReadOnlyList<DemoWorkflowStep> steps,
        IReadOnlyList<(string ReagentCode, int RequiredVolumeUl)> requirements,
        DateTimeOffset now,
        MutationCounts counts,
        CancellationToken cancellationToken)
    {
        var workflow = await dbContext.WorkflowDefinitions
            .AsSplitQuery()
            .Include(x => x.Versions)
            .ThenInclude(x => x.Steps)
            .Include(x => x.Versions)
            .ThenInclude(x => x.ReagentRequirements)
            .SingleOrDefaultAsync(x => x.Code == code, cancellationToken);
        if (workflow is null)
        {
            workflow = new WorkflowDefinition
            {
                Code = code,
                Name = name,
                WorkflowType = workflowType,
                Description = $"Development-only {name}.",
                IsEnabled = true,
                CreatedAtUtc = now
            };
            var version = new WorkflowVersion
            {
                WorkflowDefinition = workflow,
                VersionNo = 1,
                VersionLabel = "1",
                Status = WorkflowVersionStatus.Published,
                ChangeNote = "Seeded by --seed-mock-demo-data.",
                PublishedAtUtc = now,
                CreatedAtUtc = now
            };
            workflow.Versions.Add(version);
            AddWorkflowChildren(version, steps, requirements, now);
            dbContext.WorkflowDefinitions.Add(workflow);
            counts.Created++;
        }

        await EnsureTagAsync(nameof(WorkflowDefinition), workflow.Id, counts, cancellationToken);
        var selectedVersion = workflow.Versions
            .Where(x => x.Status == WorkflowVersionStatus.Published)
            .OrderBy(x => x.VersionNo)
            .FirstOrDefault();
        if (selectedVersion is null)
        {
            selectedVersion = new WorkflowVersion
            {
                WorkflowDefinition = workflow,
                VersionNo = workflow.Versions.Count == 0 ? 1 : workflow.Versions.Max(x => x.VersionNo) + 1,
                VersionLabel = (workflow.Versions.Count == 0 ? 1 : workflow.Versions.Max(x => x.VersionNo) + 1).ToString(),
                Status = WorkflowVersionStatus.Published,
                ChangeNote = "Seeded by --seed-mock-demo-data.",
                PublishedAtUtc = now,
                CreatedAtUtc = now
            };
            workflow.Versions.Add(selectedVersion);
            AddWorkflowChildren(selectedVersion, steps, requirements, now);
            counts.Created++;
        }

        await EnsureTagAsync(nameof(WorkflowVersion), selectedVersion.Id, counts, cancellationToken);
        return selectedVersion;
    }

    private async Task EnsurePrimaryAntibodyMappingsAsync(
        WorkflowVersion ihcVersion,
        DateTimeOffset now,
        MutationCounts counts,
        CancellationToken cancellationToken)
    {
        foreach (var primaryCode in new[] { "001", "P01", "P02" })
        {
            var mapping = await dbContext.PrimaryAntibodyWorkflowMappings
                .SingleOrDefaultAsync(x => x.PrimaryAntibodyCode == primaryCode && x.WorkflowVersionId == ihcVersion.Id, cancellationToken);
            if (mapping is null)
            {
                mapping = new PrimaryAntibodyWorkflowMapping
                {
                    PrimaryAntibodyCode = primaryCode,
                    WorkflowVersionId = ihcVersion.Id,
                    IsEnabled = true,
                    CreatedAtUtc = now
                };
                dbContext.PrimaryAntibodyWorkflowMappings.Add(mapping);
                counts.Created++;
            }

            await EnsureTagAsync(nameof(PrimaryAntibodyWorkflowMapping), mapping.Id, counts, cancellationToken);
        }
    }

    private async Task EnsureMockLisEntriesAsync(DateTimeOffset now, MutationCounts counts, CancellationToken cancellationToken)
    {
        foreach (var entry in DemoLisEntries())
        {
            var existing = await dbContext.MockLisEntries.SingleOrDefaultAsync(
                x => x.NormalizedCode == entry.NormalizedCode
                    && x.PrimaryAntibodyCode == entry.PrimaryAntibodyCode
                    && x.Scenario == entry.Scenario,
                cancellationToken);
            if (existing is null)
            {
                existing = new MockLisEntry
                {
                    NormalizedCode = entry.NormalizedCode,
                    PrimaryAntibodyCode = entry.PrimaryAntibodyCode,
                    Scenario = entry.Scenario,
                    IsEnabled = true,
                    MetadataJson = JsonSerializer.Serialize(new { demo = DemoKey }, JsonOptions),
                    CreatedAtUtc = now
                };
                dbContext.MockLisEntries.Add(existing);
                counts.Created++;
                await EnsureTagAsync(nameof(MockLisEntry), existing.Id, counts, cancellationToken);
            }
        }
    }

    private async Task EnsureDemoRackAsync(DateTimeOffset now, MutationCounts counts, CancellationToken cancellationToken)
    {
        if (await dbContext.ReagentScanSessions.AnyAsync(x => x.SessionCode == "MOCK-DEMO-RACK", cancellationToken))
        {
            counts.Skipped++;
            return;
        }

        var positions = await dbContext.ReagentRackPositions.OrderBy(x => x.PositionNo).ToListAsync(cancellationToken);
        var definitions = await dbContext.ReagentDefinitions.ToDictionaryAsync(x => x.ReagentCode, cancellationToken);
        foreach (var localDefinition in dbContext.ReagentDefinitions.Local)
        {
            definitions[localDefinition.ReagentCode] = localDefinition;
        }

        var session = new ReagentScanSession
        {
            SessionCode = "MOCK-DEMO-RACK",
            Status = "Completed",
            StartedAtUtc = now,
            CompletedAtUtc = now
        };
        dbContext.ReagentScanSessions.Add(session);
        await EnsureTagAsync(nameof(ReagentScanSession), session.Id, counts, cancellationToken);

        // Rack cycle must cover every reagent referenced by the published HE/IHC
        // system templates' WorkflowReagentRequirements so /api/run/preflight can
        // satisfy required_reagent checks for both experiment types:
        //   HE  requirements: HEM, WAS, ACD, EOS, ETH
        //   IHC requirements: BLK, WAS, P01, SEC, DAB, HEM
        // plus the always-needed fluidics sources (PBS, WAT) and DAB components
        // (DBA, DBB) that the DAB lifecycle mixes at run time. P01 is placed at
        // index 0 because the 40-position rack divided by 13 codes only yields 4
        // positions for the index-0 code (3 for every other); end-to-end
        // acceptance caps each P01 bottle to 80 µL and then requires ≥4 bottles
        // to fulfill the 3×100 µL cross-bottle reservation for the IHC tasks.
        var codes = new[] { "P01", "HEM", "WAS", "ACD", "EOS", "ETH", "BLK", "SEC", "DAB", "PBS", "WAT", "DBA", "DBB" };
        foreach (var position in positions)
        {
            var occupied = await dbContext.ReagentRackPlacements.AnyAsync(x => x.RemovedAtUtc == null && x.ReagentRackPositionId == position.Id, cancellationToken);
            if (occupied)
            {
                counts.Skipped++;
                continue;
            }

            var code = codes[(position.PositionNo - 1) % codes.Length];
            var batchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1).AddDays(position.PositionNo));
            var barcode = $"{code}080{batchDate:yyyyMMdd}{position.PositionNo % 1000:000}";
            var definition = definitions[code];
            var bottle = new ReagentBottle
            {
                ReagentDefinition = definition,
                FullBarcode = barcode,
                ReagentCode = code,
                ProductionBatchNo = batchDate.ToString("yyyyMMdd"),
                SerialNo = (position.PositionNo % 1000).ToString("000"),
                InitialVolumeUl = 8000,
                RemainingVolumeUl = code == "P01" ? 2500 : 8000,
                ExpirationDate = batchDate,
                Status = "Available",
                FirstScannedAtUtc = now,
                LastScannedAtUtc = now,
                CreatedAtUtc = now
            };
            dbContext.ReagentBottles.Add(bottle);
            await EnsureTagAsync(nameof(ReagentBottle), bottle.Id, counts, cancellationToken);

            session.Items.Add(new ReagentScanItem
            {
                ReagentRackPositionId = position.Id,
                ScannerChannelNo = position.ScannerChannelNo,
                ScannerChannelCode = position.ScannerChannelCode,
                LocatorCode = position.Code,
                ScanResult = ReagentScanResult.Valid,
                RawBarcode = barcode,
                ParsedReagentCode = code,
                ParsedQuantityUl = 8000,
                ParsedBatchNo = bottle.ProductionBatchNo,
                ParsedSerialNo = bottle.SerialNo,
                IsValidationPassed = true,
                ValidationMessage = "OK",
                CreatedAtUtc = now
            });

            var placement = new ReagentRackPlacement
            {
                ReagentBottle = bottle,
                ReagentRackPositionId = position.Id,
                ReagentScanSession = session,
                PlacedAtUtc = now,
                CreatedAtUtc = now
            };
            dbContext.ReagentRackPlacements.Add(placement);
            await EnsureTagAsync(nameof(ReagentRackPlacement), placement.Id, counts, cancellationToken);
            counts.Created++;
        }
    }

    private async Task EnsureDemoReagentScenariosAsync(
        DateTimeOffset now,
        MutationCounts counts,
        CancellationToken cancellationToken)
    {
        var definitions = await dbContext.ReagentDefinitions.ToDictionaryAsync(x => x.ReagentCode, cancellationToken);
        foreach (var localDefinition in dbContext.ReagentDefinitions.Local)
        {
            definitions[localDefinition.ReagentCode] = localDefinition;
        }

        if (!await dbContext.ReagentScanSessions.AnyAsync(x => x.SessionCode == "MOCK-DEMO-SCENARIOS", cancellationToken))
        {
            var positions = await dbContext.ReagentRackPositions.OrderBy(x => x.PositionNo).Take(4).ToListAsync(cancellationToken);
            if (positions.Count == 4)
            {
                var scenarioTime = now.AddMinutes(-1);
                var session = new ReagentScanSession
                {
                    SessionCode = "MOCK-DEMO-SCENARIOS",
                    Status = "Completed",
                    StartedAtUtc = scenarioTime,
                    CompletedAtUtc = scenarioTime
                };
                dbContext.ReagentScanSessions.Add(session);
                await EnsureTagAsync(nameof(ReagentScanSession), session.Id, counts, cancellationToken);

                session.Items.Add(DemoScanItem(positions[0], ReagentScanResult.Empty, null, null, false, "Empty position.", scenarioTime));
                session.Items.Add(DemoScanItem(positions[1], ReagentScanResult.Invalid, "BROKEN", null, false, "Barcode length must be exactly 17 characters.", scenarioTime));
                session.Items.Add(DemoScanItem(positions[2], ReagentScanResult.Invalid, "ZZZ05020991231001", "ZZZ", false, "Unknown reagent code: ZZZ.", scenarioTime));
                session.Items.Add(DemoScanItem(positions[3], ReagentScanResult.Invalid, "HEM08020200101090", "HEM", false, "Reagent is expired: 2020-01-01.", scenarioTime));
                counts.Created++;
            }
        }

        var specialBottles = new[]
        {
            new DemoBottle("HEM08020200101091", "HEM", "20200101", "091", 8000, 0, new DateOnly(2020, 1, 1), "Expired"),
            new DemoBottle("P0100120990101092", "P01", "20990101", "092", 100, 50, new DateOnly(2099, 1, 1), "Available")
        };
        foreach (var demo in specialBottles)
        {
            if (await dbContext.ReagentBottles.AnyAsync(x => x.FullBarcode == demo.Barcode, cancellationToken))
            {
                counts.Skipped++;
                continue;
            }

            var bottle = new ReagentBottle
            {
                ReagentDefinition = definitions[demo.ReagentCode],
                FullBarcode = demo.Barcode,
                ReagentCode = demo.ReagentCode,
                ProductionBatchNo = demo.BatchNo,
                SerialNo = demo.SerialNo,
                InitialVolumeUl = demo.InitialVolumeUl,
                RemainingVolumeUl = demo.RemainingVolumeUl,
                ExpirationDate = demo.ExpirationDate,
                Status = demo.Status,
                FirstScannedAtUtc = now.AddMinutes(-1),
                LastScannedAtUtc = now.AddMinutes(-1),
                CreatedAtUtc = now.AddMinutes(-1)
            };
            dbContext.ReagentBottles.Add(bottle);
            await EnsureTagAsync(nameof(ReagentBottle), bottle.Id, counts, cancellationToken);
            counts.Created++;
        }
    }

    private static ReagentScanItem DemoScanItem(
        ReagentRackPosition position,
        string scanResult,
        string? rawBarcode,
        string? parsedReagentCode,
        bool validationPassed,
        string validationMessage,
        DateTimeOffset scannedAtUtc)
    {
        return new ReagentScanItem
        {
            ReagentRackPositionId = position.Id,
            ScannerChannelNo = position.ScannerChannelNo,
            ScannerChannelCode = position.ScannerChannelCode,
            LocatorCode = position.Code,
            ScanResult = scanResult,
            RawBarcode = rawBarcode,
            ParsedReagentCode = parsedReagentCode,
            ParsedQuantityUl = parsedReagentCode is null || rawBarcode is null
                ? null
                : int.Parse(rawBarcode[3..6]) * 100,
            ParsedBatchNo = parsedReagentCode is null ? null : rawBarcode?[6..14],
            ParsedSerialNo = parsedReagentCode is null ? null : rawBarcode?[14..17],
            IsValidationPassed = validationPassed,
            ValidationMessage = validationMessage,
            CreatedAtUtc = scannedAtUtc
        };
    }

    private async Task<MutationCounts> ResetTaggedDataAsync(CancellationToken cancellationToken)
    {
        var counts = new MutationCounts();
        var tags = await dbContext.MockDemoDataTags
            .Where(x => x.DemoKey == DemoKey)
            .ToListAsync(cancellationToken);
        var byType = tags.GroupBy(x => x.EntityType).ToDictionary(x => x.Key, x => x.Select(t => t.EntityId).ToHashSet(StringComparer.Ordinal));

        if (byType.TryGetValue(nameof(ReagentRackPlacement), out var placementIds))
        {
            var placements = await dbContext.ReagentRackPlacements.Where(x => placementIds.Contains(x.Id)).ToListAsync(cancellationToken);
            dbContext.ReagentRackPlacements.RemoveRange(placements);
            counts.Deleted += placements.Count;
        }

        if (byType.TryGetValue(nameof(ReagentScanSession), out var sessionIds))
        {
            var sessions = await dbContext.ReagentScanSessions.Where(x => sessionIds.Contains(x.Id)).ToListAsync(cancellationToken);
            dbContext.ReagentScanSessions.RemoveRange(sessions);
            counts.Deleted += sessions.Count;
        }

        if (byType.TryGetValue(nameof(ReagentBottle), out var bottleIds))
        {
            var consumedIds = await dbContext.ReagentConsumptions
                .Where(x => bottleIds.Contains(x.ReagentBottleId))
                .Select(x => x.ReagentBottleId)
                .ToListAsync(cancellationToken);
            var consumedSet = consumedIds.ToHashSet(StringComparer.Ordinal);
            var bottles = await dbContext.ReagentBottles.Where(x => bottleIds.Contains(x.Id)).ToListAsync(cancellationToken);
            var deletable = bottles.Where(x => !consumedSet.Contains(x.Id)).ToList();
            dbContext.ReagentBottles.RemoveRange(deletable);
            counts.Deleted += deletable.Count;
            counts.Skipped += bottles.Count - deletable.Count;
        }

        if (byType.TryGetValue(nameof(PrimaryAntibodyWorkflowMapping), out var mappingIds))
        {
            var mappings = await dbContext.PrimaryAntibodyWorkflowMappings.Where(x => mappingIds.Contains(x.Id)).ToListAsync(cancellationToken);
            dbContext.PrimaryAntibodyWorkflowMappings.RemoveRange(mappings);
            counts.Deleted += mappings.Count;
        }

        if (byType.TryGetValue(nameof(MockLisEntry), out var lisIds))
        {
            var entries = await dbContext.MockLisEntries.Where(x => lisIds.Contains(x.Id)).ToListAsync(cancellationToken);
            dbContext.MockLisEntries.RemoveRange(entries);
            counts.Deleted += entries.Count;
        }

        if (byType.TryGetValue(nameof(ReagentDefinition), out var definitionIds))
        {
            byType.TryGetValue(nameof(ReagentBottle), out var taggedBottleIds);
            taggedBottleIds ??= [];
            var usedDefinitionIds = await dbContext.ReagentBottles
                .Where(x => !taggedBottleIds.Contains(x.Id))
                .Select(x => x.ReagentDefinitionId)
                .ToListAsync(cancellationToken);
            var usedSet = usedDefinitionIds.ToHashSet(StringComparer.Ordinal);
            var definitions = await dbContext.ReagentDefinitions.Where(x => definitionIds.Contains(x.Id)).ToListAsync(cancellationToken);
            var deletable = definitions.Where(x => !usedSet.Contains(x.Id)).ToList();
            dbContext.ReagentDefinitions.RemoveRange(deletable);
            counts.Deleted += deletable.Count;
            counts.Skipped += definitions.Count - deletable.Count;
        }

        // Mapping deletions must be persisted while their target versions are still published.
        // The DbContext validates changed mappings before allowing a version to be retired.
        await dbContext.SaveChangesAsync(cancellationToken);

        if (byType.TryGetValue(nameof(WorkflowVersion), out var versionIds))
        {
            var versions = await dbContext.WorkflowVersions.Where(x => versionIds.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var version in versions.Where(x => x.Status == WorkflowVersionStatus.Published))
            {
                version.Status = WorkflowVersionStatus.Retired;
                version.RetiredAtUtc = DateTimeOffset.UtcNow;
                version.UpdatedAtUtc = DateTimeOffset.UtcNow;
                counts.Updated++;
            }
        }

        dbContext.MockDemoDataTags.RemoveRange(tags);
        counts.Deleted += tags.Count;
        return counts;
    }

    private async Task EnsureTagAsync(string entityType, string entityId, MutationCounts counts, CancellationToken cancellationToken)
    {
        if (dbContext.MockDemoDataTags.Local.Any(x => x.EntityType == entityType && x.EntityId == entityId)
            || await dbContext.MockDemoDataTags.AnyAsync(x => x.EntityType == entityType && x.EntityId == entityId, cancellationToken))
        {
            return;
        }

        dbContext.MockDemoDataTags.Add(new MockDemoDataTag
        {
            EntityType = entityType,
            EntityId = entityId,
            DemoKey = DemoKey,
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
        counts.Created++;
    }

    private void AddAudit(AuthenticatedUser actor, string action, string entityType, string entityId, object details)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            ActorUserId = string.IsNullOrWhiteSpace(actor.UserId) ? null : actor.UserId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Message = JsonSerializer.Serialize(details, JsonOptions),
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
    }

    private static void AddWorkflowChildren(
        WorkflowVersion version,
        IReadOnlyList<DemoWorkflowStep> steps,
        IReadOnlyList<(string ReagentCode, int RequiredVolumeUl)> requirements,
        DateTimeOffset now)
    {
        foreach (var step in steps)
        {
            version.Steps.Add(new WorkflowStep
            {
                StepNo = step.StepNo,
                MajorStepCode = step.MajorStepCode,
                StepName = step.StepName,
                ActionType = step.ActionType,
                ReagentCode = step.ReagentCode,
                VolumeUl = step.VolumeUl,
                DurationSeconds = step.DurationSeconds,
                FailureStrategy = "Stop",
                CreatedAtUtc = now
            });
        }

        foreach (var requirement in requirements)
        {
            version.ReagentRequirements.Add(new WorkflowReagentRequirement
            {
                ReagentCode = requirement.ReagentCode,
                RequiredVolumeUl = requirement.RequiredVolumeUl,
                IsRequired = true,
                CreatedAtUtc = now
            });
        }
    }

    private static IReadOnlyList<DemoReagent> DemoReagents()
    {
        return
        [
            new("HEM", "Hematoxylin", "common", 1000),
            new("WAS", "Wash buffer", "wash", 1000),
            new("ACD", "Acid wash", "wash", 1000),
            new("EOS", "Eosin", "common", 1000),
            new("ETH", "Ethanol", "wash", 1000),
            new("PBS", "PBS", "wash", 1000),
            new("WAT", "Pure water", "wash", 1000),
            new("BLK", "Blocking reagent", "ihc", 1000),
            new("P01", "Primary antibody P01", "primary", 1000),
            new("P02", "Primary antibody P02", "primary", 1000),
            new("SEC", "Secondary antibody", "secondary", 1000),
            new("DAB", "DAB working solution", "dab", 1000),
            new("DBA", "DAB A", "dab", 1000),
            new("DBB", "DAB B", "dab", 1000)
        ];
    }

    private static IReadOnlyList<DemoLisEntry> DemoLisEntries()
    {
        return
        [
            new("HOSP-MOCK-SINGLE", "P01", MockLisScenario.Candidate),
            new("HOSP-MOCK-NONE", null, MockLisScenario.NoResult),
            new("HOSP-MOCK-MULTI", "P01", MockLisScenario.Candidate),
            new("HOSP-MOCK-MULTI", "P02", MockLisScenario.Candidate),
            new("HOSP-MOCK-TIMEOUT", null, MockLisScenario.Timeout),
            new("HOSP-MOCK-ERROR", null, MockLisScenario.Exception)
        ];
    }

    private sealed class MutationCounts
    {
        public int Created { get; set; }
        public int Updated { get; set; }
        public int Deleted { get; set; }
        public int Skipped { get; set; }
    }

    private sealed record DemoReagent(string Code, string Name, string Type, int MinimumAlarmVolumeUl);

    private sealed record DemoLisEntry(string NormalizedCode, string? PrimaryAntibodyCode, string Scenario);

    private sealed record DemoBottle(
        string Barcode,
        string ReagentCode,
        string BatchNo,
        string SerialNo,
        int InitialVolumeUl,
        int RemainingVolumeUl,
        DateOnly ExpirationDate,
        string Status);

    private sealed record DemoWorkflowStep(
        int StepNo,
        string MajorStepCode,
        string StepName,
        string ActionType,
        string? ReagentCode,
        int? VolumeUl,
        int DurationSeconds);
}
