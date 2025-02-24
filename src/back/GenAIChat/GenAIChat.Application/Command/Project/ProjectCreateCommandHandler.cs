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
            if (string.IsNullOrEmpty(request.Domain.Name)) throw new Exception("Name is required");

            var filter = new PropertyEqualsFilter(nameof(ProjectDomain.Name), request.Domain.Name);
            var sameProjectNames = await projectRepository.GetAllAsync2(filter);
            if (sameProjectNames.Any()) throw new Exception("Name already exists");

            return await projectRepository.AddAsync(request.Domain);
        }
    }

}
