using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group.UserStory
{
    public class UserStoryCreateCommandHandler(IRepositoryAdapter<UserStoryDomain> repository) : IRequestHandler<CreateCommand<UserStoryDomain>, UserStoryDomain>
    {
        public async Task<UserStoryDomain> Handle(CreateCommand<UserStoryDomain> request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrWhiteSpace(request.Domain.Name)) throw new Exception("Name is required");

            var filter = new PropertyEqualsFilter(nameof(UserStoryDomain.Name), request.Domain.Name);
            var sameNames = await repository.GetAllAsync(filter);
            if (sameNames.Any()) throw new Exception("Name already exists");

            await repository.AddAsync((UserStoryDomain)request.Domain.Clone());

            return await repository.GetByIdAsync(request.Domain.Id) ?? throw new Exception($"{nameof(UserStoryDomain)} '{request.Domain.Id}' entity created but not found in database !");
        }
    }

}
