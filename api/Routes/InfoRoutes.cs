using Microsoft.AspNetCore.Mvc;

namespace api.Routes
{
    public static class InfoRoutes
    {
        public static void RegisterInfoAPI(WebApplication app)
        {
            // endpoint to create the DB in debug mode
#if DEBUG
            app.MapGet("/db-creation", ([FromServices] DBContext dbContext) =>
            {
                dbContext.Database.EnsureCreated();
                return Results.Ok();
            });
#endif
        }
    }
}
