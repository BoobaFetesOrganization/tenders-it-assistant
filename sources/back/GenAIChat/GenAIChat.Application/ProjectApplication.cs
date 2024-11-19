using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Project;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using MediatR;
using System.Linq.Expressions;

namespace GenAIChat.Application
{
    public class ProjectApplication(IMediator mediator, IGenAiUnitOfWorkAdapter unitOfWork)
    {
        public async Task<Paged<ProjectDomain>> GetAllAsync(PaginationOptions options, Expression<Func<ProjectDomain, bool>>? filter = null)
        {
            var data = await mediator.Send(new GetAllProjectsQuery(options, filter));
            return new Paged<ProjectDomain>(options, data);
        }

        public async Task<ProjectDomain?> GetByIdAsync(int id)
        {
            return await mediator.Send(new GetProjectByIdQuery(id));
        }

        public async Task<ProjectDomain> CreateAsync(string name, string prompt, IEnumerable<DocumentDomain>? documents = null)
        {
            var result = await mediator.Send(new CreateProjectCommand(name, prompt, documents));
            await unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<ProjectDomain?> UpdateAsync(ProjectDomain project)
        {
            var result = await mediator.Send(new UpdateProjectCommand(project));
            await unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<ProjectDomain?> DeleteAsync(int id)
        {
            var result = await mediator.Send(new DeleteProjectCommand(id));
            await unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
