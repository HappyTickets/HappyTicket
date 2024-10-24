using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;

namespace Client.Services.Extensions
{
    public static class WebAssemblyHostExtension
    {
        public async static Task SetDefaultCulture(this WebAssemblyHost host)
        {
            var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
            var result = await localStorage.GetItemAsync<string>("currentCulture");
            CultureInfo culture;
            if (result != null)
                culture = new CultureInfo(result);
            else
            {
                string defaultCulture = "ar-SA";
                culture = new CultureInfo(defaultCulture);
                await localStorage.SetItemAsStringAsync("currentCulture", defaultCulture);
            }
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
