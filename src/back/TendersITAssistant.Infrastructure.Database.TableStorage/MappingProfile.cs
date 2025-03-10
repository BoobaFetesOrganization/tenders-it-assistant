using AutoMapper;
using TendersITAssistant.Domain.Common;
using TendersITAssistant.Domain.Document;
using TendersITAssistant.Domain.Project;
using TendersITAssistant.Domain.Project.Group;
using TendersITAssistant.Domain.Project.Group.UserStory;
using TendersITAssistant.Domain.Project.Group.UserStory.Task;
using TendersITAssistant.Domain.Project.Group.UserStory.Task.Cost;
using TendersITAssistant.Infrastructure.Database.TableStorage.Entity;
using TendersITAssistant.Infrastructure.Database.TableStorage.Entity.Common;


namespace TendersITAssistant.Infrastructure.Database.TableStorage
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMaps<ProjectDomain, ProjectEntity>();
            CreateMaps<DocumentDomain, DocumentEntity>();
            CreateMaps<DocumentMetadataDomain, DocumentMetadataEntity>();
            CreateMaps<UserStoryGroupDomain, UserStoryGroupEntity>();
            CreateMaps<UserStoryRequestDomain, UserStoryRequestEntity>();
            CreateMaps<UserStoryDomain, UserStoryEntity>();
            CreateMaps<TaskDomain, TaskEntity>();
            CreateMaps<TaskCostDomain, TaskCostEntity>();
        }

        private void CreateMaps<TDomain, TEntity>()
            where TDomain : IEntityDomain
            where TEntity : BaseEntity
        {
            // allow merge between to 2 entities
            CreateMap<TEntity, TEntity>();

            // allow convertion from entity to domain
            CreateMap<TDomain, TEntity>();

            // allow convertion from domain to entity
            CreateMap<TEntity, TDomain>();
        }
    }
}
