using System.ComponentModel.DataAnnotations;

namespace RealEstateWebApi.Models
{
    public class Accountant
    {    
        public int AccountantID { get; set; }

        public int UserID { get; set; }
        public User User { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
