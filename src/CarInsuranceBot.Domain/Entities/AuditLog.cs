using Domain.Entities.Base;

namespace Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        public string Action { get; set; } = null!;
        public string PerformedBy { get; set; } = null!;
        public DateTime PerformedAt { get; set; }
        public string? EntityType { get; set; }
        public string? EntityId { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }
}
