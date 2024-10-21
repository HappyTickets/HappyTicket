namespace Infrastructure.Payment.Models._PaymentResponse
{
    public class PaymentResponse
    {
        public string Method { get; set; }
        public OrderResponse Order { get; set; }
        public ErrorResponse Error { get; set; }
    }
}
