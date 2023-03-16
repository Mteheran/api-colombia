using System;
using api.Utils;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;
using static api.Utils.Messages.EndpointMetadata;

namespace api.Routes
{
    public static class NaturalAreaRoutes
    {
        public static void RegisterNaturalAreaAPI(WebApplication app)
        {
            const string API_NATURALAREA_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.NATURAL_AREA}";

            app.MapGet($"{API_NATURALAREA_ROUTE_COMPLETE}/", async (DBContext db) =>
            {
                return Results.Ok(await db.NaturalAreas.ToListAsync());
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary: NaturalAreaEndpoint.MESSAGE_LIST_SUMMARY,
                description: NaturalAreaEndpoint.MESSAGE_LIST_DESCRIPTION
                ));

            app.MapGet($"{API_NATURALAREA_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var naturalArea = await db.NaturalAreas
                .Include(p=> p.CategoryNaturalArea)
                .SingleOrDefaultAsync(p => p.Id == id);

                if (naturalArea is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(naturalArea);
            })
          .WithMetadata(new SwaggerOperationAttribute(
              summary: NaturalAreaEndpoint.MESSAGE_BYID_SUMMARY,
              description: NaturalAreaEndpoint.MESSAGE_BYID_DESCRIPTION));
        }
    }
}