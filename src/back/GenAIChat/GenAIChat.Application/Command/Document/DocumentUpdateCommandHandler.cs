using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Document;
using MediatR;

namespace GenAIChat.Application.Command.Document
{
    public class DocumentUpdateCommandHandler(IGenAiApiAdapter genAiAdapter, IRepositoryAdapter<DocumentDomain> documentRepository) : IRequestHandler<UpdateCommand<DocumentDomain>, DocumentDomain?>
    {
        public async Task<DocumentDomain?> Handle(UpdateCommand<DocumentDomain> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Entity.Name)) throw new Exception("Name should not be empty");
            if (request.Entity.Content.Length == 0) throw new Exception("Content is required");

            var document = await documentRepository.GetByIdAsync(request.Entity.Id);
            if (document is null) return null;

            document.ProjectId = request.Entity.ProjectId;
            document.Name = request.Entity.Name;
            document.Content = request.Entity.Content;
            document.Metadata = request.Entity.Metadata;

            // upload files to the GenAI and update the docs if successful
            await genAiAdapter.SendFilesAsync(
                [document],
                async doc => await documentRepository.UpdateAsync(document)
                );

            return document;
        }
    }

}
