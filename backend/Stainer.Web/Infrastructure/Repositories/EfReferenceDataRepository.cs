using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.Repositories;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Infrastructure.Repositories;

public sealed class EfReferenceDataRepository(StainerDbContext dbContext) : IReferenceDataRepository
{
    public async Task<IReadOnlyList<Role>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles.OrderBy(x => x.Code).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Drawer>> GetDrawersAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Drawers
            .Include(x => x.PhysicalSlots)
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<CoordinateProfile?> GetActiveCoordinateProfileAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.CoordinateProfiles
            .Include(x => x.CoordinatePoints)
            .Include(x => x.ActiveVersion)
            .ThenInclude(x => x!.TargetPoints)
            .Where(x => x.IsActive && x.ActiveVersionId != null)
            .OrderBy(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
