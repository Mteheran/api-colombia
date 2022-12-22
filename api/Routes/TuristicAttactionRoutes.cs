namespace api.Routes
{
    public static class TuristicAttactionRoutes
    {
        public static void RegisterTuristicAttactionAPI(WebApplication app)
        {
            const string TURISTIC_ROUTE = "TurusticAttaction";

            app.MapGet($"api/v1/{TURISTIC_ROUTE}", (DBContext db) =>
            {
                return Results.Ok(db.TouristAttractions.ToList());
            });

            app.MapGet($"api/v1/{TURISTIC_ROUTE}/{{id}}", async (int id, DBContext db) =>
            {
                var turisticAtt = await db.TouristAttractions.FindAsync(id);

                if (turisticAtt != null)
                {
                    return Results.Ok(turisticAtt);
                }
                else
                {
                    return Results.NotFound();
                }
            });

            app.MapGet($"api/v1/{TURISTIC_ROUTE}/Name/{{name}}", (string name, DBContext db) =>
            {
                var turisticAtt = db.TouristAttractions.Where(x => x.Name.ToUpper() == name.ToUpper()).ToList();

                if (turisticAtt != null)
                {
                    return Results.Ok(turisticAtt);
                }
                else
                {
                    return Results.NotFound();
                }
            });
        }
    }
}
