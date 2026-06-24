namespace Stainer.Web.Domain.Entities;

public sealed class CommandReceipt
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CommandId { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public string RequestHash { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ResponseJson { get; set; } = "{}";
    public string? ActorUserId { get; set; }
    public string? EntityType { get; set; }
    public string? EntityId { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? CompletedAtUtc { get; set; }

    public User? ActorUser { get; set; }
}
