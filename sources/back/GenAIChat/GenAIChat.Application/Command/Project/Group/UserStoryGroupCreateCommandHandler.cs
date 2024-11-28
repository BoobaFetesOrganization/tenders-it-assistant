using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Common;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupCreateCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<CreateCommand<UserStoryGroupDomain>, UserStoryGroupDomain>
    {
        public async Task<UserStoryGroupDomain> Handle(CreateCommand<UserStoryGroupDomain> request, CancellationToken cancellationToken)
        {
            var item = new UserStoryGroupDomain()
            {
                Prompt = request.Entity.Prompt,
                PromptResponse = request.Entity.PromptResponse,
                ProjectId = request.Entity.ProjectId,
                UserStories = request.Entity.UserStories
            };

            await unitOfWork.UserStoryGroup.AddAsync(item);

            return item;
        }
    }

}
