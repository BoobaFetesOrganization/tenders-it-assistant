using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectCreateCommandHandler(IRepositoryAdapter<ProjectDomain> repository) : IRequestHandler<CreateCommand<ProjectDomain>, ProjectDomain>
    {
        public async Task<ProjectDomain> Handle(CreateCommand<ProjectDomain> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Domain.Name)) throw new Exception("Name is required");

            var filter = new PropertyEqualsFilter(nameof(ProjectDomain.Name), request.Domain.Name);
            var sameNames = await repository.GetAllAsync(filter);
            if (sameNames.Any()) throw new Exception("Name already exists");

            return await repository.AddAsync(request.Domain);
        }
    }
}
