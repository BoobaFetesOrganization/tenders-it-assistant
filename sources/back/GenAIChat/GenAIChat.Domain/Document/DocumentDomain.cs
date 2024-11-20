using GenAIChat.Domain.Common;

namespace GenAIChat.Domain.Document
{
    public class DocumentDomain : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] Content { get; set; } = [];

        public DocumentMetadataDomain Metadata { get; set; } = new DocumentMetadataDomain();

        #region  navigation properties
        public int ProjectId { get; set; } = 0;
        #endregion

        public DocumentDomain() { }

        public DocumentDomain(string filename, string contentType, long length, byte[] content, int? projectId = null)
        {
            Name = filename;
            Metadata.MimeType = contentType;
            Metadata.Length = length;
            Content = content;
            if (projectId.HasValue) ProjectId = projectId.Value;
        }
        public DocumentDomain(int id, string filename, string contentType, long length, byte[] content, int? projectId = null)
            : this(filename, contentType, length, content, projectId)
        {
            Id = id;
        }
    }
}
