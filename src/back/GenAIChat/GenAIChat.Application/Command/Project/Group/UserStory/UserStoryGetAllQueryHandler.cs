using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group.UserStory
{
    public class UserStoryGetAllQueryHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<GetAllQuery<UserStoryDomain>, IEnumerable<UserStoryDomain>>
    {
        public async Task<IEnumerable<UserStoryDomain>> Handle(GetAllQuery<UserStoryDomain> request, CancellationToken cancellationToken)
            => await unitOfWork.UserStory.GetAllAsync(request.Options, request.Filter);
    }
}
