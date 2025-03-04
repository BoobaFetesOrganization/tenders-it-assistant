using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Command.Common;
using GenAIChat.Application.Usecase.Interface;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class DocumentApplication(IGenAiApiAdapter genAiAdapter, IMediator mediator) : ApplicationBase<DocumentDomain>(mediator), IApplication<DocumentDomain>
    {
        public async override Task<DocumentDomain> CreateAsync(DocumentDomain domain, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(domain.Name)) throw new Exception("Name should not be empty");
            if (domain.Content.Length == 0) throw new Exception("Content is required");

            var projectExists = await mediator.Send(new GetAllQuery<ProjectDomain>() { Filter = new PropertyEqualsFilter(nameof(ProjectDomain.Id), domain.ProjectId) }, cancellationToken);
            if (!projectExists.Any()) throw new Exception("Project not found");

            // upload files to the GenAI and add the doc if successful
            await genAiAdapter.SendFilesAsync([domain], cancellationToken);

            var result = await mediator.Send(new CreateCommand<DocumentDomain>() { Domain = domain }, cancellationToken);
            return result;
        }

        public async override Task<bool> UpdateAsync(DocumentDomain domain, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(domain.Name)) throw new Exception("Name should not be empty");
            if (domain.Content.Length == 0) throw new Exception("Content is required");

            // upload files to the GenAI and update the docs if successful
            await genAiAdapter.SendFilesAsync([domain], cancellationToken);

            return await mediator.Send(new UpdateCommand<DocumentDomain>() { Domain = domain }, cancellationToken);
        }
    }
}
