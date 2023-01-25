using api.Utils;

namespace api.Routes
{
    public static class CityRoutes
    {
        public static void RegisterCityAPI(WebApplication app)
        {
            const string API_CITY_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.CITY_ROUTE}";

            app.MapGet(API_CITY_ROUTE_COMPLETE, (DBContext db) =>
            {
                return Results.Ok(db.Cities.ToList());
            });

            app.MapGet($"{API_CITY_ROUTE_COMPLETE}/{Util.QP_ID}", async (int id, DBContext db) =>
            {
                var city = await db.Cities.FindAsync(id);

                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            });

            app.MapGet($"{API_CITY_ROUTE_COMPLETE}/{Util.QP_NAME}", (string name, DBContext db) =>
            {
                var city = db.Cities.Where(x => x.Name.ToUpper().Equals(name.Trim().ToUpper())).ToList();

                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            });
        }
    }
}
