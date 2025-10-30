using api.Models;
using api.Utils;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;
using TouristAttractionEndpointMetadata = api.Utils.Messages.EndpointMetadata.TouristAttractionsEndpoint;
using static api.Utils.Messages.EndpointMetadata;
using Microsoft.AspNetCore.Mvc;
using static api.Utils.Functions; 

namespace api.Routes
{
    public static class TouristAttractionRoutes
    {
        public static void RegisterTouristAttractionAPI(WebApplication app)
        {
            const string API_TOURISTIC_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.TOURISTIC_ROUTE}";

            app.MapGet(API_TOURISTIC_ROUTE_COMPLETE, async (DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            { 
                var queryTouristAttractions = db.TouristAttractions.Include(p => p.City).AsQueryable();
                (queryTouristAttractions, var isValidSort) = ApplySorting(queryTouristAttractions, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var listTouristAttractions = await queryTouristAttractions.ToListAsync();
                return Results.Ok(listTouristAttractions);
            })
            .Produces<List<TouristAttraction>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_LIST_SUMMARY,
                description: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_LIST_DESCRIPTION));

            app.MapGet($"{API_TOURISTIC_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var turisticAtt = await db.TouristAttractions
                                                        .Include(p => p.City)
                                                        .SingleOrDefaultAsync(p => p.Id == id);

                if (turisticAtt is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(turisticAtt);
            })
            .Produces<TouristAttraction?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_BYID_SUMMARY,
                description: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_BYID_DESCRIPTION));

            app.MapGet($"{API_TOURISTIC_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var search = name.Trim().ToUpperInvariant();
                var touristAttractions = db.TouristAttractions
                    .Include(p => p.City)
                    .Where(x => (x.Name ?? string.Empty).ToUpperInvariant().Contains(search))
                    .ToList();

                return Results.Ok(touristAttractions);
            })
            .Produces<List<TouristAttraction>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_BYNAME_SUMMARY,
                description: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_BYNAME_DESCRIPTION));

            app.MapGet($"{API_TOURISTIC_ROUTE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbTouristAttractions = db.TouristAttractions.ToList();
                var touristAttractions = Functions.FilterObjectListPropertiesByKeyword<TouristAttraction>(dbTouristAttractions, wellFormedKeyword);
                return Results.Ok(touristAttractions);
            })
            .Produces<List<TouristAttraction>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_SEARCH_SUMMARY,
                description: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_SEARCH_DESCRIPTION));

            app.MapGet($"{API_TOURISTIC_ROUTE_COMPLETE}/pagedList",
            async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }
 
                var sortBy = pagination.SortBy ?? string.Empty; 
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty; 
                var queryTouristAttractions = db.TouristAttractions.AsQueryable();
 
                (queryTouristAttractions, var isValidSort) = ApplySorting(queryTouristAttractions, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }
 
                var totalRecords = await queryTouristAttractions.CountAsync();
 
                var pagedTouristAttractions = await queryTouristAttractions
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                if (!pagedTouristAttractions.Any())
                {
                    return Results.NotFound();
                }
 
                var paginationResponse = new PaginationResponseModel<TouristAttraction>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pagedTouristAttractions
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<TouristAttraction>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_PAGEDLIST_SUMMARY,
                description: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_PAGEDLIST_DESCRIPTION
            ));
        }
    }
}
