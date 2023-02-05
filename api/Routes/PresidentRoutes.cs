using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Routes
{
    public static class PresidentRoutes
    {
        public static void RegisterPresidentApi(WebApplication app)
        {
            const string API_PRESIDENT_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.PRESIDENT_ROUTE}";

            app.MapGet(API_PRESIDENT_ROUTE_COMPLETE, (DBContext db) =>
            {
                return Results.Ok(db.Presidents.ToList());
            })
            .WithMetadata(new SwaggerOperationAttribute(summary: Messages.MESSAGE_PRESIDENT_LIST_SUMMARY, description: Messages.MESSAGE_PRESIDENT_LIST_DESCRIPTION));


            app.MapGet($"{API_PRESIDENT_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                var president = await db.Presidents
                                        .Include(p => p.City)
                                        .SingleAsync(p=> p.Id == id);

                if (president is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(president);
            })
            .WithMetadata(new SwaggerOperationAttribute(summary: Messages.MESSAGE_PRESIDENT_BYID_SUMMARY, description: Messages.MESSAGE_PRESIDENT_BYID_DESCRIPTION));


            app.MapGet($"{API_PRESIDENT_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var president = db.Presidents.Where(x => x.Name!.ToUpper().Equals(name.Trim().ToUpper())).ToList();

                if (president is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(president);
            })
            .WithMetadata(new SwaggerOperationAttribute(summary: Messages.MESSAGE_PRESIDENT_BYNAME_SUMMARY, description: Messages.MESSAGE_PRESIDENT_BYNAME_DESCRIPTION));

            app.MapGet($"{API_PRESIDENT_ROUTE_COMPLETE}/year/{{year}}", async (int year, DBContext db) =>
            {
                var presidents = db.Presidents
                                        .Include(p=> p.City)
                                        .Where(p=> (p.StartPeriodDate.Year <= year
                                         && p.EndPeriodDate.HasValue && p.EndPeriodDate.Value.Year >= year)
                                         || (p.EndPeriodDate == null && p.StartPeriodDate.Year <= year && year <= DateTime.Now.Year));

                return Results.Ok(presidents);
            })
            .WithMetadata(new SwaggerOperationAttribute(summary: Messages.MESSAGE_PRESIDENT_BYYEAR_SUMMARY, description: Messages.MESSAGE_PRESIDENT_BYYEAR_DESCRIPTION));
        }
    }
}
