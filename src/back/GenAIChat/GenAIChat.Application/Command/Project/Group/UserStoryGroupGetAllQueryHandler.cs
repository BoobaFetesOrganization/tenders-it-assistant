using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupGetAllQueryHandler(IRepositoryAdapter<UserStoryGroupDomain> userStoryGroupRepository) : IRequestHandler<GetAllQuery<UserStoryGroupDomain>, IEnumerable<UserStoryGroupDomain>>
    {
        public async Task<IEnumerable<UserStoryGroupDomain>> Handle(GetAllQuery<UserStoryGroupDomain> request, CancellationToken cancellationToken)
            => await userStoryGroupRepository.GetAllAsync(PaginationOptions.All, request.Filter);
    }
}
