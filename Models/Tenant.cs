namespace RealEstateWebApi.Models
{
    public class Tenant
    {
        public int TenantID { get; set; }

        public int UserID { get; set; }
        public User? User { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public ICollection<Lease> Leases { get; set; } = new List<Lease>();
        public ICollection<MaintenanceRequests> MaintenanceRequests { get; set; } = new List<MaintenanceRequests>();
    }
}
