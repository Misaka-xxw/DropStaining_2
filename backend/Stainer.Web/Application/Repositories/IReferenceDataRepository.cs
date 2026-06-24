using Stainer.Web.Domain.Entities;

namespace Stainer.Web.Application.Repositories;

public interface IReferenceDataRepository
{
    Task<IReadOnlyList<Role>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Drawer>> GetDrawersAsync(CancellationToken cancellationToken = default);
    Task<CoordinateProfile?> GetActiveCoordinateProfileAsync(CancellationToken cancellationToken = default);
}
