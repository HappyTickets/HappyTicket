using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities;

public class OrderItem : SoftDeletableEntity<long>
{
    [Required]
    public long OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; }

    [Required]
    public long TicketId { get; set; }

    [ForeignKey(nameof(TicketId))]
    public virtual Ticket Ticket { get; set; }

    public decimal Price { get; set; }
}
