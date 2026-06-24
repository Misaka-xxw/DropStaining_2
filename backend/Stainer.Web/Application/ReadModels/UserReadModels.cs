namespace Stainer.Web.Application.ReadModels;

public sealed record UserListItemResponse(
    string Id,
    string Username,
    string DisplayName,
    string? Role,
    IReadOnlyList<string> Roles,
    bool Enabled);

public sealed record RoleListItemResponse(
    string Id,
    string Code,
    string Name);
