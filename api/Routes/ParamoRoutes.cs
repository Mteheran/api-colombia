using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using ParamoEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.ParamoEndpoint;

namespace api.Routes
{
    public static class ParamoRoutes
    {
        public static void RegisterParamoAPI(WebApplication app)
        {
            const string API_PARAMO_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.PARAMO_ROUTE}";

            app.MapGet(API_PARAMO_ROUTE_COMPLETE, (DBContext db) =>
            {
                return Results.Ok(db.Paramos.ToList());
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary: ParamoEndpointMetadataMessages.MESSAGE_PARAMO_LIST_SUMMARY,
                 description: ParamoEndpointMetadataMessages.MESSAGE_PARAMO_LIST_DESCRIPTION
                 ));

            app.MapGet($"{API_PARAMO_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var paramo = await db.Paramos.Include(p => p.City).SingleOrDefaultAsync(p => p.Id == id);

                if (paramo is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(paramo);
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary: ParamoEndpointMetadataMessages.MESSAGE_PARAMO_BYID_SUMMARY,
                 description: ParamoEndpointMetadataMessages.MESSAGE_PARAMO_BYID_DESCRIPTION
                 ));
                        

            app.MapGet($"{API_PARAMO_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var paramo = db.Paramos.Where(x => x.Name.ToUpper().Equals(name.Trim().ToUpper())).ToList();

                if (paramo is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(paramo);
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary: ParamoEndpointMetadataMessages.MESSAGE_PARAMO_BYNAME_SUMMARY,
                description: ParamoEndpointMetadataMessages.MESSAGE_PARAMO_BYNAME_DESCRIPTION
                ));

            app.MapGet($"{API_PARAMO_ROUTE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbParamos = db.Paramos.ToList();

                var paramos = Functions.FilterObjectListPropertiesByKeyword<Paramo>(dbParamos, wellFormedKeyword);

                if (paramos.Count == 0)
                {
                    return Results.NotFound();
                }

                return Results.Ok(paramos);
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary: ParamoEndpointMetadataMessages.MESSAGE_PARAMO_SEARCH_SUMMARY,
                 description: ParamoEndpointMetadataMessages.MESSAGE_PARAMO_SEARCH_DESCRIPTION
                 ));


            app.MapGet($"{API_PARAMO_ROUTE_COMPLETE}/pagedList",
                    async (PaginationModel pagination, DBContext db) =>
                    {

                        if (pagination.Page <= 0 || pagination.PageSize <= 0)
                        {
                            return Results.BadRequest();
                        }

                        var paramos = db.Paramos.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);

                        if (await paramos?.CountAsync() == 0)
                        {
                            return Results.NotFound();
                        }

                        var paginationResponse = new PaginationResponseModel<Paramo>
                        {
                            Page = pagination.Page,
                            PageSize = pagination.PageSize,
                            TotalRecords = await db.Paramos.CountAsync(),
                            Data = await paramos.ToListAsync(),

                        };

                        return Results.Ok(paginationResponse);
                    })
           .WithMetadata(new SwaggerOperationAttribute(
               summary: ParamoEndpointMetadataMessages.MESSAGE_PARAMO_PAGEDLIST_SUMMARY,
                description: ParamoEndpointMetadataMessages.MESSAGE_PARAMO_PAGEDLIST_DESCRIPTION
                ));
        }
    }
}
