using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
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

            ResetTaskCost(project.SelectedGroup);
            project.SelectedGroup = group;
            OverrideWithDefaultCost(project.SelectedGroup);

            return group;
        }
        void ResetTaskCost(UserStoryGroupDomain? item)
        {
            if (item is null) return;
            foreach (var story in item.UserStories)
                foreach (var task in story.Tasks)
                    task.Cost = 0;
        }
        void OverrideWithDefaultCost(UserStoryGroupDomain? item)
        {
            if (item is null) return;
            foreach (var story in item.UserStories)
                foreach (var task in story.Tasks)
                {
                    // si on accepte par default la valeur de l'IA
                    var gemini = task.WorkingCosts.FirstOrDefault(cost => cost.Kind == TaskCostKind.Gemini);
                    if (task.Cost < 1 && gemini != null)
                        task.Cost = gemini.Cost;
                }
        }
    }

}
