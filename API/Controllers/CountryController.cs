using API.Helper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CountryController : BaseController
{
    private readonly CountryInfoService _countryInfoService;
    public CountryController(IHttpContextAccessor httpContextAccessor, CountryInfoService countryInfoService) : base(httpContextAccessor)
    {
        _countryInfoService = countryInfoService;
    }


    [HttpGet]
    [Route("GetCountries")]
    public ObjectResult GetCountries()
    {
        return ReturnRequest(new() { Data = _countryInfoService.Countries });
    }

    [HttpGet]
    [Route("GetCountriesPhoneInfo")]
    public ObjectResult GetCountriesPhoneInfo()
    {
        return ReturnListRequest<IEnumerable<Phone>>(new() { Data = _countryInfoService.Countries.Select(x => new Phone() { Name = x.Info.Alpha2, FullName = string.IsNullOrWhiteSpace(x.Info.LongName) ? x.Info.ShortName : x.Info.LongName, CountryCode = x.Phone.CountryCode, MobileBeginWith = x.Phone.Mobile_Begin_With, PhoneNumberLengths = x.Phone.Phone_Number_Lengths }).ToList() });
    }

    private class Phone
    {
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? CountryCode { get; set; } = string.Empty;
        public IEnumerable<string?>? MobileBeginWith { get; set; }
        public IEnumerable<string?>? PhoneNumberLengths { get; set; }
    }
}
