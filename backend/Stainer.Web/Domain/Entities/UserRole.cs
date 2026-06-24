namespace Stainer.Web.Domain.Entities;

public sealed class UserRole
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = string.Empty;
    public string RoleId { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public User? User { get; set; }
    public Role? Role { get; set; }
}
