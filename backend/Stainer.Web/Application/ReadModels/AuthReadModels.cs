namespace Stainer.Web.Application.ReadModels;

public sealed record CurrentUserResponse(
    string Id,
    string Username,
    string DisplayName,
    string ActiveRole,
    IReadOnlyList<string> Roles);

public sealed record LoginResponse(
    bool Ok,
    CurrentUserResponse User,
    string Redirect);
