using Shared.Enums;

namespace Shared.DTOs.TicketDTOs
{
    public class CreateTicketsDto
    {
        public long MatchTeamId { get; set; }
        public decimal Price { get; set; }
        public string? Notes { get; set; }
        public long BlockId { get; set; }
        public long SeatId { get; set; }
        public bool? DisplayForSale { get; set; }
        public string Location { get; set; }
        public string Class { get; set; }
        public TicketStatusDTO TicketStatus { get; set; }
        public int SeatNumber { get; set; }
        public string ExternalGate { get; set; }
        public string InternalGate { get; set; }

        public int TicketsCount { get; set; }
    }
}
