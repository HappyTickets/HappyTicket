//using Application.Interfaces.IIdentityServices;
//using Application.Interfaces.Persistence;
//using Domain.Entities.UserEntities;
//using Domain.Entities.UserEntities.AuthEntities;
//using LanguageExt;
//using LanguageExt.Common;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using Shared.Configs;
//using Shared.DTOs.Identity.TokenDTOs;
//using Shared.Exceptions;
//using Shared.Extensions;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace Application.Implementations.IdentityServices;

//public class TokenService<TUser> : ITokenService<TUser> where TUser : ApplicationUser
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IUserRepository<TUser> _userRepository;
//    private readonly UserManager<TUser> _userManager;
//    private readonly JWTConfig _jwtConfig;
//    private readonly TokenValidationParameters _tokenValidationParameters;

//    public TokenService(IUnitOfWork unitOfWork, IUserRepository<TUser> userRepository, UserManager<TUser> userManager, JWTConfig jwtConfig, TokenValidationParameters tokenValidationParameters)
//    {
//        _unitOfWork = unitOfWork;
//        _userRepository = userRepository;
//        _userManager = userManager;
//        _jwtConfig = jwtConfig;
//        _tokenValidationParameters = tokenValidationParameters;
//    }

//    public async Task<Result<string>> GenerateUserJWTAsync(TUser user, TimeSpan time = default)
//    {
//        try
//        {
//            var userClaims = await _userManager.GetClaimsAsync(user);
//            List<Claim> claims = [
//                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
//                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
//                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                new Claim("Id", user.Id),
//                new Claim("Iat", DateTimeOffset.UtcNow.ToString()),
//                new Claim("ExpDate", DateTimeOffset.UtcNow.Add(time).ToString())
//            ];
//            claims.ForEach(userClaims.Add);

//            if (time == default)
//            {
//                time = _jwtConfig.DefaultTokenLifetime;
//            }

//            ClaimsIdentity claimsIdentity = new(userClaims, JwtBearerDefaults.AuthenticationScheme);
//            Result<string> jwtToken = GenerateJWT(claimsIdentity, time);
//            return jwtToken;
//        }
//        catch (Exception ex)
//        {
//            return new Result<string>(ex);
//        }
//    }

//    public Result<string> GenerateJWT(ClaimsIdentity claimsIdentity, TimeSpan time = default)
//    {
//        try
//        {
//            if (time == default)
//            {
//                time = _jwtConfig.DefaultTokenLifetime;
//            }

//            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

//            if (_jwtConfig.Secret == null)
//            {
//                return new Result<string>(new InvalidOperationException("API Secret not found."));
//            }

//            byte[] key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
//            SecurityTokenDescriptor securityTokenDescriptor = new()
//            {
//                Subject = claimsIdentity,
//                Expires = DateTime.UtcNow.Add(time),
//                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
//            };
//            SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
//            string jwtToken = jwtSecurityTokenHandler.WriteToken(token);
//            return jwtToken;
//        }
//        catch (Exception ex)
//        {
//            return new Result<string>(ex);
//        }
//    }

//    public Result<ClaimsPrincipal> ReadJWT(string jwt, bool validateLifeTime = true)
//    {
//        try
//        {
//            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
//            TokenValidationParameters tokenValidationParameters = _tokenValidationParameters;
//            tokenValidationParameters.ValidateLifetime = validateLifeTime;
//            ClaimsPrincipal claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(jwt, tokenValidationParameters, out SecurityToken validatedToken);
//            if (validatedToken is JwtSecurityToken jwtSecurityToken)
//            {
//                if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
//                {
//                    return new(new SecurityTokenInvalidAlgorithmException());
//                }

//                string expirationDateStr = claimsPrincipal.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Exp, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
//                bool isValid = long.TryParse(expirationDateStr, out long numDate);
//                DateTimeOffset expirationDate = DateTimeOffset.FromUnixTimeSeconds(numDate).UtcDateTime;
//                return !isValid ? new(new SecurityTokenValidationException("Token does not have an expiration date."))
//                    : DateTimeOffset.UtcNow > expirationDate ? new(new SecurityTokenExpiredException()) : new(claimsPrincipal);
//            }
//            return new(new SecurityTokenValidationException("Token was not valid."));
//        }
//        catch (Exception ex)
//        {
//            return new(ex);
//        }
//    }

//    public async Task<Result<TokenDTO>> CreateAuthTokensAsync(TUser user, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            Func<Task<Result<TokenDTO>>> createAuthTokenFunc = async () => await (await GenerateUserJWTAsync(user, _jwtConfig.DefaultTokenLifetime)).Match(
//                async jwtSucc => await ReadJWT(jwtSucc).Match(
//                    async claimsPrincipalSucc =>
//                    {
//                        RefreshToken newRefreshToken = new()
//                        {
//                            Token = Guid.NewGuid().ToString(),
//                            JWTId = claimsPrincipalSucc!.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value,
//                            CreationDate = DateTimeOffset.UtcNow,
//                            ExpiryDate = DateTimeOffset.UtcNow.Add(_jwtConfig.DefaultRefreshTokenLifetime),
//                            IsUsed = false,
//                            Invalidated = false,
//                            UserId = user.Id
//                        };

//                        _ = await _unitOfWork.Repository<RefreshToken>().UpdateRangeAsync(rt => rt.UserId == user.Id, ort => { ort.IsUsed = true; return ort; });

//                        return await (await _unitOfWork.Repository<RefreshToken>().CreateAsync(newRefreshToken, cancellationToken)).Match(
//                            async addSucc => (await _unitOfWork.SaveChangesAsync()).Match(
//                                saveSucc => new Result<TokenDTO>(new TokenDTO()
//                                {
//                                    JWT = jwtSucc!,
//                                    RefreshToken = newRefreshToken!.Token!
//                                }),
//                                saveFail => new(saveFail)),
//                            async addFail => await addFail.ToResultAsync<TokenDTO>(cancellationToken));
//                    },
//                    async claimsPrincipalFail => await claimsPrincipalFail.ToResultAsync<TokenDTO>(cancellationToken)),
//                async jwtFail => await jwtFail.ToResultAsync<TokenDTO>(cancellationToken));


//            return await (await _unitOfWork.Repository<RefreshToken>().FindAsync(rt => rt.UserId == user.Id && !rt.IsUsed, cancellationToken)).Match(
//                async succ =>
//                {
//                    foreach (RefreshToken token in succ)
//                    {
//                        token.IsUsed = true;
//                    }
//                    return await _unitOfWork.Repository<RefreshToken>().UpdateRange(succ).Match(
//                        async succ => await (await _unitOfWork.SaveChangesAsync()).Match(
//                            async succ => await createAuthTokenFunc(),
//                            async fail => await fail.ToResultAsync<TokenDTO>(cancellationToken)),
//                        async fail => await fail.ToResultAsync<TokenDTO>(cancellationToken));
//                },
//                async fail =>
//                {

//                    return fail is NotFoundException ? await createAuthTokenFunc() : await fail.ToResultAsync<TokenDTO>(cancellationToken);
//                });
//        }
//        catch (Exception ex)
//        {
//            return new(ex);
//        }
//    }

//    public async Task<Result<Unit>> RevokeAuthTokensAsync(TokenDTO tokensDTO, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            var revokeRefreshTokens = async (string userId = "") => await (await _unitOfWork.Repository<RefreshToken>()
//                                        .FindAsync(rt => rt.Token == tokensDTO.RefreshToken || rt.UserId == userId, cancellationToken)).Match(
//                async findSucc =>
//                {
//                    foreach (RefreshToken token in findSucc)
//                    {
//                        token.IsUsed = true;
//                        token.Invalidated = true;
//                    }
//                    return await _unitOfWork.Repository<RefreshToken>().UpdateRange(findSucc).Match(
//                        async updateSucc => (await _unitOfWork.SaveChangesAsync()).Match<Result<Unit>>(
//                            saveSucc => new(new Unit()),
//                            saveFail => new(saveFail)),
//                        async saveFail => await saveFail.ToResultAsync<Unit>(cancellationToken));
//                },
//                async findFail => await findFail.ToResultAsync<Unit>(cancellationToken));

//            return await ReadJWT(tokensDTO.JWT).Match(
//                async claimsPrincipalSucc =>
//                {
//                    string userId = claimsPrincipalSucc.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value ?? string.Empty;
//                    return await revokeRefreshTokens(userId);
//                },
//                async claimsPrincipalFail =>
//                {
//                    return claimsPrincipalFail is SecurityTokenExpiredException
//                        ? await revokeRefreshTokens()
//                        : await claimsPrincipalFail.ToResultAsync<Unit>(cancellationToken);
//                });
//        }
//        catch (Exception ex)
//        {
//            return new(ex);
//        }
//    }

//    public async Task<Result<TokenDTO>> RefreshAuthTokensAsync(TokenDTO tokensDTO, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            if (string.IsNullOrWhiteSpace(tokensDTO.JWT) || string.IsNullOrWhiteSpace(tokensDTO.RefreshToken))
//            {
//                return new(new BadRequestException());
//            }

//            Func<Task<Result<TokenDTO>>> updateRefreshToken = async () => await (await _unitOfWork.Repository<RefreshToken>()
//                .FirstOrDefaultAsync(rf => rf.Token == tokensDTO.RefreshToken && !rf.IsUsed, cancellationToken)).Match(
//                    async refreshTokenSucc => await (await _userRepository.GetByIdAsync(refreshTokenSucc.UserId, cancellationToken)).Match(
//                        async userSucc =>
//                        {
//                            if (refreshTokenSucc.ExpiryDate < DateTimeOffset.UtcNow)
//                            {
//                                return new(new SecurityTokenExpiredException("The refresh token is expired."));
//                            }

//                            if (refreshTokenSucc.Invalidated)
//                            {
//                                return new(new SecurityTokenValidationException("The refresh token is not valid."));
//                            }

//                            if (refreshTokenSucc.IsUsed)
//                            {
//                                return new(new SecurityTokenValidationException("The refresh token was already used."));
//                            }

//                            refreshTokenSucc.IsUsed = true;

//                            return await _unitOfWork.Repository<RefreshToken>().Update(refreshTokenSucc).Match(
//                                async saveSucc => await (await _unitOfWork.SaveChangesAsync()).Match(
//                                        async createAuthTokenSucc => await CreateAuthTokensAsync(userSucc, cancellationToken),
//                                        async createAuthTokenFail => await createAuthTokenFail.ToResultAsync<TokenDTO>(cancellationToken)),
//                                async saveFail => await saveFail.ToResultAsync<TokenDTO>());
//                        },
//                        async userFail => await userFail.ToResultAsync<TokenDTO>(cancellationToken)),
//                    async refreshTokenFail => await refreshTokenFail.ToResultAsync<TokenDTO>(cancellationToken));

//            return await ReadJWT(tokensDTO.JWT, false).Match(
//                async claimsPrincipalSucc =>
//                {
//                    string? jwtId = claimsPrincipalSucc.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
//                    string? jwtUserId = claimsPrincipalSucc.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
//                    return string.IsNullOrWhiteSpace(jwtId) || string.IsNullOrWhiteSpace(jwtUserId)
//                        ? new(new SecurityTokenValidationException("The JWT is not valid."))
//                        : await updateRefreshToken();
//                },
//                async claimsPrincipalFail =>
//                {
//                    return claimsPrincipalFail is SecurityTokenExpiredException
//                        ? await updateRefreshToken()
//                        : await claimsPrincipalFail.ToResultAsync<TokenDTO>(cancellationToken);
//                });
//        }
//        catch (Exception ex)
//        {
//            return new(ex);
//        }
//    }
//}
