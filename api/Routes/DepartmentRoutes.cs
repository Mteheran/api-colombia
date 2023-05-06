using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using DepartmentEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.DepartmentEndpoint;

namespace api.Routes
{
    public static class DepartmentRoutes
    {
        public static void RegisterDepartmentAPI(WebApplication app)
        {
            const string API_DEPARTMENT_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.DEPARTMENT_ROUTE}";

            app.MapGet(API_DEPARTMENT_ROUTE_COMPLETE, async (DBContext db) =>
            {
                var listDeparments = await db.Departments.ToListAsync();
                return Results.Ok(listDeparments);
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

                var departament = await db.Departments
                                            .Include(p => p.CityCapital)
                                            .SingleOrDefaultAsync(p => p.Id == id);

                if (departament is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(departament);
            })
            .Produces<Department?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_BYID_SUMMARY,
                description: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_BYID_DESCRIPTION
                ));

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{{id}}/cities", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var listOfCitiesByDepartment = db.Cities.Where(p => p.DepartmentId == id).ToList();
                if (listOfCitiesByDepartment is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(listOfCitiesByDepartment);
            })
            .Produces<List<City>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_CITIES_SUMMARY,
                description: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_CITIES_DESCRIPTION
                ));

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{{id}}/naturalareas", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var listOfNaturalAreas = db.Departments.Include(p => p.NaturalAreas).Where(p => p.Id == id).ToList();
                if (listOfNaturalAreas is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(listOfNaturalAreas);
            })
            .Produces<List<Department>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_NATURALAREAS_SUMMARY,
                description: DepartmentEndpointMetadataMessages.MESSAGE_DEPARTMENT_NATURALAREAS_DESCRIPTION
                ));


            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{{id}}/touristicattractions", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var listOfNaturalAreas = db.TouristAttractions.Include(p => p.City)
                                                                .Join(db.Cities, t => t.CityId, c => c.Id, (t, c) => new { t, c.DepartmentId })
                                                                .Join(db.Departments, t => t.DepartmentId, d => d.Id, (t, d) => new { t.DepartmentId, t.t })
                                                                .Where(p => p.DepartmentId == id).Select(p => p.t)
                                                                .ToList();

                if (listOfNaturalAreas is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(listOfNaturalAreas);
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

                var deparmentsPaged = db.Departments.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
                if (!await deparmentsPaged?.AnyAsync())
                {
                    return Results.NotFound();
                }

                var paginationResponse = new PaginationResponseModel<Department>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = await db.Departments.CountAsync(),
                    Data = await deparmentsPaged.ToListAsync()
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
