using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class DocumentApplication : ApplicationBase<DocumentDomain>, IApplication<DocumentDomain>
    {
        private readonly IGenAiApiAdapter genAiAdapter;
        public DocumentApplication(IGenAiApiAdapter genAiAdapter, IMediator mediator) : base(mediator)
        {
            this.genAiAdapter = genAiAdapter;
        }

        public async override Task<DocumentDomain> CreateAsync(DocumentDomain domain, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(domain.Name)) throw new Exception("Name should not be empty");
            if (domain.Content.Length == 0) throw new Exception("Content is required");

            var project = await mediator.Send(new GetByIdQuery<ProjectDomain>() { Id = domain.ProjectId }, cancellationToken)
                ?? throw new Exception("Project not found");

            var filter = new AndFilter(
                new PropertyEqualsFilter(nameof(DocumentDomain.ProjectId), domain.ProjectId),
                new PropertyEqualsFilter(nameof(DocumentDomain.Name), domain.Name)
                );
            var sameNames = await mediator.Send(new GetAllQuery<ProjectDomain>() { Filter = filter }, cancellationToken);
            if (sameNames.Any()) throw new Exception("Name already exists");


            DocumentDomain document = new()
            {
                Name = domain.Name,
                Metadata = new()
                {
                    MimeType = domain.Metadata.MimeType,
                    Length = domain.Metadata.Length
                },
                Content = domain.Content,
                ProjectId = domain.ProjectId
            };

            // upload files to the GenAI and add the doc if successful
            var documents = await genAiAdapter.SendFilesAsync([document], cancellationToken);
            await mediator.Send(new CreateCommand<DocumentDomain>() { Domain = document }, cancellationToken);

            return document;
        }

        public async override Task<bool?> UpdateAsync(DocumentDomain domain, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(domain.Name)) throw new Exception("Name should not be empty");
            if (domain.Content.Length == 0) throw new Exception("Content is required");

            // upload files to the GenAI and update the docs if successful
            await genAiAdapter.SendFilesAsync([domain], cancellationToken);

            return await mediator.Send(new UpdateCommand<DocumentDomain>() { Domain = domain }, cancellationToken);
        }
    }
}
