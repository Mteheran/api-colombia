using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using CountryEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.MapEndpoint;

namespace api.Routes
{
    public static class MapsRoutes
    {
        public static void RegisterCountryAPI(WebApplication app)
        {
            const string API_MAP_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.MAP_ROUTE}";

            app.MapGet($"{API_MAP_ROUTE_COMPLETE}/", (DBContext db) =>
            {
                var maps = db.Maps.ToList();
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

                var map = await db.Maps.Include(p => p.Departament).SingleOrDefaultAsync(p => p.Id == id);
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
