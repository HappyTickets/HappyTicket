using Blazored.LocalStorage;
using Client.Services._HttpClientFacade;
using Client.Services.AuthStateProvider;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Identity.Login;
using Shared.DTOs.Identity.Logout;
using Shared.DTOs.Identity.RefreshAuthToken;
using Shared.DTOs.Identity.TokenDTOs;
using System.Net;

namespace Client.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IAuthenticationStateProvider _authStateProvider;
        private readonly IHttpClientFacade _httpClientFacade;
        private readonly ILocalStorageService _localStorage;

        public IdentityService(IAuthenticationStateProvider authStateProvider, IHttpClientFacade httpClientFacade, ILocalStorageService localStorageService)
        {
            _authStateProvider = authStateProvider;
            _httpClientFacade = httpClientFacade;
            _localStorage = localStorageService;
        }

        public async Task<BaseResponse<TokenDTO>> LoginAsync(LoginRequest request)
        {
            var response = await _httpClientFacade.PostAsync<BaseResponse<TokenDTO>>("api/identity/login", request, false);
            
            if(response.IsSuccess)
            {
                await _localStorage.SetItemAsStringAsync("T", response.Data.JWT);
                await _localStorage.SetItemAsStringAsync("RT", response.Data.RefreshToken);
                
                _authStateProvider.NotifyAuthenticationStateChanged();
            }

            return response;
        }

        public async Task<BaseResponse<Empty>> LogoutAsync()
        {
            // prepare request
            var request = new LogoutRequest
            {
                UserInfo = new TokenDTO
                {
                    JWT = await _localStorage.GetItemAsStringAsync("T"),
                    RefreshToken = await _localStorage.GetItemAsStringAsync("RT")
                }
            };

            var response = await _httpClientFacade.PostAsync<BaseResponse<Empty>>("api/identity/logout", request);

            await _localStorage.RemoveItemAsync("T");
            await _localStorage.RemoveItemAsync("RT");

            _authStateProvider.NotifyAuthenticationStateChanged();

            return response;
        }

        public async Task<BaseResponse<TokenDTO>> ReloginAsync()
        {
            if (!await _localStorage.ContainKeyAsync("T") || !await _localStorage.ContainKeyAsync("RT"))
                return new(HttpStatusCode.Unauthorized);

            var request = new RefreshAuthTokenRequest
            {
                AuthInfo = new TokenDTO
                {
                    JWT = await _localStorage.GetItemAsStringAsync("T"),
                    RefreshToken = await _localStorage.GetItemAsStringAsync("RT")
                }
            };

            await _localStorage.RemoveItemAsync("T");
            await _localStorage.RemoveItemAsync("RT");

            var response = await _httpClientFacade.PostAsync<BaseResponse<TokenDTO>>("api/identity/refreshAuthToken", request, false);

            if (response.IsSuccess)
            {
                await _localStorage.SetItemAsStringAsync("T", response.Data.JWT);
                await _localStorage.SetItemAsStringAsync("RT", response.Data.RefreshToken);
            }

            _authStateProvider.NotifyAuthenticationStateChanged();

            return response;
        }
    }
}
