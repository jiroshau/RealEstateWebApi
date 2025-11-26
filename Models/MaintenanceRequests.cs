using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstateWebApi.Models
{
    public class MaintenanceRequests
    {
        [Key]
        public int MaintenanceRequestID { get; set; }


        public int PropertyID { get; set; }
        public Property Property { get; set; } = null!;

        public int TenantID { get; set; }
        public Tenant Tenant { get; set; } = null!;
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = "Pending";
        public string Description { get; set; } = string.Empty;

        public ICollection<MaintenanceRequests> MaintenanceRequest { get; set; } = new List<MaintenanceRequests>();

    }

}
