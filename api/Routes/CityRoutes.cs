namespace api.Routes
{
    public static class CityRoutes
    {
        public static void RegisterCityAPI(WebApplication app)
        {
            const string CITY_ROUTE = "City";

            app.MapGet($"api/v1/{CITY_ROUTE}", (DBContext db) =>
            {
                return Results.Ok(db.Cities.ToList());
            });

            app.MapGet($"api/v1/{CITY_ROUTE}/{{id}}", async (int id, DBContext db) =>
            {
                var city = await db.Cities.FindAsync(id);

                if (city != null)
                {
                    return Results.Ok(city);
                }
                else
                {
                    return Results.NotFound();
                }
            });

            app.MapGet($"api/v1/{CITY_ROUTE}/Name/{{name}}", (string name, DBContext db) =>
            {
                var city = db.Cities.Where(x => x.Name.ToUpper() == name.ToUpper()).ToList();

                if (city != null)
                {
                    return Results.Ok(city);
                }
                else
                {
                    return Results.NotFound();
                }
            });
        }
    }
}
