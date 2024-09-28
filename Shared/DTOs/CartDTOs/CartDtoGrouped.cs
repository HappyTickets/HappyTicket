namespace Shared.DTOs.CartDTOs
{
    public class CartDtoGrouped
    {
        public List<CartItemGroupedDto> CartItemsGrouped { get; set; } = new();
        public decimal Total { get; set; }
    }
}
