using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebApi.Data;
using RealEstateWebApi.Models;
using RealEstateWebApi.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class TenantController : ControllerBase
{
    private readonly RealEstateWebApi.Data.RealEstateContext _context;

    public TenantController(RealEstateWebApi.Data.RealEstateContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TenantDto>>> GetTenants()
    {
        var tenants = await _context.tenants
            .Select(t => new TenantDto
            {
                TenantID = t.TenantID,
                UserID = t.UserID,
                Name = t.Name,
                Phone = t.Phone,
                Address = t.Address,
            })
            .ToListAsync();

        return Ok(tenants);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<TenantDto>> GetTenant(int id)
    {
        var tenant = await _context.tenants
            .Include(t => t.User)
            .Include(t => t.Leases)
                .ThenInclude(l => l.Property)
            .Where(t => t.TenantID == id)
            .Select(t => new TenantDto
            {
                TenantID = t.TenantID,
                Name = t.Name,
                Phone = t.Phone,
                Address = t.Address,
                Leases = t.Leases.Select(l => new LeaseDto
                {
                    LeaseID = l.LeaseID,
                    MonthlyRent = l.MonthlyRent,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    Status = l.Status,
                    Property = new PropertyDto
                    {
                        PropertyID = l.Property.PropertyID,
                        PropertyName = l.Property.PropertyName,
                        Address = l.Property.Address,
                        Type = l.Property.Type,
                        MonthlyRent = l.Property.MonthlyRent,
                        Status = l.Property.Status,
                        Description = l.Property.Description,
                        LandlordID = l.Property.LandlordID,
                        AgentID = l.Property.AgentID
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (tenant == null) return NotFound();
        return Ok(tenant);
    }


    [HttpPost]
    public async Task<ActionResult<TenantDto>> CreateTenant([FromBody] CreateTenantDto dto)
    {
        Console.WriteLine($"➡ Received DTO: UserID={dto.UserID}, Name={dto.Name}");

        var user = await _context.users.FindAsync(dto.UserID);
        if (user == null)
            return BadRequest("UserID does not exist.");

        var tenant = new Tenant
        {
            UserID = dto.UserID,
            Name = dto.Name,
            Phone = dto.Phone,
            Address = dto.Address
        };

        _context.tenants.Add(tenant);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTenant), new { id = tenant.TenantID }, new TenantDto
        {
            TenantID = tenant.TenantID,
            Name = tenant.Name,
            Phone = tenant.Phone,
            Address = tenant.Address,
            Leases = new List<LeaseDto>()
        });
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTenant(int id, [FromBody] CreateTenantDto dto)
    {
        var tenant = await _context.tenants.FindAsync(id);
        if (tenant == null) return NotFound();

        tenant.Name = dto.Name;
        tenant.Phone = dto.Phone;
        tenant.Address = dto.Address;
        tenant.UserID = dto.UserID;

        _context.Entry(tenant).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Tenant/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTenant(int id)
    {
        var tenant = await _context.tenants.FindAsync(id);
        if (tenant == null) return NotFound();

        _context.tenants.Remove(tenant);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
