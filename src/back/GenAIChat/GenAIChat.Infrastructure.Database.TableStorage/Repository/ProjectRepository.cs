using Azure.Data.Tables;
using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class ProjectRepository(TableServiceClient service) : GenericRepository<ProjectDomain>(service, "Projects")
    {
    }
}
