using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Services.AuthStateProvider
{
    public interface IAuthenticationStateProvider
    {
        Task<AuthenticationState> GetAuthenticationStateAsync();
        void NotifyAuthenticationStateChanged();
    }
}
