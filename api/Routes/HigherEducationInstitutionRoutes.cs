using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using static api.Utils.Functions;
using static api.Utils.Messages.EndpointMetadata;
using HEIMetadataMessages = api.Utils.Messages.EndpointMetadata.HigherEducationInstitutionEndpoint;
using Microsoft.AspNetCore.Mvc;

namespace api.Routes
{
    public static class HigherEducationInstitutionRoutes
    {
        public static void RegisterHigherEducationInstitutionAPI(WebApplication app)
        {
            const string API_HEI_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.HIGHER_EDUCATION_INSTITUTION_ROUTE}";
            const string API_HEI_TAG = "HigherEducationInstitution";

            // Group and tags usage
            IEndpointRouteBuilder group = app
                .MapGroup(API_HEI_COMPLETE)
                .WithTags(API_HEI_TAG)
                .CacheOutput()
                .RequireRateLimiting(Util.PublicRateLimitPolicy);

            group.MapGet(string.Empty, async (DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                var queryInstitutions = db.HigherEducationInstitutions
                    .Include(p => p.City)
                    .AsQueryable();

                (queryInstitutions, var isValidSort) = ApplySorting(queryInstitutions, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var listInstitutions = await queryInstitutions.ToListAsync();
                return Results.Ok(listInstitutions);
            })
            .Produces<List<HigherEducationInstitution>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: HEIMetadataMessages.MESSAGE_HEI_LIST_SUMMARY,
                description: HEIMetadataMessages.MESSAGE_HEI_LIST_DESCRIPTION
                ));

            group.MapGet("{id}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var institution = await db.HigherEducationInstitutions
                    .Include(p => p.City)
                    .SingleOrDefaultAsync(p => p.Id == id);
                if (institution is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(institution);
            })
            .Produces<HigherEducationInstitution?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: HEIMetadataMessages.MESSAGE_HEI_BYID_SUMMARY,
                description: HEIMetadataMessages.MESSAGE_HEI_BYID_DESCRIPTION
                ));

            group.MapGet("name/{name}", (string name, DBContext db) =>
            {
                var search = name.Trim().ToUpperInvariant();
                var institutions = db.HigherEducationInstitutions
                    .Include(p => p.City)
                    .Where(x => (x.Name ?? string.Empty).ToUpperInvariant().Contains(search))
                    .ToList();
                return Results.Ok(institutions);
            })
            .Produces<List<HigherEducationInstitution>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: HEIMetadataMessages.MESSAGE_HEI_BYNAME_SUMMARY,
                description: HEIMetadataMessages.MESSAGE_HEI_BYNAME_DESCRIPTION
                ));

            group.MapGet("search/{keyword}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbInstitutions = db.HigherEducationInstitutions.ToList();
                var institutions = Functions.FilterObjectListPropertiesByKeyword<HigherEducationInstitution>(dbInstitutions, wellFormedKeyword);
                return Results.Ok(institutions);
            })
            .Produces<List<HigherEducationInstitution>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: HEIMetadataMessages.MESSAGE_HEI_SEARCH_SUMMARY,
                description: HEIMetadataMessages.MESSAGE_HEI_SEARCH_DESCRIPTION
                ));

            group.MapGet("pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }

                var sortBy = pagination.SortBy ?? string.Empty;
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty;
                var queryInstitutions = db.HigherEducationInstitutions
                    .Include(p => p.City)
                    .AsQueryable();

                (queryInstitutions, var isValidSort) = ApplySorting(queryInstitutions, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var totalRecords = await queryInstitutions.CountAsync();

                var pagedInstitutions = await queryInstitutions
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                var paginationResponse = new PaginationResponseModel<HigherEducationInstitution>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pagedInstitutions
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<HigherEducationInstitution>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: HEIMetadataMessages.MESSAGE_HEI_PAGEDLIST_SUMMARY,
                description: HEIMetadataMessages.MESSAGE_HEI_PAGEDLIST_DESCRIPTION
                ));
        }
    }
}
