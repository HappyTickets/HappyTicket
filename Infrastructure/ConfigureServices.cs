using Application.Common.Interfaces;
using Application.Interfaces.Persistence;
using Infrastructure.Persistence.EntityFramework;
using Infrastructure.Persistence.Identity;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            // services
            services
                .AddScoped<ICurrentUser, CurrentUserService>()
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddScoped(typeof(IUserRepository<>), typeof(UserRepository<>));

            return services;
        }
    }
}
