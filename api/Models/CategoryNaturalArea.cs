namespace api.Models
{
    public class CategoryNaturalArea
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<NaturalArea> NaturalAreas {get;set;}
    }
}
