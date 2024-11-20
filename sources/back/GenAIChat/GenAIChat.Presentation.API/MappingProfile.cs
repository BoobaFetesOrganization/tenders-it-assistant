using AutoMapper;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Presentation.API.Controllers.Document;
using GenAIChat.Presentation.API.Controllers.Project;
using GenAIChat.Presentation.API.Controllers.UserStory;

namespace GenAIChat.Presentation.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            SetMappingWithBaseFor<ProjectDomain, ProjectBaseDto, ProjectDto, ProjectItemDto>();
            SetMappingWithBaseFor<DocumentDomain, DocumentBaseDto, DocumentDto, DocumentItemDto>();
            SetMappingFor<UserStoryDomain, UserStoryDto, UserStoryItemDto>();
            SetMappingFor<UserStoryTaskDomain, UserStoryTaskDto, UserStoryTaskDto>();
        }


        public void SetMappingFor<TSource, TDestination, TDestinationItem>()
            where TSource : class, IEntityDomain
            where TDestination : class
            where TDestinationItem : class
        {
            CreateMap<Paged<TSource>, Paged<TDestinationItem>>()
                .ConvertUsing(new DomainToDtoPagedConverter<TSource, TDestinationItem>());
            CreateMap<TSource, TDestination>();
            CreateMap<TSource, TDestinationItem>();
        }
        public void SetMappingWithBaseFor<TSource, TDestinationBase, TDestination, TDestinationItem>()
            where TSource : class, IEntityDomain
            where TDestination : class
            where TDestinationItem : class
        {
            CreateMap<Paged<TSource>, Paged<TDestinationItem>>()
                .ConvertUsing(new DomainToDtoPagedConverter<TSource, TDestinationItem>());
            CreateMap<TSource, TDestinationBase>();
            CreateMap<TSource, TDestination>();
            CreateMap<TSource, TDestinationItem>();
        }

        public class DomainToDtoPagedConverter<TSource, TDestination> : ITypeConverter<Paged<TSource>, Paged<TDestination>>
            where TSource : class, IEntityDomain
            where TDestination : class
        {
            public Paged<TDestination> Convert(Paged<TSource> source, Paged<TDestination> destination, ResolutionContext context)
            {
                var mappedData = source.Data?.Select(context.Mapper.Map<TDestination>) ?? Enumerable.Empty<TDestination>();
                return new Paged<TDestination>(source.Page, mappedData);
            }
        }
    }
}
