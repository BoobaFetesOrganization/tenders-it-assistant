using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Project;
using GenAIChat.Application.Command.Project.Group;
using GenAIChat.Application.Command.Project.Group.UserStory;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class UpdateCommand<TDomain> : IRequest<TDomain?> where TDomain : class, IEntityDomain
    {
        public required TDomain Domain { get; init; }
    }

    public class UpdateCommandHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<UpdateCommand<TDomain>, TDomain?> where TDomain : class, IEntityDomain
    {
        public async Task<TDomain?> Handle(UpdateCommand<TDomain> request, CancellationToken cancellationToken)
        {
            switch (request)
            {
                case UpdateCommand<ProjectDomain> projectCommand:
                    {
                        var handler = new ProjectUpdateCommandHandler((IRepositoryAdapter<ProjectDomain>)repository);
                        var result = (await handler.Handle(projectCommand, cancellationToken)) as TDomain;
                        return result!;
                    }

                case UpdateCommand<UserStoryGroupDomain> userStoryGroupCommand:
                    {
                        var handler = new UserStoryGroupUpdateCommandHandler((IRepositoryAdapter<UserStoryGroupDomain>)repository);
                        var result = (await handler.Handle(userStoryGroupCommand, cancellationToken)) as TDomain;
                        return result!;
                    }

                case UpdateCommand<UserStoryDomain> userStoryCommand:
                    {
                        var handler = new UserStoryUpdateCommandHandler((IRepositoryAdapter<UserStoryDomain>)repository);
                        var result = (await handler.Handle(userStoryCommand, cancellationToken)) as TDomain;
                        return result!;
                    }

                default:
                    return await repository.UpdateAsync((TDomain)request.Domain.Clone());
            }
        }
    }
}
