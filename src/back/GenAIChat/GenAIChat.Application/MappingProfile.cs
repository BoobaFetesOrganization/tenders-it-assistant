using AutoMapper;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;

namespace GenAIChat.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // enable clone with Automapper

            CreateMap<ProjectDomain, ProjectDomain>();
            CreateMap<DocumentDomain, DocumentDomain>();
            CreateMap<DocumentMetadataDomain, DocumentMetadataDomain>();
            CreateMap<UserStoryGroupDomain, UserStoryGroupDomain>();
            CreateMap<UserStoryRequestDomain, UserStoryRequestDomain>();
            CreateMap<UserStoryDomain, UserStoryDomain>();
            CreateMap<TaskDomain, TaskDomain>();
            CreateMap<TaskCostDomain, TaskCostDomain>();
        }
    }
}
