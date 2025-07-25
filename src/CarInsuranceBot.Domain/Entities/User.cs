using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public long TelegramUserId { get; set; }
        public string? FullName { get; set; }
        public StateType? State { get; set; }

        // Relations
        public ICollection<Document> Documents { get; set; } = [];
        public ICollection<ExtractedField> ExtractedFields { get; set; } = [];
        public ICollection<Policy> Policies { get; set; } = [];
    }
}
