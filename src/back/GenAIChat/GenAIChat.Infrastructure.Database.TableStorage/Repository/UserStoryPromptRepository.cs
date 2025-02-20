using Azure.Data.Tables;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository
{
    public class UserStoryPromptRepository(TableServiceClient service) : GenericRepository<UserStoryPromptDomain>(service, "UserStoryPrompts")
    {
    }
}
