using System.Text.Json.Serialization;

namespace api.Models
{
    public class UrbanCenter
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        [JsonIgnore]
        public virtual City City { get; set; }
    }
}
