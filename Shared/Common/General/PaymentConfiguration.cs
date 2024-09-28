namespace Shared.Common.General
{
    public static class PaymentConfiguration
    {
        public static string Test = string.Empty; // 0 for being live, 1 for being in test
        public const string Framed = "2"; // 0 for redirecting, 1 for opening in an iframe element, and 2 for directing
        public const string AuthKey = "ZjjnQ^4368~WL2H8";
        public const string StoreId = "31153";
        public const string HttpClientBaseAddress = "https://secure.telr.com/";
        public const string PostUrl = "gateway/order.json";


        //public const string AuthorisedUrl = $"https://localhost:7017/cart/";
        //public const string DeclinedUrl = $"https://localhost:7017/cart/";
        //public const string CancelledUrl = $"https://localhost:7017/cart/";


        public static string AuthorisedUrl = string.Empty;
        public static string DeclinedUrl = string.Empty;
        public static string CancelledUrl = string.Empty;


        public const string Language = "ar";
        public const string Currency = "SAR";
        public const string Description = "Tickets Payment";
        public const string Line1 = "5 th street, address line 234, next address";
        public const string City = "Riyadh";
        public const string Country = "SA";

        public enum PaymentStatusEnum
        {
            Pending = 1,    // Processing
            Authorized = 2, // Authorised (Transaction not captured)
            Paid = 3,       // Paid (Transaction captured)
            Expired = -1,   // Expired Card
            Cancelled = -2, // Cancelled by the client
            Declined = -3   // Declined By Bank
        }
    }
}
