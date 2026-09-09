using api.Models;

namespace api.Tests.TestData;

internal static class RadioSeed
{
    public const int TotalRows = 3;

    private static readonly string[] SampleStreamers = ["Streamer1", "Streamer2"];

    public static IReadOnlyList<Radio> Rows(CoreGraph core) =>
    [
        new() { Id = 1, Name = "Sample Radio",  CityId = core.Medellin.Id, City = core.Medellin, Frequency = 101.1, Band = "FM", Url = new Uri("https://example.com/radio"), Streamers = SampleStreamers },
        new() { Id = 2, Name = "Another Radio", CityId = core.Medellin.Id, City = core.Medellin, Frequency = 102.2, Band = "AM", Url = new Uri("https://example.com/radio"), Streamers = SampleStreamers },
        new() { Id = 3, Name = "Third Radio",   CityId = core.Medellin.Id, City = core.Medellin, Frequency = 103.3, Band = "AM", Url = new Uri("https://example.com/radio"), Streamers = SampleStreamers }
    ];
}
