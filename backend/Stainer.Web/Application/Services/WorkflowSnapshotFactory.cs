using Stainer.Web.Domain.Entities;

namespace Stainer.Web.Application.Services;

internal static class WorkflowSnapshotFactory
{
    public static object Create(WorkflowVersion version)
    {
        return new
        {
            workflowDefinitionId = version.WorkflowDefinitionId,
            workflowCode = version.WorkflowDefinition?.Code,
            workflowName = version.WorkflowDefinition?.Name,
            workflowType = version.WorkflowDefinition?.WorkflowType,
            workflowVersionId = version.Id,
            version.VersionNo,
            version.VersionLabel,
            version.Status,
            version.PlanningRulesJson,
            steps = version.Steps.OrderBy(x => x.StepNo).Select(x => new
            {
                x.StepNo,
                x.MajorStepCode,
                x.StepName,
                x.ActionType,
                x.ReagentCode,
                x.VolumeUl,
                x.DurationSeconds,
                x.TargetTemperatureDeciC,
                x.FailureStrategy,
                x.MixParametersJson,
                x.WashParametersJson,
                x.LegacyParametersJson
            }),
            reagentRequirements = version.ReagentRequirements.OrderBy(x => x.ReagentCode).Select(x => new
            {
                x.ReagentCode,
                x.RequiredVolumeUl,
                x.IsRequired
            })
        };
    }
}
