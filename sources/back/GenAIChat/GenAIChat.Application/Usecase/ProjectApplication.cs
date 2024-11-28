using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Usecase.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class ProjectApplication(IMediator mediator, IGenAiUnitOfWorkAdapter unitOfWork) : ApplicationBase<ProjectDomain>(mediator, unitOfWork)
    {
    }
}
