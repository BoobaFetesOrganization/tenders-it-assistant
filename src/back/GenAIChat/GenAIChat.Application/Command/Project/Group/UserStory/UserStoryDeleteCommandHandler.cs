using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.UserStory
{
    public class UserStoryDeleteCommandHandler(IMediator mediator, IRepositoryAdapter<UserStoryDomain> repository) : IRequestHandler<DeleteCommand<UserStoryDomain>, UserStoryDomain?>
    {
        public async Task<UserStoryDomain?> Handle(DeleteCommand<UserStoryDomain> request, CancellationToken cancellationToken)
        {
            if (request.Domain is null) return null;

            // cascading deletion
            List<Task> requests = new();

            requests.AddRange(
                mediator.Send(new DeleteCommand<UserStoryRequestDomain> { Domain = request.Domain.Request }, cancellationToken));

            requests.AddRange(
                request.Domain.UserStories
                .Select(item => mediator.Send(new DeleteCommand<UserStoryDomain> { Domain = item }, cancellationToken)));


            // wait for all requests to finish
            await Task.WhenAll(requests);
            return await repository.DeleteAsync(request.Domain);
        }
    }

}
