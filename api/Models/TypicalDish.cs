namespace api.Models
{
    public class TypicalDish
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Ingredients { get; set; } 
        public string? ImageUrl { get; set; }   
        public int? DepartmentId { get; set; }  
        public virtual Department Department { get; set; }
    }
}