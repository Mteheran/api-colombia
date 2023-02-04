using api.Utils;
using Microsoft.EntityFrameworkCore;


namespace api.Routes
{
    public static class PresidentRoutes
    {
        public static void RegisterPresidentApi(WebApplication app)
        {
            const string API_DEPARTMENT_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.PRESIDENT_ROUTE}";

            app.MapGet(API_DEPARTMENT_ROUTE_COMPLETE, (DBContext db) =>
            {
                return Results.Ok(db.Presidents.ToList());
            });

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                var president = await db.Presidents
                                        .Include(p => p.City)
                                        .SingleAsync(p=> p.Id == id);

                if (president is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(president);
            });

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var president = db.Presidents.Where(x => x.Name!.ToUpper().Equals(name.Trim().ToUpper())).ToList();

                if (president is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(president);
            });


            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/year/{{year}}", async (int year, DBContext db) =>
            {
                var presidents = db.Presidents
                                        .Include(p=> p.City)
                                        .Where(p=> p.StartPeriodDate.Year >= year
                                         && p.EndPeriodDate.Year <= year);

                return Results.Ok(presidents);
            });
        }
    }
}
