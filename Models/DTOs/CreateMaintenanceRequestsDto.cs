namespace RealEstateWebApi.Models.DTOs
{
    public class CreateMaintenanceRequestsDto
    {
        public int PropertyID { get; set; }
        public int TenantID { get; set; }

        public string Description { get; set; }
    }
}
