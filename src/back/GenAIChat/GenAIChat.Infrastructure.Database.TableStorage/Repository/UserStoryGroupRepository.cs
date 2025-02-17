using Azure.Data.Tables;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class UserStoryGroupRepository(TableServiceClient service) : GenericRepository<UserStoryGroupDomain>(service, "user-story-groups")
    {
    }
}
