using Stainer.Web.Application.ReadModels;

namespace Stainer.Web.Application.Repositories;

public interface IUserReadRepository
{
    Task<IReadOnlyList<UserListItemResponse>> ListUsersAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RoleListItemResponse>> ListRolesAsync(CancellationToken cancellationToken = default);
}
