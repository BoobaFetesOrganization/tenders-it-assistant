using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Project
{
    public class GetProjectByIdQuery : IRequest<ProjectDomain?>
    {
        public readonly int Id;

        public GetProjectByIdQuery(int id) => Id = id;
    }

    public class GetProjectByIdQueryHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<GetProjectByIdQuery, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Projects.GetByIdAsync(request.Id);
        }
    }
}
