namespace Shared.DTOs.CartDTOs
{
    public class CartItemGroupedDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid MatchId { get; set; }
        public string MatchName { get; set; }
        public string TeamName { get; set; }
        public string Class { get; set; }
        public string Location { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
