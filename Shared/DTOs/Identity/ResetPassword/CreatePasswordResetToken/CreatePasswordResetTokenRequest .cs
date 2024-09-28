using Shared.Common.General;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Identity.ResetPassword.CreatePasswordResetToken;

public class CreatePasswordResetTokenRequest
{
    [Required(ErrorMessageResourceName = nameof(ResourceFiles.Resource.Email_Required_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
    [RegularExpression(RegexTemplates.Email, ErrorMessageResourceName = nameof(ResourceFiles.Resource.Email_Format_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
    public string Email { get; set; } = string.Empty;
}
