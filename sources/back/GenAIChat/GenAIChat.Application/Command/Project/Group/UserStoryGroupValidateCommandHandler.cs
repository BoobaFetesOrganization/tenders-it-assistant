using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupValidateCommand : IRequest<UserStoryGroupDomain>
    {
        public required int ProjectId { get; init; }
        public required int GroupId { get; init; }
    }

    public class UserStoryGroupValidateCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<UserStoryGroupValidateCommand, UserStoryGroupDomain>
    {
        public async Task<UserStoryGroupDomain> Handle(UserStoryGroupValidateCommand request, CancellationToken cancellationToken)
        {
            var project = await unitOfWork.Project.GetByIdAsync(request.ProjectId)
                ?? throw new Exception("Project not found");
            var group = await unitOfWork.UserStoryGroup.GetByIdAsync(request.GroupId) 
                ?? throw new Exception("Group not found");

            project.Stories = group;
            project.Generated.Clear();

            return group;
        }
    }

}
