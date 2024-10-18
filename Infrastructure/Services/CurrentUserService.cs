using Application.Interfaces.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Services
{
    internal class CurrentUserService: ICurrentUser
    {
        private readonly HttpContext? _httpcontext;

        public CurrentUserService(IHttpContextAccessor httpcontextAccessor)
        {
            _httpcontext = httpcontextAccessor.HttpContext;
        }

        public long? Id
        {
            get
            {
                var isParsed = long.TryParse(_httpcontext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var result);
                return isParsed ? result : null;
            }
        }

        public string? Name => _httpcontext?.User.FindFirstValue(JwtRegisteredClaimNames.Name);
        public string? Email => _httpcontext?.User.FindFirstValue(JwtRegisteredClaimNames.Email);
    }
}
