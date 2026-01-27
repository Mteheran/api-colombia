namespace api.Models
{
    public class HeritageCity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public string Image { get; set; }
    }
}
