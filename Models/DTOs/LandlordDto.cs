namespace RealEstateWebApi.Models.DTOs
{
    public class LandlordDto
    {
        public int LandlordID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }

        public List<PropertyDto> Properties { get; set; } = new();
    }
}
