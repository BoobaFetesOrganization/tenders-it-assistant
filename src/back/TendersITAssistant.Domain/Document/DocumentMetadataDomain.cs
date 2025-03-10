﻿using System.ComponentModel.DataAnnotations.Schema;
using TendersITAssistant.Domain.Common;

namespace TendersITAssistant.Domain.Document
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
    }
}

