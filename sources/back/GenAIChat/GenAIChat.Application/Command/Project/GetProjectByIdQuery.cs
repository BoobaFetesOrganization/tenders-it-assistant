using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class GetProjectByIdQueryHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<GetByIdQuery<ProjectDomain>, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(GetByIdQuery<ProjectDomain> request, CancellationToken cancellationToken)
            => await unitOfWork.Projects.GetByIdAsync(request.Id);
    }
}
