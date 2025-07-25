﻿using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities
{
    public class Document : BaseEntity
    {
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public DocumentType Type { get; set; }
        public required string UserId { get; set; }
        public User User { get; set; } = null!;
    }
}