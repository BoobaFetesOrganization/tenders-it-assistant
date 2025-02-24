using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Project;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project;
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
            if (request is CreateCommand<ProjectDomain> projectCommand)
            {
                var handler = new ProjectCreateCommandHandler((IRepositoryAdapter<ProjectDomain>)repository);
                var result = await handler.Handle(projectCommand, cancellationToken);
                return (TDomain)(object)result;
            }
            //if (request.Entity is ProjectDomain project)
            //{
            //    var handler = new ProjectCreateCommandHandler((IRepositoryAdapter<ProjectDomain>)repository);
            //    var result = await handler.Handle(new CreateCommand<ProjectDomain> { Entity = project }, cancellationToken);
            //    return (TDomain)(object)result;
            //}
            else { return await repository.AddAsync((TDomain)request.Domain.Clone()); }
        }
    }
}
