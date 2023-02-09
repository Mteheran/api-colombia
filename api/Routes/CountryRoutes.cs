using api.Utils;
using Swashbuckle.AspNetCore.Annotations;

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
                    return Task.FromResult(Results.NotFound());
                }

                return Task.FromResult(Results.Ok(country));
            })
            .WithMetadata(new SwaggerOperationAttribute(summary: Messages.EndpointMetadata.MESSAGE_COUNTRY_SUMMARY, description: Messages.EndpointMetadata.MESSAGE_COUNTRY_DESCRIPTION));
        }
    }
}
