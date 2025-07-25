using Domain.Entities.Base;

namespace Domain.Entities
{
    public sealed class Conversation : BaseEntity
    {
        public required string UserId { get; set; }
        public User User { get; set; } = null!;
        public required string Request { get; set; }
        public required string Response { get; set; }
    }
}
