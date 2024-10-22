namespace Infrastructure.Payment
{
    internal class TelrPaymentSettings
    {
        public const string SectionName = "Telr";

        public string Test { get; set; }
        public int Framed { get; set; }
        public int StoreId { get; set; }
        public string AuthKey { get; set; }
        public string Currency { get; set; }
        public string SecretKey { get; set; }
        public string AuthorisedUrl { get; set; }
        public string DeclinedUrl { get; set; }
        public string CancelledUrl { get; set; }
    }
}
