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
            // creation d'un profil de mapping automapper pour merger les elements du domain
            // attention un merge n'est pas un clone donc il faut conserver les elements permettant l'identification d'un element
            
            SetMappingForMergeFunction<DocumentDomain>();
            SetMappingForMergeFunction<DocumentMetadataDomain>();
            SetMappingForMergeFunction<ProjectDomain>();
            SetMappingForMergeFunction<UserStoryGroupDomain>();
            SetMappingForMergeFunction<UserStoryRequestDomain>();
            SetMappingForMergeFunction<UserStoryDomain>();
            SetMappingForMergeFunction<TaskDomain>();
            SetMappingForMergeFunction<TaskCostDomain>();
        }


        public void SetMappingForMergeFunction<TSource>()
            where TSource : class, IEntityDomain
        {
            CreateMap<TSource, TSource>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Timestamp, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
