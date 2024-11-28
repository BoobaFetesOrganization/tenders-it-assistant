using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectUpdateCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<UpdateCommand<ProjectDomain>, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(UpdateCommand<ProjectDomain> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Entity.Name)) throw new Exception("Name is required");

            var item = await unitOfWork.Project.GetByIdAsync(request.Entity.Id);
            if (item is null) return null;

            var isExisting = (await unitOfWork.Project.GetAllAsync(PaginationOptions.All, p => p.Name.ToLower().Equals(request.Entity.Name.ToLower()))).Any();
            if (isExisting) throw new Exception("Name already exists");

            item.Name = request.Entity.Name;

            await unitOfWork.Project.UpdateAsync(item);

            return item;
        }
    }

}
