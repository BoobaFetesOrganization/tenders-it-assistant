using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group.UserStory
{
    public class UserStoryCreateCommandHandler(IRepositoryAdapter<UserStoryDomain> userStoryRepository) : IRequestHandler<CreateCommand<UserStoryDomain>, UserStoryDomain>
    {
        public async Task<UserStoryDomain> Handle(CreateCommand<UserStoryDomain> request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrWhiteSpace(request.Domain.Name)) throw new Exception("Name is required");

            var isExisting = (await userStoryRepository.GetAllAsync()).Any(p => p.Name.ToLower().Equals(request.Domain.Name.ToLower()));
            if (isExisting) throw new Exception("Name already exists");

            await userStoryRepository.AddAsync((UserStoryDomain)request.Domain.Clone());

            return await userStoryRepository.GetByIdAsync(request.Domain.Id) ?? throw new Exception("UserStory entity created but not found in database !");
        }
    }

}
