using Shared.Common.General;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Identity.Register;

public class RegisterRequest
{
    [Required(ErrorMessageResourceName = nameof(ResourceFiles.Resource.UserName_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessageResourceName = nameof(ResourceFiles.Resource.Email_Required_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
    [RegularExpression(RegexTemplates.Email, ErrorMessageResourceName = nameof(ResourceFiles.Resource.Email_Format_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessageResourceName = nameof(ResourceFiles.Resource.Password_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
    [RegularExpression(RegexTemplates.Password, ErrorMessageResourceName = nameof(ResourceFiles.Resource.Password_Format_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessageResourceName = nameof(ResourceFiles.Resource.Confirming_Password_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
    [Compare(nameof(Password), ErrorMessageResourceName = nameof(ResourceFiles.Resource.Passwords_NotMatching), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessageResourceName = nameof(ResourceFiles.Resource.PhoneNumber_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
    public string PhoneNumber { get; set; } = string.Empty;
}
