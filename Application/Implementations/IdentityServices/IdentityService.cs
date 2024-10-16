//using Application.Interfaces.IIdentityServices;
//using Domain.Entities.UserEntities;
//using LanguageExt;
//using LanguageExt.Common;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Localization;
//using Shared.Common;
//using Shared.Common.General;
//using Shared.DTOs.Identity.Login;
//using Shared.DTOs.Identity.Logout;
//using Shared.DTOs.Identity.RefreshAuthToken;
//using Shared.DTOs.Identity.Register;
//using Shared.DTOs.Identity.Register.ConfirmEmail;
//using Shared.DTOs.Identity.Register.SendEmailConfirmation;
//using Shared.DTOs.Identity.ResetPassword;
//using Shared.DTOs.Identity.ResetPassword.CreatePasswordResetToken;
//using Shared.Exceptions;
//using Shared.ResourceFiles;
//using System.Net;
//using System.Web;

//namespace Application.Implementations.IdentityServices;
//public class IdentityService : IIdentityService
//{
//    private readonly UserManager<ApplicationUser> _userManager;
//    private readonly ITokenService<ApplicationUser> _tokenService;
//    private readonly IEmailSender _emailSender;
//    protected IStringLocalizer<Resource> _localizer;

//    public IdentityService(UserManager<ApplicationUser> userManager, ITokenService<ApplicationUser> tokenService, IEmailSender emailSender, IStringLocalizer<Resource> localizer)
//    {
//        _userManager = userManager;
//        _tokenService = tokenService;
//        _emailSender = emailSender;
//        _localizer = localizer;
//    }

//    public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            return await Task.Run(async () =>
//            {
//                bool isNotAdmin = await _userManager.Users.AnyAsync();
//                if (registerRequest.Password != registerRequest.ConfirmPassword) return new(new RegisterResponse() { Status = HttpStatusCode.BadRequest });

//                ApplicationUser? user = null;
//                if (isNotAdmin)
//                {
//                    user = await _userManager.FindByEmailAsync(registerRequest.Email);
//                    if (user != null) return new(new RegisterResponse() { Status = HttpStatusCode.Forbidden });
//                }
//                user = new() { UserName = registerRequest.UserName, Email = registerRequest.Email, PhoneNumber = registerRequest.PhoneNumber, Cart = new() };
//                user.Cart.UserId = user.Id;
//                user.Cart.User = user;
//                var registrationResults = await _userManager.CreateAsync(user, registerRequest.Password);
//                if (!registrationResults.Succeeded) return new(new RegisterResponse() { Status = HttpStatusCode.BadRequest, ErrorList = registrationResults.Errors.Select(e => new ResponseError() { Title = e.Code, Message = e.Description }).ToList() });

//                user = (await _userManager.FindByEmailAsync(registerRequest.Email))!;
//                if (!isNotAdmin) await _userManager.AddClaimAsync(user, new("Role", "Admin"));

//                //var confResult = await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));
//                //return new Result<RegisterResponse>(confResult.Succeeded ? new RegisterResponse() { Status = HttpStatusCode.OK } : new RegisterResponse() { Status = HttpStatusCode.InternalServerError, Title = "Failed to confirm your email.", ErrorList = confResult.Errors.Select(x => new ResponseError() { Title = x.Code, Message = x.Description }).ToList() });

//                var emailResult = await SendConfirmationEmail(user, cancellationToken);
//                return emailResult.Map(e => new RegisterResponse() { Status = HttpStatusCode.OK });
//            }, cancellationToken);
//        }
//        catch (Exception ex)
//        {
//            return new(new RegisterResponse() { Status = HttpStatusCode.InternalServerError, Title = ex.Message });
//        }
//    }
//    public async Task<Result<SendEmailConfirmationResponse>> SendEmailConfirmation(SendEmailConfirmationRequest sendEmailConfirmationRequest, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            return await Task.Run(async () =>
//            {
//                var user = await _userManager.FindByEmailAsync(sendEmailConfirmationRequest.Email);
//                if (user == null) return new(new SendEmailConfirmationResponse() { Status = HttpStatusCode.NotFound });

//                return await SendConfirmationEmail(user, cancellationToken).Map(succ => new Result<SendEmailConfirmationResponse>(new SendEmailConfirmationResponse()));
//            }, cancellationToken);
//        }
//        catch (Exception ex)
//        {
//            return new(new SendEmailConfirmationResponse() { Status = HttpStatusCode.InternalServerError, Title = ex.Message });
//        }
//    }

//    private async Task<Result<Unit>> SendConfirmationEmail(ApplicationUser user, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            return await Task.Run(async () =>
//            {
//                if (user == null) return new(new BadRequestException());

//                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//                token = HttpUtility.UrlEncode(token);

//                return await _emailSender.SendEmailAsync(user.Email!, Resource.Email_Confirmation, $"{Resource.Email_Confirmation_Message}<a href=\"{UrlHelper.GetBlazorBase()}ConfirmEmail/{token}\">{Resource.Email_Confirm}</a>", cancellationToken: cancellationToken);
//            }, cancellationToken);
//        }
//        catch (Exception ex)
//        {
//            return new(ex);
//        }
//    }

//    public async Task<Result<ConfirmEmailResponse>> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            return await Task.Run(async () =>
//            {
//                var user = await _userManager.FindByEmailAsync(confirmEmailRequest.Email);
//                if (user == null) return new(new ConfirmEmailResponse() { Status = HttpStatusCode.NotFound });

//                var confirmationResults = await _userManager.ConfirmEmailAsync(user, confirmEmailRequest.Token);

//                if (!confirmationResults.Succeeded) return new(new ConfirmEmailResponse() { Status = HttpStatusCode.BadRequest, ErrorList = confirmationResults.Errors.Select(e => new ResponseError() { Title = e.Code, Message = e.Description }).ToList() }); ;

//                return new Result<ConfirmEmailResponse>(new ConfirmEmailResponse());
//            }, cancellationToken);
//        }
//        catch (Exception ex)
//        {
//            return new(new ConfirmEmailResponse() { Status = HttpStatusCode.InternalServerError, Title = ex.Message });
//        }
//    }

//    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            return await Task.Run(async () =>
//            {
//                ApplicationUser? user = await _userManager.FindByEmailAsync(loginRequest.Email);
//                if (user == null) return new(new LoginResponse() { Status = HttpStatusCode.NotFound });
//                bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
//                if (!isEmailConfirmed) return new(new LoginResponse() { Status = HttpStatusCode.BadRequest, Title = Resource.Email_Confirm, ErrorList = [new() { Title = Resource.Email_NotConfirmed, Message = Resource.Email_NotConfirmed_Message }] });
//                bool isValidAttempt = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
//                if (!isValidAttempt) return new(new LoginResponse() { Status = HttpStatusCode.BadRequest, Title = Resource.Credentials_Invalid });
//                return new Result<LoginResponse>((await _tokenService.CreateAuthTokensAsync(user, cancellationToken)).
//                    Match<LoginResponse>(succ => new() { Status = HttpStatusCode.OK, Data = succ },
//                                         fail => new() { Status = HttpStatusCode.InternalServerError, Title = fail.Message }));
//            }, cancellationToken);
//        }
//        catch (Exception ex)
//        {
//            return new(new LoginResponse() { Status = HttpStatusCode.InternalServerError, Title = ex.Message });
//        }
//    }

//    public async Task<Result<LogoutResponse>> LogoutAsync(LogoutRequest logoutRequest, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            return (await _tokenService.RevokeAuthTokensAsync(logoutRequest.UserInfo, cancellationToken)).Match<LogoutResponse>(
//                succ => new() { Status = HttpStatusCode.OK },
//                fail => new() { Status = HttpStatusCode.InternalServerError, Title = fail.Message });
//        }
//        catch (Exception ex)
//        {
//            return new(new LogoutResponse() { Status = HttpStatusCode.InternalServerError, Title = ex.Message });
//        }
//    }

//    public async Task<Result<RefreshAuthTokenResponse>> RefreshAuthTokensAsync(RefreshAuthTokenRequest refreshAuthTokenRequest, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            return (await _tokenService.RefreshAuthTokensAsync(refreshAuthTokenRequest.AuthInfo, cancellationToken))
//                .Match<RefreshAuthTokenResponse>(succ => new() { Status = HttpStatusCode.OK, Data = succ },
//                                                 fail => new() { Status = HttpStatusCode.InternalServerError, Title = fail.Message });
//        }
//        catch (Exception ex)
//        {
//            return new(new RefreshAuthTokenResponse() { Status = HttpStatusCode.InternalServerError, Title = ex.Message });
//        }
//    }

//    public async Task<Result<CreatePasswordResetTokenResponse>> CreatePasswordResetTokenAsync(CreatePasswordResetTokenRequest createPasswordResetTokenRequest, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            return await Task.Run(async () =>
//            {
//                var user = await _userManager.FindByEmailAsync(createPasswordResetTokenRequest.Email);
//                if (user == null)
//                {
//                    return new(new CreatePasswordResetTokenResponse() { Status = HttpStatusCode.NotFound });
//                }

//                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
//                if (token == null)
//                {
//                    return new(new CreatePasswordResetTokenResponse() { Status = HttpStatusCode.InternalServerError });
//                }
//                token = HttpUtility.UrlEncode(token);
//                var emailResult = await _emailSender.SendEmailAsync(user.Email!, Resource.Password_Reset, $"{Resource.Password_Reset_Message} <a href=\"{UrlHelper.GetBlazorBase()}ResetPassword/{token}\">{Resource.Password_Reset}</a>", cancellationToken: cancellationToken);
//                return emailResult.Map(_ => new CreatePasswordResetTokenResponse());
//            }, cancellationToken);
//        }
//        catch (Exception ex)
//        {
//            return new(new CreatePasswordResetTokenResponse() { Status = HttpStatusCode.InternalServerError, Title = ex.Message });
//        }
//    }

//    public async Task<Result<ResetPasswordResponse>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            return await Task.Run<Result<ResetPasswordResponse>>(async () =>
//            {
//                var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);
//                if (user == null)
//                {
//                    return new(new ResetPasswordResponse() { Status = HttpStatusCode.NotFound });
//                }

//                var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetPasswordRequest.Token, resetPasswordRequest.NewPassword);
//                if (!resetPasswordResult.Succeeded || resetPasswordResult.Errors.Any())
//                {
//                    return new(new ResetPasswordResponse() { Status = HttpStatusCode.BadRequest, ErrorList = resetPasswordResult.Errors.Select(e => new ResponseError() { Title = e.Code, Message = e.Description }).ToList() });
//                }

//                return new(new ResetPasswordResponse());
//            }, cancellationToken);
//        }
//        catch (Exception ex)
//        {
//            return new(new ResetPasswordResponse() { Status = HttpStatusCode.InternalServerError, Title = ex.Message });
//        }
//    }
//}
