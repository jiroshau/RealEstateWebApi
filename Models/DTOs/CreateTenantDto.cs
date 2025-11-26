namespace RealEstateWebApi.Models.DTOs
{
    public class CreateTenantDto
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
