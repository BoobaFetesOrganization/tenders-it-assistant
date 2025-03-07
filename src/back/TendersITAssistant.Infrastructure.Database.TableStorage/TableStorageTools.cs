using TendersITAssistant.Domain;

namespace TendersITAssistant.Infrastructure.Database.TableStorage
{
    internal static class TableStorageTools
    {
        public const string DefaultPartitionKey = "TendersITAssistant";

        public static string GetNewId()
        {
            var (partitionKey, rowKey) = ExtractKeys(string.Empty);
            return GetId(partitionKey, rowKey);
        }
        public static string GetId(string partitionKey, string rowKey) => $"{partitionKey}|{rowKey}";

        public static (string, string) ExtractKeys(string id)
        {
            var parts = string.IsNullOrWhiteSpace(id) ? [] : id.Split("|");

            if (parts.Length < 2) return (DefaultPartitionKey, DomainTools.NewId());

            var partitionKey = string.IsNullOrEmpty(parts[0]) ? DefaultPartitionKey : parts[0];
            var rowKey = string.IsNullOrEmpty(parts[1]) ? DomainTools.NewId() : parts[1];
            return (partitionKey, rowKey);
        }
    }
}
