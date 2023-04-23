using System;
using api.Utils;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;
using static api.Utils.Messages.EndpointMetadata;
using api.Models;
using Microsoft.AspNetCore.Mvc;

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
            .Produces<List<Map>>(200)
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
                .Include(p => p.CategoryNaturalArea)
                .Include(p => p.Department)
                .SingleOrDefaultAsync(p => p.Id == id);

                if (naturalArea is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(naturalArea);
            })
            .Produces<NaturalArea?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
              summary: NaturalAreaEndpoint.MESSAGE_BYID_SUMMARY,
              description: NaturalAreaEndpoint.MESSAGE_BYID_DESCRIPTION));
        

            app.MapGet($"{API_NATURALAREA_ROUTE_COMPLETE}/name/{{name}}", async (string name, DBContext db) =>
            {
                var naturalAreas = await db.NaturalAreas
                                            .Include(p=> p.CategoryNaturalArea).IgnoreAutoIncludes()
                                            .Include(p=> p.Department).IgnoreAutoIncludes()
                                            .Where(x => x.Name!.ToUpper().Equals(name.Trim().ToUpper())).ToListAsync();

                if (naturalAreas is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(naturalAreas);
            })
            .Produces<List<NaturalArea>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: NaturalAreaEndpoint.MESSAGE_BYNAME_SUMMARY,
                description: NaturalAreaEndpoint.MESSAGE_BYNAME_DESCRIPTION
                ));

            app.MapGet($"{API_NATURALAREA_ROUTE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var naturalAreas = db.NaturalAreas.ToList();
                var naturalAreasFiltered = Functions.FilterObjectListPropertiesByKeyword<NaturalArea>(naturalAreas, wellFormedKeyword);
                if (naturalAreasFiltered.Count == 0)
                {
                    return Results.NotFound();
                }

                return Results.Ok(naturalAreasFiltered);
            })
            .Produces<List<NaturalArea>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: NaturalAreaEndpoint.MESSAGE_SEARCH_SUMMARY, 
                description: NaturalAreaEndpoint.MESSAGE_SEARCH_DESCRIPTION
                ));

            app.MapGet($"{API_NATURALAREA_ROUTE_COMPLETE}/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
                  {

                      if (pagination.Page <= 0 || pagination.PageSize <= 0)
                      {
                          return Results.BadRequest();
                      }

                      var naturalAreaPaged = db.NaturalAreas.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
                      if (!await naturalAreaPaged?.AnyAsync())
                      {
                          return Results.NotFound();
                      }

                      var paginationResponse = new PaginationResponseModel<NaturalArea>
                      {
                          Page = pagination.Page,
                          PageSize = pagination.PageSize,
                          TotalRecords = await db.NaturalAreas.CountAsync(),
                          Data = await naturalAreaPaged.ToListAsync(),
                      };

                      return Results.Ok(paginationResponse);
                  })
            .Produces<PaginationResponseModel<NaturalArea>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
             summary: NaturalAreaEndpoint.MESSAGE_PAGEDLIST_SUMMARY,
              description: NaturalAreaEndpoint.MESSAGE_PAGEDLIST_DESCRIPTION
              ));
        }
    }
}