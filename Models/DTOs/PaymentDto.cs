namespace RealEstateWebApi.Models.DTOs
{
    public class PaymentDto
    {
        public int PaymentID { get; set; }

        public int LeaseID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public string PaymentMethod { get; set; }
        public string Status { get; set; }

        public LeaseDto Lease { get; set; }
        public AccountantDto Accountant { get; set; }
    }
}
