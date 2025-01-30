
namespace api.Models
{
    public class TraditionalFairAndFestival
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Month { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}