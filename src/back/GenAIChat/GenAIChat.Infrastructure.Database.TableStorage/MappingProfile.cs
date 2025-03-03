using AutoMapper;
using Azure.Data.Tables;
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
            void MapFor<TSource, TDomain>()
                where TSource : class, IEntityDomain
                where TDomain : class, ITableEntity
            {
                CreateMap<TSource, TDomain>();
                CreateMap<TDomain, TSource>();

                CreateMap<IEnumerable<TSource>, IEnumerable<TDomain>>()
                    .ConvertUsing(new IEnumerableConverter<TSource, TDomain>());
            }

            MapFor<EntityDomain, BaseEntity>();
            MapFor<ProjectDomain, ProjectEntity>();
            MapFor<DocumentDomain, DocumentEntity>();
            MapFor<DocumentMetadataDomain, DocumentMetadataEntity>();
            MapFor<UserStoryGroupDomain, UserStoryGroupEntity>();
            MapFor<UserStoryRequestDomain, UserStoryRequestEntity>();
            MapFor<UserStoryDomain, UserStoryEntity>();
            MapFor<TaskDomain, TaskEntity>();
            MapFor<TaskCostDomain, TaskCostEntity>();
        }

        public class IEnumerableConverter<TSource, TDestination> : ITypeConverter<IEnumerable<TSource>, IEnumerable<TDestination>>
            where TSource : class, IEntityDomain
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
