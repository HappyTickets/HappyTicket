using Blazored.LocalStorage;
using Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace Client.Services.Implementation
{
    public class TokenExpirationService : ITokenExpirationService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        public TokenExpirationService(ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;

        }


        public async Task<bool> CheckTokenExpiration()
        {
            var token = await _localStorage.GetItemAsync<string>("jwtToken");

            if (string.IsNullOrEmpty(token) || IsTokenExpired(token))
            {
                await Logout();

                return false;
            }

            return true;
        }


        //Who Sasys that the validation is only on date ???????
        private bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return true;

            var currentTime = DateTime.UtcNow;
            return jwtToken.ValidTo < currentTime;
        }

        private async Task Logout()
        {
            await _localStorage.RemoveItemAsync("jwtToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            await _authStateProvider.GetAuthenticationStateAsync();
        }
    }
}
