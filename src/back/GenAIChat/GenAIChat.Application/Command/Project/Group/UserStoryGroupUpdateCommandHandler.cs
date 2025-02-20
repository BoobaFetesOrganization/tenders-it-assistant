//using GenAIChat.Application.Adapter.Database;
//using GenAIChat.Application.Command.Common;
//using GenAIChat.Domain.Project.Group;
//using MediatR;

//namespace GenAIChat.Application.Command.Project.Group
//{
//    public class UserStoryGroupUpdateCommandHandler(IRepositoryAdapter<UserStoryGroupDomain> userStoryGroupRepository) : IRequestHandler<UpdateCommand<UserStoryGroupDomain>, UserStoryGroupDomain?>
//    {
//        public async Task<UserStoryGroupDomain?> Handle(UpdateCommand<UserStoryGroupDomain> request, CancellationToken cancellationToken)
//        {
//            var item = await userStoryGroupRepository.GetByIdAsync(request.Entity.Id);
//            if (item is null) return null;

//            item.Request = request.Entity.Request;
//            item.Response = request.Entity.Response;
//            item.ProjectId = request.Entity.ProjectId;
//            item.UserStories = request.Entity.UserStories;

//            await userStoryGroupRepository.UpdateAsync(item);

//            return item;
//        }
//    }

//}
