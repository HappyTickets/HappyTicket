namespace Infrastructure.Payment.Models._PaymentRequest
{
    public class ReturnRequest
    {
        public string Authorised { get; set; }
        public string Declined { get; set; }
        public string Cancelled { get; set; }
    }
}
