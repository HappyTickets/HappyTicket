using Application.Implementations;
using Application.Implementations.IdentityServices;
using Application.Implementations.PaymentServices;
using Application.Implementations.TicketServices;
using Application.Interfaces;
using Application.Interfaces.IIdentityServices;
using Application.Interfaces.ITicketServices;
using Application.Interfaces.PaymentServices;
using Application.Interfaces.Persistence;
using Application.Mapper;
using Application.Validation;
using Application.Validation.RolesValidator;
using AutoMapper.Extensions.ExpressionMapping;
using FluentValidation;
using Infrastructure.Persistence.EntityFramework;
using Infrastructure.Persistence.File;
using Infrastructure.Persistence.Identity;
using Infrastructure.Persistence.Transaction;
using Shared.DTOs;
using Shared.DTOs.Authorization.Request;
using Shared.DTOs.CartDTOs;
using Shared.DTOs.MatchDtos;
using Shared.DTOs.TicketDTOs;
using static Application.Validation.TicketDtoValidator;

namespace API.Extensions
{
    public static class DependencyInjectionServiceExtension
    {
        public static void AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { cfg.AddExpressionMapping(); }, typeof(MappingProfile));
            services.AddMemoryCache();
            services.AddLogging(configure => configure.AddConsole());



            services.AddTransient<IValidator<TicketDto>, TicketDTOValidator>();
            services.AddTransient<IValidator<TicketDto>, TicketDTOValidator>();
            services.AddTransient<IValidator<MatchDto>, MatchValidator>();
            services.AddTransient<IValidator<MatchCommandDto>, MatchCommandValidator>();
            services.AddTransient<IValidator<TeamDto>, TeamValidator>();
            services.AddTransient<IValidator<StadiumDto>, StadiumValidator>();
            services.AddTransient<IValidator<BlockDto>, BlockValidator>();
            services.AddTransient<IValidator<SeatDto>, SeatValidator>();
            services.AddTransient<IValidator<CartDto>, CartValidator>();
            services.AddTransient<IValidator<OrderDto>, OrderValidator>();

            services.AddTransient<IValidator<AddRoleDto>, AddRoleValidator>();
            services.AddTransient<IValidator<EditRoleDto>, EditRoleValidator>();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork<AppDbContext>));
            services.AddTransient(typeof(IUserRepository<>), typeof(UserRepository<>));
            services.AddTransient(typeof(ITokenService<>), typeof(TokenService<>));
            services.AddTransient(typeof(IBaseService<,>), typeof(BaseService<,>));
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddTransient<ITicketService, TicketService>();
            //services.AddSingleton<CountryInfoService>();
            //services.AddTransient<ITicketService, TicketService>();
            //services.AddSingleton<CountryInfoService>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IMatchCommandService, MatchCommandService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IStadiumService, StadiumService>();
            services.AddScoped<IBlockService, BlockService>();
            services.AddScoped<ISeatService, SeatService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IFileService, FileService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
