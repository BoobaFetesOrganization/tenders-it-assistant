using AutoMapper;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;
using GenAIChat.Infrastructure.Database.TableStorage.Entity.Common;


namespace GenAIChat.Infrastructure.Database.TableStorage
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
