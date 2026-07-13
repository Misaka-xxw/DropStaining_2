namespace Stainer.Web.Infrastructure.Web;

using Microsoft.Extensions.Hosting;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Application.Services;
using Stainer.Web.Infrastructure.Health;

public static partial class WebHostEndpointExtensions
{
    private static void MapWorkflowEndpoints(WebApplication app)
    {
        app.MapGet("/api/workflows", async (WorkflowQueryService service, CancellationToken cancellationToken) =>
            Results.Ok(await service.ListAsync(cancellationToken)));
        app.MapGet("/api/workflows/{id}", async (string id, WorkflowQueryService service, CancellationToken cancellationToken) =>
        {
            var workflow = await service.GetAsync(id, cancellationToken);
            return workflow is null ? Results.NotFound() : Results.Ok(workflow);
        });
        app.MapPost("/api/workflows", async (HttpContext context, CreateWorkflowRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.CreateWorkflowAsync(request, actor, cancellationToken));
            }));
        app.MapPost("/api/workflows/{workflowId}/versions", async (HttpContext context, string workflowId, CreateWorkflowVersionRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.CreateWorkflowVersionAsync(workflowId, request, actor, cancellationToken));
            }));
        app.MapPost("/api/workflows/drafts", async (HttpContext context, CreateWorkflowDraftRequest request, UserSessionService sessionService, WorkflowWriteService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.CreateDraftAsync(request, actor, cancellationToken));
            }));
        app.MapGet("/api/workflow-versions/{workflowVersionId}", async (string workflowVersionId, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var version = await service.GetVersionAsync(workflowVersionId, cancellationToken);
                return version is null ? Results.NotFound() : Results.Ok(version);
            }));
        app.MapPut("/api/workflow-versions/{workflowVersionId}", async (HttpContext context, string workflowVersionId, UpdateWorkflowVersionRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.UpdateVersionAsync(workflowVersionId, request, actor, cancellationToken));
            }));
        app.MapPost("/api/workflow-versions/{workflowVersionId}/copy-draft", async (HttpContext context, string workflowVersionId, CopyWorkflowVersionDraftRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.CopyVersionAsDraftAsync(workflowVersionId, request, actor, cancellationToken));
            }));
        app.MapGet("/api/workflow-versions/{workflowVersionId}/steps", async (string workflowVersionId, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var version = await service.GetVersionAsync(workflowVersionId, cancellationToken);
                return version is null ? Results.NotFound() : Results.Ok(version.Steps);
            }));
        app.MapPost("/api/workflow-versions/{workflowVersionId}/steps", async (HttpContext context, string workflowVersionId, SaveWorkflowStepRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.AddStepAsync(workflowVersionId, request, actor, cancellationToken));
            }));
        app.MapPut("/api/workflow-versions/{workflowVersionId}/steps/{stepId}", async (HttpContext context, string workflowVersionId, string stepId, SaveWorkflowStepRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.UpdateStepAsync(workflowVersionId, stepId, request, actor, cancellationToken));
            }));
        app.MapDelete("/api/workflow-versions/{workflowVersionId}/steps/{stepId}", async (HttpContext context, string workflowVersionId, string stepId, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                var commandId = context.Request.Query["commandId"].ToString();
                return Results.Ok(await service.DeleteStepAsync(workflowVersionId, stepId, commandId, actor, cancellationToken));
            }));
        app.MapPost("/api/workflow-versions/{workflowVersionId}/steps/{stepId}/move-up", async (HttpContext context, string workflowVersionId, string stepId, RunCommandRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.MoveStepAsync(workflowVersionId, stepId, true, request.CommandId, actor, cancellationToken));
            }));
        app.MapPost("/api/workflow-versions/{workflowVersionId}/steps/{stepId}/move-down", async (HttpContext context, string workflowVersionId, string stepId, RunCommandRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.MoveStepAsync(workflowVersionId, stepId, false, request.CommandId, actor, cancellationToken));
            }));
        app.MapGet("/api/workflow-versions/{workflowVersionId}/reagent-requirements", async (string workflowVersionId, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var version = await service.GetVersionAsync(workflowVersionId, cancellationToken);
                return version is null ? Results.NotFound() : Results.Ok(version.ReagentRequirements);
            }));
        app.MapPost("/api/workflow-versions/{workflowVersionId}/reagent-requirements", async (HttpContext context, string workflowVersionId, SaveWorkflowReagentRequirementRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.AddRequirementAsync(workflowVersionId, request, actor, cancellationToken));
            }));
        app.MapPut("/api/workflow-versions/{workflowVersionId}/reagent-requirements/{id}", async (HttpContext context, string workflowVersionId, string id, SaveWorkflowReagentRequirementRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.UpdateRequirementAsync(workflowVersionId, id, request, actor, cancellationToken));
            }));
        app.MapDelete("/api/workflow-versions/{workflowVersionId}/reagent-requirements/{id}", async (HttpContext context, string workflowVersionId, string id, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                var commandId = context.Request.Query["commandId"].ToString();
                return Results.Ok(await service.DeleteRequirementAsync(workflowVersionId, id, commandId, actor, cancellationToken));
            }));
        app.MapPost("/api/workflow-versions/{workflowVersionId}/reagent-requirements/recalculate", async (HttpContext context, string workflowVersionId, RunCommandRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.RecalculateRequirementsAsync(workflowVersionId, request.CommandId, actor, cancellationToken));
            }));
        app.MapGet("/api/workflow-versions/{workflowVersionId}/publish-validation", async (string workflowVersionId, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () => Results.Ok(await service.ValidatePublishAsync(workflowVersionId, cancellationToken))));
        app.MapPost("/api/workflow-versions/{workflowVersionId}/publish", async (HttpContext context, string workflowVersionId, PublishWorkflowVersionRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.PublishAsync(workflowVersionId, request, actor, cancellationToken));
            }));
        app.MapPost("/api/workflow-versions/{workflowVersionId}/set-default", async (HttpContext context, string workflowVersionId, SetDefaultWorkflowVersionRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.SetDefaultAsync(workflowVersionId, request, actor, cancellationToken));
            }));
        app.MapPost("/api/workflow-versions/{workflowVersionId}/retire", async (HttpContext context, string workflowVersionId, RetireWorkflowVersionRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.RetireAsync(workflowVersionId, request, actor, cancellationToken));
            }));
        app.MapGet("/api/primary-antibody-mappings", async (WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            Results.Ok(await service.ListPrimaryAntibodyMappingsAsync(cancellationToken)));
        app.MapPost("/api/primary-antibody-mappings", async (HttpContext context, CreatePrimaryAntibodyMappingRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.CreateMappingAsync(request, actor, cancellationToken));
            }));
        app.MapPost("/api/primary-antibody-mappings/{id}/enable", async (HttpContext context, string id, ChangePrimaryAntibodyMappingStateRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.SetMappingEnabledAsync(id, request, actor, true, cancellationToken));
            }));
        app.MapPost("/api/primary-antibody-mappings/{id}/disable", async (HttpContext context, string id, ChangePrimaryAntibodyMappingStateRequest request, UserSessionService sessionService, WorkflowMaintenanceService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireRoleAsync(context, "admin", cancellationToken);
                return Results.Ok(await service.SetMappingEnabledAsync(id, request, actor, false, cancellationToken));
            }));
        app.MapGet("/api/protocols", async (WorkflowQueryService service, CancellationToken cancellationToken) =>
            Results.Ok(await service.ListProtocolCompatAsync(cancellationToken)));
        app.MapPost("/api/channel-batches/workflow-selection", async (HttpContext context, SelectChannelWorkflowRequest request, UserSessionService sessionService, ChannelBatchWorkflowService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.SelectWorkflowAsync(request, actor, cancellationToken));
            }));
        app.MapPost("/api/channel-batches/experiment-type-selection", async (HttpContext context, SelectChannelExperimentTypeRequest request, UserSessionService sessionService, ChannelBatchWorkflowService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.SelectExperimentTypeAsync(request, actor, cancellationToken));
            }));
        app.MapPost("/api/channel-batches/active", async (HttpContext context, EnsureChannelBatchRequest request, UserSessionService sessionService, ChannelBatchWorkflowService service, CancellationToken cancellationToken) =>
            await ExecuteBusinessAsync(async () =>
            {
                var actor = await sessionService.RequireAnyRoleAsync(context, ["operator", "admin"], cancellationToken);
                return Results.Ok(await service.EnsureActiveBatchAsync(request, actor, cancellationToken));
            }));
        app.MapGet("/api/reagents/catalog", async (ReagentQueryService service, CancellationToken cancellationToken) =>
            Results.Ok(await service.ListCatalogAsync(cancellationToken)));
        app.MapGet("/api/reagents/rack", async (ReagentQueryService service, CancellationToken cancellationToken) =>
            Results.Ok(await service.ListRackAsync(cancellationToken)));
        app.MapGet("/api/reagents/scan-sessions/overview", async (ReagentQueryService service, CancellationToken cancellationToken) =>
            Results.Ok(await service.GetScanSessionOverviewAsync(cancellationToken)));
    }
}
