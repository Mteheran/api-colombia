namespace api.Models
{
    public class IndigenousReservation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public string? ProcedureType { get; set; }
        public string? AdministrativeActType { get; set; }
        public int? AdministrativeActNumber { get; set; }
        public DateTime? AdministrativeActDate { get; set; }
        public int? NativeCommunityId { get; set; }
        public virtual NativeCommunity NativeCommunity {get;set;}
        public int? DeparmentId { get; set; }
        public virtual Department Department {get;set;}
        public int? CityId { get; set; }
        public virtual City City {get;set;}
    }
}