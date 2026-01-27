using api.Models;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using static api.Utils.Functions;
using static api.Utils.Messages.EndpointMetadata;
using HeritageCityMetadataMessages = api.Utils.Messages.EndpointMetadata.HeritageCityEndpoint;

namespace api.Routes
{
    public static class HeritageCityRoutes
    {
        public static void RegisterHeritageCityAPI(WebApplication app)
        {
            const string API_HERITAGE_CITY_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.HERITAGE_CITY_ROUTE}";
            app.MapGet(API_HERITAGE_CITY_COMPLETE, async (DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                var queryHeritageCities = db.HeritageCities
                    .Include(p => p.Department)
                    .Include(p => p.City)
                    .AsQueryable();

                (queryHeritageCities, var isValidSort) = ApplySorting(queryHeritageCities, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var listHeritageCities = await queryHeritageCities.ToListAsync();
                return Results.Ok(listHeritageCities);
            })
            .Produces<List<HeritageCity>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: HeritageCityMetadataMessages.MESSAGE_HERITAGE_CITY_LIST_SUMMARY,
                description: HeritageCityMetadataMessages.MESSAGE_HERITAGE_CITY_LIST_DESCRIPTION
            ));

            app.MapGet($"{API_HERITAGE_CITY_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var heritageCity = await db.HeritageCities
                    .Include(p => p.Department)
                    .Include(p => p.City)
                    .SingleOrDefaultAsync(p => p.Id == id);

                if (heritageCity is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(heritageCity);
            })
            .Produces<HeritageCity?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: HeritageCityMetadataMessages.MESSAGE_HERITAGE_CITY_BYID_SUMMARY,
                description: HeritageCityMetadataMessages.MESSAGE_HERITAGE_CITY_BYID_DESCRIPTION
            ));

            app.MapGet($"{API_HERITAGE_CITY_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var search = name.Trim().ToUpperInvariant();
                var heritageCities = db.HeritageCities
                    .Include(p => p.Department)
                    .Include(p => p.City)
                    .Where(x => (x.Name ?? string.Empty).ToUpperInvariant().Contains(search))
                    .ToList();
                return Results.Ok(heritageCities);
            })
            .Produces<List<HeritageCity>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: HeritageCityMetadataMessages.MESSAGE_HERITAGE_CITY_BYNAME_SUMMARY,
                description: HeritageCityMetadataMessages.MESSAGE_HERITAGE_CITY_BYNAME_DESCRIPTION
            ));

            app.MapGet($"{API_HERITAGE_CITY_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbHeritageCities = db.HeritageCities
                    .Include(p => p.Department)
                    .Include(p => p.City)
                    .ToList();
                var heritageCities = Functions.FilterObjectListPropertiesByKeyword<HeritageCity>(dbHeritageCities, wellFormedKeyword);
                return Results.Ok(heritageCities);
            })
            .Produces<List<HeritageCity>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: HeritageCityMetadataMessages.MESSAGE_HERITAGE_CITY_SEARCH_SUMMARY,
                description: HeritageCityMetadataMessages.MESSAGE_HERITAGE_CITY_SEARCH_DESCRIPTION
            ));

            app.MapGet($"{API_HERITAGE_CITY_COMPLETE}/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }

                var sortBy = pagination.SortBy ?? string.Empty;
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty;
                var queryHeritageCities = db.HeritageCities
                    .Include(p => p.Department)
                    .Include(p => p.City)
                    .AsQueryable();

                (queryHeritageCities, var isValidSort) = ApplySorting(queryHeritageCities, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var totalRecords = await queryHeritageCities.CountAsync();
                var pagedHeritageCities = await queryHeritageCities
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                var paginationResponse = new PaginationResponseModel<HeritageCity>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pagedHeritageCities
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<HeritageCity>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: HeritageCityMetadataMessages.MESSAGE_HERITAGE_CITY_PAGEDLIST_SUMMARY,
                description: HeritageCityMetadataMessages.MESSAGE_HERITAGE_CITY_PAGEDLIST_DESCRIPTION
            ));
        }
    }
}
