using Application.Validation;
using Domain.Entities.UserEntities;
using Domain.Entities.UserEntities.AuthEntities;
using Infrastructure.Persistence.EntityFramework;
using Microsoft.AspNetCore.Identity;

namespace API.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, Role>(options =>
            {
                options.User.AllowedUserNameCharacters = null!;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();
            services.AddScoped<IUserValidator<ApplicationUser>, CustomUserNameValidator<ApplicationUser>>();
            return services;
        }
    }
}
