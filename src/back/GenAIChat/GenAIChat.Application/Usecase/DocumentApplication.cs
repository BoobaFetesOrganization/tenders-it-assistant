using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Command.Common;
using GenAIChat.Application.Usecase.Interface;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Usecase
{
#pragma warning disable CS9107 // Un paramètre est capturé dans l’état du type englobant et sa valeur est également passée au constructeur de base. La valeur peut également être capturée par la classe de base.
    public class DocumentApplication(IGenAiApiAdapter genAiAdapter, IMediator mediator) : ApplicationBase<DocumentDomain>(mediator), IApplication<DocumentDomain>
#pragma warning restore CS9107 // Un paramètre est capturé dans l’état du type englobant et sa valeur est également passée au constructeur de base. La valeur peut également être capturée par la classe de base.
    {
        public async override Task<DocumentDomain> CreateAsync(DocumentDomain domain, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(domain.Name)) throw new Exception("Name should not be empty");
            if (domain.Content.Length == 0) throw new Exception("Content is required");

            _ = await mediator.Send(new GetByIdQuery<ProjectDomain>() { Id = domain.ProjectId }, cancellationToken)
                ?? throw new Exception("Project not found");

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
            await genAiAdapter.SendFilesAsync([document], cancellationToken);
            await mediator.Send(new CreateCommand<DocumentDomain>() { Domain = document }, cancellationToken);

            return document;
        }

        public async override Task<bool?> UpdateAsync(DocumentDomain domain, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(domain.Name)) throw new Exception("Name should not be empty");
            if (domain.Content.Length == 0) throw new Exception("Content is required");

            // upload files to the GenAI and update the docs if successful
            await genAiAdapter.SendFilesAsync([domain], cancellationToken);

            return await mediator.Send(new UpdateCommand<DocumentDomain>() { Domain = domain }, cancellationToken);
        }
    }
}
