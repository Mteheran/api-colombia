using api.Models;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using static api.Utils.Functions;
using static api.Utils.Messages.EndpointMetadata;
using PostalCodeMetadataMessages = api.Utils.Messages.EndpointMetadata.PostalCodeEndpoint;

namespace api.Routes
{
    public static class PostalCodeRoutes
    {
        public static void RegisterPostalCodeAPI(WebApplication app)
        {
            const string API_POSTAL_CODE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.POSTAL_CODE_ROUTE}";
            const string API_POSTAL_CODE_TAG = "PostalCode";
            IEndpointRouteBuilder group = app.MapGroup(API_POSTAL_CODE_COMPLETE).WithTags(API_POSTAL_CODE_TAG);

            group.MapGet(string.Empty, async (DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                var queryPostalCodes = db.PostalCodes
                    .Include(p => p.City)
                    .AsQueryable();

                (queryPostalCodes, var isValidSort) = ApplySorting(queryPostalCodes, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var listPostalCodes = await queryPostalCodes.ToListAsync();
                return Results.Ok(listPostalCodes);
            })
            .Produces<List<PostalCode>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PostalCodeMetadataMessages.MESSAGE_POSTAL_CODE_LIST_SUMMARY,
                description: PostalCodeMetadataMessages.MESSAGE_POSTAL_CODE_LIST_DESCRIPTION
            ));

            group.MapGet("/{id}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var postalCode = await db.PostalCodes
                    .Include(p => p.City)
                    .SingleOrDefaultAsync(p => p.Id == id);

                if (postalCode is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(postalCode);
            })
            .Produces<PostalCode?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PostalCodeMetadataMessages.MESSAGE_POSTAL_CODE_BYID_SUMMARY,
                description: PostalCodeMetadataMessages.MESSAGE_POSTAL_CODE_BYID_DESCRIPTION
            ));

            group.MapGet("/code/{code}", (string code, DBContext db) =>
            {
                var search = code.Trim().ToUpper();
                var postalCodes = db.PostalCodes
                    .Include(p => p.City)
                    .Where(x => (x.Code ?? string.Empty).ToUpper().Equals(search))
                    .ToList();

                if (!postalCodes.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(postalCodes);
            })
            .Produces<List<PostalCode>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PostalCodeMetadataMessages.MESSAGE_POSTAL_CODE_BYCODE_SUMMARY,
                description: PostalCodeMetadataMessages.MESSAGE_POSTAL_CODE_BYCODE_DESCRIPTION
            ));

            group.MapGet("/search/{keyword}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbPostalCodes = db.PostalCodes
                    .Include(p => p.City)
                    .ToList();
                var postalCodes = Functions.FilterObjectListPropertiesByKeyword<PostalCode>(dbPostalCodes, wellFormedKeyword);
                return Results.Ok(postalCodes);
            })
            .Produces<List<PostalCode>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PostalCodeMetadataMessages.MESSAGE_POSTAL_CODE_SEARCH_SUMMARY,
                description: PostalCodeMetadataMessages.MESSAGE_POSTAL_CODE_SEARCH_DESCRIPTION
            ));

            group.MapGet("/city/{cityId}", (int cityId, DBContext db) =>
            {
                var postalCodes = db.PostalCodes
                    .Include(p => p.City)
                    .Where(x => x.CityId == cityId)
                    .ToList();

                if (!postalCodes.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(postalCodes);
            })
            .Produces<List<PostalCode>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PostalCodeMetadataMessages.MESSAGE_POSTAL_CODE_BYCITY_SUMMARY,
                description: PostalCodeMetadataMessages.MESSAGE_POSTAL_CODE_BYCITY_DESCRIPTION
            ));

            group.MapGet("/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }

                var sortBy = pagination.SortBy ?? string.Empty;
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty;
                var queryPostalCodes = db.PostalCodes
                    .Include(p => p.City)
                    .AsQueryable();

                (queryPostalCodes, var isValidSort) = ApplySorting(queryPostalCodes, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var totalRecords = await queryPostalCodes.CountAsync();
                var pagePostalCodes = await queryPostalCodes
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                var paginationResponse = new PaginationResponseModel<PostalCode>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pagePostalCodes
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<PostalCode>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PostalCodeMetadataMessages.MESSAGE_POSTAL_CODE_PAGEDLIST_SUMMARY,
                description: PostalCodeMetadataMessages.MESSAGE_POSTAL_CODE_PAGEDLIST_DESCRIPTION
            ));
        }
    }
}