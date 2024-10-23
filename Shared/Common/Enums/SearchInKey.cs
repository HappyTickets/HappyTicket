namespace Shared.Common.Enums
{
    public static class SearchInKey
    {
        public const string ASC = "ASC";
        public const string DESC = "DESC";
        public static class ApplicationUser
        {
            public const string UserName = "UserName";
            public const string Email = "Email";
            public const string PhoneNumber = "PhoneNumber";
        }

        public static class Order
        {
            public const string CreatedDate = "CreatedAt";
            public const string OwnerName = "OwnerName";
            public const string OwnerPhone = "PhoneNumber";
            public const string OwnerEmail = "OwnerEmail";
            public const string PaymentStatus = "PaymentStatus";
            public const string TotalAmount = "TotalAmount";
        }
    }
}
