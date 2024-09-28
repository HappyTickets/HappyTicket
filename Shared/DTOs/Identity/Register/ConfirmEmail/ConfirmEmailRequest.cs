using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Identity.Register.ConfirmEmail;

public class ConfirmEmailRequest
{
    [Required]
    public string Token { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}
