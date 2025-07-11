using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities
{
    public class PolicyEvent : BaseEntity
    {
        public required string PolicyId { get; set; }
        public Policy Policy { get; set; } = null!;
        public PolicyEventType EventType { get; set; }
        public DateTime OccurredAt { get; set; }
        public string? Description { get; set; }
    }
}
