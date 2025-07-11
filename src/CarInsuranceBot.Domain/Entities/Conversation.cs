using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Conversation : BaseEntity
    {
        public required string UserId { get; set; }
        public User User { get; set; } = null!;
        public string Request { get; set; } = null!;
        public string Response { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }
}
