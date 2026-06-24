using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Repositories;

namespace Stainer.Web.Application.Services;

public sealed class EngineeringQueryService(IEngineeringReadRepository repository)
{
    public Task<EngineeringLayoutResponse> GetLayoutAsync(CancellationToken cancellationToken = default)
    {
        return repository.GetLayoutAsync(cancellationToken);
    }

    public Task<IReadOnlyList<CoordinateProfileResponse>> ListCoordinateProfilesAsync(CancellationToken cancellationToken = default)
    {
        return repository.ListCoordinateProfilesAsync(cancellationToken);
    }

    public Task<IReadOnlyList<LiquidClassResponse>> ListLiquidClassesAsync(CancellationToken cancellationToken = default)
    {
        return repository.ListLiquidClassesAsync(cancellationToken);
    }
}
