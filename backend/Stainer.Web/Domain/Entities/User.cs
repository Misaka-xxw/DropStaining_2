namespace Stainer.Web.Domain.Entities;

public sealed class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public string? PasswordHashAlgorithm { get; set; }
    public DateTimeOffset? PasswordUpdatedAtUtc { get; set; }
    public bool IsEnabled { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
