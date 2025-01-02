
namespace api.Models
{
    public class TraditionalFairAndFestival
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Month { get; set; }
        public int CityId { get; set; }
        public required virtual City City { get; set; }
    }
}