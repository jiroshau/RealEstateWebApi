public class TenantDto
{
    public int TenantID { get; set; }
    public int UserID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public List<LeaseDto> Leases { get; set; } = new();
}
