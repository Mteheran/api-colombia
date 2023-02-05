using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

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
            })
            .WithMetadata(new SwaggerOperationAttribute(summary: Messages.MESSAGE_CITY_LIST_SUMMARY, description: Messages.MESSAGE_CITY_LIST_DESCRIPTION));

            app.MapGet($"{API_CITY_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                var city = await db.Cities.Include(p=> p.Departament).SingleAsync(p=> p.Id == id);

                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .WithMetadata(new SwaggerOperationAttribute(summary: Messages.MESSAGE_CITY_BYID_SUMMARY, description: Messages.MESSAGE_CITY_BYID_DESCRIPTION));

            app.MapGet($"{API_CITY_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var city = db.Cities.Where(x => x.Name.ToUpper().Equals(name.Trim().ToUpper())).ToList();

                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .WithMetadata(new SwaggerOperationAttribute(summary: Messages.MESSAGE_CITY_BYNAME_SUMMARY, description: Messages.MESSAGE_CITY_BYNAME_DESCRIPTION));

        }
    }
}
