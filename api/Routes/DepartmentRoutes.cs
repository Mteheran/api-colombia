using api.Models;
using api.Utils;
using static api.Utils.Functions;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using DepartmentEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.DepartmentEndpoint;
using static api.Utils.Messages.EndpointMetadata;
using Microsoft.AspNetCore.Mvc;

namespace api.Routes
{
    public static class DepartmentRoutes
    { 
        public static void RegisterDepartmentAPI(WebApplication app)
        {
            const string API_DEPARTMENT_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.DEPARTMENT_ROUTE}";

            app.MapGet(API_DEPARTMENT_ROUTE_COMPLETE, async (DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                var queryDepartments = db.Departments.Include(p => p.CityCapital).AsQueryable();
                (queryDepartments, var isValidSort) = ApplySorting(queryDepartments, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }
 
                var listDepartments = await queryDepartments.ToListAsync();
                return Results.Ok(listDepartments);
            })
            .Produces<List<Department>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_LIST_SUMMARY,
                description: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_LIST_DESCRIPTION
            ));

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var department = await db.Departments
                                            .Include(p => p.CityCapital)
                                            .SingleOrDefaultAsync(p => p.Id == id);

                if (department is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(department);
            })
            .Produces<Department?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_BYID_SUMMARY,
                description: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_BYID_DESCRIPTION
                ));

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{{id}}/cities", async (int id, DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                 if (id <= 0)
                {
                    return Results.BadRequest("Invalid department ID.");
                }

                var queryCitiesByDepartment = db.Cities.Where(p => p.DepartmentId == id).AsQueryable(); 
                (queryCitiesByDepartment, var isValidSort) = ApplySorting(queryCitiesByDepartment, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                if (!queryCitiesByDepartment.Any())
                {
                    return Results.NotFound();
                }
 
                var cities = await queryCitiesByDepartment.ToListAsync();
                return Results.Ok(cities);
            })
             .Produces<List<City>?>(200)
             .WithMetadata(new SwaggerOperationAttribute(
                 summary: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_CITIES_SUMMARY,
                 description: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_CITIES_DESCRIPTION
                 ));

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{{id}}/naturalareas", async (int id, DBContext db, 
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
               if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var queryNaturalAreas = db.Departments
                    .Include(p => p.NaturalAreas)
                    .Where(p => p.Id == id)
                    .SelectMany(p => p.NaturalAreas)
                    .AsQueryable();

                (queryNaturalAreas, var isValidSort) = ApplySorting(queryNaturalAreas, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }
 
                if (!queryNaturalAreas.Any())
                {
                    return Results.NotFound();
                }

                var naturalAreas = await queryNaturalAreas.ToListAsync();
                return Results.Ok(naturalAreas);
            })
            .Produces<List<Department>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_NATURALAREAS_SUMMARY,
                description: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_NATURALAREAS_DESCRIPTION
                ));

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{{id}}/touristicattractions", async (int id, DBContext db, 
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
          {
            if (id <= 0)
            {
                return Results.BadRequest(RequestMessages.BadRequest);
            }

            var queryTouristicAttractions = db.TouristAttractions
                .Include(p => p.City)
                .Join(db.Cities, t => t.CityId, c => c.Id, (t, c) => new { t, c.DepartmentId })
                .Join(db.Departments, t => t.DepartmentId, d => d.Id, (t, d) => new { t.DepartmentId, t.t })
                .Where(p => p.DepartmentId == id)
                .Select(p => p.t)
                .AsQueryable();
        
            (queryTouristicAttractions, var isValidSort) = ApplySorting(queryTouristicAttractions, sortBy, sortDirection);

            if (!isValidSort)
            {
                return Results.BadRequest(RequestMessages.BadRequest);
            }
        
            if (!queryTouristicAttractions.Any())
            {
                return Results.NotFound();
            }
        
            var touristAttractions = await queryTouristicAttractions.ToListAsync();
            return Results.Ok(touristAttractions);
          })
              .Produces<List<TouristAttraction>?>(200)
              .WithMetadata(new SwaggerOperationAttribute(
                  summary: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_NATURALAREAS_SUMMARY,
                  description: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_NATURALAREAS_DESCRIPTION
                  ));


            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/name/{{name}}", async (string name, DBContext db) =>
            {
                var departments = await db.Departments.Where(x => x.Name!.ToUpper().Equals(name.Trim().ToUpper())).ToListAsync();

                if (departments is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(departments);
            })
            .Produces<List<Department>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_BYNAME_SUMMARY,
                description: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_BYNAME_DESCRIPTION
                ));

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbDepartments = db.Departments.ToList();
                var departments = Functions.FilterObjectListPropertiesByKeyword<Department>(dbDepartments, wellFormedKeyword);
                if (!departments.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(departments);
            })
            .Produces<List<Department>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_SEARCH_SUMMARY,
                description: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_SEARCH_DESCRIPTION
                ));

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }
            
                var sortBy = pagination.SortBy ?? string.Empty; 
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty; 
                var queryDepartments = db.Departments.AsQueryable(); 
                
                (queryDepartments, var isValidSort) = ApplySorting(queryDepartments, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var totalRecords = await queryDepartments.CountAsync();

                var pagedDepartments = await queryDepartments
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                if (!pagedDepartments.Any())
                {
                    return Results.NotFound();
                }

                var paginationResponse = new PaginationResponseModel<Department>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pagedDepartments
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<Department>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
            summary: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_PAGEDLIST_SUMMARY,
            description: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_PAGEDLIST_DESCRIPTION
            ));
        }
    }
}
