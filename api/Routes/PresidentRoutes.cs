﻿using api.Utils;

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

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{Util.QP_ID}", async (int id, DBContext db) =>
            {
                var president = await db.Presidents.FindAsync(id);

                if (president is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(president);
            });

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{Util.NAME}/{Util.QP_NAME}", (string name, DBContext db) =>
            {
                var president = db.Presidents.Where(x => x.Name!.ToUpper().Equals(name.Trim().ToUpper())).ToList();

                if (president is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(president);
            });
        }
    }
}
