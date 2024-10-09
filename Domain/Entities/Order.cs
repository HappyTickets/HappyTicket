using Domain.Entities.CartEntity;
using Domain.Entities.UserEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public decimal TotalAmount { get; set; }

        // Payment Fields
        public int? PaymentStatus { get; set; }
        public string? PaymentOrderRef { get; set; }
        public string? PaymentUrl { get; set; }

        // Navigation property for Cart Items
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

}
