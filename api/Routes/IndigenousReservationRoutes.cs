using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using IndigenousReservationEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.IndigenousReservationEndpoint;

namespace api.Routes
{
    public static class IndigenousReservationRoutes
    {
        public static void RegisterIndigenousReservationAPI(WebApplication app)
        {
            const string API_INDIGENOUS_RESERVATION_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.INDIGENOUS_RESERVATION_ROUTE}";
            app.MapGet(API_INDIGENOUS_RESERVATION_COMPLETE, (DBContext db) =>
            {
                var listIndigenousReservations = db.IndigenousReservations
                .Include(p=> p.Department)
                .Include(p=> p.City)
                .Include(p=> p.NativeCommunity).ToList();
                return Results.Ok(listIndigenousReservations);
            })
            .Produces<List<IndigenousReservation>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: IndigenousReservationEndpointMetadataMessages.MESSAGE_INDIGENOUS_RESERVATION_LIST_SUMMARY,
                 description: IndigenousReservationEndpointMetadataMessages.MESSAGE_INDIGENOUS_RESERVATION_LIST_DESCRIPTION
                 ));

            app.MapGet($"{API_INDIGENOUS_RESERVATION_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var city = await db.IndigenousReservations
                .Include(p=> p.Department)
                .Include(p=> p.City)
                .Include(p=> p.NativeCommunity).SingleOrDefaultAsync(p => p.Id == id);
                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .Produces<City?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: IndigenousReservationEndpointMetadataMessages.MESSAGE_INDIGENOUS_RESERVATION_BYID_SUMMARY,
                 description: IndigenousReservationEndpointMetadataMessages.MESSAGE_INDIGENOUS_RESERVATION_BYID_DESCRIPTION
                 ));

            app.MapGet($"{API_INDIGENOUS_RESERVATION_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var city = db.IndigenousReservations
                .Include(p=> p.Department)
                .Include(p=> p.City)
                .Include(p=> p.NativeCommunity).Where(x => x.Name.ToUpper().Equals(name.Trim().ToUpper())).ToList();
                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .Produces<List<City>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: IndigenousReservationEndpointMetadataMessages.MESSAGE_INDIGENOUS_RESERVATION_BYNAME_SUMMARY,
                description: IndigenousReservationEndpointMetadataMessages.MESSAGE_INDIGENOUS_RESERVATION_BYNAME_DESCRIPTION
                ));

            app.MapGet($"{API_INDIGENOUS_RESERVATION_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbIndigenousReservations = db.IndigenousReservations
                 .Include(p=> p.Department)
                .Include(p=> p.City)
                .Include(p=> p.NativeCommunity).ToList();
                var IndigenousReservations = Functions.FilterObjectListPropertiesByKeyword<IndigenousReservation>(dbIndigenousReservations, wellFormedKeyword);

                if (!IndigenousReservations.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(IndigenousReservations);
            })
            .Produces<List<IndigenousReservation>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: IndigenousReservationEndpointMetadataMessages.MESSAGE_INDIGENOUS_RESERVATION_SEARCH_SUMMARY,
                 description: IndigenousReservationEndpointMetadataMessages.MESSAGE_INDIGENOUS_RESERVATION_SEARCH_DESCRIPTION
                 ));


            app.MapGet($"{API_INDIGENOUS_RESERVATION_COMPLETE}/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }

                var IndigenousReservations = db.IndigenousReservations
                 .Include(p=> p.Department)
                .Include(p=> p.City)
                .Include(p=> p.NativeCommunity).Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
                if (!await IndigenousReservations?.AnyAsync())
                {
                    return Results.NotFound();
                }

                var paginationResponse = new PaginationResponseModel<IndigenousReservation>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = await IndigenousReservations.CountAsync(),
                    Data = await IndigenousReservations.ToListAsync()
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<IndigenousReservation>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
               summary: IndigenousReservationEndpointMetadataMessages.MESSAGE_INDIGENOUS_RESERVATION_PAGEDLIST_SUMMARY,
                description: IndigenousReservationEndpointMetadataMessages.MESSAGE_INDIGENOUS_RESERVATION_PAGEDLIST_DESCRIPTION
                ));
        }
    }
}
