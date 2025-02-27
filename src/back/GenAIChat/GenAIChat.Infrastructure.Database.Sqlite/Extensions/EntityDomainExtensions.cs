using GenAIChat.Domain.Common;

namespace GenAIChat.Infrastructure.Database.Sqlite.Extensions
{
    internal static class EntityDomainExtensions
    {
        public static void SetNewTimeStamp(this IEntityDomain domain) => domain.Timestamp = DateTimeOffset.UtcNow;
    }
}
