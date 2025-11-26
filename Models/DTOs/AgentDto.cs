namespace RealEstateWebApi.Models.DTOs
{
    public class AgentDto
    {
        public int AgentID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string Username { get; set; }

        public List<PropertyDto> Properties { get; set; } = new();
        public List<LeaseDto> Leases { get; set; } = new();
    }
}
