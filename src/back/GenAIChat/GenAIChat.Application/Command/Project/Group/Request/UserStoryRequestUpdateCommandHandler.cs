using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.UserStory
{
    public class UserStoryRequestUpdateCommandHandler(IMediator mediator, IRepositoryAdapter<UserStoryRequestDomain> repository) : IRequestHandler<UpdateCommand<UserStoryRequestDomain>, UserStoryRequestDomain>
    {
        public async Task<UserStoryRequestDomain> Handle(UpdateCommand<UserStoryRequestDomain> request, CancellationToken cancellationToken)
        {
            var item = await mediator.Send(new GetByIdQuery<UserStoryRequestDomain> { Id = request.Domain.Id }, cancellationToken);
            if (item is null) throw new Exception($"{nameof(UserStoryRequestDomain)} '{request.Domain.Id}' is not found ");

            item.Context = request.Domain.Context;
            item.Personas = request.Domain.Personas;
            item.Tasks = request.Domain.Tasks;
            item.GroupId = request.Domain.GroupId;

            return await repository.UpdateAsync(request.Domain);
        }
    }
}
