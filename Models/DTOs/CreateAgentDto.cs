namespace RealEstateWebApi.Models.DTOs
{
    public class CreateAgentDto
    {
        public int UserID { get; set; } // Foreign key
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
