using Blazored.LocalStorage;
using Client.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Client.Services.AuthStateProvider
{
    public class CustomAuthStateProvider : AuthenticationStateProvider, IAuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var jwt = await _localStorage.GetItemAsStringAsync("T");

            if (jwt != null) {

                var identity = new ClaimsIdentity(JWTParser.ExtractUserClaims(jwt), "JWT");
                var clasimPrincipal = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(clasimPrincipal);

                return state;
            }

            return new(new(new ClaimsIdentity()));
        }

        public void NotifyAuthenticationStateChanged()
            => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
