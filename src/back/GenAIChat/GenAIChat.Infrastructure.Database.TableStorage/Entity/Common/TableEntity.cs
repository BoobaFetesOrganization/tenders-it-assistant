using Azure;
using Azure.Data.Tables;

namespace GenAIChat.Infrastructure.Database.TableStorage.Entity.Common
{
    internal abstract class BaseEntity : ITableEntity
    {
        private const string DefaultPartitionKey = "GenAIChat";
        private static string NewRowKey() => Guid.NewGuid().ToString();

        public string PartitionKey { get; set; } = DefaultPartitionKey;
        public string RowKey { get; set; } = NewRowKey();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string GetIdFromKeys() => $"{PartitionKey}|{RowKey}";
        public void SetKeysFromId(string id)
        {
            var parts = string.IsNullOrWhiteSpace(id) ? [] : id.Split("|");
            if (parts.Length < 2)
            {
                PartitionKey = DefaultPartitionKey;
                RowKey = NewRowKey();
            }
            else
            {
                PartitionKey = parts[0];
                RowKey = parts[1];
            }
        }
    }
}
