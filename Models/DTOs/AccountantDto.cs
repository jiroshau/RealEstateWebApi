namespace RealEstateWebApi.Models.DTOs
{
    public class AccountantDto
    {
        public int AccountantID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public List<PaymentDto> Payments { get; set; } = new();
    }
}
