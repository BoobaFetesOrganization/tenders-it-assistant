using AutoMapper;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Presentation.API.Controllers.Dto;

namespace GenAIChat.Presentation.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            SetMappingFor<ProjectDomain, ProjectBaseDto, ProjectDto>();

            SetMappingFor<DocumentDomain, DocumentBaseDto, DocumentDto>(
                convertDestination: map => map
                    .ForPath(dest => dest.MimeType, opt => opt.MapFrom(src => src.Metadata.MimeType)),
                convertSource: map => map
                    .ForPath(dest => dest.Metadata.MimeType, opt => opt.MapFrom(src => src.MimeType))
                );

            SetMappingFor<UserStoryGroupDomain, UserStoryGroupBaseDto, UserStoryGroupDto>();
            SetMappingWithoutBaseFor<UserStoryPromptDomain, UserStoryPromptDto>();

            SetMappingFor<UserStoryDomain, UserStoryBaseDto, UserStoryDto>();

            SetMappingFor<TaskDomain, TaskBaseDto, TaskDto>();
            SetMappingWithoutBaseFor<TaskCostDomain, TaskCostDto>();
        }


        public void SetMappingFor<TSource, TDestinationBase, TDestination>(
            Action<IMappingExpression<TSource, TDestination>>? convertDestination = null,
            Action<IMappingExpression<TDestination, TSource>>? convertSource = null
            )
            where TSource : class, IEntityDomain
            where TDestinationBase : class
            where TDestination : class
        {
            SetMappingWithoutBaseFor<TSource, TDestination>(convertDestination, convertSource);
            CreateMap<Paged<TSource>, Paged<TDestinationBase>>()
                .ConvertUsing(new DomainToDtoPagedConverter<TSource, TDestinationBase>());

            CreateMap<TSource, TDestinationBase>();
            CreateMap<TDestinationBase, TSource>();
        }


        public void SetMappingWithoutBaseFor<TSource, TDestination>(
            Action<IMappingExpression<TSource, TDestination>>? convertDestination = null,
            Action<IMappingExpression<TDestination, TSource>>? convertSource = null
            )
            where TSource : class, IEntityDomain
            where TDestination : class
        {
            var destinationMapExpression = CreateMap<TSource, TDestination>();
            convertDestination?.Invoke(destinationMapExpression);

            var sourceMapExpression= CreateMap<TDestination, TSource>();
            convertSource?.Invoke(sourceMapExpression);
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
