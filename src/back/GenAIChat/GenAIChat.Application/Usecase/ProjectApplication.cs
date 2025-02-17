using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Usecase.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class ProjectApplication(IMediator mediator, IRepositoryAdapter<ProjectDomain> repository) : ApplicationBase<ProjectDomain>(mediator, repository)
    {
    }
}
