namespace RealEstateWebApi.Models.DTOs
{
    public class CreatePaymentDto
    {
        public int LeaseID { get; set; }
        public int AccountantID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public string PaymentMethod { get; set; }
        public string Status { get; set; }

    }
}
