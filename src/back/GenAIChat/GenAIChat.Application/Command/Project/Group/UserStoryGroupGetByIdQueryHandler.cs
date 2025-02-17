using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupGetByIdQueryHandler(IRepositoryAdapter<UserStoryGroupDomain> userStoryGroupRepository) : IRequestHandler<GetByIdQuery<UserStoryGroupDomain>, UserStoryGroupDomain?>
    {
        public async Task<UserStoryGroupDomain?> Handle(GetByIdQuery<UserStoryGroupDomain> request, CancellationToken cancellationToken)
            => await userStoryGroupRepository.GetByIdAsync(request.Id);
    }
}
