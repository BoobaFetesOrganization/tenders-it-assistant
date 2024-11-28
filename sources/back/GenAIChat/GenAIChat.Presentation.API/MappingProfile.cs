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

            SetMappingFor<DocumentDomain, DocumentBaseDto, DocumentDto>();
            SetMappingFor<DocumentMetadataDomain, DocumentMetadataBaseDto, DocumentMetadataDto>();

            SetMappingFor<UserStoryGroupDomain, UserStoryGroupBaseDto, UserStoryGroupDto>();
            SetMappingFor<UserStoryPromptDomain, UserStoryPromptBaseDto, UserStoryPromptDto>();

            SetMappingFor<UserStoryDomain, UserStoryBaseDto, UserStoryDto>();

            SetMappingFor<TaskDomain, TaskBaseDto, TaskDto>();
            SetMappingFor<TaskCostDomain, TaskCostBaseDto, TaskCostDto>();
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
            CreateMap<TDestinationBase, TSource>();
            CreateMap<TDestination, TSource>();
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
