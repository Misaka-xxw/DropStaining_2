using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Tests;

public sealed class BusinessWriteApiIntegrationTests
{
    [Fact]
    public async Task Admin_user_management_is_authorized_idempotent_audited_and_rolls_back_on_failure()
    {
        await using var factory = CreateFactory();
        using var adminClient = factory.CreateClient();
        await LoginAsync(adminClient, "admin", "admin");

        var createRequest = new
        {
            commandId = "cmd-user-create-001",
            username = "api-user",
            displayName = "API User",
            password = "Start123!",
            roles = new[] { "operator" }
        };

        var created = await PostJsonAsync<UserMutationResponse>(adminClient, "/api/users", createRequest);
        Assert.False(created.Replayed);

        var replayed = await PostJsonAsync<UserMutationResponse>(adminClient, "/api/users", createRequest);
        Assert.True(replayed.Replayed);
        Assert.Equal(created.UserId, replayed.UserId);

        var renamed = await PutJsonAsync<UserMutationResponse>(adminClient, $"/api/users/{created.UserId}/display-name", new
        {
            commandId = "cmd-user-rename-001",
            displayName = "Renamed API User"
        });
        Assert.Equal("Renamed API User", renamed.DisplayName);

        var disabled = await PutJsonAsync<UserMutationResponse>(adminClient, $"/api/users/{created.UserId}/enabled", new
        {
            commandId = "cmd-user-disable-001",
            enabled = false
        });
        Assert.False(disabled.Enabled);

        var reset = await PutJsonAsync<UserMutationResponse>(adminClient, $"/api/users/{created.UserId}/password", new
        {
            commandId = "cmd-user-password-001",
            newPassword = "Next123!"
        });
        Assert.True(reset.Ok);

        var roles = await PutJsonAsync<UserMutationResponse>(adminClient, $"/api/users/{created.UserId}/roles", new
        {
            commandId = "cmd-user-roles-001",
            roles = new[] { "operator", "engineer" }
        });
        Assert.Contains("engineer", roles.Roles);

        var delete = await adminClient.DeleteAsync($"/api/users/{created.UserId}?commandId=cmd-user-delete-001");
        Assert.Equal(HttpStatusCode.Conflict, delete.StatusCode);

        var duplicateUsername = await adminClient.PostAsJsonAsync("/api/users", new
        {
            commandId = "cmd-user-duplicate-001",
            username = "api-user",
            displayName = "Duplicate",
            password = "Start123!",
            roles = new[] { "operator" }
        });
        Assert.Equal(HttpStatusCode.Conflict, duplicateUsername.StatusCode);

        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            Assert.Equal(1, await dbContext.Users.CountAsync(x => x.Username == "api-user"));
            Assert.Equal(1, await dbContext.CommandReceipts.CountAsync(x => x.CommandId == "cmd-user-create-001"));
            Assert.False(await dbContext.CommandReceipts.AnyAsync(x => x.CommandId == "cmd-user-duplicate-001"));
            Assert.True(await dbContext.AuditLogs.AnyAsync(x => x.Action == "user.create" && x.EntityId == created.UserId));
            Assert.True(await dbContext.AuditLogs.AnyAsync(x => x.Action == "user.set_roles" && x.EntityId == created.UserId));
        }

        using var operatorClient = factory.CreateClient();
        await LoginAsync(operatorClient, "operator", "operator");
        var forbidden = await operatorClient.PostAsJsonAsync("/api/users", new
        {
            commandId = "cmd-user-forbidden-001",
            username = "bad",
            displayName = "Bad",
            password = "Start123!",
            roles = new[] { "operator" }
        });
        Assert.Equal(HttpStatusCode.Forbidden, forbidden.StatusCode);
    }

    [Fact]
    public async Task Workflow_draft_creation_is_authorized_idempotent_audited_and_can_copy_latest_version()
    {
        await using var factory = CreateFactory();
        using var adminClient = factory.CreateClient();
        await LoginAsync(adminClient, "admin", "admin");

        var createRequest = new
        {
            commandId = "cmd-workflow-draft-create-001",
            code = "DRAFT-API",
            name = "API Draft Workflow",
            workflowType = StainingTaskType.Ihc,
            description = "Created by API integration test.",
            versionLabel = "0.1",
            changeNote = "Create blank draft."
        };

        var created = await PostJsonAsync<WorkflowDraftMutationResponse>(adminClient, "/api/workflows/drafts", createRequest);
        Assert.False(created.Replayed);
        Assert.Equal(WorkflowVersionStatus.Draft, created.Status);
        Assert.Equal(1, created.VersionNo);

        var replayed = await PostJsonAsync<WorkflowDraftMutationResponse>(adminClient, "/api/workflows/drafts", createRequest);
        Assert.True(replayed.Replayed);
        Assert.Equal(created.WorkflowVersionId, replayed.WorkflowVersionId);

        string sourceWorkflowId;
        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            var sourceVersion = await CreatePublishedWorkflowVersionAsync(dbContext, "COPY-SOURCE", StainingTaskType.Ihc, "SRC", 500);
            sourceWorkflowId = sourceVersion.WorkflowDefinition!.Id;
        }

        var copied = await PostJsonAsync<WorkflowDraftMutationResponse>(adminClient, "/api/workflows/drafts", new
        {
            commandId = "cmd-workflow-draft-copy-001",
            sourceWorkflowId,
            versionLabel = "2.0",
            changeNote = "Copy latest version."
        });
        Assert.Equal(sourceWorkflowId, copied.WorkflowDefinitionId);
        Assert.Equal(2, copied.VersionNo);
        Assert.Equal("2.0", copied.VersionLabel);
        Assert.Equal(WorkflowVersionStatus.Draft, copied.Status);

        await using (var verifyScope = factory.Services.CreateAsyncScope())
        {
            var dbContext = verifyScope.ServiceProvider.GetRequiredService<StainerDbContext>();
            var blank = await dbContext.WorkflowVersions.SingleAsync(x => x.Id == created.WorkflowVersionId);
            Assert.Equal(WorkflowVersionStatus.Draft, blank.Status);
            Assert.True(await dbContext.AuditLogs.AnyAsync(x => x.Action == "workflow.draft.create" && x.EntityId == created.WorkflowVersionId));
            Assert.True(await dbContext.AuditLogs.AnyAsync(x => x.Action == "workflow.draft.copy" && x.EntityId == copied.WorkflowVersionId));
            Assert.Equal(1, await dbContext.WorkflowSteps.CountAsync(x => x.WorkflowVersionId == copied.WorkflowVersionId));
            Assert.Equal(1, await dbContext.WorkflowReagentRequirements.CountAsync(x => x.WorkflowVersionId == copied.WorkflowVersionId));
        }

        using var operatorClient = factory.CreateClient();
        await LoginAsync(operatorClient, "operator", "operator");
        var forbidden = await operatorClient.PostAsJsonAsync("/api/workflows/drafts", new
        {
            commandId = "cmd-workflow-draft-forbidden-001",
            code = "NOPE",
            name = "Forbidden",
            workflowType = StainingTaskType.He
        });
        Assert.Equal(HttpStatusCode.Forbidden, forbidden.StatusCode);
    }

    [Fact]
    public async Task Task_creation_covers_he_manual_confirmation_and_ihc_selection_rules()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "operator", "operator");

        string heVersionId;
        string ihcVersionOneId;
        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            var heVersion = await CreatePublishedWorkflowVersionAsync(dbContext, "HE-API", StainingTaskType.He, "HEM", 2000);
            var ihcVersionOne = await CreatePublishedWorkflowVersionAsync(dbContext, "IHC-API-1", StainingTaskType.Ihc, "ABC", 1000);
            var ihcVersionTwo = await CreatePublishedWorkflowVersionAsync(dbContext, "IHC-API-2", StainingTaskType.Ihc, "ABC", 1000);
            var ihcVersionThree = await CreatePublishedWorkflowVersionAsync(dbContext, "IHC-API-3", StainingTaskType.Ihc, "XYZ", 1000);
            dbContext.PrimaryAntibodyWorkflowMappings.AddRange(
                new PrimaryAntibodyWorkflowMapping { PrimaryAntibodyCode = "PA1", WorkflowVersionId = ihcVersionOne.Id },
                new PrimaryAntibodyWorkflowMapping { PrimaryAntibodyCode = "PA1", WorkflowVersionId = ihcVersionTwo.Id },
                new PrimaryAntibodyWorkflowMapping { PrimaryAntibodyCode = "PA2", WorkflowVersionId = ihcVersionThree.Id });
            dbContext.HospitalBarcodeMappings.AddRange(
                new HospitalBarcodeMapping { HospitalCode = "HOSP-MULTI", PrimaryAntibodyCode = "PA1" },
                new HospitalBarcodeMapping { HospitalCode = "HOSP-MULTI", PrimaryAntibodyCode = "PA2" });
            await dbContext.SaveChangesAsync();

            heVersionId = heVersion.Id;
            ihcVersionOneId = ihcVersionOne.Id;
        }

        var heTask = await PostJsonAsync<TaskCreationResponse>(client, "/api/tasks/he", new
        {
            commandId = "cmd-he-task-001",
            workflowVersionId = heVersionId,
            slotCode = "A-01"
        });
        Assert.True(heTask.Ok);
        Assert.False(string.IsNullOrWhiteSpace(heTask.TaskId));

        var multiWorkflow = await client.PostAsJsonAsync("/api/tasks/ihc", new
        {
            commandId = "cmd-ihc-multi-workflow-001",
            inputMode = "DirectPrimaryAntibody",
            rawCode = "PA1",
            slotCode = "A-02"
        });
        Assert.Equal(HttpStatusCode.Conflict, multiWorkflow.StatusCode);
        var multiWorkflowBody = await multiWorkflow.Content.ReadFromJsonAsync<TaskCreationResponse>();
        Assert.True(multiWorkflowBody!.RequiresSelection);
        Assert.Equal(2, multiWorkflowBody.CandidateWorkflows.Count);

        var lisMissing = await client.PostAsJsonAsync("/api/tasks/ihc", new
        {
            commandId = "cmd-ihc-lis-missing-001",
            inputMode = "HospitalBarcode",
            rawCode = " UNKNOWN\r\n",
            slotCode = "A-02"
        });
        Assert.Equal(HttpStatusCode.NotFound, lisMissing.StatusCode);

        var multiAntibody = await client.PostAsJsonAsync("/api/tasks/ihc", new
        {
            commandId = "cmd-ihc-multi-antibody-001",
            inputMode = "HospitalBarcode",
            rawCode = " HOSP-MULTI\r\n",
            slotCode = "A-02"
        });
        Assert.Equal(HttpStatusCode.Conflict, multiAntibody.StatusCode);
        var multiAntibodyBody = await multiAntibody.Content.ReadFromJsonAsync<TaskCreationResponse>();
        Assert.True(multiAntibodyBody!.RequiresSelection);
        Assert.Equal(2, multiAntibodyBody.CandidatePrimaryAntibodyCodes.Count);

        var ihcTask = await PostJsonAsync<TaskCreationResponse>(client, "/api/tasks/ihc", new
        {
            commandId = "cmd-ihc-task-001",
            inputMode = "DirectPrimaryAntibody",
            rawCode = "PA1",
            selectedWorkflowVersionId = ihcVersionOneId,
            slotCode = "A-02"
        });
        Assert.True(ihcTask.Ok);

        await using var verifyScope = factory.Services.CreateAsyncScope();
        var verifyContext = verifyScope.ServiceProvider.GetRequiredService<StainerDbContext>();
        var persisted = await verifyContext.StainingTasks.SingleAsync(x => x.Id == ihcTask.TaskId);
        Assert.Equal("PA1", persisted.PrimaryAntibodyCode);
        Assert.Contains(ihcVersionOneId, persisted.WorkflowSnapshotJson);
    }

    [Fact]
    public async Task Reagent_scan_engineering_writes_preflight_and_transaction_rollback_are_covered()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        await LoginAsync(client, "admin", "admin");

        string heVersionId;
        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<StainerDbContext>();
            var heVersion = await CreatePublishedWorkflowVersionAsync(dbContext, "HE-PREFLIGHT", StainingTaskType.He, "ABC", 1000);
            heVersionId = heVersion.Id;
        }

        _ = await PostJsonAsync<TaskCreationResponse>(client, "/api/tasks/he", new
        {
            commandId = "cmd-preflight-task-001",
            workflowVersionId = heVersionId,
            slotCode = "B-01"
        });

        var scan = await PostJsonAsync<ReagentScanConfirmationResponse>(client, "/api/reagents/scan-confirm", new
        {
            commandId = "cmd-scan-confirm-001",
            items = new object[]
            {
                new { position = "R1", scanResult = "VALID", rawBarcode = "ABC05020260101001", locatorCode = "R1", expirationDate = "2027-01-01" },
                new { position = "R2", scanResult = "INVALID", rawBarcode = "BAD", locatorCode = "R2", expirationDate = (string?)null }
            }
        });
        Assert.Equal(1, scan.ValidCount);
        Assert.Equal(1, scan.InvalidCount);
        Assert.Equal(38, scan.EmptyCount);

        var calibration = await PostJsonAsync<EngineeringWriteResponse>(client, "/api/engineering/coordinate-points/calibrate", new
        {
            commandId = "cmd-coordinate-calibrate-001",
            profileCode = ReferenceDataSeeder.DefaultCoordinateProfileCode,
            pointCode = "R1",
            calibratedXUm = 111L,
            calibratedYUm = 222L,
            safeZUm = 1000L,
            aspirateZUm = 900L,
            dispenseZUm = 800L,
            reason = "integration test calibration"
        });
        Assert.True(calibration.Ok);

        var liquid = await PostJsonAsync<EngineeringWriteResponse>(client, "/api/engineering/liquid-classes", new
        {
            commandId = "cmd-liquid-save-001",
            code = "LC-WRITE",
            name = "Write Liquid",
            aspirateSpeedUlPerSecond = 10,
            dispenseSpeedUlPerSecond = 20,
            leadingAirGapUl = 1,
            trailingAirGapUl = 2,
            excessVolumeUl = 3,
            preWetCycles = 1,
            mixCycles = 2,
            isEnabled = true,
            reason = "integration test liquid"
        });
        Assert.True(liquid.Ok);

        var device = await PostJsonAsync<EngineeringWriteResponse>(client, "/api/engineering/device-profiles", new
        {
            commandId = "cmd-device-save-001",
            code = "DEVICE-WRITE",
            name = "Write Device",
            isActive = true,
            reason = "integration test device"
        });
        Assert.True(device.Ok);

        var failedCalibration = await client.PostAsJsonAsync("/api/engineering/coordinate-points/calibrate", new
        {
            commandId = "cmd-coordinate-rollback-001",
            profileCode = ReferenceDataSeeder.DefaultCoordinateProfileCode,
            pointCode = "NO-SUCH-POINT",
            calibratedXUm = 1L,
            calibratedYUm = 2L,
            safeZUm = 3L,
            aspirateZUm = 4L,
            dispenseZUm = 5L,
            reason = "should rollback"
        });
        Assert.Equal(HttpStatusCode.NotFound, failedCalibration.StatusCode);

        var preflight = await client.GetFromJsonAsync<PreflightValidationReportResponse>("/api/run/preflight");
        Assert.NotNull(preflight);
        Assert.False(preflight!.Ok);
        Assert.Contains(preflight.Issues, x => x.Code == "scan_has_invalid_items");

        await using var verifyScope = factory.Services.CreateAsyncScope();
        var verifyContext = verifyScope.ServiceProvider.GetRequiredService<StainerDbContext>();
        Assert.Equal(40, await verifyContext.ReagentScanItems.CountAsync(x => x.ReagentScanSessionId == scan.SessionId));
        Assert.True(await verifyContext.ReagentBottles.AnyAsync(x => x.FullBarcode == "ABC05020260101001"));
        Assert.True(await verifyContext.CoordinateCalibrationHistory.AnyAsync(x => x.CoordinatePointId == calibration.EntityId));
        Assert.True(await verifyContext.AuditLogs.AnyAsync(x => x.Action == "engineering.coordinate.calibrate" && x.Message.Contains("integration test calibration")));
        Assert.False(await verifyContext.CommandReceipts.AnyAsync(x => x.CommandId == "cmd-coordinate-rollback-001"));
    }

    private static WebApplicationFactory<Program> CreateFactory()
    {
        var databasePath = Path.Combine(Path.GetTempPath(), "stainer-business-write-tests", Guid.NewGuid().ToString("N"), "stainer.db");
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.UseSetting("ConnectionStrings:StainerDatabase", $"Data Source={databasePath}");
                builder.ConfigureAppConfiguration((_, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:StainerDatabase"] = $"Data Source={databasePath}"
                    });
                });
            });
    }

    private static async Task LoginAsync(HttpClient client, string username, string role)
    {
        var response = await client.PostAsJsonAsync("/api/login", new
        {
            username,
            password = "123456",
            role
        });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static async Task<T> PostJsonAsync<T>(HttpClient client, string url, object request)
    {
        var response = await client.PostAsJsonAsync(url, request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<T>();
        Assert.NotNull(body);
        return body!;
    }

    private static async Task<T> PutJsonAsync<T>(HttpClient client, string url, object request)
    {
        var response = await client.PutAsJsonAsync(url, request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<T>();
        Assert.NotNull(body);
        return body!;
    }

    private static async Task<WorkflowVersion> CreatePublishedWorkflowVersionAsync(
        StainerDbContext dbContext,
        string workflowCode,
        string workflowType,
        string reagentCode,
        int requiredVolumeUl)
    {
        var reagentDefinition = await dbContext.ReagentDefinitions.SingleOrDefaultAsync(x => x.ReagentCode == reagentCode);
        if (reagentDefinition is null)
        {
            reagentDefinition = new ReagentDefinition
            {
                ReagentCode = reagentCode,
                Name = $"Reagent {reagentCode}",
                ReagentType = "test",
                CreatedAtUtc = DateTimeOffset.UtcNow
            };
            dbContext.ReagentDefinitions.Add(reagentDefinition);
        }

        var workflowDefinition = new WorkflowDefinition
        {
            Code = workflowCode,
            Name = $"{workflowCode} definition",
            WorkflowType = workflowType,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
        var workflowVersion = new WorkflowVersion
        {
            WorkflowDefinition = workflowDefinition,
            VersionNo = 1,
            VersionLabel = "1.0",
            Status = WorkflowVersionStatus.Published,
            ChangeNote = "Published for API integration test.",
            PublishedAtUtc = DateTimeOffset.UtcNow,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
        workflowVersion.Steps.Add(new WorkflowStep
        {
            StepNo = 1,
            MajorStepCode = "STEP",
            StepName = "Test step",
            ActionType = "Dispense",
            ReagentCode = reagentCode,
            VolumeUl = requiredVolumeUl,
            DurationSeconds = 60,
            TargetTemperatureDeciC = 250,
            FailureStrategy = "Stop",
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
        workflowVersion.ReagentRequirements.Add(new WorkflowReagentRequirement
        {
            ReagentCode = reagentCode,
            RequiredVolumeUl = requiredVolumeUl,
            IsRequired = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
        dbContext.WorkflowVersions.Add(workflowVersion);
        await dbContext.SaveChangesAsync();
        return workflowVersion;
    }
}
