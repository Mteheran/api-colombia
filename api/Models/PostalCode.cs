namespace api.Models
{
    public class PostalCode
    {
        public int Id { get; set; }
        public int NoId { get; set; }
        public int CityId { get; set; }
        public string PostalZone { get; set; }
        public string Code { get; set; }
        public string NorthLimit { get; set; }
        public string SouthLimit { get; set; }
        public string EastLimit { get; set; }
        public string WestLimit { get; set; }
        public string Type { get; set; }
        public string NeighborhoodsContainedInPostalCode { get; set; }
        public string RuralAreasContainedInPostalCode { get; set; }
        public virtual City City { get; set; }
    }
}