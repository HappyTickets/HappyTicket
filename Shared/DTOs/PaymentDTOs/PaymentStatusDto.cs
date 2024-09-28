namespace Shared.DTOs.PaymentDTOs
{
    public class PaymentStatusDto
    {
        public int PaymentStatus { get; set; }
        public string StatusText { get; set; }
        public bool HasErrors { get; set; }
        public List<string> Errors { get; set; }
    }
}
