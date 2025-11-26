using RealEstateWebApi.Models;
using RealEstateWebApi.Models.DTOs;

public class LeaseDto
{
        public int LeaseID { get; set; }
        public decimal MonthlyRent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public TenantDto Tenant { get; set; }
        public PropertyDto Property { get; set; }

        public AgentDto? Agent { get; set; }

    public List<PaymentDto> Payments { get; set; } = new();

}


