using Blazored.LocalStorage;
using Client.Services.AuthStateProvider;
using Client.Services.Identity;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Client.Services._HttpClientFacade
{
    public class HttpClientFacade : IHttpClientFacade
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthenticationStateProvider _authStateProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;

        public HttpClientFacade(HttpClient httpClient, IAuthenticationStateProvider authStateProvider, IServiceProvider serviceProvider, ILocalStorageService localStorage, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _serviceProvider = serviceProvider;
            _localStorage = localStorage;
            _navigationManager = navigationManager;
        }

        public async Task<TResponse> GetAsync<TResponse>(string url, bool useAuth = true)
        {
            if (useAuth)
                await SetJwtAuthorizationHeaderAsync();

            var response = await _httpClient.GetAsync(url);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<TResponse> PostAsync<TResponse>(string url, object data, bool useAuth = true)
        {
            if (useAuth)
                await SetJwtAuthorizationHeaderAsync();

            var response = await _httpClient.PostAsJsonAsync(url, data);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<TResponse> PutAsync<TResponse>(string url, object data, bool useAuth = true)
        {
            if (useAuth)
                await SetJwtAuthorizationHeaderAsync();

            var response = await _httpClient.PutAsJsonAsync(url, data);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string url, bool useAuth = true)
        {
            if (useAuth)
                await SetJwtAuthorizationHeaderAsync();

            var response = await _httpClient.DeleteAsync(url);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task SetJwtAuthorizationHeaderAsync()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();

            if(state.User.Identity!.IsAuthenticated)
            {
                var expDateStr = state.User.Claims.First(c => c.Type.Equals(JwtRegisteredClaimNames.Exp)).Value;
                var expDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expDateStr)).UtcDateTime;

                if (DateTimeOffset.UtcNow < expDate) 
                {
                    var jwt = await _localStorage.GetItemAsStringAsync("T");
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                }
                else
                {
                    var identityService = _serviceProvider.GetRequiredService<IIdentityService>();
                    try
                    {
                        var result = await identityService.ReloginAsync();
                        if (result.IsSuccess)
                        {
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data!.JWT);
                        }
                        else
                        {
                            await identityService.LogoutAsync();
                            _navigationManager.NavigateTo("/login");
                        }
                    }
                    catch
                    {
                        await identityService.LogoutAsync();
                        _navigationManager.NavigateTo("/login");
                    }
                }
            }
        }
    }
}
