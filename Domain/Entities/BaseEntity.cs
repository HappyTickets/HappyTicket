using Domain.Enums;

namespace Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool IsActive { get; set; } = true;
    public int SoftDeleteCount { get; set; } = 0;
    public BaseEntityStatus? BaseEntityStatus { get; set; } = null;

    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }

}
