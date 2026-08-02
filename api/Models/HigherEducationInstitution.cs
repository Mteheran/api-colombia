namespace api.Models
{
    public class HigherEducationInstitution
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string LegalNature { get; set; }
        public string AcademicCharacter { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsHighQualityAccredited { get; set; }
        public string Website { get; set; }
    }
}
