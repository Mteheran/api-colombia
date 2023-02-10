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

            app.MapGet(API_DEPARTMENT_ROUTE_COMPLETE, (DBContext db) =>
            {
                return Results.Ok(db.Departments.ToList());
            })
            .WithMetadata(new SwaggerOperationAttribute(
                summary: DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_LIST_SUMMARY,
                description: DeparmentEndpointMetadataMessages.MESSAGE_DEPARMENT_LIST_DESCRIPTION
                ));

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                var departament = await db.Departments
                                            .Include(p => p.CityCapital)
                                            .SingleAsync(p => p.Id == id);

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


            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var departments = db.Departments.Where(x => x.Name!.ToUpper().Equals(name.Trim().ToUpper())).ToList();

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


        }
    }
}
