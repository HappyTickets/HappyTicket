using Domain.Entities.Common;

namespace Domain.Entities
{
    public class SoftDeletableEntity<TId> : BaseEntity<TId>
    {
        public bool IsActive { get; set; } = true;
        public int SoftDeleteCount { get; set; } = 0;
    }
}
