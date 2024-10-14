using Domain.Entities.Common;
using Domain.Entities.UserEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Cart : SoftDeletableEntity<long>
{
    [Required]
    public long UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? User { get; set; }

    public virtual ICollection<CartItem>? CartItems { get; set; }

    public decimal Total { get; set; }
}
