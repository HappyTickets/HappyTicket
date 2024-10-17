using Shared.Enums;

namespace Shared.DTOs.TicketDTOs
{
    public class UpdateTicketsDto
    {
        public long OldMatchTeamId { get; set; }
        public string OldClass { get; set; }
        public decimal OldPrice { get; set; }
        public string? OldNotes { get; set; }
        public long OldBlockId { get; set; }
        public long OldSeatId { get; set; }
        public bool? OldDisplayForSale { get; set; }
        public string OldLocation { get; set; }
        public TicketStatusDTO OldTicketStatus { get; set; }
        public int OldSeatNumber { get; set; }
        public string OldExternalGate { get; set; }
        public string OldInternalGate { get; set; }

        public long MatchTeamId { get; set; }
        public string Class { get; set; }
        public decimal Price { get; set; }
        public string? Notes { get; set; }
        public long BlockId { get; set; }
        public long SeatId { get; set; }
        public bool? DisplayForSale { get; set; }
        public string Location { get; set; }
        public TicketStatusDTO TicketStatus { get; set; }
        public int SeatNumber { get; set; }
        public string ExternalGate { get; set; }
        public string InternalGate { get; set; }
    }
}
