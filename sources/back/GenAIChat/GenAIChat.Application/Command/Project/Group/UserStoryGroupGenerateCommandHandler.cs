using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupGenerateCommand : IRequest<UserStoryGroupDomain>
    {
        public required UserStoryGroupDomain Entity { get; init; }
    }

    public class UserStoryGroupGenerateCommandHandler(IGenAiApiAdapter genAiAdapter, IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<UserStoryGroupGenerateCommand, UserStoryGroupDomain>
    {
        public async Task<UserStoryGroupDomain> Handle(UserStoryGroupGenerateCommand request, CancellationToken cancellationToken)
        {
            // upload files to the GenAI and store new Metadata
            IEnumerable<DocumentDomain> documents = await unitOfWork.Document.GetAllAsync(PaginationOptions.All, document => document.ProjectId == request.Entity.ProjectId);
            var expiredDocuments = documents.Where(d => d.Metadata.ExpirationTime < DateTime.Now);
            await genAiAdapter.SendFilesAsync(
                expiredDocuments,
                async doc => await unitOfWork.Document.UpdateAsync(doc)
                );

            var updateActions = expiredDocuments.Select(doc => unitOfWork.Document.UpdateAsync(doc));
            await Task.WhenAll(updateActions);

            // send prompt to the GenAI
            var response = await genAiAdapter.SendRequestAsync(request.Entity.Request.ToGenAIRequest(), documents);

            request.Entity.Response = response;
            //todo =>  group.LoadFromResponse();


            // load user stories from the GenAI result
            return request.Entity;
        }
    }

}
