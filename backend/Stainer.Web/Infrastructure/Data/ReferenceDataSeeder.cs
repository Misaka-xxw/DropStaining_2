using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.Services;
using Stainer.Web.Domain.Entities;

namespace Stainer.Web.Infrastructure.Data;

public sealed class ReferenceDataSeeder(StainerDbContext dbContext)
{
    public const string DefaultCoordinateProfileCode = "FactoryDefault-v1";
    public const string ManualPrimaryAntibodyCode = "001";
    public const string DefaultHeWorkflowCode = "SYSTEM-HE-FAST-V1";
    public const string DefaultIhcWorkflowCode = "SYSTEM-IHC-STANDARD-40C-V1";
    private const string DefaultDeviceProfileCode = "FactoryDevice-v1";
    private const string DefaultLiquidClassCode = "FactoryGeneral-v1";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;

        await RemoveEngineerRoleAndUserAsync(cancellationToken);
        await SeedRolesAsync(now, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await SeedDefaultUsersAsync(now, cancellationToken);
        await SeedDeviceProfileAsync(now, cancellationToken);
        await SeedPhysicalLayoutAsync(now, cancellationToken);
        var coordinateProfile = await SeedCoordinateProfileAsync(now, cancellationToken);
        await SeedCoordinatePointsAsync(coordinateProfile, now, cancellationToken);
        await SeedDefaultWorkflowTemplatesAsync(now, cancellationToken);
        await SeedAbLiquidClassAsync(now, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    // 标准液体类型 Ab（一抗）作为真实种子数据，与 FactoryGeneral-v1 同机制。
    // 幂等：按 code 查，已存在则跳过（绝不覆盖已有数据）。参数先用 FactoryGeneral-v1 基线占位，
    // 待设备侧按液体粘度逐个调校——可在「配置 → 液体类型」UI 里直接改并保存。
    private async Task SeedAbLiquidClassAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        const string AbLiquidClassCode = "Ab";
        if (await dbContext.LiquidClassProfiles.AsNoTracking().AnyAsync(x => x.Code == AbLiquidClassCode, cancellationToken))
        {
            return;
        }

        var liquidClass = new LiquidClassProfile
        {
            Code = AbLiquidClassCode,
            Name = "Antibody (一抗) liquid class",
            AspirateSpeedUlPerSecond = 100,
            DispenseSpeedUlPerSecond = 100,
            LeadingAirGapUl = 5,
            TrailingAirGapUl = 5,
            ExcessVolumeUl = 0,
            PreWetCycles = 1,
            MixCycles = 0,
            IsEnabled = true,
            CreatedAtUtc = now
        };
        dbContext.LiquidClassProfiles.Add(liquidClass);
        await dbContext.SaveChangesAsync(cancellationToken);

        var version = new LiquidClassVersion
        {
            LiquidClassProfile = liquidClass,
            LiquidClassProfileId = liquidClass.Id,
            VersionNo = 1,
            VersionLabel = "1",
            Name = liquidClass.Name,
            Status = LiquidClassVersionStatus.Enabled,
            ChangeReason = "Factory seeded standard Liquid Class (Ab) baseline.",
            ChangeSummaryJson = "{\"seeded\":true}",
            LiquidDetectionEnabled = true,
            LiquidDetectionSensitivityPercent = 50,
            LiquidDetectionSpeedUmPerSecond = 1_000,
            AspirateSpeedUlPerSecond = 100,
            AspirateDelayMs = 100,
            DispenseSpeedUlPerSecond = 100,
            DispenseDelayMs = 100,
            LeadingAirGapUl = 5,
            TrailingAirGapUl = 5,
            BlowoutVolumeUl = 10,
            BlowoutDelayMs = 100,
            VolumeAdjustmentUl = 0,
            PreWetCycles = 1,
            MixCycles = 0,
            LiquidFollowingDepthUm = 2000,
            RetractSpeedUmPerSecond = 1000,
            ConditioningVolumeUl = 0,
            BreakoffSpeedUlPerSecond = 0,
            PostDispenseAirGapUl = 0,
            CreatedAtUtc = now,
            PublishedAtUtc = now,
            EnabledAtUtc = now
        };
        version.ValidationRecords.Add(new LiquidClassValidationRecord
        {
            LiquidClassVersion = version,
            LiquidClassVersionId = version.Id,
            Stage = LiquidClassValidationStage.Enable,
            IsValid = true,
            ResultJson = "{\"valid\":true,\"source\":\"seed\"}",
            CreatedAtUtc = now
        });
        dbContext.LiquidClassVersions.Add(version);
        await dbContext.SaveChangesAsync(cancellationToken);
        liquidClass.EnabledVersionId = version.Id;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ManualAcceptanceSeedSummary> GetManualAcceptanceSeedSummaryAsync(CancellationToken cancellationToken = default)
    {
        var he = await LoadSeedWorkflowAsync(DefaultHeWorkflowCode, cancellationToken);
        var ihc = await LoadSeedWorkflowAsync(DefaultIhcWorkflowCode, cancellationToken);
        var mapping = await dbContext.PrimaryAntibodyWorkflowMappings
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.PrimaryAntibodyCode == ManualPrimaryAntibodyCode
                    && ihc.WorkflowVersionId != null
                    && x.WorkflowVersionId == ihc.WorkflowVersionId,
                cancellationToken);

        var reagentCodes = await dbContext.WorkflowReagentRequirements
            .AsNoTracking()
            .Where(x => x.WorkflowVersionId == he.WorkflowVersionId || x.WorkflowVersionId == ihc.WorkflowVersionId)
            .Select(x => x.ReagentCode)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync(cancellationToken);

        return new ManualAcceptanceSeedSummary(
            he.Name,
            he.Code,
            he.WorkflowVersionId,
            ihc.Name,
            ihc.Code,
            ihc.WorkflowVersionId,
            ManualPrimaryAntibodyCode,
            mapping?.IsEnabled == true,
            mapping?.WorkflowVersionId,
            reagentCodes,
            he.DefaultExperimentType == StainingTaskType.He,
            ihc.DefaultExperimentType == StainingTaskType.Ihc);
    }

    private async Task SeedRolesAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        var roles = new[]
        {
            ("operator", "Operator"),
            ("admin", "Administrator")
        };

        foreach (var (code, name) in roles)
        {
            if (!await dbContext.Roles.AnyAsync(x => x.Code == code, cancellationToken))
            {
                dbContext.Roles.Add(new Role { Code = code, Name = name, CreatedAtUtc = now });
            }
        }
    }

    private async Task RemoveEngineerRoleAndUserAsync(CancellationToken cancellationToken)
    {
        // 工程师角色已下线：从数据库移除遗留的 engineer 角色与同名演示账号（及其 user_roles）。
        // 幂等——不存在时为空操作；每次启动执行，确保旧库里残留的 engineer 也被清掉。
        var engineerRole = await dbContext.Roles.FirstOrDefaultAsync(x => x.Code == "engineer", cancellationToken);
        if (engineerRole is not null)
        {
            dbContext.UserRoles.RemoveRange(dbContext.UserRoles.Where(x => x.RoleId == engineerRole.Id).ToList());
            dbContext.Roles.Remove(engineerRole);
        }

        var engineerUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == "engineer", cancellationToken);
        if (engineerUser is not null)
        {
            dbContext.UserRoles.RemoveRange(dbContext.UserRoles.Where(x => x.UserId == engineerUser.Id).ToList());
            dbContext.Users.Remove(engineerUser);
        }

        if (engineerRole is not null || engineerUser is not null)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task SeedDefaultUsersAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        var passwordHashService = new PasswordHashService();
        var roles = await dbContext.Roles.ToDictionaryAsync(x => x.Code, cancellationToken);
        var users = new[]
        {
            ("operator", "Operator", new[] { "operator" }),
            ("admin", "Administrator", new[] { "admin" })
        };

        foreach (var (username, displayName, roleCodes) in users)
        {
            var user = await dbContext.Users
                .Include(x => x.UserRoles)
                .SingleOrDefaultAsync(x => x.Username == username, cancellationToken);
            if (user is null)
            {
                user = new User
                {
                    Username = username,
                    DisplayName = displayName,
                    PasswordHash = passwordHashService.Hash("123456"),
                    PasswordHashAlgorithm = "PBKDF2-SHA256",
                    PasswordUpdatedAtUtc = now,
                    CreatedAtUtc = now
                };
                dbContext.Users.Add(user);
            }
            else if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                user.PasswordHash = passwordHashService.Hash("123456");
                user.PasswordHashAlgorithm = "PBKDF2-SHA256";
                user.PasswordUpdatedAtUtc = now;
            }

            foreach (var roleCode in roleCodes)
            {
                var role = roles[roleCode];
                if (!user.UserRoles.Any(x => x.RoleId == role.Id))
                {
                    user.UserRoles.Add(new UserRole
                    {
                        RoleId = role.Id,
                        CreatedAtUtc = now
                    });
                }
            }
        }
    }

    private async Task SeedDeviceProfileAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        if (!await dbContext.DeviceProfiles.AnyAsync(x => x.Code == DefaultDeviceProfileCode, cancellationToken))
        {
            dbContext.DeviceProfiles.Add(new DeviceProfile
            {
                Code = DefaultDeviceProfileCode,
                Name = "Factory device profile v1",
                IsActive = true,
                CreatedAtUtc = now
            });
        }
    }

    private async Task SeedPhysicalLayoutAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        var drawerCodes = new[] { "A", "B", "C", "D" };
        for (var i = 0; i < drawerCodes.Length; i++)
        {
            var code = drawerCodes[i];
            if (!await dbContext.Drawers.AnyAsync(x => x.Code == code, cancellationToken))
            {
                dbContext.Drawers.Add(new Drawer
                {
                    Code = code,
                    Name = $"Drawer {code}",
                    SortOrder = i + 1,
                    HeatBoardId = i,
                    CreatedAtUtc = now
                });
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var drawers = await dbContext.Drawers.ToDictionaryAsync(x => x.Code, cancellationToken);
        foreach (var drawerCode in drawerCodes)
        {
            var drawer = drawers[drawerCode];
            for (var slotNo = 1; slotNo <= 4; slotNo++)
            {
                var code = $"{drawerCode}-{slotNo:00}";
                if (!await dbContext.PhysicalSlots.AnyAsync(x => x.Code == code, cancellationToken))
                {
                    dbContext.PhysicalSlots.Add(new PhysicalSlot
                    {
                        DrawerId = drawer.Id,
                        Code = code,
                        SlotNo = slotNo,
                        VerticalOrderFromBottom = slotNo,
                        HeatPointId = slotNo - 1,
                        CreatedAtUtc = now
                    });
                }
            }
        }

        for (var column = 1; column <= 5; column++)
        {
            for (var row = 1; row <= 8; row++)
            {
                var positionNo = ((column - 1) * 8) + row;
                var code = $"R{positionNo}";
                if (!await dbContext.ReagentRackPositions.AnyAsync(x => x.Code == code, cancellationToken))
                {
                    dbContext.ReagentRackPositions.Add(new ReagentRackPosition
                    {
                        Code = code,
                        PositionNo = positionNo,
                        ColumnNo = column,
                        RowNo = row,
                        ScannerChannelNo = column,
                        ScannerChannelCode = $"ch{column}",
                        CreatedAtUtc = now
                    });
                }
            }
        }

        for (var positionNo = 1; positionNo <= 8; positionNo++)
        {
            var code = $"M{positionNo}";
            if (!await dbContext.DabMixPositions.AnyAsync(x => x.Code == code, cancellationToken))
            {
                dbContext.DabMixPositions.Add(new DabMixPosition
                {
                    Code = code,
                    PositionNo = positionNo,
                    CreatedAtUtc = now
                });
            }
        }

        var washPositions = new[]
        {
            ("WashInnerLeft", "Inner"),
            ("WashInnerRight", "Inner"),
            ("WashOuterLeft", "Outer"),
            ("WashOuterRight", "Outer")
        };

        foreach (var (code, washType) in washPositions)
        {
            if (!await dbContext.WashPositions.AnyAsync(x => x.Code == code, cancellationToken))
            {
                dbContext.WashPositions.Add(new WashPosition
                {
                    Code = code,
                    WashType = washType,
                    CreatedAtUtc = now
                });
            }
        }
    }

    private async Task<CoordinateProfile> SeedCoordinateProfileAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        var profile = await dbContext.CoordinateProfiles
            .Include(x => x.Versions)
            .FirstOrDefaultAsync(x => x.Code == DefaultCoordinateProfileCode, cancellationToken);
        if (profile is null)
        {
            profile = new CoordinateProfile
            {
                Code = DefaultCoordinateProfileCode,
                Name = "Factory default coordinate profile v1",
                Status = CoordinateProfileStatus.Enabled,
                IsActive = true,
                OriginDefinition = "Needle1 is the innermost needle and defines mechanical origin (0, 0). Needle2 offset is (0, +25000 um).",
                CreatedAtUtc = now
            };
            dbContext.CoordinateProfiles.Add(profile);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var activeVersion = profile.Versions.SingleOrDefault(x => x.IsActive);
        if (activeVersion is null)
        {
            activeVersion = new CoordinateProfileVersion
            {
                CoordinateProfileId = profile.Id,
                VersionNo = profile.Versions.Count == 0 ? 1 : profile.Versions.Max(x => x.VersionNo) + 1,
                VersionLabel = "1",
                Status = CoordinateProfileVersionStatus.Draft,
                IsActive = false,
                UsageScope = CoordinateVersionUsageScope.MockOnly,
                VerificationStatus = CoordinateVersionVerificationStatus.Unverified,
                ChangeReason = "Factory default coordinate baseline.",
                ChangeSummaryJson = "{\"seeded\":true}",
                ValidationResultJson = "{\"status\":\"FactoryDefault\"}",
                CreatedAtUtc = now
            };
            dbContext.CoordinateProfileVersions.Add(activeVersion);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        profile.ActiveVersionId = activeVersion.Id;
        await dbContext.SaveChangesAsync(cancellationToken);
        return profile;
    }

    private async Task SeedCoordinatePointsAsync(CoordinateProfile profile, DateTimeOffset now, CancellationToken cancellationToken)
    {
        var versionId = profile.ActiveVersionId
            ?? (await dbContext.CoordinateProfileVersions.SingleAsync(x => x.CoordinateProfileId == profile.Id && x.IsActive, cancellationToken)).Id;

        var version = await dbContext.CoordinateProfileVersions.SingleAsync(x => x.Id == versionId, cancellationToken);

        // A coordinate version's point set is immutable once it leaves Draft or is
        // referenced by a channel batch / machine run (enforced in
        // StainerDbContext.ValidateCoordinateVersionChanges). Default points are only
        // populated into a version that is still mutable; databases seeded before a
        // default point was added or renamed keep their historical, now-locked point
        // set instead of failing startup by mutating a protected version in place.
        var canPopulatePoints = version.Status == CoordinateProfileVersionStatus.Draft
            && !await dbContext.MachineRuns.AnyAsync(x => x.CoordinateProfileVersionId == versionId, cancellationToken)
            && !await dbContext.ChannelBatches.AnyAsync(x => x.CoordinateProfileVersionId == versionId, cancellationToken);

        if (canPopulatePoints)
        {
            await PopulateDefaultCoordinatePointsAsync(profile.Id, versionId, now, cancellationToken);
        }

        if (version.Status == CoordinateProfileVersionStatus.Draft)
        {
            version.Status = CoordinateProfileVersionStatus.Active;
            version.IsActive = true;
            version.UsageScope = CoordinateVersionUsageScope.MockOnly;
            version.VerificationStatus = CoordinateVersionVerificationStatus.Unverified;
            version.PublishedAtUtc = now;
            version.ActivatedAtUtc = now;
        }

        profile.Status = CoordinateProfileStatus.Enabled;
        profile.IsActive = true;
        profile.ActiveVersionId = versionId;
    }

    private async Task PopulateDefaultCoordinatePointsAsync(
        string coordinateProfileId,
        string coordinateProfileVersionId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        await EnsureCoordinatePointAsync(coordinateProfileId, coordinateProfileVersionId, "Needle1", "Needle", 0, 0, false, now, cancellationToken);
        await EnsureCoordinatePointAsync(coordinateProfileId, coordinateProfileVersionId, "Needle2", "Needle", 0, 25000, false, now, cancellationToken);

        var slotCodes = await dbContext.PhysicalSlots.Select(x => x.Code).ToListAsync(cancellationToken);
        foreach (var code in slotCodes)
        {
            await EnsureCoordinatePointAsync(coordinateProfileId, coordinateProfileVersionId, code, "PhysicalSlot", null, null, true, now, cancellationToken);
        }

        var reagentCodes = await dbContext.ReagentRackPositions.Select(x => x.Code).ToListAsync(cancellationToken);
        foreach (var code in reagentCodes)
        {
            await EnsureCoordinatePointAsync(coordinateProfileId, coordinateProfileVersionId, code, "ReagentRackPosition", null, null, true, now, cancellationToken);
        }

        var dabCodes = await dbContext.DabMixPositions.Select(x => x.Code).ToListAsync(cancellationToken);
        foreach (var code in dabCodes)
        {
            await EnsureCoordinatePointAsync(coordinateProfileId, coordinateProfileVersionId, code, "DabMixPosition", null, null, true, now, cancellationToken);
        }

        var washCodes = await dbContext.WashPositions.Select(x => x.Code).ToListAsync(cancellationToken);
        foreach (var code in washCodes)
        {
            await EnsureCoordinatePointAsync(coordinateProfileId, coordinateProfileVersionId, code, "WashPosition", null, null, true, now, cancellationToken);
        }

        // Sample scanner calibration/test reference only. The arm-mounted sample
        // scanner (DCR55) is a tool reference, not a fixed per-version movement
        // target (the arm carries it to a slot/ScannerRegion, stops, then reads).
        // Reagent scanning uses the fixed multi-channel scanner module, not arm
        // movement. Kept as a non-required calibration point so legacy/seeded
        // baselines still expose a named sample-scanner anchor without being
        // enforced as an executable target by CoordinateProfileLifecycleService.
        await EnsureCoordinatePointAsync(coordinateProfileId, coordinateProfileVersionId, "SampleScannerCalibrationPoint", "SampleScannerCalibration", null, null, true, now, cancellationToken);
        await EnsureCoordinatePointAsync(coordinateProfileId, coordinateProfileVersionId, "DabA", "DabSourceBottle", null, null, true, now, cancellationToken);
        await EnsureCoordinatePointAsync(coordinateProfileId, coordinateProfileVersionId, "DabB", "DabSourceBottle", null, null, true, now, cancellationToken);
    }

    private async Task EnsureCoordinatePointAsync(
        string coordinateProfileId,
        string coordinateProfileVersionId,
        string pointCode,
        string pointType,
        long? xUm,
        long? yUm,
        bool requiresCalibration,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        if (await dbContext.CoordinatePoints.AnyAsync(x => x.CoordinateProfileVersionId == coordinateProfileVersionId && x.PointCode == pointCode, cancellationToken))
        {
            return;
        }

        dbContext.CoordinatePoints.Add(new CoordinatePoint
        {
            CoordinateProfileId = coordinateProfileId,
            CoordinateProfileVersionId = coordinateProfileVersionId,
            PointCode = pointCode,
            PointType = pointType,
            PresetXUm = xUm,
            PresetYUm = yUm,
            CalibratedXUm = xUm,
            CalibratedYUm = yUm,
            RequiresCalibration = requiresCalibration,
            ValidationStatus = requiresCalibration ? CoordinateTargetPointValidationStatus.NeedsCalibration : CoordinateTargetPointValidationStatus.Validated,
            ValidationMessage = requiresCalibration ? "Factory default point requires calibration." : "Factory default coordinate point.",
            CreatedAtUtc = now
        });
    }

    private async Task SeedDefaultWorkflowTemplatesAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        await SeedManualAcceptanceReagentsAsync(now, cancellationToken);

        // These two definitions are the only built-in templates.  The UI reads them
        // from the workflow API and creates editable drafts by copying their versions.
        // Remember whether they existed before this run so an upgrade switches the old
        // sample defaults once, without overwriting an administrator's later choice.
        var systemTemplatesAlreadyExisted = await dbContext.WorkflowDefinitions
            .CountAsync(x => x.Code == DefaultHeWorkflowCode || x.Code == DefaultIhcWorkflowCode, cancellationToken) == 2;

        var defaultHeVersion = await EnsureSeedWorkflowAsync(
            DefaultHeWorkflowCode,
            "HE 快速染色模板",
            StainingTaskType.He,
            "系统 HE 默认流程模板；由配置/流程页面统一读取和版本化管理。",
            DefaultHeSteps(),
            [("HEM", 100), ("WAS", 100), ("ACD", 100), ("EOS", 100), ("ETH", 100)],
            now,
            cancellationToken,
            planningRulesJson: DefaultHePlanningRulesJson());

        var defaultIhcVersion = await EnsureSeedWorkflowAsync(
            DefaultIhcWorkflowCode,
            "IHC 标准流程 40℃",
            StainingTaskType.Ihc,
            "系统 IHC 默认流程模板；由配置/流程页面统一读取和版本化管理。",
            DefaultIhcSteps(),
            [("BLK", 100), ("WAS", 500), ("P01", 100), ("SEC", 100), ("DAB", 100), ("HEM", 100)],
            now,
            cancellationToken,
            planningRulesJson: DefaultIhcPlanningRulesJson());

        if (!systemTemplatesAlreadyExisted)
        {
            await ReplaceDefaultsWithSystemTemplatesAsync(defaultHeVersion, defaultIhcVersion, cancellationToken);
        }

        await EnsureDefaultWorkflowAsync(StainingTaskType.He, defaultHeVersion, cancellationToken);
        await EnsureDefaultWorkflowAsync(StainingTaskType.Ihc, defaultIhcVersion, cancellationToken);
        await EnsurePrimaryAntibodyMappingAsync(ManualPrimaryAntibodyCode, defaultIhcVersion.Id, now, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await PruneNonDefaultWorkflowDefinitionsAsync(cancellationToken);
    }

    private async Task ReplaceDefaultsWithSystemTemplatesAsync(
        WorkflowVersion defaultHeVersion,
        WorkflowVersion defaultIhcVersion,
        CancellationToken cancellationToken)
    {
        var existingDefaults = await dbContext.WorkflowVersions
            .Where(x => x.DefaultExperimentType == StainingTaskType.He || x.DefaultExperimentType == StainingTaskType.Ihc)
            .ToListAsync(cancellationToken);
        foreach (var version in existingDefaults)
        {
            version.DefaultExperimentType = null;
            version.UpdatedAtUtc = DateTimeOffset.UtcNow;
        }

        defaultHeVersion.DefaultExperimentType = StainingTaskType.He;
        defaultIhcVersion.DefaultExperimentType = StainingTaskType.Ihc;
        defaultHeVersion.UpdatedAtUtc = DateTimeOffset.UtcNow;
        defaultIhcVersion.UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    private async Task PruneNonDefaultWorkflowDefinitionsAsync(CancellationToken cancellationToken)
    {
        const string targetVersions = """
            SELECT v.id
            FROM workflow_versions v
            JOIN workflow_definitions d ON d.id = v.workflow_definition_id
            WHERE d.code <> 'SYSTEM-HE-FAST-V1'
              AND d.code <> 'SYSTEM-IHC-STANDARD-40C-V1'
            """;

        await dbContext.Database.ExecuteSqlRawAsync("""
            UPDATE staining_tasks
            SET workflow_definition_id = (
                    SELECT nd.id
                    FROM workflow_definitions nd
                    WHERE nd.code = CASE WHEN staining_tasks.task_type = 'HE' THEN 'SYSTEM-HE-FAST-V1' ELSE 'SYSTEM-IHC-STANDARD-40C-V1' END),
                workflow_version_id = (
                    SELECT nv.id
                    FROM workflow_versions nv
                    JOIN workflow_definitions nd ON nd.id = nv.workflow_definition_id
                    WHERE nd.code = CASE WHEN staining_tasks.task_type = 'HE' THEN 'SYSTEM-HE-FAST-V1' ELSE 'SYSTEM-IHC-STANDARD-40C-V1' END
                      AND nv.status = 'Published'
                    ORDER BY nv.default_experiment_type IS NOT NULL DESC, nv.version_no DESC
                    LIMIT 1)
            WHERE workflow_definition_id IN (
                    SELECT id FROM workflow_definitions WHERE code <> 'SYSTEM-HE-FAST-V1' AND code <> 'SYSTEM-IHC-STANDARD-40C-V1')
               OR workflow_version_id IN (
            """ + targetVersions + """
            )
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            UPDATE channel_batches
            SET selected_workflow_version_id = (
                    SELECT nv.id
                    FROM workflow_versions ov
                    JOIN workflow_definitions od ON od.id = ov.workflow_definition_id
                    JOIN workflow_definitions nd ON nd.code = CASE WHEN od.workflow_type = 'HE' THEN 'SYSTEM-HE-FAST-V1' ELSE 'SYSTEM-IHC-STANDARD-40C-V1' END
                    JOIN workflow_versions nv ON nv.workflow_definition_id = nd.id
                    WHERE ov.id = channel_batches.selected_workflow_version_id
                      AND nv.status = 'Published'
                    ORDER BY nv.default_experiment_type IS NOT NULL DESC, nv.version_no DESC
                    LIMIT 1)
            WHERE selected_workflow_version_id IN (
            """ + targetVersions + """
            )
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            UPDATE workflow_executions
            SET workflow_version_id = (
                    SELECT nv.id
                    FROM workflow_versions ov
                    JOIN workflow_definitions od ON od.id = ov.workflow_definition_id
                    JOIN workflow_definitions nd ON nd.code = CASE WHEN od.workflow_type = 'HE' THEN 'SYSTEM-HE-FAST-V1' ELSE 'SYSTEM-IHC-STANDARD-40C-V1' END
                    JOIN workflow_versions nv ON nv.workflow_definition_id = nd.id
                    WHERE ov.id = workflow_executions.workflow_version_id
                      AND nv.status = 'Published'
                    ORDER BY nv.default_experiment_type IS NOT NULL DESC, nv.version_no DESC
                    LIMIT 1)
            WHERE workflow_version_id IN (
            """ + targetVersions + """
            )
            """, cancellationToken);

        var deleteTagsSql = """
            DELETE FROM mock_demo_data_tags
            WHERE (entity_type = 'WorkflowVersion' AND entity_id IN (
            """ + targetVersions + """
            ))
               OR (entity_type = 'WorkflowDefinition' AND entity_id IN (
                    SELECT d.id
                    FROM workflow_definitions d
                    WHERE d.code <> 'SYSTEM-HE-FAST-V1'
                      AND d.code <> 'SYSTEM-IHC-STANDARD-40C-V1'
                      AND NOT EXISTS (
                          SELECT 1
                          FROM staining_tasks t
                          WHERE t.workflow_definition_id = d.id)))
            """;
        await dbContext.Database.ExecuteSqlRawAsync(deleteTagsSql, cancellationToken);

        var deleteMappingsSql = """
            DELETE FROM primary_antibody_workflow_mappings
            WHERE workflow_version_id IN (
            """ + targetVersions + """
            )
            """;
        await dbContext.Database.ExecuteSqlRawAsync(deleteMappingsSql, cancellationToken);

        var deleteStepsSql = """
            DELETE FROM workflow_steps
            WHERE workflow_version_id IN (
            """ + targetVersions + """
            )
            """;
        await dbContext.Database.ExecuteSqlRawAsync(deleteStepsSql, cancellationToken);

        var deleteRequirementsSql = """
            DELETE FROM workflow_reagent_requirements
            WHERE workflow_version_id IN (
            """ + targetVersions + """
            )
            """;
        await dbContext.Database.ExecuteSqlRawAsync(deleteRequirementsSql, cancellationToken);

        var deleteVersionsSql = """
            DELETE FROM workflow_versions
            WHERE id IN (
            """ + targetVersions + """
            )
            """;
        await dbContext.Database.ExecuteSqlRawAsync(deleteVersionsSql, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            DELETE FROM workflow_definitions
            WHERE code <> 'SYSTEM-HE-FAST-V1'
              AND code <> 'SYSTEM-IHC-STANDARD-40C-V1'
              AND NOT EXISTS (
                  SELECT 1
                  FROM workflow_versions v
                  WHERE v.workflow_definition_id = workflow_definitions.id)
              AND NOT EXISTS (
                  SELECT 1
                  FROM staining_tasks t
                  WHERE t.workflow_definition_id = workflow_definitions.id)
            """, cancellationToken);
    }

    private async Task EnsureDefaultWorkflowAsync(
        string experimentType,
        WorkflowVersion seedVersion,
        CancellationToken cancellationToken)
    {
        if (dbContext.WorkflowVersions.Local.Any(x => x.DefaultExperimentType == experimentType)
            || await dbContext.WorkflowVersions.AnyAsync(x => x.DefaultExperimentType == experimentType, cancellationToken))
        {
            return;
        }

        seedVersion.DefaultExperimentType = experimentType;
        seedVersion.UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    private async Task SeedManualAcceptanceReagentsAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        var liquidClass = await dbContext.LiquidClassProfiles
            .Include(x => x.EnabledVersion)
            .SingleOrDefaultAsync(x => x.Code == DefaultLiquidClassCode, cancellationToken);
        if (liquidClass is null)
        {
            liquidClass = new LiquidClassProfile
            {
                Code = DefaultLiquidClassCode,
                Name = "Factory general liquid class v1",
                AspirateSpeedUlPerSecond = 100,
                DispenseSpeedUlPerSecond = 100,
                LeadingAirGapUl = 5,
                TrailingAirGapUl = 5,
                ExcessVolumeUl = 0,
                PreWetCycles = 1,
                MixCycles = 0,
                IsEnabled = true,
                CreatedAtUtc = now
            };
            dbContext.LiquidClassProfiles.Add(liquidClass);
            await dbContext.SaveChangesAsync(cancellationToken);
            var version = new LiquidClassVersion
            {
                LiquidClassProfile = liquidClass,
                LiquidClassProfileId = liquidClass.Id,
                VersionNo = 1,
                VersionLabel = "1",
                Name = liquidClass.Name,
                Status = LiquidClassVersionStatus.Enabled,
                ChangeReason = "Factory seeded Liquid Class baseline.",
                ChangeSummaryJson = "{\"seeded\":true}",
                LiquidDetectionEnabled = true,
                LiquidDetectionSensitivityPercent = 50,
                LiquidDetectionSpeedUmPerSecond = 1_000,
                AspirateSpeedUlPerSecond = 100,
                AspirateDelayMs = 100,
                DispenseSpeedUlPerSecond = 100,
                DispenseDelayMs = 100,
                LeadingAirGapUl = 5,
                TrailingAirGapUl = 5,
                BlowoutVolumeUl = 10,
                BlowoutDelayMs = 100,
                VolumeAdjustmentUl = 0,
                PreWetCycles = 1,
                MixCycles = 0,
                LiquidFollowingDepthUm = 2000,
                RetractSpeedUmPerSecond = 1000,
                ConditioningVolumeUl = 0,
                BreakoffSpeedUlPerSecond = 0,
                PostDispenseAirGapUl = 0,
                CreatedAtUtc = now,
                PublishedAtUtc = now,
                EnabledAtUtc = now
            };
            version.ValidationRecords.Add(new LiquidClassValidationRecord
            {
                LiquidClassVersion = version,
                LiquidClassVersionId = version.Id,
                Stage = LiquidClassValidationStage.Enable,
                IsValid = true,
                ResultJson = "{\"valid\":true,\"source\":\"seed\"}",
                CreatedAtUtc = now
            });
            dbContext.LiquidClassVersions.Add(version);
            await dbContext.SaveChangesAsync(cancellationToken);
            liquidClass.EnabledVersionId = version.Id;
        }

        foreach (var reagent in ManualAcceptanceReagents())
        {
            var definition = await dbContext.ReagentDefinitions
                .SingleOrDefaultAsync(x => x.ReagentCode == reagent.Code, cancellationToken);
            if (definition is null)
            {
                dbContext.ReagentDefinitions.Add(new ReagentDefinition
                {
                    ReagentCode = reagent.Code,
                    Name = reagent.Name,
                    ReagentType = reagent.Type,
                    LiquidClassProfile = liquidClass,
                    LiquidClassProfileId = liquidClass.Id,
                    MinimumAlarmVolumeUl = reagent.MinimumAlarmVolumeUl,
                    LegacyMetadataJson = JsonSerializer.Serialize(new
                    {
                        manualAcceptance = true,
                        barcodePrefix = reagent.Code,
                        alias = reagent.Alias
                    }, JsonOptions),
                    IsEnabled = true,
                    CreatedAtUtc = now
                });
                continue;
            }

            var changed = false;
            if (definition.LiquidClassProfileId is null)
            {
                definition.LiquidClassProfile = liquidClass;
                definition.LiquidClassProfileId = liquidClass.Id;
                changed = true;
            }
            if (string.IsNullOrWhiteSpace(definition.Name) || definition.Name.StartsWith("Definition ", StringComparison.Ordinal))
            {
                definition.Name = reagent.Name;
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(definition.ReagentType) || definition.ReagentType == "test")
            {
                definition.ReagentType = reagent.Type;
                changed = true;
            }

            if (!definition.IsEnabled)
            {
                definition.IsEnabled = true;
                changed = true;
            }

            if (definition.MinimumAlarmVolumeUl is null)
            {
                definition.MinimumAlarmVolumeUl = reagent.MinimumAlarmVolumeUl;
                changed = true;
            }

            if (changed)
            {
                definition.UpdatedAtUtc = now;
            }
        }
    }

    private async Task<WorkflowVersion> EnsureSeedWorkflowAsync(
        string code,
        string name,
        string workflowType,
        string description,
        IReadOnlyList<SeedWorkflowStep> steps,
        IReadOnlyList<(string ReagentCode, int RequiredVolumeUl)> requirements,
        DateTimeOffset now,
        CancellationToken cancellationToken,
        string? planningRulesJson = null)
    {
        var definition = await dbContext.WorkflowDefinitions
            .AsSplitQuery()
            .Include(x => x.Versions)
            .ThenInclude(x => x.Steps)
            .Include(x => x.Versions)
            .ThenInclude(x => x.ReagentRequirements)
            .SingleOrDefaultAsync(x => x.Code == code, cancellationToken);

        if (definition is null)
        {
            definition = new WorkflowDefinition
            {
                Code = code,
                Name = name,
                WorkflowType = workflowType,
                Description = description,
                IsEnabled = true,
                CreatedAtUtc = now
            };
            dbContext.WorkflowDefinitions.Add(definition);
        }
        else
        {
            var changed = false;
            if (definition.Name != name)
            {
                definition.Name = name;
                changed = true;
            }

            if (definition.WorkflowType != workflowType)
            {
                definition.WorkflowType = workflowType;
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(definition.Description))
            {
                definition.Description = description;
                changed = true;
            }

            if (!definition.IsEnabled)
            {
                definition.IsEnabled = true;
                changed = true;
            }

            if (changed)
            {
                definition.UpdatedAtUtc = now;
            }
        }

        var version = definition.Versions.SingleOrDefault(x => x.VersionNo == 1);
        if (version is null)
        {
            version = new WorkflowVersion
            {
                WorkflowDefinition = definition,
                VersionNo = 1,
                VersionLabel = "1",
                Status = WorkflowVersionStatus.Published,
                ChangeNote = "Seeded for manual Mock acceptance.",
                PlanningRulesJson = planningRulesJson ?? "{}",
                PublishedAtUtc = now,
                CreatedAtUtc = now
            };
            definition.Versions.Add(version);
            AddWorkflowChildren(version, steps, requirements, now);
            return version;
        }

        if (version.Status == WorkflowVersionStatus.Published)
        {
            return version;
        }

        version.VersionLabel = "1";
        version.Status = WorkflowVersionStatus.Published;
        version.ChangeNote = "Seeded for manual Mock acceptance.";
        version.PublishedAtUtc ??= now;
        version.RetiredAtUtc = null;
        version.UpdatedAtUtc = now;
        dbContext.WorkflowSteps.RemoveRange(version.Steps);
        dbContext.WorkflowReagentRequirements.RemoveRange(version.ReagentRequirements);
        AddWorkflowChildren(version, steps, requirements, now);
        return version;
    }

    private static void AddWorkflowChildren(
        WorkflowVersion version,
        IReadOnlyList<SeedWorkflowStep> steps,
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
                TargetTemperatureDeciC = step.TargetTemperatureDeciC,
                MixParametersJson = "{}",
                WashParametersJson = "{}",
                LegacyParametersJson = step.LegacyParametersJson,
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

    private async Task EnsurePrimaryAntibodyMappingAsync(
        string primaryAntibodyCode,
        string workflowVersionId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var mapping = await dbContext.PrimaryAntibodyWorkflowMappings
            .SingleOrDefaultAsync(
                x => x.PrimaryAntibodyCode == primaryAntibodyCode
                    && x.WorkflowVersionId == workflowVersionId,
                cancellationToken);
        if (mapping is null)
        {
            dbContext.PrimaryAntibodyWorkflowMappings.Add(new PrimaryAntibodyWorkflowMapping
            {
                PrimaryAntibodyCode = primaryAntibodyCode,
                WorkflowVersionId = workflowVersionId,
                IsEnabled = true,
                CreatedAtUtc = now
            });
            return;
        }

        if (!mapping.IsEnabled)
        {
            mapping.IsEnabled = true;
        }
    }

    private async Task<SeedWorkflowSummary> LoadSeedWorkflowAsync(string code, CancellationToken cancellationToken)
    {
        var workflow = await dbContext.WorkflowDefinitions
            .AsNoTracking()
            .Include(x => x.Versions)
            .SingleAsync(x => x.Code == code, cancellationToken);
        var version = workflow.Versions.Single(x => x.VersionNo == 1);
        return new SeedWorkflowSummary(workflow.Code, workflow.Name, version.Id, version.DefaultExperimentType);
    }

    private static IReadOnlyList<SeedReagentDefinition> ManualAcceptanceReagents()
    {
        return
        [
            new("DBA", "DAB A", "dab", 1000, "DAB-A"),
            new("DBB", "DAB B", "dab", 1000, "DAB-B"),
            new("BLK", "测试封闭液", "blocking", 1000, "BLOCKER"),
            new("P01", "测试一抗 001", "primary", 1000, "PRIMARY-001"),
            new("SEC", "测试二抗", "secondary", 1000, "SECONDARY"),
            new("DAB", "测试 DAB 显色液", "dab", 1000, "DAB-A/B"),
            new("HEM", "测试苏木素", "common", 1000, "HEMATOXYLIN"),
            new("WAS", "测试清洗液", "wash", 1000, "WASH"),
            new("EOS", "测试伊红染色液", "common", 1000, "EOSIN"),
            new("ACD", "测试酸洗液", "wash", 1000, "ACID-WASH"),
            new("ETH", "测试无水乙醇", "wash", 1000, "ETHANOL")
        ];
    }

    private static IReadOnlyList<SeedWorkflowStep> HeSteps()
    {
        return
        [
            new(1, "HEMATOXYLIN", "HE hematoxylin", "Dispense", "HEM", 200, 4),
            new(2, "TERMINAL_WASH", "HE terminal wash", "Wash", "WAS", 100, 3)
        ];
    }

    private static IReadOnlyList<SeedWorkflowStep> IhcSteps()
    {
        return
        [
            new(1, "BLOCKING", "Blocking", "Dispense", "BLK", 100, 3),
            new(2, "PRIMARY_ANTIBODY", "Primary antibody 001", "Dispense", "P01", 100, 4),
            new(3, "WASH_AFTER_PRIMARY", "Wash after primary", "Wash", "WAS", 100, 3),
            new(4, "SECONDARY_ANTIBODY", "Secondary antibody", "Dispense", "SEC", 100, 4),
            new(5, "WASH_AFTER_SECONDARY", "Wash after secondary", "Wash", "WAS", 100, 3),
            new(6, "DAB", "DAB development", "Dab", "DAB", 100, 4),
            new(7, "WASH_AFTER_DAB", "Wash after DAB", "Wash", "WAS", 100, 3),
            new(8, "HEMATOXYLIN", "Hematoxylin counterstain", "Dispense", "HEM", 100, 4),
            new(9, "FINAL_WASH", "Final wash", "Wash", "WAS", 100, 3)
        ];
    }

    private static IReadOnlyList<SeedWorkflowStep> DefaultHeSteps()
    {
        return
        [
            UiStep(1, "HEMATOXYLIN", "苏木素染色", "Dispense", "HEM", "hematoxylin", "hematoxylin", 10, false),
            UiStep(2, "WASH", "水洗", "Wash", "WAS", "water", "water", 10, true),
            UiStep(3, "ACID_WASH", "酸洗", "Dispense", "ACD", "acid", "acid", 5, true),
            UiStep(4, "EOSIN", "伊红染色", "Dispense", "EOS", "eosin", "eosin", 8, false),
            UiStep(5, "ETHANOL_WASH", "无水乙醇清洗", "Dispense", "ETH", "ethanol", "ethanol", 12, true)
        ];
    }

    private static IReadOnlyList<SeedWorkflowStep> DefaultIhcSteps()
    {
        return
        [
            UiStep(1, "BLOCKING", "内源性酶阻断剂", "Dispense", "BLK", "block", "blocker", 20, false, notes: "阻断内源性酶活性"),
            UiStep(2, "WASH", "清洗液", "Wash", "WAS", "water", "wash", 15, true, notes: "阻断后立即清洗，避免残留影响一抗"),
            UiStep(3, "PRIMARY_ANTIBODY", "一抗", "Dispense", "P01", "primary", "primary", 270, false, allowMultiPrimary: true, notes: "一抗种类可多路映射；默认 4.5 min"),
            UiStep(4, "WASH_AFTER_PRIMARY", "清洗液", "Wash", "WAS", "water", "wash", 15, true, 400, "一抗后立即清洗；从本步开始进入 40℃ 控温段"),
            UiStep(5, "SECONDARY_ANTIBODY", "二抗", "Dispense", "SEC", "secondary", "secondary", 90, false, 400, "默认 1.5 min"),
            UiStep(6, "WASH_AFTER_SECONDARY", "清洗液", "Wash", "WAS", "water", "wash", 15, true, 400, "二抗后立即清洗"),
            UiStep(7, "DAB", "DAB", "Dab", "DAB", "dabColor", "dab", 90, false, 400, "DAB A:B:纯水 = 1:1:18，建议每轮现配"),
            UiStep(8, "WASH_AFTER_DAB", "水洗", "Wash", "WAS", "water", "water", 5, true, 400, "DAB 终止水洗"),
            UiStep(9, "HEMATOXYLIN", "苏木素", "Dispense", "HEM", "hematoxylin", "hematoxylin", 10, false, 400, "核复染"),
            UiStep(10, "FINAL_WASH", "水洗", "Wash", "WAS", "water", "water", 10, true, 400, "复染后水洗")
        ];
    }

    private static string DefaultHePlanningRulesJson() => JsonSerializer.Serialize(new
    {
        schemaVersion = 1,
        targetTempC = (int?)null,
        tempControlFromStep = (int?)null,
        allowMultiPrimary = false,
        dabRatio = (object?)null,
        notes = "HE 模板用于快速染色；可复制为草稿后按实验要求调整。"
    }, JsonOptions);

    private static string DefaultIhcPlanningRulesJson() => JsonSerializer.Serialize(new
    {
        schemaVersion = 1,
        targetTempC = 40,
        tempControlFromStep = 4,
        allowMultiPrimary = true,
        dabRatio = new { a = 1, b = 1, pureWater = 18, preparePolicy = "per_run" },
        notes = "一抗可按玻片/通道映射不同抗体；共用试剂统一调度；DAB 推荐每轮现配。"
    }, JsonOptions);

    private static SeedWorkflowStep UiStep(
        int stepNo,
        string majorStepCode,
        string stepName,
        string actionType,
        string reagentCode,
        string opKey,
        string reagentRole,
        int durationSeconds,
        bool immediateAfterPrevious,
        int? targetTemperatureDeciC = null,
        string notes = "",
        bool allowMultiPrimary = false)
    {
        var legacy = JsonSerializer.Serialize(new
        {
            ui = new
            {
                opKey,
                label = stepName,
                toleranceSec = 0,
                immediateAfterPrev = immediateAfterPrevious,
                requiresTemp = targetTemperatureDeciC is not null,
                reagentRole,
                allowMultiPrimary,
                notes
            }
        }, JsonOptions);
        return new SeedWorkflowStep(stepNo, majorStepCode, stepName, actionType, reagentCode, 100, durationSeconds, targetTemperatureDeciC, legacy);
    }

    private sealed record SeedReagentDefinition(string Code, string Name, string Type, int MinimumAlarmVolumeUl, string Alias);

    private sealed record SeedWorkflowStep(
        int StepNo,
        string MajorStepCode,
        string StepName,
        string ActionType,
        string? ReagentCode,
        int? VolumeUl,
        int DurationSeconds,
        int? TargetTemperatureDeciC = null,
        string LegacyParametersJson = "{}");

    private sealed record SeedWorkflowSummary(string Code, string Name, string WorkflowVersionId, string? DefaultExperimentType);
}

public sealed record ManualAcceptanceSeedSummary(
    string HeWorkflowName,
    string HeWorkflowCode,
    string HeWorkflowVersionId,
    string IhcWorkflowName,
    string IhcWorkflowCode,
    string IhcWorkflowVersionId,
    string PrimaryAntibodyCode,
    bool PrimaryAntibodyMappingEnabled,
    string? PrimaryAntibodyWorkflowVersionId,
    IReadOnlyList<string> RequiredReagentCodes,
    bool HeIsDefault,
    bool IhcIsDefault);
