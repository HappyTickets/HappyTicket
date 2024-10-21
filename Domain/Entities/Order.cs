using Domain.Entities.Common;
using Domain.Entities.UserEntities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Order : SoftDeletableEntity<long>
{
    [Required]
    public long UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; }

    public decimal TotalAmount { get; set; }
    
    public int? PaymentStatus { get; set; }

    public string? PaymentOrderRef { get; set; }

    public string? PaymentUrl { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
