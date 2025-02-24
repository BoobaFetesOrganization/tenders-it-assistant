using GenAIChat.Application.Command.Common;
using GenAIChat.Application.Command.Project.Group;
using GenAIChat.Application.Resources;
using GenAIChat.Application.Usecase.Common;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class UserStoryGroupApplication(IMediator mediator, ProjectApplication projectApplication, EmbeddedResource resource) : ApplicationBase<UserStoryGroupDomain>(mediator)
    {
        public override Task<UserStoryGroupDomain> CreateAsync(UserStoryGroupDomain domain)
        {
            throw new Exception("Use CreateAsync(int projectId) instead, because a group needs to be initialized from the server only");
        }

        public async Task<UserStoryGroupDomain> CreateAsync(string projectId)
        {
            UserStoryGroupDomain domain = (UserStoryGroupDomain)resource.UserStoryPrompt.Clone();
            domain.ProjectId = projectId;

            var result = await base.CreateAsync(domain);
            if (!string.IsNullOrEmpty(result.Id))
            {
                try
                {
                    var item = await mediator.Send(new UserStoryGroupGenerateCommand { Entity = result });
                }
                catch (Exception ex)
                {
                    // silent catch, because it's not required to get the generation of user stories done right now...
                    Console.WriteLine(ex.Message);
                }
            }

            return result;
        }

        public async Task<UserStoryGroupDomain?> UpdateRequestAsync(string id, UserStoryRequestDomain request)
        {
            UserStoryGroupDomain group = await GetByIdAsync(id) ?? throw new Exception("UserStoryGroup not found");

            group.Request = request;
            group.Response = null;
            group.ClearUserStories();

            return await base.UpdateAsync(group);
        }

        public async Task<UserStoryGroupDomain?> UpdateUserStoriesAsync(string id, ICollection<UserStoryDomain> userStories)
        {
            UserStoryGroupDomain group = await GetByIdAsync(id) ?? throw new Exception("UserStoryGroup not found");

            group.UserStories = userStories;

            return await base.UpdateAsync(group);
        }

        public async Task<UserStoryGroupDomain?> GenerateAsync(string projectId, string id)
        {
            var group = await GetByIdAsync(id);
            if (group is null || group.ProjectId != projectId) return null;

            var item = await mediator.Send(new UserStoryGroupGenerateCommand { Entity = group });
            if (item is not null)
            {
                var project = await projectApplication.GetByIdAsync(projectId)
                    ?? throw new Exception("Le projet avec l'ID spécifié n'a pas été trouvé.");

                project.SelectedGroup = null;
                await mediator.Send(new UpdateCommand<ProjectDomain> { Domain = project });
            }

            return item;
        }

        public async Task<UserStoryGroupDomain> Validate(string projectId, string groupId)
        {
            var item = await mediator.Send(new UserStoryGroupValidateCommand { ProjectId = projectId, GroupId = groupId });

            return item;
        }
    }
}
