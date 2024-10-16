using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Identity.UserDTOs;

public class ApplicationUserDTO
{
    public long? Id { get; set; }

    [Required]
    public string? UserName { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }
}
