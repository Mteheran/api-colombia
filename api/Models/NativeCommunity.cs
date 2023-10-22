using System;
namespace api.Models
{
	public class NativeCommunity
	{
        public int Id { get; set; }
		public string? Name { get; set; }
	    public string? Description { get; set; }
        public string? Languages { get; set; }
        public string[]? Images { get; set; }

         public virtual ICollection<IndigenousReservation> IndigenousReservations { get; set; }
    }
}

