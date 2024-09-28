using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public Guid MatchId { get; set; }
        [ForeignKey(nameof(MatchId))]
        public virtual Match? Match { get; set; }
        public Guid TeamId { get; set; }
        [ForeignKey(nameof(TeamId))]
        public virtual Team? Team { get; set; }
        public decimal Price { get; set; }
        public string? Notes { get; set; }
        public virtual Block? Block { get; set; }
        public virtual Seat? Seat { get; set; }
        public bool IsActive { get; set; }
        public bool? DisplayForSale { get; set; }
        public string? QRCode { get; set; }
        public string? Barcode { get; set; }
        public string Location { get; set; }
        public string Class { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public int SeatNumber { get; set; }
    }

}
