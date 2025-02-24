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
            if (string.IsNullOrEmpty(request.Domain.Name)) throw new Exception("Name should not be empty");
            if (request.Domain.Content.Length == 0) throw new Exception("Content is required");

            var document = await documentRepository.GetByIdAsync(request.Domain.Id);
            if (document is null) return null;

            document.ProjectId = request.Domain.ProjectId;
            document.Name = request.Domain.Name;
            document.Content = request.Domain.Content;
            document.Metadata = request.Domain.Metadata;

            // upload files to the GenAI and update the docs if successful
            await genAiAdapter.SendFilesAsync(
                [document],
                async doc => await documentRepository.UpdateAsync(document)
                );

            return document;
        }
    }

}
