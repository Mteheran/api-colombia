namespace api.Models
{
    public class Airport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IataCode { get; set; }
        public string OaciCode { get; set; }
        public string Type { get; set; }
        public int DeparmentId { get; set; }
        public virtual Department Department { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
