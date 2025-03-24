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
    }
}
