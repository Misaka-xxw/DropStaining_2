using Stainer.Web.Application.ReadModels;

namespace Stainer.Web.Application.Services;

public sealed class BusinessSelectionRequiredException(TaskCreationResponse response)
    : Exception(response.Message)
{
    public TaskCreationResponse Response { get; } = response;
}
