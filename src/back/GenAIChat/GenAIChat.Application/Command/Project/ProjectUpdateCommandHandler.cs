using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectUpdateCommandHandler(IRepositoryAdapter<ProjectDomain> repository) : IRequestHandler<UpdateCommand<ProjectDomain>, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(UpdateCommand<ProjectDomain> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Domain.Name)) throw new Exception("Name is required");

            var item = await repository.GetByIdAsync(request.Domain.Id);
            if (item is null) return null;

            var filter = new AndFilter(
                new PropertyEqualsFilter(nameof(ProjectDomain.Name), request.Domain.Name),
                new PropertyEqualsFilter(nameof(ProjectDomain.Id), request.Domain.Id, false));
            var sameNames = await repository.GetAllAsync(filter);
            if (sameNames.Any()) throw new Exception("Name already exists");

            item.Name = request.Domain.Name;
            item.SelectedGroupId = request.Domain.SelectedGroupId;
            
            return await repository.UpdateAsync(item);
        }
    }
}
