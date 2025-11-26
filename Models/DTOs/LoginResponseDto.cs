namespace RealEstateWebApi.Models.DTOs
{
    public class LoginResponseDto
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
