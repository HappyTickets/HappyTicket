using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class BaseEntity<TId>
{
    [Key]
    public TId Id { get; set; }
    public bool IsActive { get; set; } = true;
    public int SoftDeleteCount { get; set; } = 0;

    public BaseEntityStatus? BaseEntityStatus { get; set; } = null;

    [Required]
    public TId CreatedBy { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    public TId? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
