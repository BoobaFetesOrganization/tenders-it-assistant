using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Common;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using MediatR;

namespace GenAIChat.Application.Command.Document
{
    public class DocumentCreateCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork, IGenAiApiAdapter genAiAdapter) : IRequestHandler<CreateCommand<DocumentDomain>, DocumentDomain>
    {
        public async Task<DocumentDomain> Handle(CreateCommand<DocumentDomain> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Entity.Name)) throw new Exception("Name should not be empty");
            if (request.Entity.Content.Length == 0) throw new Exception("Content is required");

            var project = await unitOfWork.Project.GetByIdAsync(request.Entity.ProjectId)
                ?? throw new Exception("Project not found");

            var isExisting = (await unitOfWork.Document.GetAllAsync(PaginationOptions.All,
                p => p.ProjectId == request.Entity.ProjectId
                && p.Name.ToLower().Equals(request.Entity.Name.ToLower()))
                ).Any();
            if (isExisting) throw new Exception("Name already exists");

            DocumentDomain document = new(request.Entity.Name, request.Entity.Metadata.MimeType, request.Entity.Metadata.Length, request.Entity.Content, request.Entity.ProjectId);

            // upload files to the GenAI and add the doc if successful
            await genAiAdapter.SendFilesAsync(
                [document],
                async doc => await unitOfWork.Document.AddAsync(document)
                );

            return document;
        }
    }

}
