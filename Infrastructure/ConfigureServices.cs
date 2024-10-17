using Application.Interfaces.Infrastructure.Persistence;
using Application.Interfaces.Infrastructure.Services;
using Infrastructure.Persistence.EntityFramework;
using Infrastructure.Persistence.Identity;
using Infrastructure.Persistence.Repositories;
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
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddScoped(typeof(IUserRepository<>), typeof(UserRepository<>))
                .AddScoped<ICurrentUser, CurrentUserService>()
                .AddScoped<ITicketRepository, TicketRepository>();

            return services;
        }
    }
}
