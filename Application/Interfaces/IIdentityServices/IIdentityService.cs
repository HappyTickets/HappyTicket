using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Identity.Login;
using Shared.DTOs.Identity.Logout;
using Shared.DTOs.Identity.RefreshAuthToken;
using Shared.DTOs.Identity.Register;
using Shared.DTOs.Identity.Register.ConfirmEmail;
using Shared.DTOs.Identity.Register.SendEmailConfirmation;
using Shared.DTOs.Identity.ResetPassword;
using Shared.DTOs.Identity.ResetPassword.CreatePasswordResetToken;
using Shared.DTOs.Identity.TokenDTOs;

namespace Application.Interfaces.IIdentityServices;

public interface IIdentityService
{
    Task<BaseResponse<TokenDTO?>> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken = default);
    Task<BaseResponse<Empty>> SendEmailConfirmation(SendEmailConfirmationRequest sendEmailConfirmationRequest, CancellationToken cancellationToken = default);
    Task<BaseResponse<Empty>> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest, CancellationToken cancellationToken = default);
    Task<BaseResponse<TokenDTO?>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default);
    Task<BaseResponse<Empty>> LogoutAsync(LogoutRequest logoutRequest, CancellationToken cancellationToken = default);
    Task<BaseResponse<TokenDTO?>> RefreshAuthTokensAsync(RefreshAuthTokenRequest refreshAuthTokenRequest, CancellationToken cancellationToken = default);
    Task<BaseResponse<Empty>> CreatePasswordResetTokenAsync(CreatePasswordResetTokenRequest createPasswordResetTokenRequest, CancellationToken cancellationToken = default);
    Task<BaseResponse<Empty>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken = default);
}
