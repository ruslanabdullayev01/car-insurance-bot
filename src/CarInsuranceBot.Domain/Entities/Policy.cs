using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities
{
    public class Policy : BaseEntity
    {
        public string PolicyNumber { get; set; } = null!;
        public PolicyStatus Status { get; set; } = PolicyStatus.Pending;
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string FilePath { get; set; } = null!;

        // Relations
        public required string UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
