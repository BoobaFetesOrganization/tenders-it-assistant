using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Common;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group.UserStory
{
    public class UserStoryCreateCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<CreateCommand<UserStoryDomain>, UserStoryDomain>
    {
        public async Task<UserStoryDomain> Handle(CreateCommand<UserStoryDomain> request, CancellationToken cancellationToken)
        {
            var isExisting = (await unitOfWork.UserStory.GetAllAsync(PaginationOptions.All, p => p.Name.ToLower().Equals(request.Entity.Name.ToLower()))).Any();
            if (isExisting) throw new Exception("Name already exists");

            var item = new UserStoryDomain(request.Entity, true);
            await unitOfWork.UserStory.AddAsync(item);

            return item;
        }
    }

}
