namespace RealEstateWebApi.Models.DTOs
{
    public class CreateLeaseDto
    {
        public int PropertyID { get; set; }
        public int TenantID { get; set; }
        public int? AgentID { get; set; }
        public decimal MonthlyRent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}
