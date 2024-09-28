namespace Shared.Configs;

public class Country
{
    public string Name { get; set; } = string.Empty;
    public Currency Currency { get; set; } = new();
    public CountryInfo Info { get; set; } = new();
    public IEnumerable<string> Languages { get; set; } = [];
    public PhoneInfo Phone { get; set; } = new();
    public PostalInfo Postal { get; set; } = new();
    public IEnumerable<string> States { get; set; } = [];
}
public class CountryInfo
{
    public string ShortName { get; set; } = string.Empty;
    public string LongName { get; set; } = string.Empty;
    public string Alpha2 { get; set; } = string.Empty;
    public string Alpha3 { get; set; } = string.Empty;
    public string? ISONumericCode { get; set; }
    public string? IOC { get; set; }
    public string? CapitalCity { get; set; }
    public string? TLD { get; set; }
}
public class Currency
{
    public IEnumerable<string>? CurrencyCode { get; set; }
    public IEnumerable<string>? CurrencyName { get; set; }
    public IEnumerable<string>? CurrencySymbol { get; set; }
}
public class PhoneInfo
{
    public string CountryCode { get; set; } = string.Empty;
    public IEnumerable<string> Mobile_Begin_With { get; set; } = [];
    public IEnumerable<string> Phone_Number_Lengths { get; set; } = [];
}
public class PostalInfo
{
    public string Description { get; set; } = string.Empty;
    public string? RedundantCharacters { get; set; }
    public string? ValidationRegex { get; set; }
    public string charSet { get; set; } = string.Empty;
    public IEnumerable<string> postalLengths { get; set; } = [];
    public IEnumerable<string> postalFormats { get; set; } = [];
}

public class CountryRoot
{
    public IEnumerable<Country> Country { get; set; } = [];
}
