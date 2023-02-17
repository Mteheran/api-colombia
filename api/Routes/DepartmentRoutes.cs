using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using DeparmentEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.DepartmentEndpoint;

namespace api.Routes
{
    public static class DepartmentRoutes
    {
        public static void RegisterDepartmentAPI(WebApplication app)
        {
            const string API_DEPARTMENT_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.DEPARTMENT_ROUTE}";

            app.MapGet(API_DEPARTMENT_ROUTE_COMPLETE, async (DBContext db) =>
            {
                return Results.Ok(await db.Departments.ToListAsync());
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary: DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_LIST_SUMMARY,
                description: DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_LIST_DESCRIPTION
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
            .WithMetadata(new SwaggerOperationAttribute(
                summary: DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_BYID_SUMMARY,
                description: DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_BYID_DESCRIPTION
                ));


            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{{id}}/cities", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var listOfCitiesByDeparment = db.Cities.Where(p => p.DepartamentId == id);

                if (listOfCitiesByDeparment is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(listOfCitiesByDeparment);
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary: DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_CITIES_SUMMARY,
                description: DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_CITIES_DESCRIPTION
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
            .WithMetadata(new SwaggerOperationAttribute(
                summary: DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_BYNAME_SUMMARY,
                description: DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_BYNAME_DESCRIPTION
                ));

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbDepartments = db.Departments.ToList();

                var departments = Functions.FilterObjectListPropertiesByKeyword<Department>(dbDepartments, wellFormedKeyword);

                if (departments.Count == 0)
                {
                    return Results.NotFound();
                }

                return Results.Ok(departments);
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary:DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_SEARCH_SUMMARY, 
                description: DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_SEARCH_DESCRIPTION
                ));

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/pagedList",
                  async (PaginationModel pagination, DBContext db) =>
                  {

                      if (pagination.Page <= 0 || pagination.PageSize <= 0)
                      {
                          return Results.BadRequest();
                      }

                      var deparmentsPaged = db.Departments.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);

                      if (await deparmentsPaged?.CountAsync() == 0)
                      {
                          return Results.NotFound();
                      }

                      var paginationResponse = new PaginationResponseModel<Department>
                      {
                          Page = pagination.Page,
                          PageSize = pagination.PageSize,
                          TotalRecords = await db.Departments.CountAsync(),
                          Data = await deparmentsPaged.ToListAsync(),

                      };

                      return Results.Ok(paginationResponse);
                  })
         .WithMetadata(new SwaggerOperationAttribute(
             summary: DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_PAGEDLIST_SUMMARY,
              description: DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_PAGEDLIST_DESCRIPTION
              ));
        }
    }
}
