namespace RealEstateWebApi.Models.DTOs
{
    public class UserDto
    {
            public int UserID { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }

            public List<AgentDto> Agents { get; set; } = new();
            public List<TenantDto> Tenants { get; set; } = new();
            public List<LandlordDto> Landlords { get; set; } = new();
            public List<AccountantDto> Accountants { get; set; } = new();
        
    }
}
