using GenAIChat.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenAIChat.Domain.Document
{
    public class DocumentMetadataDomain : EntityDomain
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

        //used to deserialize the response from the API
        [NotMapped]
        public long SizeBytes { get => Length; set => Length = value; }

        public override object Clone()
        {
            DocumentMetadataDomain clone = new();

            clone.Name = Name;
            clone.DisplayName = DisplayName;
            clone.MimeType = MimeType;
            clone.Length = Length;
            clone.CreateTime = CreateTime;
            clone.UpdateTime = UpdateTime;
            clone.ExpirationTime = ExpirationTime;
            clone.Sha256Hash = Sha256Hash;
            clone.Uri = Uri;
            clone.State = State;
            clone.DocumentId = DocumentId;

            return clone;
        }
    }
}

