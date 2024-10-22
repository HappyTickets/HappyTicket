using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Identity.ResetPassword;

public class ResetPasswordRequest
{
    //[Required]
    //public string Token { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string OldPassword { get; set; } = string.Empty;

    [Required]
    public string NewPassword { get; set; } = string.Empty;
    [Required, Compare(nameof(NewPassword))]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
