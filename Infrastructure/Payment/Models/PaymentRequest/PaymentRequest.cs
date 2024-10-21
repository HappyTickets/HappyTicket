using Infrastructure.Payment.Models.PaymentRequest._PaymentRequest;
using Infrastructure.Payment.Models.PaymentRequest.PaymentRequest;

namespace Infrastructure.Payment.Models._PaymentRequest
{
    public class PaymentRequest
    {
        public string Method { get; set; }
        public int Store { get; set; }
        public string AuthKey { get; set; }
        public int Framed { get; set; }
        public OrderRequest Order { get; set; }
        public ReturnRequest Return { get; set; }
        public CustomerRequest Customer { get; set; }
    }
}
