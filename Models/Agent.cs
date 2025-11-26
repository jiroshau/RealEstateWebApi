using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateWebApi.Models
{
    [Table("agents")]
    public class Agent
    {
        public int AgentID { get; set; }
        public int UserID { get; set; }
        public User? User { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public ICollection<Lease> Leases { get; set; } = new List<Lease>();
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
