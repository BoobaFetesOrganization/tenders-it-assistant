using GenAIChat.Application.Resources;
using GenAIChat.Application.Usecase;
using GenAIChat.Application.Usecase.Interface;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using Microsoft.Extensions.DependencyInjection;

namespace GenAIChat.Application
{
    public static class ConfigureService
    {
        public static void AddGenAiChatApplication(this IServiceCollection services, Action<string>? writeLine = null)
        {
            writeLine?.Invoke("configure Application : usecases services");

            // resource registration
            services.AddSingleton<EmbeddedResource>();

            // application registration
            services.AddScoped<IApplication<DocumentDomain>, DocumentApplication>();
            services.AddScoped<IApplication<ProjectDomain>, ProjectApplication>();
            services.AddScoped<IUserStoryGroupApplication, UserStoryGroupApplication>();
            services.AddScoped<IApplication<UserStoryRequestDomain>, ApplicationBase<UserStoryRequestDomain>>();
            services.AddScoped<IApplication<UserStoryDomain>, ApplicationBase<UserStoryDomain>>();
            services.AddScoped<IApplication<TaskDomain>, ApplicationBase<TaskDomain>>();
            services.AddScoped<IApplication<TaskCostDomain>, ApplicationBase<TaskCostDomain>>();

            // register MediatR to scan all assemblies in the current domain
            services.AddMediatR(cfg =>
            {
                cfg.RegisterGenericHandlers = true;
                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });
        }
    }
}
