using System;
using api.Utils;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;
using static api.Utils.Messages.EndpointMetadata;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using static api.Utils.Functions; 

namespace api.Routes
{
    public static class CategoryNaturalAreaRoutes
    {
        public static void RegisterCategoryNaturalAreaAPI(WebApplication app)
        {
            const string API_CATEGORY_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.CATEGORY_NATURAL_AREA}";

            app.MapGet($"{API_CATEGORY_ROUTE_COMPLETE}/", async (DBContext db,
                [FromQuery, SwaggerParameter(Description = "It can be sorted by any of the fields that have numerical, string, or date values (for example: Id, name, description, etc.).")] string? sortBy,
                [FromQuery, SwaggerParameter(Description = "Possible values: 'asc' or 'desc'.")] string? sortDirection) =>
            {
                var queryCategoryNaturalAreas = db.CategoryNaturalAreas.AsQueryable();
                (queryCategoryNaturalAreas, var isValidSort) = ApplySorting(queryCategoryNaturalAreas, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var listCategoryNaturalAreas = await queryCategoryNaturalAreas.ToListAsync();
                return Results.Ok(listCategoryNaturalAreas);
            })
            .Produces<List<CategoryNaturalArea>?>(200)
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

                var region = await db.CategoryNaturalAreas.SingleOrDefaultAsync(p => p.Id == id);

                if (region is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(region);
            })
            .Produces<CategoryNaturalArea?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
              summary: CategoryNaturalAreaEndpoint.MESSAGE_BYID_SUMMARY,
              description: CategoryNaturalAreaEndpoint.MESSAGE_BYID_DESCRIPTION));

            app.MapGet($"{API_CATEGORY_ROUTE_COMPLETE}/{{id}}/NaturalAreas", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var categoryNaturalArea = await db.CategoryNaturalAreas
                    .Include(p => p.NaturalAreas)
                    .ThenInclude(p => p.Department)
                    .SingleOrDefaultAsync(p => p.Id == id);
                if (categoryNaturalArea is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(categoryNaturalArea);
            })
            .Produces<CategoryNaturalArea?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
             summary: CategoryNaturalAreaEndpoint.MESSAGE_BYID_SUMMARY,
             description: CategoryNaturalAreaEndpoint.MESSAGE_BYID_DESCRIPTION));
        }
    }
}

