using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static api.Utils.Messages.EndpointMetadata; 
using api.Models; 
using static api.Utils.Functions;
using Microsoft.EntityFrameworkCore;  

namespace api.Routes
{

    public static class TraditionalFairAndFestivalRoutes
    { 
        public static void RegisterTraditionalFairAndFestivalAPI(WebApplication app)
        {

             const string API_TRADICTIONAL_FAIR_AND_FESTIVAL_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.TRADITIONAL_FAIR_AND_FESTIVAL_ROUTE}";

             app.MapGet(API_TRADICTIONAL_FAIR_AND_FESTIVAL_ROUTE_COMPLETE, async (DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                var queryTraditionalFairAndFestival = db.TraditionalFairAndFestival.Include(p => p.City).AsQueryable();
                (queryTraditionalFairAndFestival, var isValidSort) = ApplySorting(queryTraditionalFairAndFestival, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }
 
                var listTraditionalFairAndFestival = await queryTraditionalFairAndFestival.ToListAsync();
                return Results.Ok(listTraditionalFairAndFestival);
            })
            .Produces<List<TraditionalFairAndFestival>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TraditionalFairAndFestivalEndpoint.MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_LIST_SUMMARY,
                description: TraditionalFairAndFestivalEndpoint.MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_LIST_DESCRIPTION
            ));

              app.MapGet($"{API_TRADICTIONAL_FAIR_AND_FESTIVAL_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var traditionalFairAndFestival = await db.TraditionalFairAndFestival
                                            .Include(p => p.City)
                                            .SingleOrDefaultAsync(p => p.Id == id);

                if (traditionalFairAndFestival is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(traditionalFairAndFestival);
            })
            .Produces<TraditionalFairAndFestival?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TraditionalFairAndFestivalEndpoint.MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_BYID_SUMMARY,
                description: TraditionalFairAndFestivalEndpoint.MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_BYID_DESCRIPTION
                ));


               app.MapGet($"{API_TRADICTIONAL_FAIR_AND_FESTIVAL_ROUTE_COMPLETE}/{{id}}/city", async (int id, DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                 if (id <= 0)
                {
                    return Results.BadRequest("Invalid city ID.");
                }

                var queryTraditionalFairAndFestivalByCity = db.TraditionalFairAndFestival.Include(p => p.City).Where(p => p.CityId == id).AsQueryable(); 
                (queryTraditionalFairAndFestivalByCity, var isValidSort) = ApplySorting(queryTraditionalFairAndFestivalByCity, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                if (!queryTraditionalFairAndFestivalByCity.Any())
                {
                    return Results.NotFound();
                }
 
                var traditionalFairAndFestival = await queryTraditionalFairAndFestivalByCity.ToListAsync();
                return Results.Ok(traditionalFairAndFestival);
            })
             .Produces<List<TraditionalFairAndFestival>?>(200)
             .WithMetadata(new SwaggerOperationAttribute(
                 summary: TraditionalFairAndFestivalEndpoint.MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_BYCITY_SUMMARY,
                 description: TraditionalFairAndFestivalEndpoint.MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_BYCITY_DESCRIPTION
                 ));

            app.MapGet($"{API_TRADICTIONAL_FAIR_AND_FESTIVAL_ROUTE_COMPLETE}/name/{{name}}", async (string name, DBContext db) =>
            {
                var traditionalFairAndFestival = await db.TraditionalFairAndFestival.Include(p => p.City).Where(x => x.Name!.ToUpper().Equals(name.Trim().ToUpper())).ToListAsync();

                if (traditionalFairAndFestival is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(traditionalFairAndFestival);
            })
            .Produces<List<TraditionalFairAndFestival>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TraditionalFairAndFestivalEndpoint.MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_BYNAME_SUMMARY,
                description: TraditionalFairAndFestivalEndpoint.MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_BYNAME_DESCRIPTION
                ));

             app.MapGet($"{API_TRADICTIONAL_FAIR_AND_FESTIVAL_ROUTE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbTraditionalFairAndFestival= db.TraditionalFairAndFestival.Include(p => p.City).ToList();
                var traditionalFairAndFestival = Functions.FilterObjectListPropertiesByKeyword<TraditionalFairAndFestival>(dbTraditionalFairAndFestival, wellFormedKeyword);
                if (!dbTraditionalFairAndFestival.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(traditionalFairAndFestival);
            })
            .Produces<List<TraditionalFairAndFestival>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TraditionalFairAndFestivalEndpoint.MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_SEARCH_SUMMARY,
                description: TraditionalFairAndFestivalEndpoint.MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_SEARCH_DESCRIPTION
                ));

             app.MapGet($"{API_TRADICTIONAL_FAIR_AND_FESTIVAL_ROUTE_COMPLETE}/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }
            
                var sortBy = pagination.SortBy ?? string.Empty; 
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty; 
                var queryTraditionalFairAndFestival = db.TraditionalFairAndFestival.Include(p => p.City).AsQueryable(); 
                
                (queryTraditionalFairAndFestival, var isValidSort) = ApplySorting(queryTraditionalFairAndFestival, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var totalRecords = await queryTraditionalFairAndFestival.CountAsync();

                var pageTraditionalFairAndFestival = await queryTraditionalFairAndFestival
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                if (!pageTraditionalFairAndFestival.Any())
                {
                    return Results.NotFound();
                }

                var paginationResponse = new PaginationResponseModel<TraditionalFairAndFestival>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pageTraditionalFairAndFestival
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<TraditionalFairAndFestival>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
            summary: TraditionalFairAndFestivalEndpoint.MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_PAGEDLIST_SUMMARY,
            description: TraditionalFairAndFestivalEndpoint.MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_PAGEDLIST_DESCRIPTION
            ));

        }

    }
}