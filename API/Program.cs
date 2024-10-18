using API.Extensions;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Common.General;
using System.Globalization;
using System.Text.Json.Serialization;


var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs/.log"));
//builder.Host.UseSerilog((_, config) =>
//{
//    config.MinimumLevel.Debug();
//    config.Enrich.FromLogContext();
//    config.WriteTo.Console();
//    config.WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs/.log"),
//                        rollOnFileSizeLimit: true,
//                        rollingInterval: RollingInterval.Day,
//                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}");
//    config.ReadFrom.Configuration(builder.Configuration.GetSection("Serilog"));
//});

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

//var file = File.CreateText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "Serilog.log"));
//Serilog.Debugging.SelfLog.Enable(Console.Error);
//Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
//Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(file));

var appSettings = builder.Configuration.GetSection("BaseUrls");
var blazorBaseUrl = appSettings["BlazorBaseUrl"];
var apiBaseUrl = appSettings["ApiBaseUrl"];
UrlHelper.SetCommunicationUrls(blazorBaseUrl!, apiBaseUrl!);

PaymentConfiguration.Test = builder.Configuration["IsPaymentTest"] ?? "1";
PaymentConfiguration.AuthorisedUrl = builder.Configuration["PaymentRedirectUrl"] ?? "https://ticketshappy.com/cart/";
PaymentConfiguration.DeclinedUrl = builder.Configuration["PaymentRedirectUrl"] ?? "https://ticketshappy.com/cart/";
PaymentConfiguration.CancelledUrl = builder.Configuration["PaymentRedirectUrl"] ?? "https://ticketshappy.com/cart/";

// Add services
builder.Services.AddConfiguration(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddSwaggerServices();
builder.Services.AddDatabaseServices(builder.Configuration, DatabaseType.SqlServer); // Change this enum to MySql
builder.Services.AddDependencyInjectionServices();
builder.Services.AddCORsServices();
builder.Services.AddControllers().AddJsonOptions(opt => { opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; });
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddScoped<WarmUpService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCulture = new[] { new CultureInfo("ar-SA"), new CultureInfo("ar-EG"), new CultureInfo("en-GB"), new CultureInfo("en-US") };
    options.DefaultRequestCulture = new RequestCulture(supportedCulture[0]);
    options.SupportedCultures = supportedCulture;
    options.SupportedUICultures = supportedCulture;
});

builder.Services.AddControllers().AddViewLocalization().AddDataAnnotationsLocalization();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();
//using (var scope = app.Services.CreateScope())
//{
//    var warmUpService = scope.ServiceProvider.GetRequiredService<WarmUpService>();
//    await warmUpService.WarmUpAsync();
//    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
//    logger.LogInformation(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs/.log"));
//}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>()!.Value);

app.UseSerilogRequestLogging();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
