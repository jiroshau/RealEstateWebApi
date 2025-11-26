using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealEstateWebApi.Models
{
    public class Lease
    {
        [Key]
        public int LeaseID { get; set; }

        public int PropertyID { get; set; }
        public Property Property { get; set; } = null!;

        public int TenantID { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public int? AgentID { get; set; }
        public Agent Agent { get; set; } = null!;

        
        public decimal MonthlyRent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Active";
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
