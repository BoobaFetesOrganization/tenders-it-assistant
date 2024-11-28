using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupGetAllQueryHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<GetAllQuery<UserStoryGroupDomain>, IEnumerable<UserStoryGroupDomain>>
    {
        public async Task<IEnumerable<UserStoryGroupDomain>> Handle(GetAllQuery<UserStoryGroupDomain> request, CancellationToken cancellationToken)
            => await unitOfWork.UserStoryGroup.GetAllAsync(request.Options, request.Filter);
    }
}
