using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectDeleteCommandHandler(IMediator mediator, IRepositoryAdapter<ProjectDomain> repository) : IRequestHandler<DeleteCommand<ProjectDomain>, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(DeleteCommand<ProjectDomain> request, CancellationToken cancellationToken)
        {
            if (request.Domain is null) return null;

            /* c'est à faire dans la couche infra */ // cascade
            /* c'est à faire dans la couche infra */ List<Task> requests = new();
            /* c'est à faire dans la couche infra */ 
            /* c'est à faire dans la couche infra */ requests.AddRange(
            /* c'est à faire dans la couche infra */     request.Domain.Documents
            /* c'est à faire dans la couche infra */     .Select(item => mediator.Send(new DeleteCommand<DocumentDomain> { Domain = item }, cancellationToken)));
            /* c'est à faire dans la couche infra */ 
            /* c'est à faire dans la couche infra */ requests.AddRange(
            /* c'est à faire dans la couche infra */     request.Domain.Groups
            /* c'est à faire dans la couche infra */     .Select(item => mediator.Send(new DeleteCommand<UserStoryGroupDomain> { Domain = item }, cancellationToken)));


            // wait for all requests to finish
            await Task.WhenAll(requests);

            return await repository.DeleteAsync(request.Domain);
        }
    }
}
