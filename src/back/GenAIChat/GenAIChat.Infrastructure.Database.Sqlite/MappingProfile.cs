using AutoMapper;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;

namespace GenAIChat.Infrastructure.Database.Sqlite
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // map to enable the merge between domain objects to EF core entities

            CreateMap<EntityDomain, EntityDomain>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp));

            CreateMap<ProjectDomain, ProjectDomain>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SelectedGroupId, opt => opt.MapFrom(src => src.SelectedGroupId));

            CreateMap<DocumentDomain, DocumentDomain>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => src.Metadata));

            CreateMap<DocumentMetadataDomain, DocumentMetadataDomain>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType))
                .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime))
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.UpdateTime))
                .ForMember(dest => dest.ExpirationTime, opt => opt.MapFrom(src => src.ExpirationTime))
                .ForMember(dest => dest.Sha256Hash, opt => opt.MapFrom(src => src.Sha256Hash))
                .ForMember(dest => dest.Uri, opt => opt.MapFrom(src => src.Uri))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(src => src.DocumentId));

            CreateMap<UserStoryGroupDomain, UserStoryGroupDomain>()
                .ForMember(dest => dest.Request, opt => opt.MapFrom(src => src.Request))
                .ForMember(dest => dest.Response, opt => opt.MapFrom(src => src.Response))
                .ForMember(dest => dest.UserStories, opt => opt.MapFrom(src => src.UserStories))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId));

            CreateMap<UserStoryRequestDomain, UserStoryRequestDomain>()
                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.Context))
                .ForMember(dest => dest.Personas, opt => opt.MapFrom(src => src.Personas))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks))
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId));


            CreateMap<UserStoryDomain, UserStoryDomain>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks))
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId));

            CreateMap<TaskDomain, TaskDomain>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
                .ForMember(dest => dest.WorkingCosts, opt => opt.MapFrom(src => src.WorkingCosts))
                .ForMember(dest => dest.UserStoryId, opt => opt.MapFrom(src => src.UserStoryId));

            CreateMap<TaskCostDomain, TaskCostDomain>()
                .ForMember(dest => dest.Kind, opt => opt.MapFrom(src => src.Kind))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId));
        }
    }
}
