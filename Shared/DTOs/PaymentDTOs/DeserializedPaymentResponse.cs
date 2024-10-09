namespace Shared.DTOs.PaymentDTOs
{
    public class DeserializedPaymentResponse
    {
        public string? Method { get; set; }

        public string? Trace { get; set; }

        public DeserializedPaymentOrder? Order { get; set; }

        public DeserializedPaymentError? Error { get; set; }

    }
}
