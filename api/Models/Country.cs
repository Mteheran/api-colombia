namespace api.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? StateCapital { get; set; }        
        public float Surface { get; set; }
        public float Population { get; set; }
        public string? Language { get; set; }
        public string? TimeZone { get; set; }
        public string? Currency { get; set; }
        public string? ISOCode { get; set; }
        public string? InternetDomain { get; set; }
        public string? PhonePrefix { get; set; }
        public string? RadioPrefix { get; set; }
        public string? AircraftPrefix { get; set; }
    }
}
