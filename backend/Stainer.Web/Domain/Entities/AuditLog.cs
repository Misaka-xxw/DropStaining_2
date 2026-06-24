namespace Stainer.Web.Domain.Entities;

public sealed class AuditLog
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? ActorUserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public User? ActorUser { get; set; }
}
