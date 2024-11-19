using GenAIChat.Domain.Common;

namespace GenAIChat.Domain.Document
{
    public class DocumentDomain : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] Content { get; set; }

        public DocumentMetadataDomain Metadata { get; set; } = new DocumentMetadataDomain();

        #region  navigation properties
        public int ProjectId { get; set; }
        #endregion

        public DocumentDomain() { }

        public DocumentDomain(string filename) : this()
        {
            Name = filename;
        }

        public DocumentDomain(string filename, string contentType, long length, byte[] content) : this(filename)
        {
            Metadata.MimeType = contentType;
            Metadata.SizeBytes = length;
            Content = content;

        }
    }
}
