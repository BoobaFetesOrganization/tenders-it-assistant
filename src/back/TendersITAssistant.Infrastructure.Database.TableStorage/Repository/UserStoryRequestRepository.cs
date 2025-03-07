using AutoMapper;
using Azure.Data.Tables;
using TendersITAssistant.Domain.Project.Group;
using TendersITAssistant.Infrastructure.Database.TableStorage.Entity;
using TendersITAssistant.Infrastructure.Database.TableStorage.Repository.Generic;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Repository
{
    internal class UserStoryRequestRepository(TableServiceClient service, IMapper mapper) : GenericRepository<UserStoryRequestDomain, UserStoryRequestEntity>(service, "UserStoryRequests", mapper)
    {
    }
}
