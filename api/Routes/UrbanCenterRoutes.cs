using api.Models;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using static api.Utils.Functions;
using static api.Utils.Messages.EndpointMetadata;

namespace api.Routes
{
    public static class UrbanCenterRoutes
    {
        public static void RegisterUrbanCenterAPI(WebApplication app)
        {
            const string API_URBAN_CENTER_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.URBAN_CENTER_ROUTE}";
            const string API_URBAN_CENTER_TAG = "UrbanCenter";
            IEndpointRouteBuilder group = app.MapGroup(API_URBAN_CENTER_ROUTE_COMPLETE).WithTags(API_URBAN_CENTER_TAG).CacheOutput();

            group.MapGet(string.Empty, async (DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                var queryUrbanCenters = db.UrbanCenters.AsQueryable();
                (queryUrbanCenters, var isValidSort) = ApplySorting(queryUrbanCenters, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var listUrbanCenters = await queryUrbanCenters.ToListAsync();
                return Results.Ok(listUrbanCenters);
            })
            .Produces<List<UrbanCenter>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: UrbanCenterEndpoint.MESSAGE_URBAN_CENTER_LIST_SUMMARY,
                description: UrbanCenterEndpoint.MESSAGE_URBAN_CENTER_LIST_DESCRIPTION
            ));

            group.MapGet("/{id}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var urbanCenter = await db.UrbanCenters.SingleOrDefaultAsync(p => p.Id == id);

                if (urbanCenter is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(urbanCenter);
            })
            .Produces<UrbanCenter?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: UrbanCenterEndpoint.MESSAGE_URBAN_CENTER_BYID_SUMMARY,
                description: UrbanCenterEndpoint.MESSAGE_URBAN_CENTER_BYID_DESCRIPTION
                ));

            group.MapGet("/code/{code}", async (string code, DBContext db) =>
            {
                var urbanCenters = await db.UrbanCenters.Where(x => x.Code.ToUpper().Equals(code.Trim().ToUpper())).ToListAsync();

                if (urbanCenters is null || !urbanCenters.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(urbanCenters);
            })
            .Produces<List<UrbanCenter>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: UrbanCenterEndpoint.MESSAGE_URBAN_CENTER_BYCODE_SUMMARY,
                description: UrbanCenterEndpoint.MESSAGE_URBAN_CENTER_BYCODE_DESCRIPTION
                ));

            group.MapGet("/city/{cityId}", async (int cityId, DBContext db) =>
            {
                if (cityId <= 0)
                {
                    return Results.BadRequest();
                }

                var urbanCenters = await db.UrbanCenters.Where(x => x.CityId == cityId).ToListAsync();

                if (urbanCenters is null || !urbanCenters.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(urbanCenters);
            })
            .Produces<List<UrbanCenter>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: UrbanCenterEndpoint.MESSAGE_URBAN_CENTER_BYCITY_SUMMARY,
                description: UrbanCenterEndpoint.MESSAGE_URBAN_CENTER_BYCITY_DESCRIPTION
                ));

            group.MapGet("/search/{keyword}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbUrbanCenters = db.UrbanCenters.ToList();
                var urbanCenters = Functions.FilterObjectListPropertiesByKeyword<UrbanCenter>(dbUrbanCenters, wellFormedKeyword);
                
                if (urbanCenters is null || !urbanCenters.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(urbanCenters);
            })
            .Produces<List<UrbanCenter>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: UrbanCenterEndpoint.MESSAGE_URBAN_CENTER_SEARCH_SUMMARY,
                description: UrbanCenterEndpoint.MESSAGE_URBAN_CENTER_SEARCH_DESCRIPTION
                ));

            group.MapGet("/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }

                var sortBy = pagination.SortBy ?? string.Empty;
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty;
                var queryUrbanCenters = db.UrbanCenters.AsQueryable();

                (queryUrbanCenters, var isValidSort) = ApplySorting(queryUrbanCenters, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var totalRecords = await queryUrbanCenters.CountAsync();

                var pageUrbanCenters = await queryUrbanCenters
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                if (!pageUrbanCenters.Any())
                {
                    return Results.NotFound();
                }

                var paginationResponse = new PaginationResponseModel<UrbanCenter>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pageUrbanCenters
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<UrbanCenter>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: UrbanCenterEndpoint.MESSAGE_URBAN_CENTER_PAGEDLIST_SUMMARY,
                description: UrbanCenterEndpoint.MESSAGE_URBAN_CENTER_PAGEDLIST_DESCRIPTION
            ));
        }
    }
}
