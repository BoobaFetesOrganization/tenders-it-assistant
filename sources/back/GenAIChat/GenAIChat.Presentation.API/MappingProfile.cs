using AutoMapper;
using GenAIChat.Domain;
using GenAIChat.Domain.Common;
using GenAIChat.Presentation.API.Controllers.Response.Project;

namespace GenAIChat.Presentation.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // mapping du domain en DTO
            CreateMap<Paged<ProjectDomain>, Paged<ProjectDto>>()
                .ConvertUsing(new PagedDomainToPagedDtoConverter<ProjectDomain, ProjectDto>());
            CreateMap<ProjectDomain, ProjectDto>();
            CreateMap<ProjectDomain, ProjectCreateDto>();


        }

        public class PagedDomainToPagedDtoConverter<TSource, TDestination> : ITypeConverter<Paged<TSource>, Paged<TDestination>>
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
