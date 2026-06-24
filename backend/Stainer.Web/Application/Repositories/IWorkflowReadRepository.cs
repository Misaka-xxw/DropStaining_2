using Stainer.Web.Application.ReadModels;

namespace Stainer.Web.Application.Repositories;

public interface IWorkflowReadRepository
{
    Task<IReadOnlyList<WorkflowSummaryResponse>> ListWorkflowsAsync(CancellationToken cancellationToken = default);
    Task<WorkflowDetailResponse?> GetWorkflowAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProtocolCompatResponse>> ListProtocolCompatAsync(CancellationToken cancellationToken = default);
}
