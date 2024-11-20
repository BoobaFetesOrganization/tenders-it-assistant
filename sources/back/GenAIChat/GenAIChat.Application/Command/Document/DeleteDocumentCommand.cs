using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Document;
using MediatR;

namespace GenAIChat.Application.Command.Document
{
    public class DeleteDocumentCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<DeleteCommand<DocumentDomain>, DocumentDomain?>
    {
        public async Task<DocumentDomain?> Handle(DeleteCommand<DocumentDomain> request, CancellationToken cancellationToken)
        {
            var document = await unitOfWork.Documents.GetByIdAsync(request.Id);

            if (document is not null) await unitOfWork.Documents.DeleteAsync(document);

            return document;
        }
    }

}
