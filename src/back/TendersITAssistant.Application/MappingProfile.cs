using AutoMapper;
using TendersITAssistant.Domain.Common;
using TendersITAssistant.Domain.Document;
using TendersITAssistant.Domain.Project;
using TendersITAssistant.Domain.Project.Group;
using TendersITAssistant.Domain.Project.Group.UserStory;
using TendersITAssistant.Domain.Project.Group.UserStory.Task;
using TendersITAssistant.Domain.Project.Group.UserStory.Task.Cost;

namespace TendersITAssistant.Application
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
