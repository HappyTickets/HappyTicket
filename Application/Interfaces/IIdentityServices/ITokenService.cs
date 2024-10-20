using Microsoft.AspNetCore.Identity;
using Shared.DTOs.Identity.TokenDTOs;
using System.Security.Claims;

namespace Application.Interfaces.IIdentityServices;

public interface ITokenService<TUser> where TUser : IdentityUser<long>
{

    Task<string?> GenerateUserJWTAsync(TUser user, TimeSpan? lifetime = default);
    string? GenerateJWT(ClaimsIdentity claimsIdentity, TimeSpan time = default);

    ClaimsPrincipal? ReadJWT(string jwt, bool validateLifeTime = true);

    Task<TokenDTO?> CreateAuthTokensAsync(TUser user, CancellationToken cancellationToken = default);

    Task<TokenDTO?> RefreshAuthTokensAsync(TokenDTO tokensDTO, CancellationToken cancellationToken = default);
    Task RevokeAuthTokensAsync(TokenDTO tokensDTO, CancellationToken cancellationToken = default);
}
