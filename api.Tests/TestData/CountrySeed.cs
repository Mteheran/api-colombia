using api.Models;

namespace api.Tests.TestData;

internal static class CountrySeed
{
    public const int TotalRows = 1;

    public static IReadOnlyList<Country> Rows(CoreGraph core) =>
    [
        new()
        {
            Id = 1,
            Name = "Colombia",
            InternetDomain = "CO",
            Departments = new List<Department> { core.Antioquia },
            Currency = "Peso",
            CurrencyCode = "COP",
            CurrencySymbol = "$",
            Flags = ["https://restcountries.com/data/col.svg"],
            Borders = ["BRA", "ECU", "PAN", "PER", "VEN"],
            Region = "Americas",
            Languages = ["Spanish"],
            Population = 50882891,
            Surface = 1141748
        }
    ];
}
