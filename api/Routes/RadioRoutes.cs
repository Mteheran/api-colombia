using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using RadioMetadataMessages = api.Utils.Messages.EndpointMetadata.RadioEndpoint;
using static api.Utils.Functions;
using Microsoft.AspNetCore.Mvc;
using static api.Utils.Messages.EndpointMetadata;

namespace api.Routes
{
    public static class RadioRoutes
    {
        public static void RegisterRadioRoutesAPI(WebApplication app)
        {
            const string API_RADIO_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.RADIO}";
            app.MapGet(API_RADIO_COMPLETE, (DBContext db,
                [FromQuery, SwaggerParameter(Description = "It can be sorted by any of the fields that have numerical, string, or date values (for example: Id, name, description, etc.).")] string? sortBy,
                [FromQuery, SwaggerParameter(Description = "Possible values: 'asc' or 'desc'.")] string? sortDirection) =>
            {
                var queryRadios = db.Radios
                    .Include(p => p.City)
                    .AsQueryable();

                (queryRadios, var isValidSort) = ApplySorting(queryRadios, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var listRadios = queryRadios.ToList();
                return Results.Ok(listRadios); 
            })
            .Produces<List<Radio>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: RadioMetadataMessages.MESSAGE_RADIO_LIST_SUMMARY,
                 description: RadioMetadataMessages.MESSAGE_RADIO_LIST_DESCRIPTION
                 ));

            app.MapGet($"{API_RADIO_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var city = await db.Radios
                .Include(p => p.City).SingleOrDefaultAsync(p => p.Id == id);
                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .Produces<City?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: RadioMetadataMessages.MESSAGE_RADIO_BYID_SUMMARY,
                 description: RadioMetadataMessages.MESSAGE_RADIO_BYID_DESCRIPTION
                 ));

            app.MapGet($"{API_RADIO_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var city = db.Radios
                .Include(p => p.City).Where(x => x.Name.ToUpper().Equals(name.Trim().ToUpper())).ToList();
                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .Produces<List<City>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: RadioMetadataMessages.MESSAGE_RADIO_BYNAME_SUMMARY,
                description: RadioMetadataMessages.MESSAGE_RADIO_BYNAME_DESCRIPTION
                ));

            app.MapGet($"{API_RADIO_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbRadios = db.Radios
                .Include(p => p.City).ToList();
                var Radios = Functions.FilterObjectListPropertiesByKeyword<Radio>(dbRadios, wellFormedKeyword);

                if (!Radios.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(Radios);
            })
            .Produces<List<Radio>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: RadioMetadataMessages.MESSAGE_RADIO_SEARCH_SUMMARY,
                 description: RadioMetadataMessages.MESSAGE_RADIO_SEARCH_DESCRIPTION
                 ));


            app.MapGet($"{API_RADIO_COMPLETE}/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                } 
                var sortBy = pagination.SortBy ?? string.Empty;
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty;
 
                var queryRadios = db.Radios.Include(p => p.City).AsQueryable();
 
                (queryRadios, var isValidSort) = ApplySorting(queryRadios, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                } 
                var totalRecords = await queryRadios.CountAsync();
                var pagedRadios = await queryRadios
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                if (!pagedRadios.Any())
                {
                    return Results.NotFound();
                }
 
                var paginationResponse = new PaginationResponseModel<Radio>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pagedRadios
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<Radio>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
               summary: RadioMetadataMessages.MESSAGE_RADIO_PAGEDLIST_SUMMARY,
                description: RadioMetadataMessages.MESSAGE_RADIO_PAGEDLIST_DESCRIPTION
                ));
        }
    }
}

