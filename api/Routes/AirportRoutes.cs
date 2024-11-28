using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using static api.Utils.Functions;
using static api.Utils.Messages.EndpointMetadata;
using AirportMetadataMessages = api.Utils.Messages.EndpointMetadata.AirportEndpoint;
using Microsoft.AspNetCore.Mvc;


namespace api.Routes
{
    public static class AirportRoutes
    {
        public static void RegisterAirportAPI(WebApplication app)
        {
            const string API_AIRPORT_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.AIRPORT}";
            app.MapGet(API_AIRPORT_COMPLETE, async (DBContext db,
                [FromQuery, SwaggerParameter(Description = "It can be sorted by any of the fields that have numerical, string, or date values (for example: Id, name, description, etc.).")] string? sortBy,
                [FromQuery, SwaggerParameter(Description = "Possible values: 'asc' or 'desc'.")] string? sortDirection) =>
              {
                  var queryAirports = db.Airports
                   .Include(p => p.Department)
                   .Include(p => p.City)
                   .AsQueryable();

                  (queryAirports, var isValidSort) = ApplySorting(queryAirports, sortBy, sortDirection);

                  if (!isValidSort)
                  {
                      return Results.BadRequest(RequestMessages.BadRequest);
                  }

                  var listAirports = await queryAirports.ToListAsync();
                  return Results.Ok(listAirports);
              })
               .Produces<List<Airport>?>(200)
               .WithMetadata(new SwaggerOperationAttribute(
                   summary: AirportMetadataMessages.MESSAGE_AIRPORT_LIST_SUMMARY,
                    description: AirportMetadataMessages.MESSAGE_AIRPORT_LIST_DESCRIPTION
                    ));

            app.MapGet($"{API_AIRPORT_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var city = await db.Airports
                .Include(p => p.Department)
                .Include(p => p.City).SingleOrDefaultAsync(p => p.Id == id);
                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .Produces<City?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: AirportMetadataMessages.MESSAGE_AIRPORT_BYID_SUMMARY,
                 description: AirportMetadataMessages.MESSAGE_AIRPORT_BYID_DESCRIPTION
                 ));

            app.MapGet($"{API_AIRPORT_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var city = db.Airports
                .Include(p => p.Department)
                .Include(p => p.City).Where(x => x.Name.ToUpper().Equals(name.Trim().ToUpper())).ToList();
                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .Produces<List<City>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: AirportMetadataMessages.MESSAGE_AIRPORT_BYNAME_SUMMARY,
                description: AirportMetadataMessages.MESSAGE_AIRPORT_BYNAME_DESCRIPTION
                ));

            app.MapGet($"{API_AIRPORT_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbAirports = db.Airports
                .Include(p => p.Department)
                .Include(p => p.City).ToList();
                var Airports = Functions.FilterObjectListPropertiesByKeyword<Airport>(dbAirports, wellFormedKeyword);

                if (!Airports.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(Airports);
            })
            .Produces<List<Airport>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: AirportMetadataMessages.MESSAGE_AIRPORT_SEARCH_SUMMARY,
                 description: AirportMetadataMessages.MESSAGE_AIRPORT_SEARCH_DESCRIPTION
                 ));

            app.MapGet($"{API_AIRPORT_COMPLETE}/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {

                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }
                var sortBy = pagination.SortBy ?? string.Empty;
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty;

                var queryAirports = db.Airports
                    .Include(p => p.Department)
                    .Include(p => p.City)
                    .AsQueryable();

                (queryAirports, var isValidSort) = ApplySorting(queryAirports, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }
                var totalRecords = await queryAirports.CountAsync();

                var pagedAirports = await queryAirports
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                if (!pagedAirports.Any())
                    return Results.NotFound();

                var paginationResponse = new PaginationResponseModel<Airport>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pagedAirports
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<Airport>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
               summary: AirportMetadataMessages.MESSAGE_AIRPORT_PAGEDLIST_SUMMARY,
                description: AirportMetadataMessages.MESSAGE_AIRPORT_PAGEDLIST_DESCRIPTION
                ));
        }
    }
}

