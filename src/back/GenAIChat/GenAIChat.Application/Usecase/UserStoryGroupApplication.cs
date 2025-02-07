using GenAIChat.Application.Adapter.Database;
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
    public class UserStoryGroupApplication : ApplicationBase<UserStoryGroupDomain>
    {
        private readonly IMediator _mediator;
        private readonly ProjectApplication _projectApplication;
        private readonly IGenAiUnitOfWorkAdapter _unitOfWork;
        private readonly EmbeddedResource _resource;

        public UserStoryGroupApplication(IMediator mediator, IGenAiUnitOfWorkAdapter unitOfWork, ProjectApplication projectApplication, EmbeddedResource resource) : base(mediator, unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _projectApplication = projectApplication;
            _resource = resource;
        }

        public override Task<UserStoryGroupDomain> CreateAsync(UserStoryGroupDomain entity)
        {
            throw new Exception("Use CreateAsync(int projectId) instead, because a group needs to be initialized from the server only");
        }

        public async Task<UserStoryGroupDomain> CreateAsync(int projectId)
        {
            UserStoryPromptDomain prompt = new(_resource.UserStoryPrompt);
            UserStoryGroupDomain domain = new(prompt) { ProjectId = projectId };

            var result = await base.CreateAsync(domain);
            if (result.Id > 0)
            {
                try
                {
                    var item = await _mediator.Send(new UserStoryGroupGenerateCommand { Entity = result });
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // silent catch, because it's not required to get the generation of user stories done right now...
                    Console.WriteLine(ex.Message);
                }
            }

            return result;
        }

        public async Task<UserStoryGroupDomain?> UpdateRequestAsync(int id, UserStoryPromptDomain request)
        {
            UserStoryGroupDomain group = await GetByIdAsync(id) ?? throw new Exception("UserStoryGroup not found");

            group.Request = request;
            group.Response = null;
            group.ClearUserStories();

            return await base.UpdateAsync(group);
        }

        public async Task<UserStoryGroupDomain?> UpdateUserStoriesAsync(int id, ICollection<UserStoryDomain> userStories)
        {
            UserStoryGroupDomain group = await GetByIdAsync(id) ?? throw new Exception("UserStoryGroup not found");

            group.UserStories = userStories;

            return await base.UpdateAsync(group);
        }

        public async Task<UserStoryGroupDomain?> GenerateAsync(int projectId, int id)
        {
            var group = await GetByIdAsync(id);
            if (group is null || group.ProjectId != projectId) return null;

            var item = await _mediator.Send(new UserStoryGroupGenerateCommand { Entity = group });
            if (item is not null)
            {
                var project = await _projectApplication.GetByIdAsync(projectId)
                    ?? throw new Exception("Le projet avec l'ID spécifié n'a pas été trouvé.");

                project.SelectedGroup = null;
                var result = await _mediator.Send(new UpdateCommand<ProjectDomain> { Entity = project });
            }
            await _unitOfWork.SaveChangesAsync();
            return item;
        }

        public async Task<UserStoryGroupDomain> Validate(int projectId, int groupId)
        {
            var item = await _mediator.Send(new UserStoryGroupValidateCommand { ProjectId = projectId, GroupId = groupId });
            await _unitOfWork.SaveChangesAsync();
            return item;
        }
    }
}
