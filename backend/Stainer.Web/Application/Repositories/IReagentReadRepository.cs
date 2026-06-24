using Stainer.Web.Application.ReadModels;

namespace Stainer.Web.Application.Repositories;

public interface IReagentReadRepository
{
    Task<IReadOnlyList<ReagentCatalogItemResponse>> ListCatalogAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ReagentRackPositionResponse>> ListRackAsync(CancellationToken cancellationToken = default);
}
