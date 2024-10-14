using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities;

public class CartItem : SoftDeletableEntity<long>
{
    [Required]
    public long CartId { get; set; }

    [ForeignKey(nameof(CartId))]
    public virtual Cart Cart { get; set; }

    [Required]
    public long TicketId { get; set; }

    [ForeignKey(nameof(TicketId))]
    public virtual Ticket Ticket { get; set; }

    public bool IsCheckedOut { get; set; }
}
