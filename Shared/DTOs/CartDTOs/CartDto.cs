namespace Shared.DTOs.CartDTOs
{
    public class CartDto
    {
        public string UserId { get; set; }
        public List<CartItemDto>? CartItems { get; set; }
        public decimal Total { get; set; }
    }
}
