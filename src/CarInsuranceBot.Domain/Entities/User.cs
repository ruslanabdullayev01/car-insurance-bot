using Domain.Entities.Base;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string TelegramUserId { get; set; } = null!;
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? State { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Relations
        public ICollection<Document> Documents { get; set; } = [];
        public ICollection<Policy> Policies { get; set; } = [];
        public ICollection<Conversation> Conversations { get; set; } = [];
    }
}
