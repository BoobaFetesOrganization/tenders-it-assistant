using Serilog;
using Serilog.Core;

namespace TendersITAssistant.Infrastructure.Api.Gemini.Extensions
{
    internal static class SerilogExtensions
    {
        public static ILogger ForGeminiContext(this ILogger logger)
        {
            return logger.ForContext(Constants.SourceContextPropertyName, $"Gemini");
        }
    }
}
