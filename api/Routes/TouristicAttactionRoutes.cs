using api.Models;
using api.Utils;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;
using TouristAttractionEndpointMetadata =api.Utils.Messages.EndpointMetadata.TouristAttractionsEndpoint ;
namespace api.Routes
{
    public static class TuristicAttactionRoutes
    {
        public static void RegisterTuristicAttactionAPI(WebApplication app)
        {
            const string API_TOURISTIC_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.TOURISTIC_ROUTE}";

            app.MapGet(API_TOURISTIC_ROUTE_COMPLETE, (DBContext db) =>
            {
                return Results.Ok(db.TouristAttractions.Include(p=> p.City).ToList());
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_LIST_SUMMARY, 
                description: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_LIST_DESCRIPTION));

            app.MapGet($"{API_TOURISTIC_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var turisticAtt = await db.TouristAttractions
                .Include(p=> p.City)
                .SingleOrDefaultAsync(p=> p.Id == id);

                if (turisticAtt is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(turisticAtt);
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_BYID_SUMMARY, 
                description: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_BYID_DESCRIPTION));

            app.MapGet($"{API_TOURISTIC_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var turisticAtt = db.TouristAttractions.Where(x => x.Name!.ToUpper().Equals(name.ToUpper())).ToList();

                if (turisticAtt is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(turisticAtt);
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_BYNAME_SUMMARY, 
                description: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_BYNAME_DESCRIPTION));

            app.MapGet($"{API_TOURISTIC_ROUTE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbTouristAttractions = db.TouristAttractions.ToList();

                var touristAttractions = Functions.FilterObjectListPropertiesByKeyword<TouristAttraction>(dbTouristAttractions, wellFormedKeyword);

                if (touristAttractions.Count == 0)
                {
                    return Results.NotFound();
                }

                return Results.Ok(touristAttractions);
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_SEARCH_SUMMARY, 
                description: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_SEARCH_DESCRIPTION));

            app.MapGet($"{API_TOURISTIC_ROUTE_COMPLETE}/pagedList",
           async (PaginationModel pagination, DBContext db) =>
           {

               if (pagination.Page <= 0 || pagination.PageSize <= 0)
               {
                   return Results.BadRequest();
               }

               var touristAttractionsPaged = db.TouristAttractions.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);

               if (await touristAttractionsPaged?.CountAsync() == 0)
               {
                   return Results.NotFound();
               }

               var paginationResponse = new PaginationResponseModel<TouristAttraction>
               {
                   Page = pagination.Page,
                   PageSize = pagination.PageSize,
                   TotalRecords = await db.Presidents.CountAsync(),
                   Data = await touristAttractionsPaged.ToListAsync(),

               };

               return Results.Ok(paginationResponse);
           })
  .WithMetadata(new SwaggerOperationAttribute(
      summary: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_PAGEDLIST_SUMMARY,
       description: TouristAttractionEndpointMetadata.MESSAGE_TOURIST_ATTRACTION_PAGEDLIST_DESCRIPTION
       ));

        }
    }
}
