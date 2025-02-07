using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Usecase.Common;
using GenAIChat.Domain.Document;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class DocumentApplication(IMediator mediator, IGenAiUnitOfWorkAdapter unitOfWork) : ApplicationBase<DocumentDomain>(mediator, unitOfWork)
    {
    }
}
