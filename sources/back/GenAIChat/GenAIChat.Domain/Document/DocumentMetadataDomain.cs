using GenAIChat.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenAIChat.Domain.Document
{
    public class DocumentMetadataDomain : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;

        //used to 
        [NotMapped]
        public long SizeBytes { get => Length; set=> Length=value; }
        public long Length { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public string Sha256Hash { get; set; }
        public string Uri { get; set; }
        public string State { get; set; }

        #region  navigation properties
        public int DocumentId { get; set; }
        #endregion
    }
}
