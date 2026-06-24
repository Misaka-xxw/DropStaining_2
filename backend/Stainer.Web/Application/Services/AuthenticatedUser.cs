namespace Stainer.Web.Application.Services;

public sealed record AuthenticatedUser(
    string UserId,
    string Username,
    string DisplayName,
    string ActiveRole,
    IReadOnlyList<string> Roles)
{
    public bool HasRole(string role)
    {
        return Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
    }
}
