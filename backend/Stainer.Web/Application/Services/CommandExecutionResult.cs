namespace Stainer.Web.Application.Services;

public sealed record CommandExecutionResult<T>(
    T Response,
    string? EntityType,
    string? EntityId);
