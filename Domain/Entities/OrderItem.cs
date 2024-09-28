using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int Id { get; set; }

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("Ticket")]
        public Guid TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
