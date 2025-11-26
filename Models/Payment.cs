using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstateWebApi.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }
        public int AccountantID { get; set; }
        public Accountant Accountant { get; set; } = null!;

        public int LeaseID { get; set; }
        public Lease Lease { get; set; } = null!;

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
    }
}
