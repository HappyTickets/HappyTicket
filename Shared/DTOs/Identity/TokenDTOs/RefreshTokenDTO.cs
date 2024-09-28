using Shared.DTOs.Identity.UserDTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.Identity.TokenDTOs;

public class RefreshTokenDTO
{
    [Required]
    public string? Token { get; set; }
    [Required]
    public DateTime CreationDate { get; set; }
    [Required]
    public DateTime ExpiryDate { get; set; }
    [Required]
    public bool Used { get; set; }
    [Required]
    public bool Invalidated { get; set; }

    [Required]
    public string UserId { get; set; } = "";
    [Required]
    [ForeignKey(nameof(UserId))]
    public ApplicationUserDTO User { get; set; } = new();
}
