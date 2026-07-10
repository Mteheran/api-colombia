using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using PresidentEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.PresidentEndpoint;
using static api.Utils.Functions;
using static api.Utils.Messages.EndpointMetadata;
using Microsoft.AspNetCore.Mvc;

namespace api.Routes
{
    public static class PresidentRoutes
    { 
        public static void RegisterPresidentApi(WebApplication app)
        {
            const string API_PRESIDENT_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.PRESIDENT_ROUTE}";
            const string API_PRESIDENT_TAG = "President";
            IEndpointRouteBuilder group = app.MapGroup(API_PRESIDENT_ROUTE_COMPLETE).WithTags(API_PRESIDENT_TAG).CacheOutput();

           group.MapGet(string.Empty, async (DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                var queryPresidents = db.Presidents.AsQueryable(); 
                (queryPresidents, var isValidSort) = ApplySorting(queryPresidents, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }
        
                var listPresidents = await queryPresidents.ToListAsync(); 
                return Results.Ok(listPresidents); 
            }) 
            .Produces<List<President>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_LIST_SUMMARY,
                description: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_LIST_DESCRIPTION
                ));

            group.MapGet("/{id}", async (int id, DBContext db) =>
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

            group.MapGet("/name/{name}", async (string name, DBContext db) =>
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Results.BadRequest();
                }

                var wellFormedName = name.Trim().ToUpper();
                var presidents = await db.Presidents
                                            .Where(x => x.Name!.ToUpper().Equals(wellFormedName))
                                            .ToListAsync();

                if (!presidents.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(presidents);
            })
            .Produces<President?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_BYNAME_SUMMARY,
                description: PresidentEndpointMetadataMessages.MESSAGE_PRESIDENT_BYNAME_DESCRIPTION
                ));

            group.MapGet("/year/{year}", async (int year, DBContext db) =>
            {
                if (year <= 0)
                {
                    return Results.BadRequest();
                }

                var presidents = await db.Presidents
                                        .Include(p => p.City)
                                        .Where(p => (p.StartPeriodDate.Year <= year
                                                        && p.EndPeriodDate.HasValue && p.EndPeriodDate.Value.Year >= year)
                                                        || (p.EndPeriodDate == null && p.StartPeriodDate.Year <= year && year <= DateTime.Now.Year))
                                        .ToListAsync();
                if (!presidents.Any())
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

            group.MapGet("/search/{keyword}", async (string keyword, DBContext db) =>
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return Results.BadRequest();
                }

                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbPresidents = await db.Presidents.ToListAsync();
                var presidents = Functions.FilterObjectListPropertiesByKeyword<President>(dbPresidents, wellFormedKeyword);
                if (!presidents.Any())
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

            group.MapGet("/pagedList",
            async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                 if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }
 
                var sortBy = pagination.SortBy ?? string.Empty; 
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty; 
                var queryPresidents = db.Presidents.AsQueryable();
 
                (queryPresidents, var isValidSort) = ApplySorting(queryPresidents, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }
 
                var totalRecords = await queryPresidents.CountAsync();
 
                var pagedPresidents = await queryPresidents
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                if (!pagedPresidents.Any())
                {
                    return Results.NotFound();
                }
 
                var paginationResponse = new PaginationResponseModel<President>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pagedPresidents
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
