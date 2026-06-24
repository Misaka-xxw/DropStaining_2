using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Repositories;

namespace Stainer.Web.Application.Services;

public sealed class WorkflowQueryService(IWorkflowReadRepository repository)
{
    public Task<IReadOnlyList<WorkflowSummaryResponse>> ListAsync(CancellationToken cancellationToken = default)
    {
        return repository.ListWorkflowsAsync(cancellationToken);
    }

    public Task<WorkflowDetailResponse?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return repository.GetWorkflowAsync(id, cancellationToken);
    }

    public Task<IReadOnlyList<ProtocolCompatResponse>> ListProtocolCompatAsync(CancellationToken cancellationToken = default)
    {
        return repository.ListProtocolCompatAsync(cancellationToken);
    }
}
