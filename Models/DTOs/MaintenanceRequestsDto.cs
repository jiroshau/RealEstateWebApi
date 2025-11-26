namespace RealEstateWebApi.Models.DTOs
{
    public class MaintenanceRequestsDto
    {
        public int MaintenanceRequestID { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        public PropertyDto Property { get; set; }
        public TenantDto Tenant { get; set; }

    }
}
