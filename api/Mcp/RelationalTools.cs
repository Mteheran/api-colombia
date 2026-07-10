using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;

namespace api.Mcp;

/// <summary>
/// Tools for relationships between resources, mirroring the existing department
/// sub-resource REST routes (e.g. api/v1/Department/{id}/cities).
/// </summary>
[McpServerToolType]
public class RelationalTools
{
    [McpServerTool(Name = "get_cities_by_department", ReadOnly = true, Idempotent = true),
     Description("Returns the cities that belong to a department, identified by the department's numeric Id.")]
    public static object GetCitiesByDepartment(
        DBContext db,
        [Description("Numeric Id of the department (must be greater than 0).")] int departmentId)
    {
        if (departmentId <= 0)
            return new { error = "departmentId must be greater than 0." };

        return db.Cities.Where(c => c.DepartmentId == departmentId).ToList();
    }

    [McpServerTool(Name = "get_natural_areas_by_department", ReadOnly = true, Idempotent = true),
     Description("Returns the natural areas located in a department, identified by the department's numeric Id.")]
    public static object GetNaturalAreasByDepartment(
        DBContext db,
        [Description("Numeric Id of the department (must be greater than 0).")] int departmentId)
    {
        if (departmentId <= 0)
            return new { error = "departmentId must be greater than 0." };

        return db.Departments
            .Include(d => d.NaturalAreas)
            .Where(d => d.Id == departmentId)
            .SelectMany(d => d.NaturalAreas)
            .ToList();
    }

    [McpServerTool(Name = "get_touristic_attractions_by_department", ReadOnly = true, Idempotent = true),
     Description("Returns the tourist attractions located in a department, identified by the department's numeric Id.")]
    public static object GetTouristicAttractionsByDepartment(
        DBContext db,
        [Description("Numeric Id of the department (must be greater than 0).")] int departmentId)
    {
        if (departmentId <= 0)
            return new { error = "departmentId must be greater than 0." };

        return db.TouristAttractions
            .Include(t => t.City)
            .Where(t => t.City.DepartmentId == departmentId)
            .ToList();
    }
}
