using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.Configs;
using System.Text;

namespace API.Extensions
{
    public static class AuthenticationServiceExtension
    {
        public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            JWTConfig jwtConfig = services.BuildServiceProvider().GetService<JWTConfig>() ?? throw new InvalidOperationException("The JWTConfig was not injected in the DI container.");
            TokenValidationParameters tokenValidationParameters = services.BuildServiceProvider().GetService<TokenValidationParameters>() ?? throw new InvalidOperationException("The TokenValidationParameters was not injected in the DI container.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParameters;
            });
        }
    }
}
