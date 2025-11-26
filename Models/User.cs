namespace RealEstateWebApi.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public ICollection<Agent> Agents { get; set; } = new List<Agent>();
        public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
        public ICollection<Landlord> Landlords { get; set; } = new List<Landlord>();
        public ICollection<Accountant> Accountants { get; set; } = new List<Accountant>();
    }
}
