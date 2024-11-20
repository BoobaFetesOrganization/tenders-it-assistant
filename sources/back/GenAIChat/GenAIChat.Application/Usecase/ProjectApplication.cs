using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Project;
using GenAIChat.Application.Usecase.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class ProjectApplication : ApplicationBase<ProjectDomain>
    {
        private readonly IMediator _mediator;
        private readonly IGenAiUnitOfWorkAdapter _unitOfWork;

        public ProjectApplication(IMediator mediator, IGenAiUnitOfWorkAdapter unitOfWork) : base(mediator, unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectDomain> GenerateUserStories(int id)
        {
            var project = await _mediator.Send(new ProjectGenerateUserStoriesCommand { Id = id });
            await _unitOfWork.SaveChangesAsync();
            return project;
        }
    }
}
