using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupCountQueryHandler(IRepositoryAdapter<UserStoryGroupDomain> userStoryGroupRepository) : IRequestHandler<CountQuery<UserStoryGroupDomain>, int>
    {
        public async Task<int> Handle(CountQuery<UserStoryGroupDomain> request, CancellationToken cancellationToken)
            => await userStoryGroupRepository.CountAsync(request.Filter);
    }
}
