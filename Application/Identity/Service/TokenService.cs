using Application.Common.Interfaces.Persistence;
using Application.Interfaces.IIdentityServices;
using Domain.Entities.UserEntities;
using Domain.Entities.UserEntities.AuthEntities;
using LanguageExt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Configs;
using Shared.DTOs.Identity.TokenDTOs;
using Shared.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Identity.Service;

public class TokenService<TUser> : ITokenService<TUser> where TUser : ApplicationUser
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository<TUser> _userRepository;
    private readonly UserManager<TUser> _userManager;
    private readonly JWTConfig _jwtConfig;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public TokenService(IUnitOfWork unitOfWork, IUserRepository<TUser> userRepository, UserManager<TUser> userManager, JWTConfig jwtConfig, TokenValidationParameters tokenValidationParameters)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _userManager = userManager;
        _jwtConfig = jwtConfig;
        _tokenValidationParameters = tokenValidationParameters;
    }

    public async Task<string?> GenerateUserJWTAsync(TUser user, TimeSpan? lifetime = null)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        List<Claim> claims = [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        ];
        claims.ForEach(userClaims.Add);


        lifetime ??= _jwtConfig.DefaultTokenLifetime;

        ClaimsIdentity claimsIdentity = new(userClaims, JwtBearerDefaults.AuthenticationScheme);

        return GenerateJWT(claimsIdentity, lifetime.Value);

    }

    public string? GenerateJWT(ClaimsIdentity claimsIdentity, TimeSpan time = default)
    {
        if (time == default)
        {
            time = _jwtConfig.DefaultTokenLifetime;
        }

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

        if (_jwtConfig.Secret == null)
        {
            return null;
        }

        byte[] key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
        SecurityTokenDescriptor securityTokenDescriptor = new()
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.Add(time),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
        };
        SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        string jwtToken = jwtSecurityTokenHandler.WriteToken(token);
        return jwtToken;
    }

    public ClaimsPrincipal? ReadJWT(string jwt, bool validateLifeTime = true)
    {
        try
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            TokenValidationParameters tokenValidationParameters = _tokenValidationParameters;
            tokenValidationParameters.ValidateLifetime = validateLifeTime;
            ClaimsPrincipal claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(jwt, tokenValidationParameters, out SecurityToken validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                string expirationDateStr = claimsPrincipal.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Exp, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
                bool isValid = long.TryParse(expirationDateStr, out long numDate);
                DateTimeOffset expirationDate = DateTimeOffset.FromUnixTimeSeconds(numDate).UtcDateTime;

                if (!isValid || DateTimeOffset.UtcNow > expirationDate)
                {
                    return null;
                }

                return claimsPrincipal;
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }


    public async Task<TokenDTO?> CreateAuthTokensAsync(TUser user, CancellationToken cancellationToken = default)
    {
        // Helper function to create the Auth Tokens
        async Task<TokenDTO?> CreateAuthTokenAsync()
        {
            var jwt = await GenerateUserJWTAsync(user, _jwtConfig.DefaultTokenLifetime);
            if (string.IsNullOrEmpty(jwt)) return null;

            var claimsPrincipal = ReadJWT(jwt);
            if (claimsPrincipal == null) return null;

            var jwtId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (jwtId == null) return null;

            // Create new refresh token
            var newRefreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                JWTId = jwtId,
                CreationDate = DateTimeOffset.UtcNow,
                ExpiryDate = DateTimeOffset.UtcNow.Add(_jwtConfig.DefaultRefreshTokenLifetime),
                IsUsed = false,
                Invalidated = false,
                UserId = user.Id
            };

            var refreshTokens = await _unitOfWork.Repository<RefreshToken>().ListAsync(rt => rt.UserId == user.Id);
            refreshTokens.ForEach(rt => rt.IsUsed = true);
            _unitOfWork.Repository<RefreshToken>().UpdateRange(refreshTokens);

            _unitOfWork.Repository<RefreshToken>().Create(newRefreshToken);

            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (saveResult == 0) return null;

            return new TokenDTO
            {
                JWT = jwt,
                RefreshToken = newRefreshToken.Token
            };
        }

        // Fetch unused tokens for the user
        var existingTokens = await _unitOfWork.Repository<RefreshToken>().ListAsync(rt => rt.UserId == user.Id && !rt.IsUsed, cancellationToken: cancellationToken);

        if (existingTokens.Count > 0) // Check if there are existing tokens
        {
            existingTokens.ForEach(token => token.IsUsed = true);
            _unitOfWork.Repository<RefreshToken>().UpdateRange(existingTokens);

            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (saveResult > 0) // If existing tokens were updated successfully
            {
                return await CreateAuthTokenAsync();
            }
        }

        // If no existing tokens or update fails, create new auth tokens
        return await CreateAuthTokenAsync();
    }

    public async Task RevokeAuthTokensAsync(TokenDTO tokensDTO, CancellationToken cancellationToken = default)
    {
        // Helper function to revoke refresh tokens
        async Task RevokeRefreshTokensAsync(long userId)
        {
            var refreshTokens = await _unitOfWork.Repository<RefreshToken>()
                .ListAsync(rt => rt.Token == tokensDTO.RefreshToken || rt.UserId == userId, null, cancellationToken);

            if (refreshTokens == null || !refreshTokens.Any())
                return; // No tokens to revoke

            foreach (var token in refreshTokens)
            {
                token.IsUsed = true;
                token.Invalidated = true;
            }

            _unitOfWork.Repository<RefreshToken>().UpdateRange(refreshTokens);
            await _unitOfWork.SaveChangesAsync(); // Await here to ensure the operation completes
        }

        // Read the JWT and extract the user ID
        var claimsPrincipal = ReadJWT(tokensDTO.JWT);
        if (claimsPrincipal != null)
        {
            var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim != null && long.TryParse(userIdClaim, out long id))
            {
                await RevokeRefreshTokensAsync(id);
            }
        }
    }

    public async Task<TokenDTO?> RefreshAuthTokensAsync(TokenDTO tokensDTO, CancellationToken cancellationToken = default)
    {

        if (string.IsNullOrWhiteSpace(tokensDTO.JWT) || string.IsNullOrWhiteSpace(tokensDTO.RefreshToken))
        {
            throw new BadRequestException();
        }

        // Read the JWT to extract claims
        var claimsPrincipal = ReadJWT(tokensDTO.JWT, false);
        if (claimsPrincipal is null)
        {
            throw new SecurityTokenValidationException("The JWT is not valid.");
        }

        string? jwtId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        string? jwtUserId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrWhiteSpace(jwtId) || string.IsNullOrWhiteSpace(jwtUserId))
        {
            throw new SecurityTokenValidationException("The JWT is not valid.");
        }

        // Fetch the refresh token
        var refreshToken = await _unitOfWork.Repository<RefreshToken>()
            .FirstOrDefaultAsync(rf => rf.Token == tokensDTO.RefreshToken && !rf.IsUsed, null, cancellationToken);

        if (refreshToken is null)
        {
            throw new SecurityTokenValidationException("The refresh token is not valid.");
        }

        if (refreshToken.ExpiryDate < DateTimeOffset.UtcNow)
        {
            throw new SecurityTokenExpiredException("The refresh token is expired.");
        }

        if (refreshToken.Invalidated)
        {
            throw new SecurityTokenValidationException("The refresh token is not valid.");
        }

        if (refreshToken.IsUsed)
        {
            throw new SecurityTokenValidationException("The refresh token was already used.");
        }

        refreshToken.IsUsed = true;

        // Update the refresh token in the database
        _unitOfWork.Repository<RefreshToken>().Update(refreshToken);
        var saveResult = await _unitOfWork.SaveChangesAsync();

        if (saveResult <= 0)
        {
            return null;
        }

        var res = await _userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);
        if (res.Data is null)
        {
            return null;
        }

        return await CreateAuthTokensAsync(res.Data, cancellationToken);
    }

}