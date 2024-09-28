namespace Shared.DTOs.PaymentDTOs
{
    public class PaymentRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public string CartId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
