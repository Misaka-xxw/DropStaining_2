using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Repositories;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Infrastructure.Repositories;

public sealed class EfUserReadRepository(StainerDbContext dbContext) : IUserReadRepository
{
    public async Task<IReadOnlyList<UserListItemResponse>> ListUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await dbContext.Users
            .AsNoTracking()
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .OrderBy(x => x.Username)
            .ToListAsync(cancellationToken);

        return users
            .Select(x =>
            {
                var roles = x.UserRoles
                    .Where(userRole => userRole.Role is not null)
                    .Select(userRole => userRole.Role!.Code)
                    .OrderBy(code => code)
                    .ToList();

                return new UserListItemResponse(
                    x.Id,
                    x.Username,
                    x.DisplayName,
                    roles.FirstOrDefault(),
                    roles,
                    x.IsEnabled);
            })
            .ToList();
    }

    public async Task<IReadOnlyList<RoleListItemResponse>> ListRolesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .Select(x => new RoleListItemResponse(x.Id, x.Code, x.Name))
            .ToListAsync(cancellationToken);
    }
}
