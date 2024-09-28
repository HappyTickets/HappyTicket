using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Identity.TokenDTOs;

public class TokenDTO
{
    [Required]
    public string JWT { get; set; } = string.Empty;
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
