using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupValidateCommand : IRequest<UserStoryGroupDomain>
    {
        public required string ProjectId { get; init; }
        public required string GroupId { get; init; }
    }

    public class UserStoryGroupValidateCommandHandler(IRepositoryAdapter<ProjectDomain> projectRepository, IRepositoryAdapter<UserStoryGroupDomain> userStoryGroupRepository) : IRequestHandler<UserStoryGroupValidateCommand, UserStoryGroupDomain>
    {
        public async Task<UserStoryGroupDomain> Handle(UserStoryGroupValidateCommand request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByIdAsync(request.ProjectId)
                ?? throw new Exception("Project not found");
            var group = await userStoryGroupRepository.GetByIdAsync(request.GroupId)
                ?? throw new Exception("Group not found");

            ResetTaskCost(project.SelectedGroup);
            project.SelectedGroup = group;
            OverrideWithDefaultCost(project.SelectedGroup);

            await projectRepository.UpdateAsync(project);

            return group;
        }

        private static void ResetTaskCost(UserStoryGroupDomain? item)
        {
            if (item is null) return;
            foreach (var story in item.UserStories)
                foreach (var task in story.Tasks)
                    task.Cost = 0;
        }
        private static void OverrideWithDefaultCost(UserStoryGroupDomain? item)
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
