using Shared.Common.General;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Identity.Login;

public class LoginRequest
{
    [Required(ErrorMessageResourceName = nameof(ResourceFiles.Resource.Email_Required_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
    [RegularExpression(RegexTemplates.Email, ErrorMessageResourceName = nameof(ResourceFiles.Resource.Email_Format_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessageResourceName = nameof(ResourceFiles.Resource.Password_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
    public string Password { get; set; } = string.Empty;
}
