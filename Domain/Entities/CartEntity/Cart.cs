using Domain.Entities.UserEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CartEntity;

public class Cart : BaseEntity
{
    [Required]
    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? User { get; set; }
    public virtual ICollection<CartItem>? CartItems { get; set; }
    public decimal Total { get; set; }
}
