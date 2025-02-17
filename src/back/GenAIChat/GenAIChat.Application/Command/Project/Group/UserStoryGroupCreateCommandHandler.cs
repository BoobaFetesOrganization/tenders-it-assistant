using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupCreateCommandHandler(IRepositoryAdapter<UserStoryGroupDomain> userStoryGroupRepository) : IRequestHandler<CreateCommand<UserStoryGroupDomain>, UserStoryGroupDomain>
    {
        public async Task<UserStoryGroupDomain> Handle(CreateCommand<UserStoryGroupDomain> request, CancellationToken cancellationToken)
        {
            UserStoryGroupDomain item = new(request.Entity) { Id = EntityDomain.NewId() };

            await userStoryGroupRepository.AddAsync(item);

            return item;
        }
    }

}
