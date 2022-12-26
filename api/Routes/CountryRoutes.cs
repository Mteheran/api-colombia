using Swashbuckle.AspNetCore.Annotations;

namespace api.Routes
{
    public static class CountryRoutes
    {
        public static void RegisterCountryAPI(WebApplication app)
        {
            const string COUNTRY_ROUTE = "Country";

            app.MapGet($"api/v1/{COUNTRY_ROUTE}/Colombia", (int id, DBContext db) =>
            {
                var country = db.Countries.FirstOrDefault();
                if (country != null)
                {
                    return Task.FromResult(Results.Ok(country));
                }
                else
                {
                    return Task.FromResult(Results.NotFound());
                }
            }) 
            .WithMetadata(new SwaggerOperationAttribute(summary: "This is the Colombian endpoint", description: "This endpoint return the information from Colombia"));
        }
    }
}
