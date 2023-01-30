using api.Utils;

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
            });

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                var departament = await db.Departments.FindAsync(id);

                if (departament is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(departament);
            });

            app.MapGet($"{API_DEPARTMENT_ROUTE_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var departments = db.Departments.Where(x => x.Name!.ToUpper().Equals(name.Trim().ToUpper())).ToList();

                if (departments is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(departments);
            });
        }
    }
}
