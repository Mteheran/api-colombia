namespace api.Models
{
    public class TouristAttraction
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string[]? Images { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}
