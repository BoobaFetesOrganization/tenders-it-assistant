using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Application.Common;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using MediatR;

namespace GenAIChat.Application.Command.Document
{
    public class CreateDocumentCommandHandler(IGenAiApiAdapter genAiAdapter, IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<CreateCommand<DocumentDomain>, DocumentDomain>
    {
        public async Task<DocumentDomain> Handle(CreateCommand<DocumentDomain> request, CancellationToken cancellationToken)
        {
            var project = await unitOfWork.Projects.GetByIdAsync(request.Entity.ProjectId) 
                ?? throw new Exception("Project not found");

            var isExisting = (await unitOfWork.Documents.GetAllAsync(PaginationOptions.All,
                p => p.ProjectId == request.Entity.ProjectId
                && p.Name.ToLower().Equals(request.Entity.Name.ToLower()))
                ).Any();
            if (isExisting) throw new Exception("Document with the same name already exists for the project");

            var document = new DocumentDomain(request.Entity.Name, request.Entity.Metadata.MimeType, request.Entity.Metadata.Length, request.Entity.Content, request.Entity.ProjectId);

            // upload files to the GenAI
            await genAiAdapter.SendFilesAsync([document]);

            await unitOfWork.Documents.AddAsync(document);

            return document;
        }
    }

}
