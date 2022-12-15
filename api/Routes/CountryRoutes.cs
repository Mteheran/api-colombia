using Swashbuckle.AspNetCore.Annotations;

namespace api.Routes
{
    public static class CountryRoutes
    {
        public static void RegisterCountryAPI(WebApplication app)
        {
            string route = "Country";

            app.MapGet($"{route}/Colombia", async (int id, DBContext db) =>
            {
                var country = db.Countries.FirstOrDefault();
                if (country != null)
                {
                    return Results.Ok(country);
                }
                else 
                { 
                    return Results.NotFound(); 
                }
            }) 
            .WithMetadata(new SwaggerOperationAttribute(summary: "This is the Colombian endpoint", description: "This endpoint return the information from Colombia"));
        }
    }
}
