using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using CityEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.CityEndpoint;
using static api.Utils.Functions;
using Microsoft.AspNetCore.Mvc;
using static api.Utils.Messages.EndpointMetadata;

namespace api.Routes
{
    public static class CityRoutes
    {
        public static void RegisterCityAPI(WebApplication app)
        {
            const string API_CITY_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.CITY_ROUTE}";
            app.MapGet(API_CITY_ROUTE_COMPLETE, async (DBContext db, [FromQuery] string? sortBy, [FromQuery] string? sortDirection) =>
            {
                 var queryCities = db.Regions.AsQueryable();
                (queryCities, var isValidSort) = ApplySorting(queryCities, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var listCities = await queryCities.ToListAsync();
                return Results.Ok(listCities);
            })
            .Produces<List<City>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: CityEndpointMetadataMessages.MESSAGE_CITY_LIST_SUMMARY,
                 description: CityEndpointMetadataMessages.MESSAGE_CITY_LIST_DESCRIPTION
                 ));

            app.MapGet($"{API_CITY_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var city = await db.Cities.Include(p=> p.Department).SingleOrDefaultAsync(p=> p.Id == id);
                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .Produces<City?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: CityEndpointMetadataMessages.MESSAGE_CITY_BYID_SUMMARY,
                 description: CityEndpointMetadataMessages.MESSAGE_CITY_BYID_DESCRIPTION
                 ));

            app.MapGet($"{API_CITY_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var city = db.Cities.Where(x => x.Name.ToUpper().Equals(name.Trim().ToUpper())).ToList(); 
                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .Produces<List<City>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: CityEndpointMetadataMessages.MESSAGE_CITY_BYNAME_SUMMARY, 
                description: CityEndpointMetadataMessages.MESSAGE_CITY_BYNAME_DESCRIPTION
                ));

            app.MapGet($"{API_CITY_ROUTE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbCities = db.Cities.ToList();
                var cities = Functions.FilterObjectListPropertiesByKeyword<City>(dbCities, wellFormedKeyword);
                
                if (!cities.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(cities);
            })
            .Produces<List<City>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: CityEndpointMetadataMessages.MESSAGE_CITY_SEARCH_SUMMARY,
                 description: CityEndpointMetadataMessages.MESSAGE_CITY_SEARCH_DESCRIPTION
                 ));


            app.MapGet($"{API_CITY_ROUTE_COMPLETE}/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }
 
                var sortBy = pagination.SortBy ?? string.Empty; 
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty; 
                var queryCities = db.Cities.AsQueryable();
 
                (queryCities, var isValidSort) = ApplySorting(queryCities, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var totalRecords = await queryCities.CountAsync();

                var pagedCities = await queryCities
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                if (!pagedCities.Any())
                {
                    return Results.NotFound();
                }

                var paginationResponse = new PaginationResponseModel<City>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pagedCities
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<City>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
               summary: CityEndpointMetadataMessages.MESSAGE_CITY_PAGEDLIST_SUMMARY,
                description: CityEndpointMetadataMessages.MESSAGE_CITY_PAGEDLIST_DESCRIPTION
                ));
        }
    }
}
