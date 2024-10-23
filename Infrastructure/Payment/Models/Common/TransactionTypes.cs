namespace Infrastructure.Payment.Models.Common
{
    internal class TransactionTypes
    {
        public const string Sale = "sale";
        public const string Auth = "auth";
        public const string Capture = "capture";
        public const string Release = "release";
        public const string Cancel = "cancel";
        public const string Refund = "refund";
    }
}
