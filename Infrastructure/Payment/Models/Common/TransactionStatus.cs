namespace Infrastructure.Payment.Models.Common
{
    internal class TransactionStatus
    {
        public const char Authorized = 'A';
        public const char Hold = 'H';
        public const char Declined = 'D';
    }
}
