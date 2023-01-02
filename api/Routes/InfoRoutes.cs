using Microsoft.AspNetCore.Mvc;

namespace api.Routes
{
    public static class InfoRoutes
    {
        public static void RegisterInfoAPI(WebApplication app)
        {
            app.MapGet("/dbcreation", async ([FromServices] DBContext dbContext) =>
            {
                dbContext.Database.EnsureCreated();
                return Results.Ok();
            });
        }
    }
}
