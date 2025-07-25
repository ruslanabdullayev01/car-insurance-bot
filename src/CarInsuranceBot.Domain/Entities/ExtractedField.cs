using Domain.Entities.Base;

namespace Domain.Entities
{
    public class ExtractedField : BaseEntity
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;

        public required string UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
