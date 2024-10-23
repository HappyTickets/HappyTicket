using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Client.Helpers
{
    public static class JWTParser
    {
        public static IEnumerable<Claim> ExtractUserClaims(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var decodedPayload = DecodeBase64WithoutPadding(payload);
            
            var claimsDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(decodedPayload);
            return claimsDictionary?.Select(kv => new Claim(kv.Key, kv.Value.ToString()!)) ?? Enumerable.Empty<Claim>();
        }

        private static string DecodeBase64WithoutPadding(string base64)
        {
            base64 = base64
                .Replace('_', '/')
                .Replace('-', '+');

            switch (base64.Length % 4)
            {
                case 2: 
                    base64 += "=="; 
                    break;
                case 3: 
                    base64 += "=";
                    break;
            }

            var bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
