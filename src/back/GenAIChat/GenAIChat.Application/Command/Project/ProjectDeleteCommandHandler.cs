using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectDeleteCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<DeleteCommand<ProjectDomain>, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(DeleteCommand<ProjectDomain> request, CancellationToken cancellationToken)
        {
            var item = await unitOfWork.Project.GetByIdAsync(request.Id);

            if (item is not null) await unitOfWork.Project.DeleteAsync(item);

            return item;
        }
    }

}
