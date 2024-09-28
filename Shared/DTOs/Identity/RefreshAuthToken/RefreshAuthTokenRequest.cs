using Shared.DTOs.Identity.TokenDTOs;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Identity.RefreshAuthToken;

public class RefreshAuthTokenRequest
{
    [Required]
    public TokenDTO AuthInfo { get; set; } = new();
}
