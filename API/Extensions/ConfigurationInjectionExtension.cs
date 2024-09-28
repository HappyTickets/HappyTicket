using Microsoft.IdentityModel.Tokens;
using Shared.Configs;
using System.Text;

namespace API.Extensions
{
    public static class ConfigurationInjectionExtension
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddJWTConfiguration(configuration).AddEmailConfiguration(configuration);
        }

        public static IServiceCollection AddJWTConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            JWTConfig jwtConfig = new();
            configuration.Bind(nameof(JWTConfig), jwtConfig);

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret ?? throw new InvalidOperationException("API Secret not found."))),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false
            };

            services.AddSingleton(jwtConfig);
            services.AddSingleton(tokenValidationParameters);
            return services;
        }
        public static IServiceCollection AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            EmailConfig emailConfig = new();
            configuration.Bind(nameof(EmailConfig), emailConfig);
            services.AddSingleton(emailConfig);
            return services;
        }
    }
}
