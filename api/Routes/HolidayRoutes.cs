using api.Models;
using api.Utils;
using Swashbuckle.AspNetCore.Annotations;
using HolidayEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.HolidayEndpoint;


namespace api.Routes
{
    public static class HolidayRoutes
    {
        public static void RegisterHolidayAPI(WebApplication app)
        {
            const string API_HOLIDAY_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.HOLIDAY_ROUTE}";

            app.MapGet($"{API_HOLIDAY_ROUTE_COMPLETE}/year/{{year}}", async (int year, DBContext db) =>
            { 
                 if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
                {
                    return Results.BadRequest(Messages.EndpointMetadata.HolidayEndpoint.BadRequestInvalidYear);
                }

                List<Holiday> holidays = ColombiaHolidays.GetHolidaysByYear(year);
                return Results.Ok(holidays);
            })
            .Produces<List<Holiday>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                 summary: HolidayEndpointMetadataMessages.MESSAGE_HOLIDAY_BY_YEAR_LIST_SUMMARY,
                 description: HolidayEndpointMetadataMessages.MESSAGE_HOLIDAY_BY_YEAR_LIST_DESCRIPTION
            ));

 
            app.MapGet($"{API_HOLIDAY_ROUTE_COMPLETE}/year/{{year}}/month/{{month}}", async (int year, int month, DBContext db) =>
            { 
                 if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
                {
                    return Results.BadRequest(Messages.EndpointMetadata.HolidayEndpoint.BadRequestInvalidYear);
                }

                 if ((month < 1 || month > 12))
                {
                    return Results.BadRequest(Messages.EndpointMetadata.HolidayEndpoint.BadRequestInvalidMonth);
                }
 
                List<Holiday> holidays = ColombiaHolidays.GetHolidaysByYear(year);
                 
                var filteredHolidays = holidays
                    .Where(h => h.Date.Month == month)  
                    .ToList();

                return Results.Ok(filteredHolidays);
            })
            .Produces<List<Holiday>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                 summary: HolidayEndpointMetadataMessages.MESSAGE_HOLIDAY_BY_YEAR_MONTH_LIST_SUMMARY,
                 description: HolidayEndpointMetadataMessages.MESSAGE_HOLIDAY_BY_YEAR_MONTH_LIST_DESCRIPTION
            ));

        } 
    }
}

public static class ColombiaHolidays
{
 public static List<Holiday> GetHolidaysByYear(int year)
{
    HashSet<DateTime> holidayDates = new HashSet<DateTime>();
    List<Holiday> holidays = new List<Holiday>();
 
    int january = (int) Months.January;
    int march = (int) Months.March;
    int may = (int) Months.May;
    int june = (int) Months.June;
    int july = (int) Months.July;
    int august = (int) Months.August;
    int october = (int) Months.Octuber;
    int november = (int) Months.November;
    int december = (int) Months.December;
 

    holidays.Add(new Holiday(new DateTime(year, january, 1), Messages.EndpointMetadata.Holidays.NEW_YEAR_DESCRIPTION));

    DateTime wiseMenDay = GetNextMondayAfterSpecifiyDate(year, baseMonth: january, baseDay: 6);
    holidays.Add(new Holiday(wiseMenDay, Messages.EndpointMetadata.Holidays.WISE_MEN_DESCRIPTION));
 
    DateTime sanJose = GetNextMondayAfterSpecifiyDate(year, baseMonth: march, baseDay: 19);
    holidays.Add(new Holiday(sanJose, Messages.EndpointMetadata.Holidays.SAINT_JOSEPH_DESCRIPTION));

    DateTime easterSunday = CalculateEasterSunday(year);
    DateTime sundaypalms = easterSunday.AddDays(-7);
    DateTime holyThursday = easterSunday.AddDays(-3);
    DateTime holyFriday = easterSunday.AddDays(-2);

    holidays.Add(new Holiday(sundaypalms, Messages.EndpointMetadata.Holidays.SUNDAY_PALMS_DESCRIPTION));
    holidays.Add(new Holiday(holyThursday, Messages.EndpointMetadata.Holidays.HOLY_THURSDAY_DESCRIPTION)); 
    holidays.Add(new Holiday(holyFriday, Messages.EndpointMetadata.Holidays.HOLY_FRIDAY_DESCRIPTION));
    holidays.Add(new Holiday(easterSunday, Messages.EndpointMetadata.Holidays.RESURRECTION_DESCRIPTION));

    holidays.Add(new Holiday(new DateTime(year, month: may, day: 1), Messages.EndpointMetadata.Holidays.LABOR_DAY_DESCRIPTION));
   
    DateTime ascensionDay = GetNextMondayAfterSpecifiyDate(easterSunday.AddDays(39));
    holidays.Add(new Holiday(ascensionDay, Messages.EndpointMetadata.Holidays.ASCENSION_DESCRIPTION));
  
    DateTime corpusChristiDay = GetNextMondayAfterSpecifiyDate(easterSunday.AddDays(60));
    holidays.Add(new Holiday(corpusChristiDay, Messages.EndpointMetadata.Holidays.CORPUS_CHRISTI_DESCRIPTION));
  
    DateTime sacredHeartDay = GetCorpusChristi(corpusChristiDay);
    holidays.Add(new Holiday(sacredHeartDay, Messages.EndpointMetadata.Holidays.SACRED_HEART_JESUS_DESCRIPTION));
 
    DateTime saintPeterPaulDay = GetNextMondayAfterSpecifiyDate(year, baseMonth: june, baseDay: 29);
    holidays.Add(new Holiday(saintPeterPaulDay, Messages.EndpointMetadata.Holidays.SAINT_PETER_PAUL_DESCRIPTION)); 
 
    holidays.Add(new Holiday(new DateTime(year, month: july, day: 20), Messages.EndpointMetadata.Holidays.INDEPENDENCE_DESCRIPTION));
 
    holidays.Add(new Holiday(new DateTime(year, month: august, day: 07), Messages.EndpointMetadata.Holidays.BOYACA_BATTLE_DESCRIPTION));

    DateTime AssumptionOfTheVirginDay = GetNextMondayAfterSpecifiyDate(year, baseMonth: august, baseDay: 15);
    holidays.Add(new Holiday(AssumptionOfTheVirginDay, Messages.EndpointMetadata.Holidays.ASSUMPTION_VIRGIN_DESCRIPTION));

    DateTime raceday = GetNextMondayAfterSpecifiyDate(year, baseMonth: october, baseDay: 12);
    holidays.Add(new Holiday(raceday, Messages.EndpointMetadata.Holidays.RACE_DESCRIPTION));

    DateTime allSantsDay = GetNextMondayAfterSpecifiyDate(year, baseMonth: november, baseDay: 1);
    holidays.Add(new Holiday(allSantsDay, Messages.EndpointMetadata.Holidays.ALL_SANTS_DESCRIPTION));

    DateTime independenceCartagenaDay = GetNextMondayAfterSpecifiyDate(year, baseMonth: november, baseDay: 11);
    holidays.Add(new Holiday(independenceCartagenaDay, Messages.EndpointMetadata.Holidays.CARTAGENA_INDEPENDENCE_DESCRIPTION));

    holidays.Add(new Holiday(new DateTime(year, month: december, day: 8), Messages.EndpointMetadata.Holidays.INMACULATE_CONCEPTION_DESCRIPTION));
    holidays.Add(new Holiday(new DateTime(year, month: december, day: 25), Messages.EndpointMetadata.Holidays.CHRISTMAS_DESCRIPTION));
 
    return holidays.OrderBy(h => h.Date).ToList();
}

    private static DateTime GetNextMondayAfterSpecifiyDate(int year, int baseMonth, int baseDay)
    {
        DateTime selectedDay = new DateTime(year, baseMonth, baseDay);
         
        if (selectedDay.DayOfWeek == DayOfWeek.Monday)
        {
            return selectedDay;
        }
        
        return selectedDay.AddDays((8 - (int)selectedDay.DayOfWeek) % 7);  
    }

        private static DateTime GetNextMondayAfterSpecifiyDate(DateTime baseDate)
     { 
         
        if (baseDate.DayOfWeek == DayOfWeek.Monday)
        {
            return baseDate;
        }
        
        return baseDate.AddDays((8 - (int)baseDate.DayOfWeek) % 7);  
     }

    private static DateTime GetCorpusChristi(DateTime date)
    {  

        int daysToAdd = ((int)DayOfWeek.Monday - (int)date.DayOfWeek + 7) % 7;
        return daysToAdd == 0 ? date.AddDays(7) : date.AddDays(daysToAdd);
    } 
 
    private static DateTime CalculateEasterSunday(int year)
    {
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int month = (h + l - 7 * m + 114) / 31;
        int day = ((h + l - 7 * m + 114) % 31) + 1;

        return new DateTime(year, month, day);
    }
}
 