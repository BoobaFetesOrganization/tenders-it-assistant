using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.UserStory
{
    public class UserStoryRequestDeleteCommandHandler(IRepositoryAdapter<UserStoryRequestDomain> repository) : IRequestHandler<DeleteCommand<UserStoryRequestDomain>, UserStoryRequestDomain?>
    {
        public async Task<UserStoryRequestDomain?> Handle(DeleteCommand<UserStoryRequestDomain> request, CancellationToken cancellationToken)
        {
            if (request.Domain is null) return null;
                       
            return await repository.DeleteAsync(request.Domain);
        }
    }

}
