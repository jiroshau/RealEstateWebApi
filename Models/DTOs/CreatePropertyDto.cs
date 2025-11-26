namespace RealEstateWebApi.Models.DTOs
{
    public class CreatePropertyDto
    {
        public int? LandlordID { get; set; }
        public int? AgentID { get; set; }
        public string PropertyName { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public decimal MonthlyRent { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
