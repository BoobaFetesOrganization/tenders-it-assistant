using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Document
{
    public class DocumentCreateCommandHandler(IRepositoryAdapter<ProjectDomain> projectRepository, IRepositoryAdapter<DocumentDomain> documentRepository, IGenAiApiAdapter genAiAdapter) : IRequestHandler<CreateCommand<DocumentDomain>, DocumentDomain>
    {
        public async Task<DocumentDomain> Handle(CreateCommand<DocumentDomain> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Entity.Name)) throw new Exception("Name should not be empty");
            if (request.Entity.Content.Length == 0) throw new Exception("Content is required");

            var project = await projectRepository.GetByIdAsync(request.Entity.ProjectId)
                ?? throw new Exception("Project not found");

            var isExisting = (await documentRepository.GetAllAsync(p => p.ProjectId == request.Entity.ProjectId))
                .Where(p => p.Name != null && p.Name.ToLower().Equals(request.Entity.Name.ToLower()))
                .Any();
            if (isExisting) throw new Exception("Name already exists");

            DocumentDomain document = new()
            {
                Name = request.Entity.Name,
                Metadata = new()
                {
                    MimeType = request.Entity.Metadata.MimeType,
                    Length = request.Entity.Metadata.Length
                },
                Content = request.Entity.Content,
                ProjectId = request.Entity.ProjectId
            };

            // upload files to the GenAI and add the doc if successful
            await genAiAdapter.SendFilesAsync(
                [document],
                async doc => await documentRepository.AddAsync(document)
                );

            return document;
        }
    }

}
