namespace api.Routes
{
    public static class PresidentRoutes
    {
        public static void RegisterPresidentApi(WebApplication app)
        {
            string route = "President";

            app.MapGet($"{route}", (DBContext db) =>
            {
                return Results.Ok(db.Presidents.ToList());
            });

            app.MapGet($"{route}/{{id}}", async (int id, DBContext db) =>
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

            app.MapGet($"{route}/Name/{{name}}", (string name, DBContext db) =>
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
