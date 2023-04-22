using api.Utils;
using Swashbuckle.AspNetCore.Annotations;
using CountryEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.CountryEndpoint;

namespace api.Routes
{
    public static class CountryRoutes
    {
        public static void RegisterCountryAPI(WebApplication app)
        {
            const string API_COUNTRY_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.COUNTRY_ROUTE}";

            app.MapGet($"{API_COUNTRY_ROUTE_COMPLETE}/{Util.COLOMBIA}", (DBContext db) =>
            {
                var country = db.Countries.FirstOrDefault();
                if (country is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(country);
            }).Produces<Models.Country>(200).AddMetaData<Models.Country>(
                tag:"",
                summary:CountryEndpointMetadataMessages.MESSAGE_COUNTRY_SUMMARY,
                description:CountryEndpointMetadataMessages.MESSAGE_COUNTRY_DESCRIPTION
            );
        }
    }
}
