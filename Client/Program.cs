using Blazor.WebAssembly.DynamicCulture.Extensions;
using Blazored.LocalStorage;
using Client;
using Client.Services.Helper;
using Client.Services.Implementation;
using Client.Services.Implementation.UI;
using Client.Services.Interfaces;
using Client.Services.Interfaces.UI;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Shared.Common.General;
using Shared.Common.Interfaces;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var appSettings = builder.Configuration.GetSection("BaseUrls");
var blazorBaseUrl = appSettings["BlazorBaseUrl"] ?? throw new NullReferenceException("BlazorBaseUrl configuration is missing.");
var apiBaseUrl = appSettings["ApiBaseUrl"] ?? throw new NullReferenceException("ApiBaseUrl configuration is missing.");
UrlHelper.SetCommunicationUrls(blazorBaseUrl, apiBaseUrl);
builder.Services.AddScoped(sp => new HttpClient(new LocalizationDelegatingHandler()) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddLocalization();
builder.Services.AddLocalizationDynamic(options =>
{
    options.SetDefaultCulture("ar-SA");
    options.AddSupportedCultures("ar-SA", "ar-EG", "en-US", "en-GB");
    options.AddSupportedUICultures("ar-SA", "ar-EG", "en-US", "en-GB");
});
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped(typeof(IAuthStateProvider), typeof(AuthStateProvider));

builder.Services.AddScoped<CustomAuthStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddAuthorizationCore(conf => conf.AddPolicy("IsAdmin", policy => policy.RequireAuthenticatedUser().RequireClaim("Role", "Admin")));
//builder.Services.AddHttpClient();

builder.Services.AddScoped(typeof(IPaginationSearchModel), typeof(PaginationSearchModel));
builder.Services.AddScoped(typeof(IHttpClientHelper), typeof(HttpClientHelper));
builder.Services.AddScoped(typeof(IApiResponse), typeof(ApiResponse));
builder.Services.AddScoped(typeof(BIMatchService), typeof(BMatchService));
builder.Services.AddScoped(typeof(BIStadiumService), typeof(BStadiumService));
builder.Services.AddScoped(typeof(BITeamService), typeof(BTeamService));
builder.Services.AddScoped(typeof(BITicketService), typeof(BTicketService));
builder.Services.AddScoped(typeof(BIFavoriteTeamService), typeof(BFavoriteTeamService));
builder.Services.AddScoped(typeof(BICartService), typeof(BCartService));
builder.Services.AddScoped(typeof(ITokenExpirationService), typeof(TokenExpirationService));
builder.Services.AddScoped<ICustomSnackbarProvider, CustomSnackbarProvider>();
builder.Services.AddScoped<IBOrderService, BOrderService>();
builder.Services.AddTransient<MudRTLProvider>();

builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IPopUpService, PopUpService>();

builder.Services.AddMudServices();

var app = builder.Build();

await app.SetMiddlewareCulturesAsync();
//await app.SetDefaultCulture();

await app.RunAsync();

