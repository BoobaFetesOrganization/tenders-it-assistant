using AutoMapper;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;


namespace GenAIChat.Infrastructure.Database.TableStorage
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // project
            CreateMap<ProjectDomain, ProjectEntity>();
            CreateMap<ProjectEntity, ProjectDomain>();

            //// document
            CreateMap<DocumentDomain, DocumentEntity>();
            CreateMap<DocumentEntity, DocumentDomain>();

            //// document metadata
            CreateMap<DocumentMetadataDomain, DocumentMetadataEntity>();
            CreateMap<DocumentMetadataEntity, DocumentMetadataDomain>();

            //// user story group
            CreateMap<UserStoryGroupDomain, UserStoryGroupEntity>();
            CreateMap<UserStoryGroupEntity, UserStoryGroupDomain>();

            //// user story request
            CreateMap<UserStoryRequestDomain, UserStoryRequestEntity>();
            CreateMap<UserStoryRequestEntity, UserStoryRequestDomain>();

            //// user story
            CreateMap<UserStoryDomain, UserStoryEntity>();
            CreateMap<UserStoryEntity, UserStoryDomain>();

            //// task
            CreateMap<TaskDomain, TaskEntity>();
            CreateMap<TaskEntity, TaskDomain>();

            //// task cost
            CreateMap<TaskCostDomain, TaskCostEntity>();
            CreateMap<TaskCostEntity, TaskCostDomain>();
        }

        public class IEnumerableConverter<TSource, TDestination> : ITypeConverter<IEnumerable<TSource>, IEnumerable<TDestination>>
            where TSource : class
            where TDestination : class
        {
            public IEnumerable<TDestination> Convert(IEnumerable<TSource> source, IEnumerable<TDestination> destination, ResolutionContext context)
            {
                var mappedData = source?.Select(context.Mapper.Map<TDestination>) ?? [];
                return mappedData;
            }
        }
    }
}
