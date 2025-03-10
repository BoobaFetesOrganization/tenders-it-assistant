using TendersITAssistant.Domain.Document;
using TendersITAssistant.Domain.Project;
using TendersITAssistant.Domain.Project.Group;
using TendersITAssistant.Domain.Project.Group.UserStory;
using TendersITAssistant.Domain.Project.Group.UserStory.Task;
using TendersITAssistant.Domain.Project.Group.UserStory.Task.Cost;
using Microsoft.Extensions.DependencyInjection;
using TendersITAssistant.Application.Resources;
using TendersITAssistant.Application.Usecase;
using TendersITAssistant.Application.Usecase.Interface;

namespace TendersITAssistant.Application
{
    public static class ConfigureService
    {
        public static void AddApplication(this IServiceCollection services, Action<string>? writeLine = null)
        {
            writeLine?.Invoke("configure Application : usecases services");

            // resource registration
            services.AddSingleton<EmbeddedResource>();

            // application registration
            services.AddScoped<IApplication<DocumentDomain>, DocumentApplication>();
            services.AddScoped<IApplication<ProjectDomain>, ProjectApplication>();
            services.AddScoped<IUserStoryGroupApplication, UserStoryGroupApplication>();
            services.AddScoped<IApplication<UserStoryRequestDomain>, UserStoryRequestApplication>();
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
