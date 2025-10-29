using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using InvasiveSpecieEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.InvasiveSpecieEndpoint;
using Microsoft.AspNetCore.Mvc;
using static api.Utils.Functions;
using static api.Utils.Messages.EndpointMetadata;

namespace api.Routes
{
    public static class InvasiveSpecieRoutes
    {
        public static void RegisterInvasiveSpecieAPI(WebApplication app)
        {
            const string API_INVASIVE_SPECIE_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.INVASIVE_SPECIE_ROUTE}";
            app.MapGet(API_INVASIVE_SPECIE_ROUTE_COMPLETE, (DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                var queryInvasiveSpecies = db.InvasiveSpecies.AsQueryable();
                (queryInvasiveSpecies, var isValidSort) = ApplySorting(queryInvasiveSpecies, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var listInvasiveSpecies = queryInvasiveSpecies.ToList(); 
                return Results.Ok(listInvasiveSpecies); 
            })
            .Produces<List<InvasiveSpecie>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_LIST_SUMMARY,
                 description: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_LIST_DESCRIPTION
                 ));

            app.MapGet($"{API_INVASIVE_SPECIE_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var invasiveSpecie = await db.InvasiveSpecies.SingleOrDefaultAsync(p => p.Id == id);
                if (invasiveSpecie is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(invasiveSpecie);
            })
            .Produces<InvasiveSpecie?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_BYID_SUMMARY,
                 description: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_BYID_DESCRIPTION
                 ));

            app.MapGet($"{API_INVASIVE_SPECIE_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var search = name.Trim().ToUpperInvariant();
                var invasiveSpecies = db.InvasiveSpecies
                    .Where(x => (x.Name ?? string.Empty).ToUpperInvariant().Contains(search))
                    .ToList();
                return Results.Ok(invasiveSpecies);
            })
            .Produces<List<InvasiveSpecie>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_BYNAME_SUMMARY,
                description: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_BYNAME_DESCRIPTION
                ));

            app.MapGet($"{API_INVASIVE_SPECIE_ROUTE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbInvasiveSpecies = db.InvasiveSpecies.ToList();
                var invasiveSpecies = Functions.FilterObjectListPropertiesByKeyword<InvasiveSpecie>(dbInvasiveSpecies, wellFormedKeyword);
                return Results.Ok(invasiveSpecies);
            })
            .Produces<List<InvasiveSpecie>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_SEARCH_SUMMARY,
                 description: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_SEARCH_DESCRIPTION
                 ));


            app.MapGet($"{API_INVASIVE_SPECIE_ROUTE_COMPLETE}/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }

                var InvasiveSpecies = db.InvasiveSpecies.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
                if (!await InvasiveSpecies?.AnyAsync())
                {
                    return Results.NotFound();
                }

                var paginationResponse = new PaginationResponseModel<InvasiveSpecie>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = await db.InvasiveSpecies.CountAsync(),
                    Data = await InvasiveSpecies.ToListAsync()
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<City>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
               summary: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_PAGEDLIST_SUMMARY,
                description: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_PAGEDLIST_DESCRIPTION
                ));
        }
    }
}
