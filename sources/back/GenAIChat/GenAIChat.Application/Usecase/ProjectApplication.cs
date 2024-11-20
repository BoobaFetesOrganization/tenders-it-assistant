using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Usecase.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class ProjectApplication(IMediator mediator, IGenAiUnitOfWorkAdapter unitOfWork) : ApplicationBase<ProjectDomain>(mediator, unitOfWork)
    {
        public async Task<ProjectDomain> CreateAsync(string name, string prompt, IEnumerable<DocumentDomain>? documents = null)
            => await CreateAsync(new ProjectDomain(name, prompt, documents));
    }
}
