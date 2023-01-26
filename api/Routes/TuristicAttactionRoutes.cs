using api.Utils;

namespace api.Routes
{
    public static class TuristicAttactionRoutes
    {
        public static void RegisterTuristicAttactionAPI(WebApplication app)
        {
            const string API_DEPARTMENT_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.TURISTIC_ROUTE}";

            app.MapGet(API_DEPARTMENT_ROUTE_COMPLETE, (DBContext db) =>
            {
                return Results.Ok(db.TouristAttractions.ToList());
            });

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                var turisticAtt = await db.TouristAttractions.FindAsync(id);

                if (turisticAtt is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(turisticAtt);
            });

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var turisticAtt = db.TouristAttractions.Where(x => x.Name!.ToUpper().Equals(name.ToUpper())).ToList();

                if (turisticAtt is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(turisticAtt);
            });
        }
    }
}
