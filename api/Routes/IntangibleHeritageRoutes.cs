using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static api.Utils.Messages.EndpointMetadata;
using api.Models;
using static api.Utils.Functions;
using Microsoft.EntityFrameworkCore;

namespace api.Routes
{

    public static class IntangibleHeritageRoutes
    {
        public static void RegisterIntangibleHeritageAPI(WebApplication app)
        {
            const string API_INTANGIBLE_HERITAGE_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.INTANGIBLE_HERITAGE_ROUTE}";

            app.MapGet(API_INTANGIBLE_HERITAGE_ROUTE_COMPLETE, async (DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                var queryIntangibleHeritage = db.IntangibleHeritages.Include(p => p.Department).AsQueryable();
                (queryIntangibleHeritage, var isValidSort) = ApplySorting(queryIntangibleHeritage, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var listIntangibleHeritage = await queryIntangibleHeritage.ToListAsync();
                return Results.Ok(listIntangibleHeritage);
            })
            .Produces<List<IntangibleHeritage>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: IntangibleHeritageEndpoint.MESSAGE_INTANGIBLE_HERITAGE_LIST_SUMMARY,
                description: IntangibleHeritageEndpoint.MESSAGE_INTANGIBLE_HERITAGE_LIST_DESCRIPTION
            ));

            app.MapGet($"{API_INTANGIBLE_HERITAGE_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var intangibleHeritage = await db.IntangibleHeritages
                                            .Include(p => p.Department)
                                            .SingleOrDefaultAsync(p => p.Id == id);

                if (intangibleHeritage is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(intangibleHeritage);
            })
            .Produces<IntangibleHeritage?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: IntangibleHeritageEndpoint.MESSAGE_INTANGIBLE_HERITAGE_BYID_SUMMARY,
                description: IntangibleHeritageEndpoint.MESSAGE_INTANGIBLE_HERITAGE_BYID_DESCRIPTION
                ));

            app.MapGet($"{API_INTANGIBLE_HERITAGE_ROUTE_COMPLETE}/{{id}}/department", async (int id, DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest("Invalid department ID.");
                }

                var queryIntangibleHeritagesByDepartment = db.IntangibleHeritages.Include(p => p.Department).Where(p => p.DepartmentId == id).AsQueryable();
                (queryIntangibleHeritagesByDepartment, var isValidSort) = ApplySorting(queryIntangibleHeritagesByDepartment, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                if (!queryIntangibleHeritagesByDepartment.Any())
                {
                    return Results.NotFound();
                }

                var intangibleHeritages = await queryIntangibleHeritagesByDepartment.ToListAsync();
                return Results.Ok(intangibleHeritages);
            })
            .Produces<List<IntangibleHeritage>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: IntangibleHeritageEndpoint.MESSAGE_INTANGIBLE_HERITAGE_DEPARTMENT_SUMMARY,
                description: IntangibleHeritageEndpoint.MESSAGE_INTANGIBLE_HERITAGE_DEPARTMENT_DESCRIPTION
                ));

            app.MapGet($"{API_INTANGIBLE_HERITAGE_ROUTE_COMPLETE}/name/{{name}}", async (string name, DBContext db) =>
            {
                var intangibleHeritages = await db.IntangibleHeritages.Include(p => p.Department).Where(x => x.Name!.ToUpper().Equals(name.Trim().ToUpper())).ToListAsync();

                if (intangibleHeritages is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(intangibleHeritages);
            })
            .Produces<List<IntangibleHeritage>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: IntangibleHeritageEndpoint.MESSAGE_INTANGIBLE_HERITAGE_BYNAME_SUMMARY,
                description: IntangibleHeritageEndpoint.MESSAGE_INTANGIBLE_HERITAGE_BYNAME_DESCRIPTION
                ));

            app.MapGet($"{API_INTANGIBLE_HERITAGE_ROUTE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbIntangibleHeritages = db.IntangibleHeritages.Include(p => p.Department).ToList();
                var intangibleHeritages = Functions.FilterObjectListPropertiesByKeyword<IntangibleHeritage>(dbIntangibleHeritages, wellFormedKeyword);
                if (!dbIntangibleHeritages.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(intangibleHeritages);
            })
            .Produces<List<IntangibleHeritage>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: IntangibleHeritageEndpoint.MESSAGE_INTANGIBLE_HERITAGE_SEARCH_SUMMARY,
                description: IntangibleHeritageEndpoint.MESSAGE_INTANGIBLE_HERITAGE_SEARCH_DESCRIPTION
                ));

            app.MapGet($"{API_INTANGIBLE_HERITAGE_ROUTE_COMPLETE}/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }

                var sortBy = pagination.SortBy ?? string.Empty;
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty;
                var queryIntangibleHeritages = db.IntangibleHeritages.Include(p => p.Department).AsQueryable();

                (queryIntangibleHeritages, var isValidSort) = ApplySorting(queryIntangibleHeritages, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var totalRecords = await queryIntangibleHeritages.CountAsync();

                var pageIntangibleHeritages = await queryIntangibleHeritages
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                if (!pageIntangibleHeritages.Any())
                {
                    return Results.NotFound();
                }

                var paginationResponse = new PaginationResponseModel<IntangibleHeritage>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pageIntangibleHeritages
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<IntangibleHeritage>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: IntangibleHeritageEndpoint.MESSAGE_INTANGIBLE_HERITAGE_PAGEDLIST_SUMMARY,
                description: IntangibleHeritageEndpoint.MESSAGE_INTANGIBLE_HERITAGE_PAGEDLIST_DESCRIPTION
            ));

        }

    }

}
