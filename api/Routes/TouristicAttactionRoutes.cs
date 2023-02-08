﻿using api.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Routes
{
    public static class TuristicAttactionRoutes
    {
        public static void RegisterTuristicAttactionAPI(WebApplication app)
        {
            const string API_TOURISTIC_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.TOURISTIC_ROUTE}";

            app.MapGet(API_TOURISTIC_ROUTE_COMPLETE, (DBContext db) =>
            {
                return Results.Ok(db.TouristAttractions.ToList());
            }).WithMetadata(new SwaggerOperationAttribute(summary: "aaaa", description: "hhhh"));
            //.WithMetadata(new SwaggerOperationAttribute(summary: Messages.MESSAGE_TOURIST_ATTRACTION_LIST_SUMMARY, description: Messages.MESSAGE_TOURIST_ATTRACTION_LIST_DESCRIPTION));

            app.MapGet($"{API_TOURISTIC_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                var turisticAtt = await db.TouristAttractions.FindAsync(id);

                if (turisticAtt is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(turisticAtt);
            }).WithMetadata(new SwaggerOperationAttribute(summary: Messages.MESSAGE_TOURIST_ATTRACTION_BYID_SUMMARY, description: Messages.MESSAGE_TOURIST_ATTRACTION_BYID_DESCRIPTION));

            app.MapGet($"{API_TOURISTIC_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var turisticAtt = db.TouristAttractions.Where(x => x.Name!.ToUpper().Equals(name.ToUpper())).ToList();

                if (turisticAtt is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(turisticAtt);
            }).WithMetadata(new SwaggerOperationAttribute(summary: Messages.MESSAGE_TOURIST_ATTRACTION_BYNAME_SUMMARY, description: Messages.MESSAGE_TOURIST_ATTRACTION_BYNAME_DESCRIPTION));
        }
    }
}