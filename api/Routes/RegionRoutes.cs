using System;
using api.Utils;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;
using static api.Utils.Messages.EndpointMetadata;
using api.Models;

namespace api.Routes
{
    public static class RegionRoutes
    {
        public static void RegisterRegionAPI(WebApplication app)
        {
            const string API_REGION_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.REGION}";

            app.MapGet($"{API_REGION_ROUTE_COMPLETE}/", async (DBContext db) =>
            {
                var listRegions = await db.Regions.ToListAsync();
                return Results.Ok(listRegions);
            })
            .Produces<List<Region>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: RegionEndpoint.MESSAGE_REGION_LIST_SUMMARY,
                description: RegionEndpoint.MESSAGE_REGION_LIST_DESCRIPTION
                ));

            app.MapGet($"{API_REGION_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var region = await db.Regions
                .SingleOrDefaultAsync(p => p.Id == id);

                if (region is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(region);
            })
            .Produces<Region?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
              summary: RegionEndpoint.MESSAGE_REGION_BYID_SUMMARY,
              description: RegionEndpoint.MESSAGE_REGION_BYID_DESCRIPTION));

            app.MapGet($"{API_REGION_ROUTE_COMPLETE}/{{id}}/deparments", async (int id, DBContext db) =>
             {
                 if (id <= 0)
                 {
                     return Results.BadRequest();
                 }

                 var region = await db.Regions.Include(p => p.Departments)
                     .SingleOrDefaultAsync(p => p.Id == id);

                 if (region is null)
                 {
                     return Results.NotFound();
                 }

                 return Results.Ok(region);
             })
            .Produces<Region?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
               summary: RegionEndpoint.MESSAGE_BYID_DEPARMENTS_SUMMARY,
               description: RegionEndpoint.MESSAGE_BYID_DEPARMENTS_DESCRIPTION));
        }
    }
}

