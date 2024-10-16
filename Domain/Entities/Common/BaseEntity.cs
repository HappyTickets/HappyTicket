using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Common;

public class BaseEntity<TId>
{
    [Key]
    public TId Id { get; set; }

    public BaseEntityStatus? BaseEntityStatus { get; set; } = null;

    [Required]
    public TId CreatedBy { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    public TId? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
