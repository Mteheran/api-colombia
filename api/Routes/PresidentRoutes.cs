using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using PresidentEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.PresidentEndpoint;
namespace api.Routes
{
    public static class PresidentRoutes
    {
        public static void RegisterPresidentApi(WebApplication app)
        {
            const string API_PRESIDENT_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.PRESIDENT_ROUTE}";

            app.MapGet(API_PRESIDENT_ROUTE_COMPLETE, (DBContext db) =>
            {
                return Results.Ok(db.Presidents.ToList());
            })
            .Produces<List<President>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_LIST_SUMMARY,
                description: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_LIST_DESCRIPTION
                ));


            app.MapGet($"{API_PRESIDENT_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var president = await db.Presidents
                                                .Include(p => p.City)
                                                .SingleOrDefaultAsync(p => p.Id == id);

                if (president is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(president);
            })
            .Produces<President?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_BYID_SUMMARY,
                 description: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_BYID_DESCRIPTION
                 ));


            app.MapGet($"{API_PRESIDENT_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var president = db.Presidents.Where(x => x.Name!.ToUpper().Equals(name.Trim().ToUpper())).ToList();

                if (president is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(president);
            })
            .Produces<President?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_BYNAME_SUMMARY,
                 description: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_BYNAME_DESCRIPTION
                 ));

            app.MapGet($"{API_PRESIDENT_ROUTE_COMPLETE}/year/{{year}}", async (int year, DBContext db) =>
            {
                //Require validation to not year greater than actual year
                var presidents = db.Presidents
                                        .Include(p => p.City)
                                        .Where(p => (p.StartPeriodDate.Year <= year
                                                        && p.EndPeriodDate.HasValue && p.EndPeriodDate.Value.Year >= year)
                                                        || (p.EndPeriodDate == null && p.StartPeriodDate.Year <= year && year <= DateTime.Now.Year))
                                        .ToList();
                if (presidents is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(presidents);
            })
            .Produces<List<President>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_BYYEAR_SUMMARY,
                description: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_BYYEAR_DESCRIPTION
                ));

            app.MapGet($"{API_PRESIDENT_ROUTE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
               string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
               var dbPresidents = db.Presidents.ToList();
               var presidents = Functions.FilterObjectListPropertiesByKeyword<President>(dbPresidents, wellFormedKeyword);
               if (presidents.Count == 0)
               {
                   return Results.NotFound();
               }

               return Results.Ok(presidents);
            })
            .Produces<List<President>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_SEARCH_SUMMARY, 
                description: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_SEARCH_DESCRIPTION
            ));

            app.MapGet($"{API_PRESIDENT_ROUTE_COMPLETE}/pagedList",
            async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {

                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }

                var presidentsPaged = db.Presidents.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
                if (await presidentsPaged?.CountAsync() == 0)
                {
                    return Results.NotFound();
                }

                var paginationResponse = new PaginationResponseModel<President>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = await db.Presidents.CountAsync(),
                    Data = await presidentsPaged.ToListAsync(),

                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<President>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_PAGEDLIST_SUMMARY,
                description: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_PAGEDLIST_DESCRIPTION
            ));
        }
    }
}
