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
    public static class NaturalAreaRoutes
    {
        public static void RegisterNaturalAreaAPI(WebApplication app)
        {
            const string API_NATURALAREA_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.NATURAL_AREA}";

            app.MapGet($"{API_NATURALAREA_ROUTE_COMPLETE}/", async (DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                 var queryNaturalAreas = db.NaturalAreas.AsQueryable();
                (queryNaturalAreas, var isValidSort) = ApplySorting(queryNaturalAreas, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var listNaturalAreas = await queryNaturalAreas.ToListAsync();
                return Results.Ok(listNaturalAreas);
            })
            .Produces<List<NaturalArea>>(200)
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
                                                    .Include(p => p.CategoryNaturalArea).IgnoreAutoIncludes()
                                                    .Include(p => p.Department).IgnoreAutoIncludes()
                                                    .Where(x => (x.Name ?? string.Empty).ToUpper().Contains(name.Trim().ToUpper()))
                                                    .ToListAsync();

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
        
                    var sortBy = pagination.SortBy ?? string.Empty; 
                    var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty;
        
                    var queryNaturalAreas = db.NaturalAreas.AsQueryable(); 
                    (queryNaturalAreas, var isValidSort) = ApplySorting(queryNaturalAreas, sortBy, sortDirectionStr);

                    if (!isValidSort)
                    {
                        return Results.BadRequest(RequestMessages.BadRequest);
                    }
        
                    var totalRecords = await queryNaturalAreas.CountAsync();
        
                    var pagedNaturalAreas = await queryNaturalAreas
                        .Skip((pagination.Page - 1) * pagination.PageSize)
                        .Take(pagination.PageSize)
                        .ToListAsync();

                    if (!pagedNaturalAreas.Any())
                    {
                        return Results.NotFound();
                    }
        
                    var paginationResponse = new PaginationResponseModel<NaturalArea>
                    {
                        Page = pagination.Page,
                        PageSize = pagination.PageSize,
                        TotalRecords = totalRecords,
                        Data = pagedNaturalAreas
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