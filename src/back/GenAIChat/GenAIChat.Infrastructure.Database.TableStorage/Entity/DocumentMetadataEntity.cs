using GenAIChat.Infrastructure.Database.TableStorage.Entity.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Entity
{
    internal class DocumentMetadataEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;

        public long Length { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public string Sha256Hash { get; set; } = string.Empty;
        public string Uri { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;

        #region  navigation properties
        public string DocumentId { get; set; } = string.Empty;
        #endregion
    }
}

