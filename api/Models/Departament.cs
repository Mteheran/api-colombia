namespace api.Models
{
    public class Departament
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Capital { get; set; }
        public string? Municipalities { get; set; }
        public float Surface { get; set; }
        public float? Population { get; set; }
        public string? PhonePrefix { get; set; }
        public int CountryId { get; set; }
    }
}
