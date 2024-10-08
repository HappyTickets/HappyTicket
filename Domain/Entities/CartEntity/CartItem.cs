using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CartEntity
{
    public class CartItem : BaseEntity
    {
        [Required]
        [ForeignKey(nameof(CartId))]
        public Guid CartId { get; set; }
        public virtual Cart Cart { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Guid? OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        [ForeignKey(nameof(TicketId))]
        public Guid TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        public bool IsCheckedOut { get; set; }
    }
}
