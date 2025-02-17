using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectDeleteCommandHandler(IRepositoryAdapter<ProjectDomain> projectRepository) : IRequestHandler<DeleteCommand<ProjectDomain>, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(DeleteCommand<ProjectDomain> request, CancellationToken cancellationToken)
        {
            var item = await projectRepository.GetByIdAsync(request.Id);

            if (item is not null) await projectRepository.DeleteAsync(item);

            return item;
        }
    }

}
