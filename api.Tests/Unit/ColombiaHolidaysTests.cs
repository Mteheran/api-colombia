using api.Routes;

namespace api.Tests.Unit;

/// <summary>
/// The holiday calendar is computed, not stored, so its correctness is pure logic and belongs in a
/// unit test. The integration tests check shape (right year, right month, Chiquinquirá's rule); these
/// pin whole official calendars, which is what catches an off-by-one in the Easter computation or
/// in the "Ley Emiliani" move-to-Monday rule.
/// </summary>
public class ColombiaHolidaysTests
{
    private static List<DateTime> Dates(int year, bool includeSundays = false) =>
        [.. ColombiaHolidays.GetHolidaysByYear(year, includeSundays).Select(h => h.Date)];

    private static DateTime D(int year, int month, int day) => new(year, month, day);

    /// <summary>The official 2024 calendar (18 holidays).</summary>
    [Fact]
    public void Calendar2024_MatchesTheOfficialCalendar()
    {
        DateTime[] expected =
        [
            D(2024, 1, 1), D(2024, 1, 8), D(2024, 3, 25), D(2024, 3, 28), D(2024, 3, 29),
            D(2024, 5, 1), D(2024, 5, 13), D(2024, 6, 3), D(2024, 6, 10), D(2024, 7, 1),
            D(2024, 7, 20), D(2024, 8, 7), D(2024, 8, 19), D(2024, 10, 14), D(2024, 11, 4),
            D(2024, 11, 11), D(2024, 12, 8), D(2024, 12, 25),
        ];

        Assert.Equal(expected, Dates(2024));
    }

    /// <summary>
    /// The official 2025 calendar. Sacred Heart and Saint Peter &amp; Paul both land on Monday 30
    /// June that year, so the date appears twice — two holidays, one day off.
    /// </summary>
    [Fact]
    public void Calendar2025_MatchesTheOfficialCalendar()
    {
        DateTime[] expected =
        [
            D(2025, 1, 1), D(2025, 1, 6), D(2025, 3, 24), D(2025, 4, 17), D(2025, 4, 18),
            D(2025, 5, 1), D(2025, 6, 2), D(2025, 6, 23), D(2025, 6, 30), D(2025, 6, 30),
            D(2025, 7, 20), D(2025, 8, 7), D(2025, 8, 18), D(2025, 10, 13), D(2025, 11, 3),
            D(2025, 11, 17), D(2025, 12, 8), D(2025, 12, 25),
        ];

        Assert.Equal(expected, Dates(2025));
    }

    [Theory]
    [InlineData(2024, 3, 31)]
    [InlineData(2025, 4, 20)]
    [InlineData(2026, 4, 5)]
    [InlineData(2027, 3, 28)]
    [InlineData(2038, 4, 25)] // latest possible Easter in the century
    [InlineData(2285, 3, 22)] // earliest possible Easter
    public void IncludeSundays_AddsPalmSundayAndEasterSunday(int year, int month, int day)
    {
        var easter = D(year, month, day);

        var withSundays = Dates(year, includeSundays: true);
        var without = Dates(year);

        Assert.Contains(easter, withSundays);
        Assert.Contains(easter.AddDays(-7), withSundays);
        Assert.DoesNotContain(easter, without);
        Assert.Equal(without.Count + 2, withSundays.Count);

        // Holy Thursday and Good Friday are anchored to the same Easter.
        Assert.Contains(easter.AddDays(-3), without);
        Assert.Contains(easter.AddDays(-2), without);
    }

    [Theory]
    [InlineData(2024)]
    [InlineData(2025)]
    [InlineData(2026)]
    [InlineData(2030)]
    public void Calendar_IsSortedAndEveryMovableHolidayIsAMonday(int year)
    {
        var holidays = ColombiaHolidays.GetHolidaysByYear(year);
        var dates = holidays.Select(h => h.Date).ToList();

        Assert.Equal(dates.OrderBy(d => d).ToList(), dates);

        // Only these are fixed (or computed off Easter as Thursday/Friday); everything else moves to Monday.
        var fixedDays = new HashSet<(int, int)> { (1, 1), (5, 1), (7, 20), (8, 7), (12, 8), (12, 25) };
        var easterBased = holidays.Where(h => h.Date.DayOfWeek is DayOfWeek.Thursday or DayOfWeek.Friday
                                              && h.Date.Month is 3 or 4).ToList();

        var movable = holidays
            .Where(h => !fixedDays.Contains((h.Date.Month, h.Date.Day)))
            .Except(easterBased)
            // Chiquinquirá keeps a weekend date (Ley 2578 de 2026).
            .Where(h => h.Date.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday));

        Assert.All(movable, h => Assert.Equal(DayOfWeek.Monday, h.Date.DayOfWeek));
    }

    [Fact]
    public void BeforeTheLaw_ChiquinquiraIsNotAHoliday()
    {
        Assert.Equal(18, Dates(2025).Count);
        Assert.Equal(19, Dates(2026).Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(9999)]
    public void Calendar_AtTheEdgesOfDateTime_DoesNotOverflow(int year)
    {
        var dates = Dates(year, includeSundays: true);

        Assert.NotEmpty(dates);
        Assert.All(dates, d => Assert.Equal(year, d.Year));
    }

    [Fact]
    public void EveryHoliday_HasAName() =>
        Assert.All(ColombiaHolidays.GetHolidaysByYear(2026, includeSundays: true),
            h => Assert.False(string.IsNullOrWhiteSpace(h.Name)));
}
