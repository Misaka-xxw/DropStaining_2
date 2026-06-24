using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Repositories;

namespace Stainer.Web.Application.Services;

public sealed class UserQueryService(IUserReadRepository repository)
{
    public Task<IReadOnlyList<UserListItemResponse>> ListUsersAsync(CancellationToken cancellationToken = default)
    {
        return repository.ListUsersAsync(cancellationToken);
    }

    public Task<IReadOnlyList<RoleListItemResponse>> ListRolesAsync(CancellationToken cancellationToken = default)
    {
        return repository.ListRolesAsync(cancellationToken);
    }
}
