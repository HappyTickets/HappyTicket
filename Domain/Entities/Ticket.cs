using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Ticket : SoftDeletableEntity<long>
{
    [Required]
    public long MatchTeamId { get; set; }

    [ForeignKey(nameof(MatchTeamId))]
    public virtual MatchTeam MatchTeam { get; set; }

    [Required]
    public decimal Price { get; set; }

    public string? Notes { get; set; }

    public long BlockId { get; set; }
    public virtual Block? Block { get; set; }

    public long SeatId { get; set; }
    public virtual Seat? Seat { get; set; }

    public bool? DisplayForSale { get; set; }

    [Required]
    public string Location { get; set; }

    [Required]
    public string Class { get; set; }

    public TicketStatus TicketStatus { get; set; }

    public int SeatNumber { get; set; }

    public string ExternalGate { get; set; }

    public string InternalGate { get; set; }

    public virtual ICollection<CartItem>? CartItems { get; set; }
}
