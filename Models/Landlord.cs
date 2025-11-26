using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RealEstateWebApi.Models
{
    public class Landlord
    {
        public int LandlordID { get; set; }

        public int UserID { get; set; }
        public User User { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
