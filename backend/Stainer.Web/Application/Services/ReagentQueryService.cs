using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Repositories;

namespace Stainer.Web.Application.Services;

public sealed class ReagentQueryService(IReagentReadRepository repository)
{
    public Task<IReadOnlyList<ReagentCatalogItemResponse>> ListCatalogAsync(CancellationToken cancellationToken = default)
    {
        return repository.ListCatalogAsync(cancellationToken);
    }

    public Task<IReadOnlyList<ReagentRackPositionResponse>> ListRackAsync(CancellationToken cancellationToken = default)
    {
        return repository.ListRackAsync(cancellationToken);
    }

    public Task<ReagentScanSessionOverviewResponse> GetScanSessionOverviewAsync(CancellationToken cancellationToken = default)
    {
        return repository.GetScanSessionOverviewAsync(cancellationToken);
    }
}
