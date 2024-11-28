using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Project.Group;
using GenAIChat.Application.Resources;
using GenAIChat.Application.Usecase.Common;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class UserStoryGroupApplication : ApplicationBase<UserStoryGroupDomain>
    {
        private readonly IMediator _mediator;
        private readonly IGenAiUnitOfWorkAdapter _unitOfWork;
        private readonly EmbeddedResource _resource;

        public UserStoryGroupApplication(IMediator mediator, IGenAiUnitOfWorkAdapter unitOfWork, EmbeddedResource resource) : base(mediator, unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _resource = resource;
        }

        public async Task<UserStoryGroupDomain> Generate(int projectId, UserStoryPromptDomain prompt)
        {
            UserStoryPromptDomain promptToUse = new(prompt, _resource.UserStoryPrompt.Results);
            var item = await _mediator.Send(new UserStoryGroupGenerateCommand { ProjectId = projectId, Prompt = promptToUse });
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
