using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectUpdateCommandHandler(IRepositoryAdapter<ProjectDomain> projectRepository) : IRequestHandler<UpdateCommand<ProjectDomain>, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(UpdateCommand<ProjectDomain> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Entity.Name)) throw new Exception("Name is required");

            var item = await projectRepository.GetByIdAsync(request.Entity.Id);
            if (item is null) return null;

            var isExisting = (await projectRepository.GetAllAsync()).Any(p => p.Id != item.Id && p.Name.ToLower().Equals(request.Entity.Name.ToLower()));
            if (isExisting) throw new Exception("Name already exists");

            item.Name = request.Entity.Name;

            await projectRepository.UpdateAsync(item);

            return item;
        }
    }

}
