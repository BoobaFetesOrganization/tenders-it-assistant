using Serilog;
using Serilog.Core;
using System.Dynamic;
using TendersITAssistant.Application.Usecase;
using TendersITAssistant.Domain.Common;
using TendersITAssistant.Domain.Document;
using TendersITAssistant.Domain.Filter;
using TendersITAssistant.Domain.Project;
using TendersITAssistant.Domain.Project.Group.UserStory;
using TendersITAssistant.Domain.Project.Group.UserStory.Task;
using TendersITAssistant.Domain.Project.Group.UserStory.Task.Cost;

namespace TendersITAssistant.Application.Extensions
{
    internal static class SerilogExtensions
    {
        public static ILogger ForApplicationContext<TDomain>(this ILogger logger) where TDomain : EntityDomain, new()
        {
            return logger.ForContext(Constants.SourceContextPropertyName, $"Application<{typeof(TDomain).Name}>");
        }
        public static object GetLogEntry<TDomain>(this ApplicationBase<TDomain> _, TDomain domain) where TDomain : EntityDomain, new()
        {
            dynamic item = new ExpandoObject();
            item.Kind = domain.GetType().Name;
            item.Id = domain.Id;
            item.Timestamp = domain.Timestamp;
            switch (domain)
            {
                case ProjectDomain project:
                    item.Name = project.Name;
                    break;

                case DocumentDomain document:
                    item.Name = document.Name;
                    item.ProjectId = document.ProjectId;
                    item.Length = document.Metadata.Length;
                    break;

                case UserStoryDomain userStory:
                    item.Name = userStory.Name;
                    item.GroupId = userStory.GroupId;
                    break;

                case TaskDomain task:
                    item.Name = task.Name;
                    item.UserStoryId = task.UserStoryId;
                    item.Cost = task.Cost;
                    break;

                case TaskCostDomain taskCost:
                    item.Kind = taskCost.Kind;
                    item.TaskId = taskCost.TaskId;
                    item.Cost = taskCost.Cost;
                    break;

                default: break;
            };
            return item;
        }

        public static object GetLogEntry<TDomain>(this ApplicationBase<TDomain> applicationBase, TDomain domain, TDomain? result) where TDomain : EntityDomain, new()
        {
            dynamic item = applicationBase.GetLogEntry(domain);
            item.Result = result;
            return item;
        }
        public static object GetLogEntry<TDomain>(this ApplicationBase<TDomain> applicationBase, TDomain domain, bool result) where TDomain : EntityDomain, new()
        {
            dynamic item = applicationBase.GetLogEntry(domain);
            item.Result = result;
            return item;
        }

        public static object GetLogEntry<TDomain>(this ApplicationBase<TDomain> applicationBase, string id, TDomain? result) where TDomain : EntityDomain, new()
        {
            dynamic item = new ExpandoObject();
            item.Id = id;
            item.Result = result is null ? null : applicationBase.GetLogEntry(result);
            return item;
        }

        public static object GetLogEntry<TDomain>(this ApplicationBase<TDomain> _, string id, bool result) where TDomain : EntityDomain, new()
        {
            dynamic item = new ExpandoObject();
            item.Id = id;
            item.Result = result;
            return item;
        }

        public static object GetLogEntry<TDomain>(this ApplicationBase<TDomain> applicationBase, PaginationOptions options, IFilter? filter, Paged<TDomain>? results) where TDomain : EntityDomain, new()
        {
            dynamic item = new ExpandoObject();
            item.Options = options;
            item.Filter = filter;
            item.Results = results?.Data.Select(i => applicationBase.GetLogEntry(i));
            return item;
        }
    }
}
