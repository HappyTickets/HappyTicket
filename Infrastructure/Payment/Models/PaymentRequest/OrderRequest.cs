namespace Infrastructure.Payment.Models.PaymentRequest._PaymentRequest
{
    public class OrderRequest
    {
        public string CartId { get; set; }
        public string Test { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string? Trantype { get; set; }
    }
}
