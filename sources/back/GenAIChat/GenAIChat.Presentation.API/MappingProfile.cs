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
            SetMappingFor<ProjectDomain, ProjectBaseDto, ProjectDto>();
            SetMappingFor<DocumentDomain, DocumentBaseDto, DocumentDto>();
            SetMappingFor<UserStoryDomain, UserStoryDto, UserStoryBaseDto>();
            SetMappingFor<UserStoryTaskDomain, UserStoryTaskDto, UserStoryTaskDto>();
        }


        public void SetMappingFor<TSource, TDestinationBase, TDestination>()
            where TSource : class, IEntityDomain
            where TDestinationBase : class
            where TDestination : class
        {
            CreateMap<Paged<TSource>, Paged<TDestinationBase>>()
                .ConvertUsing(new DomainToDtoPagedConverter<TSource, TDestinationBase>());
            CreateMap<TSource, TDestinationBase>();
            CreateMap<TSource, TDestination>();
        }

        public class DomainToDtoPagedConverter<TSource, TDestination> : ITypeConverter<Paged<TSource>, Paged<TDestination>>
            where TSource : class, IEntityDomain
            where TDestination : class
        {
            public Paged<TDestination> Convert(Paged<TSource> source, Paged<TDestination> destination, ResolutionContext context)
            {
                var mappedData = source.Data?.Select(context.Mapper.Map<TDestination>) ?? [];
                return new Paged<TDestination>(source.Page, mappedData);
            }
        }
    }
}
