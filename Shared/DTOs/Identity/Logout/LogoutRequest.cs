using Shared.DTOs.Identity.TokenDTOs;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Identity.Logout;

public class LogoutRequest
{
    [Required]
    public TokenDTO UserInfo { get; set; } = new();
}
