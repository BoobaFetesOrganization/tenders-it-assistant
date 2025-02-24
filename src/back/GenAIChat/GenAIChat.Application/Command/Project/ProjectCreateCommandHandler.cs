using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectCreateCommandHandler(IRepositoryAdapter<ProjectDomain> projectRepository) : IRequestHandler<CreateCommand<ProjectDomain>, ProjectDomain>
    {
        public async Task<ProjectDomain> Handle(CreateCommand<ProjectDomain> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Entity.Name)) throw new Exception("Name is required");

            var filter = new PropertyEqualsFilter(nameof(ProjectDomain.Name), request.Entity.Name);
            var exists = (await projectRepository.GetAllAsync()).Any(p => p.Name.ToLower().Equals(request.Entity.Name.ToLower()));
            if (exists) throw new Exception("Name already exists");

            var item = await projectRepository.AddAsync(request.Entity);
            return item;
        }
    }

}
