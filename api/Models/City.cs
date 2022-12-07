namespace api.Models
{
    public class City
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public float Surface { get; set; }
        public float Population { get; set; }
        public string? PostalCode { get; set; }
        public string? PhonePrefix { get; set; }
        public int DepartamentId { get; set; }
    }
}
