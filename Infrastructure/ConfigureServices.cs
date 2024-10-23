using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Infrastructure.Payment;
using Infrastructure.Persistence.EntityFramework;
using Infrastructure.Persistence.Extensions.Infrastructure.Persistence;
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
                .AddHttpClient();

            // dependency injection
            services
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddScoped(typeof(IUserRepository<>), typeof(UserRepository<>))
                .AddScoped<ICurrentUser, CurrentUserService>()
                .AddScoped<ITicketRepository, TicketRepository>()
                .AddScoped<IPayment, TelrPaymentService>()
                .AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped<DbSeeder>();

            // configs
            services.Configure<TelrPaymentSettings>(config.GetSection(TelrPaymentSettings.SectionName));

            return services;
        }
    }
}
