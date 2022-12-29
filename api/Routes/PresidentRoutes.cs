namespace api.Routes
{
    public static class PresidentRoutes
    {
        public static void RegisterPresidentApi(WebApplication app)
        {
            const string PRESIDENT_ROUTE = "President";

            app.MapGet($"api/v1/{PRESIDENT_ROUTE}", (DBContext db) =>
            {
                return Results.Ok(db.Presidents.ToList());
            });

            app.MapGet($"api/v1/{PRESIDENT_ROUTE}/{{id}}", async (int id, DBContext db) =>
            {
                var president = await db.Presidents.FindAsync(id);

                if (president != null)
                {
                    return Results.Ok(president);
                }
                else
                {
                    return Results.NotFound();
                }
            });

            app.MapGet($"api/v1/{PRESIDENT_ROUTE}/Name/{{name}}", (string name, DBContext db) =>
            {
                var president = db.Presidents.Where(x => x.Name.ToUpper() == name.ToUpper()).ToList();

                if (president != null)
                {
                    return Results.Ok(president);
                }
                else
                {
                    return Results.NotFound();
                }
            });
        }
    }
}
