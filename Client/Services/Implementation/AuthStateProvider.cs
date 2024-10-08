using Blazored.LocalStorage;
using Client.Services.Interfaces;
using LanguageExt;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace Client.Services.Implementation
{
    public class AuthStateProvider : IAuthStateProvider
    {
        public bool IsAuthenticated { get; private set; }

        public event Action OnChange;

        public void SetAuthenticationState(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string? token = await _localStorage.GetItemAsStringAsync("jwtToken");


            var claims = ParseClaimsFromJwt(token);
            var identity = claims.Any() ? new ClaimsIdentity(claims, "jwt") : new();
            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string? jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt) || !jwt.Any(x => x == '.')) return [];
            var payload = jwt.Split('.')[1];
            var json = Encoding.UTF8.GetString(ParseBase64WithoutPadding(payload));
            var keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty)) ?? [];
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            try
            {
                Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
                if (Convert.TryFromBase64String(base64, buffer, out int bytesParsed))
                {
                    return buffer.ToArray();
                }
                base64 = base64.Replace('_', '/').Replace('-', '+');
                switch (base64.Length % 4)
                {
                    case 2: base64 += "=="; break;
                    case 3: base64 += "="; break;
                }
                if (Convert.TryFromBase64String(base64, buffer, out bytesParsed))
                {
                    return buffer.ToArray();
                }
                return [];
            }
            catch (Exception ex)
            {
                return [];
            }
        }
    }
}
