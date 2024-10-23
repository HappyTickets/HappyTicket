using Blazor.WebAssembly.DynamicCulture.Extensions;
using Blazored.LocalStorage;
using Client;
using Client.Services._File;
using Client.Services._HttpClientFacade;
using Client.Services.AuthStateProvider;
using Client.Services.Helper;
using Client.Services.Identity;
using Client.Services.Implementation;
using Client.Services.Implementation.UI;
using Client.Services.Interfaces;
using Client.Services.Interfaces.UI;
using Client.Services.Sponsors;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Shared.Common.General;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var appSettings = builder.Configuration.GetSection("BaseUrls");
var blazorBaseUrl = appSettings["BlazorBaseUrl"] ?? throw new NullReferenceException("BlazorBaseUrl configuration is missing.");
var apiBaseUrl = appSettings["ApiBaseUrl"] ?? throw new NullReferenceException("ApiBaseUrl configuration is missing.");
UrlHelper.SetCommunicationUrls(blazorBaseUrl, apiBaseUrl);

// localization
builder.Services
    .AddLocalization()
    .AddLocalizationDynamic(options =>
{
    options.SetDefaultCulture("ar-SA");
    options.AddSupportedCultures("ar-SA", "ar-EG", "en-US", "en-GB");
    options.AddSupportedUICultures("ar-SA", "ar-EG", "en-US", "en-GB");
});

// services
builder.Services
    .AddBlazoredLocalStorage()
    .AddMudServices();

// authorization
builder.Services
    .AddAuthorizationCore(conf => conf.AddPolicy("IsAdmin", policy => policy.RequireAuthenticatedUser().RequireClaim("Role", "Admin")))
    .AddCascadingAuthenticationState()
    .AddScoped<CustomAuthStateProvider>()
    .AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>())
    .AddScoped<IAuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());

// http client
builder.Services
    .AddTransient(sp => new HttpClient(new LocalizationDelegatingHandler()) { BaseAddress = new Uri(appSettings["ApiBaseUrl"]!) })
    .AddScoped<IHttpClientFacade, HttpClientFacade>();

// dependency injection
builder.Services
    .AddScoped<IIdentityService, IdentityService>()
    .AddScoped<ISponsorService, SponsorService>()
    .AddScoped<IFileService, FileService>()
    .AddScoped<IPopUpService, PopUpService>()
    .AddScoped<ICustomSnackbarProvider, CustomSnackbarProvider>();

//builder.Services.AddScoped(typeof(IPaginationSearchModel), typeof(PaginationSearchModel));
//builder.Services.AddScoped(typeof(IApiResponse), typeof(ApiResponse));
//builder.Services.AddScoped(typeof(BIMatchService), typeof(BMatchService));
//builder.Services.AddScoped(typeof(BIStadiumService), typeof(BStadiumService));
//builder.Services.AddScoped(typeof(BITeamService), typeof(BTeamService));
//builder.Services.AddScoped(typeof(BITicketService), typeof(BTicketService));
//builder.Services.AddScoped(typeof(BIFavoriteTeamService), typeof(BFavoriteTeamService));
//builder.Services.AddScoped(typeof(BICartService), typeof(BCartService));
builder.Services.AddScoped(typeof(ITokenExpirationService), typeof(TokenExpirationService));
//builder.Services.AddScoped<IBOrderService, BOrderService>();
builder.Services.AddTransient<MudRTLProvider>();

//builder.Services.AddScoped<IIdentityService, IdentityService>();
//builder.Services.AddScoped<IFileService, FileService>();
//builder.Services.AddScoped<ISponsorService, SponsorService>();
//builder.Services.AddScoped<IChampionService, ChampionService>();



var app = builder.Build();

await app.SetMiddlewareCulturesAsync();
//await app.SetDefaultCulture();

await app.RunAsync();

