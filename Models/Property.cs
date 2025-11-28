using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealEstateWebApi.Models
{
    public class Property
    {
        [Key]
        public int PropertyID { get; set; }


        public int? LandlordID { get; set; }
        public Landlord? Landlord { get; set; } = null!;

        public int? AgentID { get; set; }
        public Agent? Agent { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal MonthlyRent { get; set; }
        public string Status { get; set; } = "Available";
        public string Description { get; set; } = string.Empty;

        public ICollection<Lease> Leases { get; set; } = new List<Lease>();
        public ICollection<MaintenanceRequests> MaintenanceRequests { get; set; } = new List<MaintenanceRequests>();
    }
}
