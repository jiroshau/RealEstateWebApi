namespace RealEstateWebApi.Models.DTOs
{
    public class AccountantDto
    {
        public int AccountantID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }

        public List<PaymentDto> Payments { get; set; } = new();
    }

}
