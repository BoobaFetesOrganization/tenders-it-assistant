using Serilog;
using Serilog.Core;
using TendersITAssistant.Domain.Common;

namespace TendersITAssistant.Application.Extensions
{
    internal static class SerilogExtensions
    {
        public static ILogger ForApplicationContext<TDomain>(this ILogger logger) where TDomain : EntityDomain, new()
        {
            return logger.ForContext(Constants.SourceContextPropertyName, $"Application<{typeof(TDomain).Name}>");
        }
        public static ILogger ForCommandContext<TDomain>(this ILogger logger, string action) where TDomain : class, IEntityDomain
        {
            return logger.ForContext(Constants.SourceContextPropertyName, $"{action}Command<{typeof(TDomain).Name}>");
        }
    }
}
