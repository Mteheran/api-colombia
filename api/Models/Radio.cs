namespace api.Models
{
    public class Radio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Frequency { get; set; }
        public string Band { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public Uri Url { get; set; }
        public string[] Streamers { get; set;}   
    }
}
