using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Error : BaseEntity
    {
        public string Message { get; set; } = null!;
        public string? StackTrace { get; set; }
        public DateTime OccurredAt { get; set; }
        public string? UserId { get; set; }
        public string? Context { get; set; }
    }
}
