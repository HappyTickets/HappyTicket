using Application.Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Identity.Login;
using Shared.DTOs.Identity.Logout;
using Shared.DTOs.Identity.RefreshAuthToken;
using Shared.DTOs.Identity.Register;
using Shared.DTOs.Identity.Register.ConfirmEmail;
using Shared.DTOs.Identity.Register.SendEmailConfirmation;
using Shared.DTOs.Identity.ResetPassword;
using Shared.DTOs.Identity.ResetPassword.CreatePasswordResetToken;
using Shared.DTOs.Identity.TokenDTOs;

namespace API.Controllers;

//[Authorize]
public class IdentityController : BaseController
{
    private readonly IIdentityService _userService;

    public IdentityController(IHttpContextAccessor httpContextAccessor, IIdentityService userService) : base(httpContextAccessor)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("Register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register([FromBody] RegisterRequest registerRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            return ReturnBaseResult(await _userService.RegisterAsync(registerRequest, cancellationToken));
        }
        catch (Exception ex)
        {
            return ReturnException(ex);
        }
    }
    
    [HttpPost]
    [Route("SendEmailConfirmation")]
    [AllowAnonymous]
    public async Task<ActionResult> SendEmailConfirmation([FromBody] SendEmailConfirmationRequest sendEmailConfirmationRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            return ReturnBaseResult(await _userService.SendEmailConfirmation(sendEmailConfirmationRequest, cancellationToken));
        }
        catch (Exception ex)
        {
            return ReturnException(ex);
        }
    }

    [HttpPost]
    [Route("ConfirmEmail")]
    [AllowAnonymous]
    public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest confirmEmailRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            return ReturnBaseResult(await _userService.ConfirmEmailAsync(confirmEmailRequest, cancellationToken));
        }
        catch (Exception ex)
        {
            return ReturnException(ex);
        }
    }

    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            return ReturnResult<LoginResponse, TokenDTO>(await _userService.LoginAsync(loginRequest, cancellationToken));
        }
        catch (Exception ex)
        {
            return ReturnException(ex);
        }
    }

    [HttpPost]
    [Route("Logout")]
    public async Task<ActionResult> Logout([FromBody] LogoutRequest logoutRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            return (await _userService.LogoutAsync(logoutRequest, cancellationToken)).Match(succ =>
            {

                if (succ.IsSuccess)
                {
                    _ = Response.Headers.Remove("Authorization");
                }
                return ReturnRequest(succ);
            },
            ReturnException);
        }
        catch (Exception ex)
        {
            return ReturnException(ex);
        }
    }

    [HttpPost]
    [Route("RefreshAuthToken")]
    public async Task<ActionResult> RefreshAuthToken([FromBody] RefreshAuthTokenRequest refreshAuthTokenRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            return ReturnResult<RefreshAuthTokenResponse, TokenDTO>(await _userService.RefreshAuthTokensAsync(refreshAuthTokenRequest, cancellationToken));
        }
        catch (Exception ex)
        {
            return ReturnException(ex);
        }
    }

    [HttpPost]
    [Route("CreatePasswordResetToken")]
    [AllowAnonymous]
    public async Task<ActionResult> CreatePasswordResetToken([FromBody] CreatePasswordResetTokenRequest createPasswordResetTokenRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            return ReturnBaseResult(await _userService.CreatePasswordResetTokenAsync(createPasswordResetTokenRequest, cancellationToken));
        }
        catch (Exception ex)
        {
            return ReturnException(ex);
        }
    }

    [HttpPost]
    [Route("ResetPassword")]
    [AllowAnonymous]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            return ReturnResult(await _userService.ResetPasswordAsync(resetPasswordRequest, cancellationToken));
        }
        catch (Exception ex)
        {
            return ReturnException(ex);
        }
    }
}
