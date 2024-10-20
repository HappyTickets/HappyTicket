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

namespace API.Controllers;

//[Authorize]
public class IdentityController(IIdentityService userService) : BaseController
{
    private readonly IIdentityService _userService = userService;


    [HttpPost]
    [Route("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest, CancellationToken cancellationToken = default)
    {

        return Result(await _userService.RegisterAsync(registerRequest, cancellationToken));
    }

    [HttpPost]
    [Route("SendEmailConfirmation")]
    [AllowAnonymous]
    public async Task<IActionResult> SendEmailConfirmation([FromBody] SendEmailConfirmationRequest sendEmailConfirmationRequest, CancellationToken cancellationToken = default)
    {
        return Result(await _userService.SendEmailConfirmation(sendEmailConfirmationRequest, cancellationToken));

    }

    [HttpPost]
    [Route("ConfirmEmail")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest confirmEmailRequest, CancellationToken cancellationToken = default)
    {
        return Result(await _userService.ConfirmEmailAsync(confirmEmailRequest, cancellationToken));
    }

    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        return Result(await _userService.LoginAsync(loginRequest, cancellationToken));
    }

    [HttpPost]
    [Route("Logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest logoutRequest, CancellationToken cancellationToken = default)
    {

        return Result(await _userService.LogoutAsync(logoutRequest, cancellationToken));


    }

    [HttpPost]
    [Route("RefreshAuthToken")]
    public async Task<IActionResult> RefreshAuthToken([FromBody] RefreshAuthTokenRequest refreshAuthTokenRequest, CancellationToken cancellationToken = default)
    {

        return Result(await _userService.RefreshAuthTokensAsync(refreshAuthTokenRequest, cancellationToken));
    }

    [HttpPost]
    [Route("CreatePasswordResetToken")]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePasswordResetToken([FromBody] CreatePasswordResetTokenRequest createPasswordResetTokenRequest, CancellationToken cancellationToken = default)
    {

        return Result(await _userService.CreatePasswordResetTokenAsync(createPasswordResetTokenRequest, cancellationToken));
    }

    [HttpPost]
    [Route("ResetPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken = default)
    {
        return Result(await _userService.ResetPasswordAsync(resetPasswordRequest, cancellationToken));
    }
}
