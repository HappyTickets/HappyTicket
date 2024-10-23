namespace Shared.DTOs.CartDTOs
{
    public class CartItemDto
    {
        public long Id { get; set; }
        public string TicketTeam { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Class { get; set; }
        public string Location { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
