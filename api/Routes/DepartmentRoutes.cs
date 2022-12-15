namespace api.Routes
{
    public static class DepartmentRoutes
    {
        public static void RegisterDepartmentAPI(WebApplication app)
        {
            string route = "Department";

            app.MapGet($"{route}", (DBContext db) =>
            {
                return Results.Ok(db.Departments.ToList());
            });

            app.MapGet($"{route}/{{id}}", async (int id, DBContext db) =>
            {
                var dept = await db.Departments.FindAsync(id);

                if (dept != null)
                {
                    return Results.Ok(dept);
                }
                else 
                {
                    return Results.NotFound();                
                }
            });

            app.MapGet($"{route}/Name/{{name}}", (string name, DBContext db) =>
            {
                var dept = db.Departments.Where(x => x.Name.ToUpper() == name.ToUpper()).ToList();

                if (dept != null)
                {
                    return Results.Ok(dept);
                }
                else
                {
                    return Results.NotFound();
                }
            });
        }
    }
}
