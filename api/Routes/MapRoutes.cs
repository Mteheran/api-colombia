using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using CountryEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.MapEndpoint; 
using Microsoft.AspNetCore.Mvc; 
using static api.Utils.Functions;
using static api.Utils.Messages.EndpointMetadata;

namespace api.Routes
{
    public static class MapsRoutes
    {
        public static void RegisterCountryAPI(WebApplication app)
        {
            const string API_MAP_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.MAP_ROUTE}";

            app.MapGet($"{API_MAP_ROUTE_COMPLETE}/",async (DBContext db,[FromQuery] string? sortBy, [FromQuery] string? sortDirection) =>
            {
                 var queryMaps = db.Maps.AsQueryable();
                (queryMaps, var isValidSort) = ApplySorting(queryMaps, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var maps = await queryMaps.ToListAsync();
                return Results.Ok(maps);
            })
            .Produces<List<Map>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: CountryEndpointMetadataMessages.MESSAGE_LIST_SUMMARY,
                description: CountryEndpointMetadataMessages.MESSAGE_LIST_DESCRIPTION
                ));

            app.MapGet($"{API_MAP_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var map = await db.Maps.SingleOrDefaultAsync(p => p.Id == id);
                if (map is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(map);
            })
            .Produces<Map?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
               summary: CountryEndpointMetadataMessages.MESSAGE_BYID_SUMMARY,
                description: CountryEndpointMetadataMessages.MESSAGE_BYID_DESCRIPTION
                ));
        }
    }
}
