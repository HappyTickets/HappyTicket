using Infrastructure.Payment.Models.PaymentRequest._PaymentRequest;

namespace Infrastructure.Payment.Models.PaymentRequest.PaymentRequest
{
    public class CustomerRequest
    {
        public string Ref { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public NameRequest Name { get; set; }
        public AddressRequest Address { get; set; }
    }
}
