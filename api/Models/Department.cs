namespace api.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? CityCapitalId { get; set; }
        public int? Municipalities { get; set; }
        public float Surface { get; set; }
        public float? Population { get; set; }
        public string? PhonePrefix { get; set; }
        public int CountryId { get; set; }
        public virtual City CityCapital {get;set;}
        public virtual Country Country {get;set;}
        public ICollection<City> Cities {get;set;}
    }
}