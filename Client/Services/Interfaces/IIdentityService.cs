using LanguageExt.Common;
using Shared.DTOs.Identity.Login;
using Shared.DTOs.Identity.Logout;
using Shared.DTOs.Identity.RefreshAuthToken;
using Shared.DTOs.Identity.Register;
using Shared.DTOs.Identity.Register.ConfirmEmail;
using Shared.DTOs.Identity.Register.SendEmailConfirmation;
using Shared.DTOs.Identity.ResetPassword;
using Shared.DTOs.Identity.ResetPassword.CreatePasswordResetToken;

namespace Client.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest);
        Task<Result<SendEmailConfirmationResponse>> SendEmailConfirmationAsync(SendEmailConfirmationRequest confirmEmailRequest);
        Task<Result<ConfirmEmailResponse>> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest);
        Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest);
        Task<Result<CreatePasswordResetTokenResponse>> CreatePasswordResetTokenAsync(CreatePasswordResetTokenRequest createPasswordResetTokenRequest);
        Task<Result<ResetPasswordResponse>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
        Task<Result<RefreshAuthTokenResponse>> RefreshAuthToken(RefreshAuthTokenRequest refreshAuthTokenRequest);
        Task<Result<LogoutResponse>> LogoutAsync(LogoutRequest logoutRequest);
    }
}
