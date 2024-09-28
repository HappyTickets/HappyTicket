using LanguageExt.Common;
using Shared.DTOs.Identity.Login;
using Shared.DTOs.Identity.Logout;
using Shared.DTOs.Identity.RefreshAuthToken;
using Shared.DTOs.Identity.Register;
using Shared.DTOs.Identity.Register.ConfirmEmail;
using Shared.DTOs.Identity.Register.SendEmailConfirmation;
using Shared.DTOs.Identity.ResetPassword;
using Shared.DTOs.Identity.ResetPassword.CreatePasswordResetToken;

namespace Application.Interfaces.IIdentityServices;

public interface IIdentityService
{
    Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken = default);
    Task<Result<SendEmailConfirmationResponse>> SendEmailConfirmation(SendEmailConfirmationRequest sendEmailConfirmationRequest, CancellationToken cancellationToken = default);
    Task<Result<ConfirmEmailResponse>> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest, CancellationToken cancellationToken = default);
    Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default);
    Task<Result<LogoutResponse>> LogoutAsync(LogoutRequest logoutRequest, CancellationToken cancellationToken = default);
    Task<Result<RefreshAuthTokenResponse>> RefreshAuthTokensAsync(RefreshAuthTokenRequest refreshAuthTokenRequest, CancellationToken cancellationToken = default);
    Task<Result<CreatePasswordResetTokenResponse>> CreatePasswordResetTokenAsync(CreatePasswordResetTokenRequest createPasswordResetTokenRequest, CancellationToken cancellationToken = default);
    Task<Result<ResetPasswordResponse>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken = default);
}
