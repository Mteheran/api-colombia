using System;
using api.Utils;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;
using static api.Utils.Messages.EndpointMetadata;

namespace api.Routes
{
    public static class CategoryNaturalAreaRoutes
    {
        public static void RegisterCategoryNaturalAreaAPI(WebApplication app)
        {
            const string API_CATEGORY_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.CATEGORY_NATURAL_AREA}";

            app.MapGet($"{API_CATEGORY_ROUTE_COMPLETE}/", async (DBContext db) =>
            {
                return Results.Ok(await db.CategoryNaturalAreas.ToListAsync());
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary: CategoryNaturalAreaEndpoint.MESSAGE_LIST_SUMMARY,
                description: CategoryNaturalAreaEndpoint.MESSAGE_LIST_DESCRIPTION
                ));

            app.MapGet($"{API_CATEGORY_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var region = await db.CategoryNaturalAreas
                .SingleOrDefaultAsync(p => p.Id == id);

                if (region is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(region);
            })
          .WithMetadata(new SwaggerOperationAttribute(
              summary: CategoryNaturalAreaEndpoint.MESSAGE_BYID_SUMMARY,
              description: CategoryNaturalAreaEndpoint.MESSAGE_BYID_DESCRIPTION));
        }
    }
}

