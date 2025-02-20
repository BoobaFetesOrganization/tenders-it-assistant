using Azure.Data.Tables;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class UserStoryRepository(TableServiceClient service) : GenericRepository<UserStoryDomain>(service, "UserStories")
    {
    }
}
