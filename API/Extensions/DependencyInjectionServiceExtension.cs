﻿using Application.Identity.Service;
using Application.Implementations;
using Application.Interfaces;
using Application.Interfaces.IIdentityServices;
using Application.Interfaces.Persistence;
using Application.Orders.Service;
using Application.Permissions.Service;
using Infrastructure.Persistence.Identity;
using Infrastructure.Persistence.Transaction;

namespace API.Extensions
{
    public static class DependencyInjectionServiceExtension
    {
        public static void AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddLogging(configure => configure.AddConsole());
            services.AddMemoryCache();

            services.AddTransient<IAuthorizationRepository, AuthorizationRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();

            //services.AddAutoMapper(cfg => { cfg.AddExpressionMapping(); }, typeof(MappingProfile));
            //services.AddValidatorsFromAssembly(typeof(TicketDTOValidator).Assembly);
            //services.AddFluentValidationAutoValidation();

            //services.AddTransient<IValidator<TicketDto>, TicketDTOValidator>();
            //services.AddTransient<IValidator<TicketDto>, TicketDTOValidator>();
            //services.AddTransient<IValidator<MatchDto>, MatchValidator>();
            //services.AddTransient<IValidator<MatchCommandDto>, MatchCommandValidator>();
            //services.AddTransient<IValidator<TeamDto>, TeamValidator>();
            //services.AddTransient<IValidator<StadiumDto>, StadiumValidator>();
            //services.AddTransient<IValidator<BlockDto>, BlockValidator>();
            //services.AddTransient<IValidator<SeatDto>, SeatValidator>();
            //services.AddTransient<IValidator<CartDto>, CartValidator>();
            //services.AddTransient<IValidator<OrderDto>, OrderValidator>();
            //services.AddTransient<IValidator<SponsorDto>, SponsorValidator>();

            //services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            //services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            //services.AddTransient(typeof(IUserRepository<>), typeof(UserRepository<>));
            services.AddTransient(typeof(ITokenService<>), typeof(TokenService<>));
            //services.AddTransient(typeof(IBaseService<,>), typeof(BaseService<,>));
            services.AddTransient<IIdentityService, IdentityService>();
            //services.AddTransient<ITicketService, TicketService>();
            //services.AddTransient<ITestMatchService, TestMatchService>();
            //services.AddSingleton<CountryInfoService>();
            //services.AddTransient<ITicketService, TicketService>();
            //services.AddSingleton<CountryInfoService>();
            services.AddSingleton<IEmailSender, EmailSender>();
            //services.AddScoped<IMatchService, MatchService>();
            //services.AddScoped<IMatchService, MatchService>();
            //services.AddScoped<IMatchCommandService, MatchCommandService>();
            //services.AddScoped<ITeamService, TeamService>();
            //services.AddScoped<IStadiumService, StadiumService>();
            //services.AddScoped<IBlockService, BlockService>();
            //services.AddScoped<ISeatService, SeatService>();
            //services.AddTransient<ICartService, CartService>();
            //services.AddTransient<IFileService, FileService>();
            //services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<ISponsorService, SponsorService>();
            //services.AddScoped<IChampionService, ChampionService>();
        }
    }
}
