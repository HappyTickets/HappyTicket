namespace Shared.DTOs.CartDTOs;

public class CheckoutRequestDto
{
    public string UserId { get; set; } = string.Empty;

    public string? PaymentUrl { get; set; }

    public string? PaymentRef { get; set; }
}