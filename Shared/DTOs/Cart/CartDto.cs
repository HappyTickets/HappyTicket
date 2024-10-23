namespace Shared.DTOs.CartDTOs
{
    public class CartDto
    {
        public IEnumerable<CartItemDto>? CartItems { get; set; }
        public decimal Total { get; set; }
    }
}
