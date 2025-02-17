using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Document;
using MediatR;

namespace GenAIChat.Application.Command.Document
{
    public class DocumentGetByIdQueryHandler(IRepositoryAdapter<DocumentDomain> documentRepository) : IRequestHandler<GetByIdQuery<DocumentDomain>, DocumentDomain?>
    {
        public async Task<DocumentDomain?> Handle(GetByIdQuery<DocumentDomain> request, CancellationToken cancellationToken)
            => await documentRepository.GetByIdAsync(request.Id);
    }
}
