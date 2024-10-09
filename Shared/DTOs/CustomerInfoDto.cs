using Shared.Common.General;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public class CustomerInfoDto
    {
        [Required(ErrorMessageResourceName = nameof(ResourceFiles.Resource.UserName_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = nameof(ResourceFiles.Resource.Email_Required_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
        [RegularExpression(RegexTemplates.Email, ErrorMessageResourceName = nameof(ResourceFiles.Resource.Email_Format_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = nameof(ResourceFiles.Resource.UserName_Validation), ErrorMessageResourceType = typeof(ResourceFiles.Resource))]
        public string Message { get; set; } = string.Empty;
    }
}
