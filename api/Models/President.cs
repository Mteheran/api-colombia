namespace api.Models
{
    public class President
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public DateOnly Beginning { get; set; }
        public DateOnly Final { get; set; }
        public string? PoliticalParty { get; set; }
        public string? Desription { get; set; }
        public int CountryId { get; set; }
    }
}
