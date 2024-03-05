namespace api.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public float? Surface { get; set; }
        public float? Population { get; set; }
        public string? PostalCode { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<TouristAttraction> TouristAttractions { get; set; }
        public virtual ICollection<President> Presidents { get; set; }
        public virtual ICollection<IndigenousReservation> IndigenousReservations { get; set; }
        public virtual ICollection<Airport> Airports { get; set; }
        public virtual ICollection<Radio> Radios { get; set; }

    }
}
