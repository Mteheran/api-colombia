using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static api.Utils.Messages.EndpointMetadata; 
using api.Models; 
using static api.Utils.Functions;
using Microsoft.EntityFrameworkCore;  

namespace api.Routes
{

    public static class TypicalDishRoutes
    { 
        public static void RegisterTypicalDishAPI(WebApplication app)
        {
             const string API_TYPICAL_DISH_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.TYPICAL_DISH_ROUTE}";

             app.MapGet(API_TYPICAL_DISH_ROUTE_COMPLETE, async (DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                var queryTypicalDish = db.TypicalDishes.Include(p => p.Department).AsQueryable();
                (queryTypicalDish, var isValidSort) = ApplySorting(queryTypicalDish, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }
 
                var listTypicalDish = await queryTypicalDish.ToListAsync();
                return Results.Ok(listTypicalDish);
            })
            .Produces<List<TypicalDish>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TypicalDishEndpoint.MESSAGE_TYPICAL_DISH_LIST_SUMMARY,
                description: TypicalDishEndpoint.MESSAGE_TYPICAL_DISH_LIST_DESCRIPTION
            ));

            app.MapGet($"{API_TYPICAL_DISH_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var typicalDish = await db.TypicalDishes
                                            .Include(p => p.Department)
                                            .SingleOrDefaultAsync(p => p.Id == id);

                if (typicalDish is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(typicalDish);
            })
            .Produces<TypicalDish?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TypicalDishEndpoint.MESSAGE_TYPICAL_DISH_BYID_SUMMARY,
                description: TypicalDishEndpoint.MESSAGE_TYPICAL_DISH_BYID_DESCRIPTION
                ));

            app.MapGet($"{API_TYPICAL_DISH_ROUTE_COMPLETE}/{{id}}/department", async (int id, DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                 if (id <= 0)
                {
                    return Results.BadRequest("Invalid department ID.");
                }

                var queryTypicalDishesByDepartment = db.TypicalDishes.Include(p => p.Department).Where(p => p.DepartmentId == id).AsQueryable(); 
                (queryTypicalDishesByDepartment, var isValidSort) = ApplySorting(queryTypicalDishesByDepartment, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                if (!queryTypicalDishesByDepartment.Any())
                {
                    return Results.NotFound();
                }
 
                var typicalDishes = await queryTypicalDishesByDepartment.ToListAsync();
                return Results.Ok(typicalDishes);
            })
             .Produces<List<TypicalDish>?>(200)
             .WithMetadata(new SwaggerOperationAttribute(
                 summary: TypicalDishEndpoint.MESSAGE_TYPICAL_DISH_DEPARTMENT_SUMMARY,
                 description: TypicalDishEndpoint.MESSAGE_TYPICAL_DISH_DEPARTMENT_DESCRIPTION
                 ));

            app.MapGet($"{API_TYPICAL_DISH_ROUTE_COMPLETE}/name/{{name}}", async (string name, DBContext db) =>
            {
                var typicalDishes = await db.TypicalDishes.Include(p => p.Department).Where(x => x.Name!.ToUpper().Equals(name.Trim().ToUpper())).ToListAsync();

                if (typicalDishes is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(typicalDishes);
            })
            .Produces<List<TypicalDish>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TypicalDishEndpoint.MESSAGE_TYPICAL_DISH_BYNAME_SUMMARY,
                description: TypicalDishEndpoint.MESSAGE_TYPICAL_DISH_BYNAME_DESCRIPTION
                ));

            app.MapGet($"{API_TYPICAL_DISH_ROUTE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbTypicalDished = db.TypicalDishes.Include(p => p.Department).ToList();
                var typicalDished = Functions.FilterObjectListPropertiesByKeyword<TypicalDish>(dbTypicalDished, wellFormedKeyword);
                if (!dbTypicalDished.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(typicalDished);
            })
            .Produces<List<TypicalDish>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: TypicalDishEndpoint.MESSAGE_TYPICAL_DISH_SEARCH_SUMMARY,
                description: TypicalDishEndpoint.MESSAGE_TYPICAL_DISH_SEARCH_DESCRIPTION
                ));

             app.MapGet($"{API_TYPICAL_DISH_ROUTE_COMPLETE}/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }
            
                var sortBy = pagination.SortBy ?? string.Empty; 
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty; 
                var queryTypicalDished = db.TypicalDishes.Include(p => p.Department).AsQueryable(); 
                
                (queryTypicalDished, var isValidSort) = ApplySorting(queryTypicalDished, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var totalRecords = await queryTypicalDished.CountAsync();

                var pageTypicalDishes = await queryTypicalDished
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                if (!pageTypicalDishes.Any())
                {
                    return Results.NotFound();
                }

                var paginationResponse = new PaginationResponseModel<TypicalDish>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pageTypicalDishes
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<TypicalDish>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
            summary: TypicalDishEndpoint.MESSAGE_TYPICAL_DISH_PAGEDLIST_SUMMARY,
            description: TypicalDishEndpoint.MESSAGE_TYPICAL_DISH_PAGEDLIST_DESCRIPTION
            ));

        }

    }

}