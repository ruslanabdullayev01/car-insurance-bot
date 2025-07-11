using Domain.Entities.Base;
using Microsoft.VisualBasic.FileIO;

namespace Domain.Entities
{
    public class ExtractedField : BaseEntity
    {
        public string FieldName { get; set; } = null!;
        public string Value { get; set; } = null!;
        public FieldType Type { get; set; }

        public required string DocumentId { get; set; }
        public Document Document { get; set; } = null!;
    }
}
