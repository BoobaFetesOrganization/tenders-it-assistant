using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group.UserStory
{
    public class UserStoryGetByIdQueryHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<GetByIdQuery<UserStoryDomain>, UserStoryDomain?>
    {
        public async Task<UserStoryDomain?> Handle(GetByIdQuery<UserStoryDomain> request, CancellationToken cancellationToken)
            => await unitOfWork.UserStory.GetByIdAsync(request.Id);
    }
}
