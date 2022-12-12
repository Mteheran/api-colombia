namespace api.Models
{
    public class President
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public DateOnly StartPeriodDate { get; set; }
        public DateOnly EndPeriodDate { get; set; }
        public string? PoliticalParty { get; set; }
        public string? Description { get; set; }
        public int CountryId { get; set; }
    }
}
