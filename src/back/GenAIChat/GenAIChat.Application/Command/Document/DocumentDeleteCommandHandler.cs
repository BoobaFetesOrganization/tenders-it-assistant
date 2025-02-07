using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Document;
using MediatR;

namespace GenAIChat.Application.Command.Document
{
    public class DocumentDeleteCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<DeleteCommand<DocumentDomain>, DocumentDomain?>
    {
        public async Task<DocumentDomain?> Handle(DeleteCommand<DocumentDomain> request, CancellationToken cancellationToken)
        {
            var document = await unitOfWork.Document.GetByIdAsync(request.Id);

            if (document is not null) await unitOfWork.Document.DeleteAsync(document);

            return document;
        }
    }

}
