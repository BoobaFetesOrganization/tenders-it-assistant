using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Project;
using GenAIChat.Application.Command.Project.Group.UserStory;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class CreateCommand<TDomain> : IRequest<TDomain> where TDomain : class, IEntityDomain, ICloneable
    {
        public required TDomain Domain { get; init; }
    }

    public class GetCreateCommandHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<CreateCommand<TDomain>, TDomain> where TDomain : class, IEntityDomain, ICloneable
    {
        public async Task<TDomain> Handle(CreateCommand<TDomain> request, CancellationToken cancellationToken)
        {
            switch (request)
            {
                case CreateCommand<ProjectDomain> projectCommand:
                    {
                        var handler = new ProjectCreateCommandHandler((IRepositoryAdapter<ProjectDomain>)repository);
                        var result = (await handler.Handle(projectCommand, cancellationToken)) as TDomain;
                        return result!;
                    }

                case CreateCommand<UserStoryDomain> userStoryCommand:
                    {
                        var handler = new UserStoryCreateCommandHandler((IRepositoryAdapter<UserStoryDomain>)repository);
                        var result = (await handler.Handle(userStoryCommand, cancellationToken)) as TDomain;
                        return result!;
                    }

                default:
                    return await repository.AddAsync(request.Domain);
            }
        }
    }
}
