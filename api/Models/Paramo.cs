namespace api.Models
{
    public class Paramo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public float? Surface { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}