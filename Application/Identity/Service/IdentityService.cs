using Application.Common.Interfaces.Persistence;
using Application.Implementations;
using Application.Interfaces.IIdentityServices;
using Domain.Entities.UserEntities;
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Common.Enums;
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
using Shared.Exceptions;
using Shared.ResourceFiles;
using System.Net;
using System.Web;

namespace Application.Identity.Service;
public class IdentityService(UserManager<ApplicationUser> userManager, ITokenService<ApplicationUser> tokenService, IEmailSender emailSender, IUnitOfWork unitOfWork) : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ITokenService<ApplicationUser> _tokenService = tokenService;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    //public async Task<BaseResponse<TokenDTO?>> RegisterAsync2(RegisterRequest registerRequest, CancellationToken cancellationToken = default)
    //{
    //    return await Task.Run(async () =>
    //    {
    //        bool isNotAdmin = await _userManager.Users.AnyAsync(cancellationToken);

    //        if (registerRequest.Password != registerRequest.ConfirmPassword)
    //        {
    //            return new BaseResponse<TokenDTO?>()
    //            {
    //                Status = HttpStatusCode.BadRequest,
    //                Title = Resource.Passwords_NotMatching,
    //                ErrorList = new List<ResponseError>
    //            {
    //                new ResponseError { Title = Resource.Error_Occurred, Message = Resource.Passwords_NotMatching}
    //            }
    //            };
    //        }

    //        ApplicationUser? user = null;

    //        if (isNotAdmin)
    //        {
    //            user = await _userManager.FindByEmailAsync(registerRequest.Email);
    //            if (user != null)
    //            {
    //                return new BaseResponse<TokenDTO?>()
    //                {
    //                    Status = HttpStatusCode.Conflict,
    //                    Title = Resource.UserAlreadyExist,
    //                    ErrorList = new List<ResponseError>
    //                {
    //                    new ResponseError { Title = Resource.Error_Occurred, Message = Resource.UserAlreadyExist}
    //                }
    //                };
    //            }
    //        }

    //        user = new ApplicationUser
    //        {
    //            UserName = registerRequest.UserName,
    //            Email = registerRequest.Email,
    //            PhoneNumber = registerRequest.PhoneNumber,
    //            Cart = new Cart { User = user },
    //            CreatedDate = DateTime.UtcNow,
    //        };

    //        var registrationResults = await _userManager.CreateAsync(user, registerRequest.Password);
    //        if (!registrationResults.Succeeded)
    //        {
    //            return new()
    //            {
    //                Status = HttpStatusCode.BadRequest,
    //                ErrorList = registrationResults.Errors.Select(e => new ResponseError { Title = e.Code, Message = e.Description }).ToList()
    //            };
    //        }

    //        user = await _userManager.FindByEmailAsync(registerRequest.Email);
    //        if (user is null)
    //        {
    //            return new BaseResponse<TokenDTO?>()
    //            {
    //                Status = HttpStatusCode.InternalServerError,
    //                Title = Resource.NotFound,
    //                ErrorList = new List<ResponseError>
    //            {
    //                new ResponseError { Title = Resource.Error_Occurred, Message =Resource.NotFoundInDB_Message}
    //            }
    //            };
    //        }

    //        // Assign admin role if applicable
    //        if (!isNotAdmin)
    //        {
    //            await _userManager.AddClaimAsync(user, new Claim("Role", "Admin"));
    //        }

    //        // Send confirmation email
    //        var emailResult = await SendConfirmationEmail(user, cancellationToken);
    //        if (!emailResult.IsSuccess)
    //        {
    //            return new BaseResponse<TokenDTO?>()
    //            {
    //                Status = HttpStatusCode.InternalServerError,
    //                Title = Resource.Email_Confirmation_Fail,
    //                ErrorList = emailResult.ErrorList
    //            };
    //        }

    //        // Generate tokens for the user
    //        var tokenDTO = await _tokenService.CreateAuthTokensAsync(user, cancellationToken);
    //        if (tokenDTO is null)
    //        {
    //            return new BaseResponse<TokenDTO?>()
    //            {
    //                Status = HttpStatusCode.InternalServerError,
    //                Title = Resource.FailedToGenerateToken,
    //                ErrorList = new List<ResponseError>
    //            {
    //                new ResponseError { Title = Resource.Error_Occurred, Message = Resource.FailedToGenerateToken  }
    //            }
    //            };
    //        }
    //        return new BaseResponse<TokenDTO?>
    //        {
    //            Status = HttpStatusCode.OK,
    //            Data = tokenDTO
    //        };
    //    }, cancellationToken);
    //}
    public async Task<BaseResponse<TokenDTO?>> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken = default)
    {

        var isAdmin = !await _userManager.Users.AnyAsync();

        var user = new ApplicationUser
        {
            UserName = registerRequest.UserName,
            Email = registerRequest.Email,
            Cart = new(),
            PhoneNumber = registerRequest.PhoneNumber,
            CreatedDate = DateTime.UtcNow,
        };
        var registrationResults = await _userManager.CreateAsync(user, registerRequest.Password);
        if (!registrationResults.Succeeded)
        {
            return new()
            {
                Status = HttpStatusCode.BadRequest,
                ErrorList = registrationResults.Errors.Select(e => new ResponseError { Title = e.Code, Message = e.Description }).ToList()
            };
        }

        if (isAdmin)
        {
            await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
        }

        _ = Task.Run(async () => await SendConfirmationEmail(user, cancellationToken));

        var tokenDTO = await _tokenService.CreateAuthTokensAsync(user, cancellationToken);
        if (tokenDTO is null)
        {
            return new BaseResponse<TokenDTO?>()
            {
                Status = HttpStatusCode.InternalServerError,
                Title = Resource.FailedToGenerateToken,
                ErrorList = new List<ResponseError>
                {
                    new ResponseError { Title = Resource.Error_Occurred, Message = Resource.FailedToGenerateToken  }
                }
            };
        }

        return new BaseResponse<TokenDTO?>
        {
            Status = HttpStatusCode.OK,
            Data = tokenDTO
        };
    }


    public async Task<BaseResponse<Empty>> SendEmailConfirmation(SendEmailConfirmationRequest sendEmailConfirmationRequest, CancellationToken cancellationToken = default)
    {
        return await Task.Run(async () =>
           {
               var user = await _userManager.FindByEmailAsync(sendEmailConfirmationRequest.Email);
               if (user is null)
                   return new NotFoundException(Resource.Email_NotFound);

               return await SendConfirmationEmail(user, cancellationToken);

           }, cancellationToken);

    }

    private async Task<BaseResponse<Empty>> SendConfirmationEmail(ApplicationUser user, CancellationToken cancellationToken = default)
        => await Task.Run(async () =>
        {
            if (user == null) return new BadRequestException();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = HttpUtility.UrlEncode(token);

            return await _emailSender.SendEmailAsync(user.Email!, Resource.Email_Confirmation, $"{Resource.Email_Confirmation_Message}<a href=\"{UrlHelper.GetBlazorBase()}ConfirmEmail/{token}\">{Resource.Email_Confirm}</a>", cancellationToken: cancellationToken);
        }, cancellationToken);

    public async Task<BaseResponse<Empty>> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest, CancellationToken cancellationToken = default)
    {
        return await Task.Run(async () =>
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailRequest.Email);
            if (user == null)
                return new NotFoundException(Resource.NotFoundInDB_Message);

            var confirmationResults = await _userManager.ConfirmEmailAsync(user, confirmEmailRequest.Token);

            if (!confirmationResults.Succeeded)
            {
                return new()
                {
                    Status = HttpStatusCode.InternalServerError,
                    ErrorList = confirmationResults.Errors.Select(e => new ResponseError() { Title = e.Code, Message = e.Description })
                };

            }

            return new BaseResponse<Empty>();
        }, cancellationToken);

    }

    public async Task<BaseResponse<TokenDTO?>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {

        return await Task.Run(async () =>
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user is null)
                return new NotFoundException(Resource.NotFoundInDB_Message);

            bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (!isEmailConfirmed)
                return new()
                {
                    Status = HttpStatusCode.BadRequest,
                    Title = Resource.Email_Confirm,
                    ErrorList = [new() { Title = Resource.Email_NotConfirmed, Message = Resource.Email_NotConfirmed_Message }]
                };

            bool isValidAttempt = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            if (!isValidAttempt)
                return new() { Status = HttpStatusCode.BadRequest, Title = Resource.Credentials_Invalid };
            var token = await _tokenService.CreateAuthTokensAsync(user, cancellationToken);
            return new BaseResponse<TokenDTO?>(token);
        }, cancellationToken);

    }

    public async Task<BaseResponse<Empty>> LogoutAsync(LogoutRequest logoutRequest, CancellationToken cancellationToken = default)
    {
        await _tokenService.RevokeAuthTokensAsync(logoutRequest.UserInfo, cancellationToken);
        return new();
    }

    public async Task<BaseResponse<TokenDTO?>> RefreshAuthTokensAsync(RefreshAuthTokenRequest refreshAuthTokenRequest, CancellationToken cancellationToken = default)
            => await _tokenService.RefreshAuthTokensAsync(refreshAuthTokenRequest.AuthInfo, cancellationToken);

    public async Task<BaseResponse<Empty>> CreatePasswordResetTokenAsync(CreatePasswordResetTokenRequest createPasswordResetTokenRequest, CancellationToken cancellationToken = default)
    {

        return await Task.Run(async () =>
        {
            var user = await _userManager.FindByEmailAsync(createPasswordResetTokenRequest.Email);
            if (user is null)
            {
                return new NotFoundException();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (token == null)
            {
                return new NotFoundException();
            }
            token = HttpUtility.UrlEncode(token);

            return await _emailSender.SendEmailAsync(user.Email!, Resource.Password_Reset, $"{Resource.Password_Reset_Message} <a href=\"{UrlHelper.GetBlazorBase()}ResetPassword/{token}\">{Resource.Password_Reset}</a>", cancellationToken: cancellationToken);

        }, cancellationToken);

    }
    public async Task<BaseResponse<Empty>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken = default)
    {
        // Find the user by email
        var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);

        if (user is null)
        {
            return new()
            {
                Status = HttpStatusCode.NotFound,
                Title = Resource.Credentials_Invalid
            };
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, resetPasswordRequest.OldPassword, resetPasswordRequest.NewPassword);

        if (!changePasswordResult.Succeeded || changePasswordResult.Errors.Any())
        {
            return new()
            {
                Status = HttpStatusCode.BadRequest,
                Title = Resource.Password_Reset_Fail,
                ErrorList = changePasswordResult.Errors.Select(e => new ResponseError { Title = e.Code, Message = e.Description }).ToList()
            };
        }

        return new()
        {
            Status = HttpStatusCode.OK,
            Title = Resource.Password_Reset_Succeed
        };
    }
}
