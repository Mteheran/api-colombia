namespace api.Models
{
    public class InvasiveSpecie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ScientificName { get; set; }
        public string CommonNames { get; set; }
        public string Impact { get; set; }
        public string Manage { get; set; }
        public RiskLevel RiskLevel { get; set; }
        public string UrlImage { get; set; }
    }

    public enum RiskLevel
    {
        Low, Medium, High
    }
}
