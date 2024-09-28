namespace Shared.DTOs.CartDTOs;

public class AddItemRequest
{
    public string UserId { get; set; } = string.Empty;
    public CartItemDto Item { get; set; } = new();
}
