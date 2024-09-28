namespace Shared.Common.General
{
    public static class RandomGenerator
    {
        private static Random Random = new Random();
        public static string GenerateString(int length)
        {
            //App Setting
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[Random.Next(x.Length)]).ToArray());
        }
        public static int GenerateNumber(int min, int max)
        {
            return Random.Next(min, max);
        }
        public static string GenerateSignatureCode(string domainName)
        {
            if (string.IsNullOrEmpty(domainName))
            {
                throw new ArgumentException("Domain Name Cannot Be Null Or Empty...", nameof(domainName));
            }
            string datePart = DateTime.Now.ToString("ddMMyyyy");
            return $"ORG-{domainName}-{datePart}";
        }
    }
}
