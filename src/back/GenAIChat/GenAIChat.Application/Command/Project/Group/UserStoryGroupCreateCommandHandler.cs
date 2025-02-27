using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Application.Resources;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupCreateCommandHandler(IMediator mediator, IRepositoryAdapter<UserStoryGroupDomain> repository, EmbeddedResource resources) : IRequestHandler<CreateCommand<UserStoryGroupDomain>, UserStoryGroupDomain?>
    {
        public async Task<UserStoryGroupDomain?> Handle(CreateCommand<UserStoryGroupDomain> request, CancellationToken cancellationToken)
        {
            var domain = await repository.AddAsync(new()
            {
                ProjectId = request.Domain.ProjectId,
                /* c'est à faire dans la couche infra */ Request = (UserStoryRequestDomain)resources.UserStoryRequest.Clone()
            });

            try
            {
                var item = await mediator.Send(new UserStoryGroupGenerateCommand { ProjectId = domain.ProjectId, GroupId = domain.Id }, cancellationToken);
            }
            catch (Exception ex)
            {
                // silent catch, because it's not required to get the generation of user stories done right now..
                Console.WriteLine(ex.Message);
            }

            return domain;
        }
    }
}
