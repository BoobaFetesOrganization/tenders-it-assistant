using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Document;
using MediatR;

namespace GenAIChat.Application.Command.Document
{
    public class DocumentDeleteCommandHandler(IRepositoryAdapter<DocumentDomain> documentRepository) : IRequestHandler<DeleteCommand<DocumentDomain>, DocumentDomain?>
    {
        public async Task<DocumentDomain?> Handle(DeleteCommand<DocumentDomain> request, CancellationToken cancellationToken)
        {
            var document = await documentRepository.GetByIdAsync(request.Id);

            if (document is not null) await documentRepository.DeleteAsync(document);

            return document;
        }
    }

}
