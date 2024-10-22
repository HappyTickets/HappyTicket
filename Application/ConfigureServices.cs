using Application.Implementations;
using Application.Interfaces;
using Application.Payments.Service;
using Application.Stadiums.Service;
using Application.Tests.Matches.Service;
using Application.Tickets.Service;
using AutoMapper.Extensions.ExpressionMapping;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddAutoMapper(cfg => cfg.AddExpressionMapping(), Assembly.GetExecutingAssembly())
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddFluentValidationAutoValidation();

            services
                .AddScoped<ITicketService, TicketService>()
                .AddScoped<ITestMatchService, TestMatchService>()
                .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<IStadiumService, StadiumService>()
                .AddScoped<IChampionService, ChampionService>()
                .AddScoped<ISponsorService, SponsorService>()
                .AddScoped<ICartService, CartService>()
                .AddScoped<IPaymentService, PaymentService>();

            return services;
        }
    }
}
