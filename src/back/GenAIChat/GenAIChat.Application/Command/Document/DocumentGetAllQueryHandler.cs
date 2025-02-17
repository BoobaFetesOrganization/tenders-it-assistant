using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Document;
using MediatR;

namespace GenAIChat.Application.Command.Document
{
    public class DocumentGetAllQueryHandler(IRepositoryAdapter<DocumentDomain> documentRepository) : IRequestHandler<GetAllQuery<DocumentDomain>, IEnumerable<DocumentDomain>>
    {
        public async Task<IEnumerable<DocumentDomain>> Handle(GetAllQuery<DocumentDomain> request, CancellationToken cancellationToken)
            => await documentRepository.GetAllAsync(request.Options, request.Filter);
    }
}
