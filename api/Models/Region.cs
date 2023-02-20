using System;
namespace api.Models
{
	public class Region
	{
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }
}

