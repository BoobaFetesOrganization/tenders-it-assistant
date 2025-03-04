using AutoMapper;
using Azure.Data.Tables;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    internal class UserStoryRequestRepository(TableServiceClient service, IMapper mapper) : GenericRepository<UserStoryRequestDomain, UserStoryRequestEntity>(service, "UserStoryRequests", mapper)
    {
    }
}
