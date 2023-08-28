using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using InvasiveSpecieEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.InvasiveSpecieEndpoint;

namespace api.Routes
{
    public static class InvasiveSpecieRoutes
    {
        public static void RegisterInvasiveSpecieAPI(WebApplication app)
        {
            const string API_INVASIVE_SPECIE_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.INVASIVE_SPECIE_ROUTE}";
            app.MapGet(API_INVASIVE_SPECIE_ROUTE_COMPLETE, (DBContext db) =>
            {
                var listInvasiveSpecies = db.InvasiveSpecies.ToList();
                return Results.Ok(listInvasiveSpecies);
            })
            .Produces<List<City>?>(200)
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

                var city = await db.InvasiveSpecies.SingleOrDefaultAsync(p => p.Id == id);
                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .Produces<City?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_BYID_SUMMARY,
                 description: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_BYID_DESCRIPTION
                 ));

            app.MapGet($"{API_INVASIVE_SPECIE_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var city = db.InvasiveSpecies.Where(x => x.Name.ToUpper().Equals(name.Trim().ToUpper())).ToList();
                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .Produces<List<City>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_BYNAME_SUMMARY,
                description: InvasiveSpecieEndpointMetadataMessages.MESSAGE_INVASIVE_SPECIE_BYNAME_DESCRIPTION
                ));

            app.MapGet($"{API_INVASIVE_SPECIE_ROUTE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbInvasiveSpecies = db.InvasiveSpecies.ToList();
                var InvasiveSpecies = Functions.FilterObjectListPropertiesByKeyword<InvasiveSpecie>(dbInvasiveSpecies, wellFormedKeyword);

                if (!InvasiveSpecies.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(InvasiveSpecies);
            })
            .Produces<List<City>>(200)
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
