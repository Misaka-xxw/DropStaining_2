using Stainer.Web.Application.ReadModels;

namespace Stainer.Web.Application.Repositories;

public interface IEngineeringReadRepository
{
    Task<EngineeringLayoutResponse> GetLayoutAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CoordinateProfileResponse>> ListCoordinateProfilesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LiquidClassResponse>> ListLiquidClassesAsync(CancellationToken cancellationToken = default);
}
