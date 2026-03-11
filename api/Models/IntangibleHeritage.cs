namespace api.Models
{
    public class IntangibleHeritage
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? DepartmentId { get; set; }
        public string? Scope { get; set; }
        public int? InclusionYear { get; set; }
        public virtual Department Department { get; set; }
    }
}
