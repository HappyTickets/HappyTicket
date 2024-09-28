namespace Shared.DTOs.CartDTOs
{
    public class RemoveItemRequest
    {
        public Guid ItemId { get; set; }
        public CartItemDto Item { get; set; } = new();

    }
}
