using Application.Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Shared.DTOs.Identity.Login;
using Shared.DTOs.Identity.Logout;
using Shared.DTOs.Identity.RefreshAuthToken;
using Shared.DTOs.Identity.Register;
using Shared.DTOs.Identity.Register.ConfirmEmail;
using Shared.DTOs.Identity.Register.SendEmailConfirmation;
using Shared.DTOs.Identity.ResetPassword;
using Shared.DTOs.Identity.ResetPassword.CreatePasswordResetToken;
using Shared.DTOs.Identity.TokenDTOs;
using System.Net;

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
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAuthToken([FromBody] RefreshAuthTokenRequest refreshAuthTokenRequest, CancellationToken cancellationToken = default)
    {
        //return Result(new BaseResponse<TokenDTO>(HttpStatusCode.Unauthorized));
        //return Result(new BaseResponse<TokenDTO>(new TokenDTO { JWT = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJSb2xlIjoiQWRtaW4iLCJzdWIiOiI3IiwibmFtZSI6ImhlbWEiLCJlbWFpbCI6ImhlbWFAZ21haWwuY29tIiwianRpIjoiZTMxZDZmZDktNDJmNi00Zjg1LWJiNTQtYjIwM2FhMGVjM2Q3IiwibmJmIjoxNzI5NzYxODI2LCJleHAiOjE3Mjk3NzIwMjYsImlhdCI6MTcyOTc2MTgyNn0.KF76ddzfa1I4AxPtztWPOTMYMvnQ1llhfIlQ5FVxadvY1j1gsAN-9RXAi3G0ciFZ8qPPbFm4Yu2eLm98hcwN3A", RefreshToken= "b184819b-0b68-42dd-9bf2-840039154a77" }));
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
