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
            // project
            CreateMap<ProjectDomain, ProjectDto>();
            CreateMap<ProjectDto, ProjectDomain>();
            CreateMap<ProjectDomain, ProjectBaseDto>();
            CreateMap<ProjectBaseDto, ProjectDomain>();
            CreateMap<Paged<ProjectDomain>, Paged<ProjectBaseDto>>()
                .ConvertUsing(new DomainToDtoPagedConverter<ProjectDomain, ProjectBaseDto>());

            // document
            CreateMap<DocumentDomain, DocumentDto>()
                .ForPath(dest => dest.MimeType, opt => opt.MapFrom(src => src.Metadata.MimeType));
            CreateMap<DocumentDto, DocumentDomain>()
                    .ForPath(dest => dest.Metadata.MimeType, opt => opt.MapFrom(src => src.MimeType));
            CreateMap<DocumentDomain, DocumentBaseDto>();
            CreateMap<DocumentBaseDto, DocumentDomain>();
            CreateMap<Paged<DocumentDomain>, Paged<DocumentBaseDto>>()
                .ConvertUsing(new DomainToDtoPagedConverter<DocumentDomain, DocumentBaseDto>());

            // user story group
            CreateMap<UserStoryGroupDomain, UserStoryGroupDto>();
            CreateMap<UserStoryGroupDto, UserStoryGroupDomain>();
            CreateMap<UserStoryGroupDomain, UserStoryGroupBaseDto>();
            CreateMap<UserStoryGroupBaseDto, UserStoryGroupDomain>();
            CreateMap<Paged<UserStoryGroupDomain>, Paged<UserStoryGroupBaseDto>>()
                .ConvertUsing(new DomainToDtoPagedConverter<UserStoryGroupDomain, UserStoryGroupBaseDto>());

            // user story request
            CreateMap<UserStoryRequestDomain, UserStoryRequestDto>();
            CreateMap<UserStoryRequestDto, UserStoryRequestDomain>();

            // user story
            CreateMap<UserStoryDomain, UserStoryDto>();
            CreateMap<UserStoryDto, UserStoryDomain>();
            CreateMap<UserStoryDomain, UserStoryBaseDto>();
            CreateMap<UserStoryBaseDto, UserStoryDomain>();
            CreateMap<Paged<UserStoryDomain>, Paged<UserStoryBaseDto>>()
                .ConvertUsing(new DomainToDtoPagedConverter<UserStoryDomain, UserStoryBaseDto>());

            // task
            CreateMap<TaskDomain, TaskDto>();
            CreateMap<TaskDto, TaskDomain>();
            CreateMap<TaskDomain, TaskBaseDto>();
            CreateMap<TaskBaseDto, TaskDomain>();
            CreateMap<Paged<TaskDomain>, Paged<TaskBaseDto>>()
                .ConvertUsing(new DomainToDtoPagedConverter<TaskDomain, TaskBaseDto>());

            // task cost
            CreateMap<TaskCostDomain, TaskCostDto>();
            CreateMap<TaskCostDto, TaskCostDomain>();
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
