using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using AirportMetadataMessages = api.Utils.Messages.EndpointMetadata.AirportEndpoint;

namespace api.Routes
{
    public static class AirportRoutes
    {
        public static void RegisterAirportAPI(WebApplication app)
        {
            const string API_AIRPORT_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.AIRPORT}";
            app.MapGet(API_AIRPORT_COMPLETE, async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                var listAirports = db.Airports
                .Include(p => p.Department)
                .Include(p => p.City).ToList();
                return Results.Ok(Functions.SortList(listAirports, pagination));
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

                var Airports = db.Airports
                 .Include(p => p.Department)
                .Include(p => p.City).Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
                if (!await Airports?.AnyAsync())
                {
                    return Results.NotFound();
                }

                var paginationResponse = new PaginationResponseModel<Airport>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = await Airports.CountAsync(),
                    Data = await Airports.ToListAsync()
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

