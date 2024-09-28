using Shared.Configs;
using System.Text.Json;

namespace API.Helper;

public class CountryInfoService
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public CountryInfoService(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    private IEnumerable<Country> _countries = [];
    public IEnumerable<Country> Countries { get { return _countries.Any() ? _countries : GetCountriesInfo(); } }

    private IEnumerable<Country> GetCountriesInfo()
    {
        if (_countries == null || !_countries.Any())
        {
            string countryDataPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "CountryData.json");

            using StreamReader r = new(countryDataPath);
            string json = r.ReadToEnd();
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };
            _countries = (JsonSerializer.Deserialize<CountryRoot>(json, options))?.Country ?? [];
        }
        return _countries;
    }
}
