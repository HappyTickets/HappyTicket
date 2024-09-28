using Client.Services.Interfaces;
using LanguageExt.Common;
using Shared.DTOs.Identity.Login;
using Shared.DTOs.Identity.Logout;
using Shared.DTOs.Identity.RefreshAuthToken;
using Shared.DTOs.Identity.Register;
using Shared.DTOs.Identity.Register.ConfirmEmail;
using Shared.DTOs.Identity.Register.SendEmailConfirmation;
using Shared.DTOs.Identity.ResetPassword;
using Shared.DTOs.Identity.ResetPassword.CreatePasswordResetToken;

namespace Client.Services.Implementation
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpClientHelper _httpClient;

        public IdentityService(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest)
        {
            return await _httpClient.PostBaseAsync<RegisterRequest, RegisterResponse>("api/Identity/Register", registerRequest, false);
        }

        public async Task<Result<ConfirmEmailResponse>> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest)
        {
            return await _httpClient.PostBaseAsync<ConfirmEmailRequest, ConfirmEmailResponse>("api/Identity/ConfirmEmail", confirmEmailRequest, false);
        }

        public async Task<Result<SendEmailConfirmationResponse>> SendEmailConfirmationAsync(SendEmailConfirmationRequest sendEmailConfirmationRequest)
        {
            return await _httpClient.PostBaseAsync<SendEmailConfirmationRequest, SendEmailConfirmationResponse>("api/Identity/SendEmailConfirmation", sendEmailConfirmationRequest, false);
        }

        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            return await _httpClient.PostBaseAsync<LoginRequest, LoginResponse>(@$"api/Identity/Login", loginRequest, false);
        }

        public async Task<Result<CreatePasswordResetTokenResponse>> CreatePasswordResetTokenAsync(CreatePasswordResetTokenRequest createPasswordResetTokenRequest)
        {
            return await _httpClient.PostBaseAsync<CreatePasswordResetTokenRequest, CreatePasswordResetTokenResponse>("api/Identity/CreatePasswordResetToken", createPasswordResetTokenRequest, false);
        }

        public async Task<Result<ResetPasswordResponse>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            return await _httpClient.PostBaseAsync<ResetPasswordRequest, ResetPasswordResponse>("api/Identity/ResetPassword", resetPasswordRequest, false);
        }

        public async Task<Result<RefreshAuthTokenResponse>> RefreshAuthToken(RefreshAuthTokenRequest refreshAuthTokenRequest)
        {
            return await _httpClient.PostBaseAsync<RefreshAuthTokenRequest, RefreshAuthTokenResponse>("api/Identity/RefreshAuthToken", refreshAuthTokenRequest, false);
        }

        public async Task<Result<LogoutResponse>> LogoutAsync(LogoutRequest logoutRequest)
        {
            return await _httpClient.PostBaseAsync<LogoutRequest, LogoutResponse>("api/Identity/Logout", logoutRequest, false);
        }
    }
}
