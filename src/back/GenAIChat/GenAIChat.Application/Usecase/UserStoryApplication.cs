using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Usecase.Common;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class UserStoryApplication(IMediator mediator, IGenAiUnitOfWorkAdapter unitOfWork) : ApplicationBase<UserStoryDomain>(mediator, unitOfWork)
    {
    }
}
