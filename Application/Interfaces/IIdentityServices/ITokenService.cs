using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs.Identity.TokenDTOs;
using System.Security.Claims;

namespace Application.Interfaces.IIdentityServices;

public interface ITokenService<TUser> where TUser : IdentityUser
{
    /// <summary>
    /// Generate a JWT based on the use's claims.
    /// </summary>
    /// <typeparam name="T">The User's type "<typeparamref name="TUser"/>".</typeparam>
    /// <param name="user">The <typeparamref name="TUser"/> entity.</param>
    /// <param name="time">The Expiry for the token (default (0, 5, 0) 5 minutes).</param>
    /// <returns>The JWT in string format.</returns>
    public Task<Result<string>> GenerateUserJWTAsync(TUser user, TimeSpan time = default);

    /// <summary>
    /// Generate a JWT based on the claims provided.
    /// </summary>
    /// <param name="claimsIdentity">The claims that will be put in the token.</param>
    /// <param name="time">The Expiry for the token (default (0, 5, 0) 5 minutes).</param>
    /// <returns>The JWT in string format.</returns>
    /// <exception cref="NullReferenceException"></exception>
    public Result<string> GenerateJWT(ClaimsIdentity claimsIdentity, TimeSpan time = default);

    /// <summary>
    /// Validates and Reads a string JWT.
    /// </summary>
    /// <param name="jwt">The JWT in string format.</param>
    /// <param name="validateLifeTime">Validate the JWT LifeTime.</param>
    /// <returns>The Claims Principal.</returns>
    public Result<ClaimsPrincipal> ReadJWT(string jwt, bool validateLifeTime = true);

    /// <summary>
    /// Create the JWT and the refresh token associated with it.
    /// </summary>
    /// <param name="user">The user whose info will be used for the token creation.</param>
    /// <returns>The AuthTokensDTO (contains the new JWT and refresh token combination).</returns>
    public Task<Result<TokenDTO>> CreateAuthTokensAsync(TUser user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refresh the JWT and the refresh token associated with it.
    /// </summary>
    /// <param name="tokensDTO">The JWT in string format.</param>
    /// <returns>The AuthTokensDTO (contains the new JWT and refresh token combination).</returns>
    public Task<Result<TokenDTO>> RefreshAuthTokensAsync(TokenDTO tokensDTO, CancellationToken cancellationToken = default);

    /// <summary>
    /// Revoke the refresh token.
    /// </summary>
    /// <param name="tokensDTO">The JWT in string format.</param>
    /// <returns>The AuthTokensDTO (contains the new JWT and refresh token combination).</returns>
    Task<Result<Unit>> RevokeAuthTokensAsync(TokenDTO tokensDTO, CancellationToken cancellationToken = default);
}
