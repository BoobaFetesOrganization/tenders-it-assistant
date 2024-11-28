using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using MediatR;

namespace GenAIChat.Application.Command.Document
{
    public class DocumentUpdateCommandHandler(IGenAiApiAdapter genAiAdapter, IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<UpdateCommand<DocumentDomain>, DocumentDomain?>
    {
        public async Task<DocumentDomain?> Handle(UpdateCommand<DocumentDomain> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Entity.Name)) throw new Exception("Name should not be empty");
            if (request.Entity.Content.Length == 0) throw new Exception("Content is required");

            var document = await unitOfWork.Document.GetByIdAsync(request.Entity.Id);
            if (document is null) return null;

            var isExisting = (await unitOfWork.Document.GetAllAsync(PaginationOptions.All,
                p => p.ProjectId == request.Entity.ProjectId
                && p.Name.ToLower().Equals(request.Entity.Name.ToLower()))
                ).Any();
            if (isExisting) throw new Exception("Document with the same name already exists for the project");

            document.ProjectId = request.Entity.ProjectId;
            document.Name = request.Entity.Name;
            document.Content = request.Entity.Content;
            document.Metadata = request.Entity.Metadata;

            // upload files to the GenAI and update the docs if successful
            await genAiAdapter.SendFilesAsync(
                [document],
                async doc => await unitOfWork.Document.UpdateAsync(document)
                );

            return document;
        }
    }

}
