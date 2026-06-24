namespace Stainer.Web.Domain.Entities;

public sealed class StainingTask
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string TaskCode { get; set; } = string.Empty;
    public string TaskType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string PhysicalSlotId { get; set; } = string.Empty;
    public string WorkflowDefinitionId { get; set; } = string.Empty;
    public string WorkflowVersionId { get; set; } = string.Empty;
    public string WorkflowSnapshotJson { get; set; } = "{}";
    public string? InputMode { get; set; }
    public string? RawCode { get; set; }
    public string? NormalizedCode { get; set; }
    public string? PrimaryAntibodyCode { get; set; }
    public string CandidateResultsJson { get; set; } = "[]";
    public string? CreatedByUserId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public PhysicalSlot? PhysicalSlot { get; set; }
    public WorkflowDefinition? WorkflowDefinition { get; set; }
    public WorkflowVersion? WorkflowVersion { get; set; }
    public User? CreatedByUser { get; set; }
}
