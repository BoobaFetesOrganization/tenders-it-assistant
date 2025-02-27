using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.UserStory
{
    public class UserStoryGroupDeleteCommandHandler(IMediator mediator, IRepositoryAdapter<UserStoryGroupDomain> repository) : IRequestHandler<DeleteCommand<UserStoryGroupDomain>, UserStoryGroupDomain?>
    {
        public async Task<UserStoryGroupDomain?> Handle(DeleteCommand<UserStoryGroupDomain> request, CancellationToken cancellationToken)
        {
            if (request.Domain is null) return null;

            // cascading deletion
            /* c'est à faire dans la couche infra */ List<Task> requests = new();
            /* c'est à faire dans la couche infra */ 
            /* c'est à faire dans la couche infra */ requests.AddRange(
            /* c'est à faire dans la couche infra */     mediator.Send(new DeleteCommand<UserStoryRequestDomain> { Domain = request.Domain.Request }, cancellationToken));
            /* c'est à faire dans la couche infra */ 
            /* c'est à faire dans la couche infra */ requests.AddRange(
            /* c'est à faire dans la couche infra */     request.Domain.UserStories
            /* c'est à faire dans la couche infra */     .Select(item => mediator.Send(new DeleteCommand<UserStoryDomain> { Domain = item }, cancellationToken)));


            // wait for all requests to finish
            await Task.WhenAll(requests);
            return await repository.DeleteAsync(request.Domain);
        }
    }

}
