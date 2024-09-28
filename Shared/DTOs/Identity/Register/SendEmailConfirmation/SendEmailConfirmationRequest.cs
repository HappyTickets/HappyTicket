using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Identity.Register.SendEmailConfirmation;

public class SendEmailConfirmationRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}
