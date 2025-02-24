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


namespace GenAIChat.Infrastructure.Database.TableStorage
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            SetMappingFor<DocumentDomain, DocumentEntity>();
            SetMappingFor<DocumentMetadataDomain, DocumentMetadataEntity>();
            SetMappingFor<ProjectDomain, ProjectEntity>();
            SetMappingFor<TaskDomain, TaskEntity>();
            SetMappingFor<TaskCostDomain, TaskCostEntity>();
            SetMappingFor<UserStoryGroupDomain, UserStoryGroupEntity>();
            SetMappingFor<UserStoryRequestDomain, UserStoryPromptEntity>();
            SetMappingFor<UserStoryDomain, UserStoryEntity>();
        }

        public void SetMappingFor<TSource, TDestination>(
            Action<IMappingExpression<TSource, TDestination>>? convertDestination = null,
            Action<IMappingExpression<TDestination, TSource>>? convertSource = null
            )
            where TSource : class, IEntityDomain
            where TDestination : class, ITableEntity
        {
            CreateMap<Paged<TSource>, Paged<TDestination>>()
                .ConvertUsing(new PagedConverter<TSource, TDestination>());

            CreateMap<ICollection<TSource>, ICollection<TDestination>>()
                .ConvertUsing(new ListConverter<TSource, TDestination>());

            CreateMap<TSource, TDestination>();
            CreateMap<TDestination, TSource>();
        }

        public class ListConverter<TSource, TDestination> : ITypeConverter<ICollection<TSource>, ICollection<TDestination>>
            where TSource : class, IEntityDomain
            where TDestination : class, ITableEntity
        {
            public ICollection<TDestination> Convert(ICollection<TSource> source, ICollection<TDestination> destination, ResolutionContext context)
            {
                var mappedData = source?.Select(context.Mapper.Map<TDestination>) ?? new List<TDestination>();
                return new List<TDestination>(mappedData);
            }
        }

        public class PagedConverter<TSource, TDestination> : ITypeConverter<Paged<TSource>, Paged<TDestination>>
            where TSource : class, IEntityDomain
            where TDestination : class
        {
            public Paged<TDestination> Convert(Paged<TSource> source, Paged<TDestination> destination, ResolutionContext context)
            {
                var mappedData = source.Data?.Select(context.Mapper.Map<TDestination>) ?? new List<TDestination>();
                return new Paged<TDestination>(source.Page, mappedData);
            }
        }
    }
}
