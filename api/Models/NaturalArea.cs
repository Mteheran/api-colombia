namespace api.Models
{
    public class NaturalArea
    {
        public int Id { get; set; }
        public int AreaGroupId { get; set; }
        public int CategoryNaturalAreaId { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public int DaneCode { get; set; }
        public double? LandArea { get; set; }
        public double? MaritimeArea { get; set; }
        public virtual Department Department { get; set; }
        public virtual CategoryNaturalArea CategoryNaturalArea { get; set; }
    }
}
