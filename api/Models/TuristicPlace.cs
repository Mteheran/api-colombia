namespace api.Models
{
    public class TuristicPlace
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public int DepartamentId { get; set; }
    }
}
