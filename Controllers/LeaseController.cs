using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebApi.Data;
using RealEstateWebApi.Models;
using RealEstateWebApi.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class LeaseController : ControllerBase
{
    private readonly RealEstateWebApi.Data.RealEstateContext _context;

    public LeaseController(RealEstateWebApi.Data.RealEstateContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaseDto>>> GetLeases()
    {
        var leases = await _context.Leases
            .Include(l => l.Tenant)
            .Include(l => l.Property)
                .ThenInclude(p => p.Agent)
            .Select(l => new LeaseDto
            {
                LeaseID = l.LeaseID,
                MonthlyRent = l.MonthlyRent,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                Status = l.Status,
                Tenant = new TenantDto
                {
                    TenantID = l.Tenant.TenantID,
                    Name = l.Tenant.Name,
                    Phone = l.Tenant.Phone,
                    Address = l.Tenant.Address
                },
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
                    AgentID = l.Property.AgentID,
                    Agent = l.Property.Agent != null ? new AgentDto
                    {
                        AgentID = l.Property.Agent.AgentID,
                        Name = l.Property.Agent.Name,
                        Phone = l.Property.Agent.Phone,
                        Address = l.Property.Agent.Address,
                        Username = l.Property.Agent.User.Username,
                    } : null
                },
                Agent = l.Property.Agent != null ? new AgentDto
                {
                    AgentID = l.Property.Agent.AgentID,
                    Name = l.Property.Agent.Name,
                    Phone = l.Property.Agent.Phone,
                    Address = l.Property.Agent.Address,
                    Username = l.Property.Agent.User.Username,
                } : null
            })
            .ToListAsync();

        return Ok(leases);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeaseDto>> GetLease(int id)
    {
        var lease = await _context.Leases
            .Include(l => l.Tenant)
            .Include(l => l.Property)
                .ThenInclude(p => p.Agent)
            .Where(l => l.LeaseID == id)
            .Select(l => new LeaseDto
            {
                LeaseID = l.LeaseID,
                MonthlyRent = l.MonthlyRent,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                Status = l.Status,
                Tenant = new TenantDto
                {
                    TenantID = l.Tenant.TenantID,
                    Name = l.Tenant.Name,
                    Phone = l.Tenant.Phone,
                    Address = l.Tenant.Address
                },
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
                    AgentID = l.Property.AgentID,
                    Agent = l.Property.Agent != null ? new AgentDto
                    {
                        AgentID = l.Property.Agent.AgentID,
                        Name = l.Property.Agent.Name,
                        Phone = l.Property.Agent.Phone,
                        Address = l.Property.Agent.Address,
                        Username = l.Property.Agent.User.Username,

                    } : null
                },
                Agent = l.Property.Agent != null ? new AgentDto
                {
                    AgentID = l.Property.Agent.AgentID,
                    Name = l.Property.Agent.Name,
                    Phone = l.Property.Agent.Phone,
                    Address = l.Property.Agent.Address,
                    Username = l.Property.Agent.User.Username,
                } : null
            })
            .FirstOrDefaultAsync();

        if (lease == null) return NotFound();
        return Ok(lease);
    }


    [HttpPost]
    public async Task<ActionResult<LeaseDto>> CreateLease(LeaseDto dto)
    {
        var lease = new Lease
        {
            PropertyID = dto.Property.PropertyID,
            TenantID = dto.Tenant.TenantID,
            AgentID = dto.Property.AgentID,
            MonthlyRent = dto.MonthlyRent,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status
        };

        _context.Leases.Add(lease);
        await _context.SaveChangesAsync();

        dto.LeaseID = lease.LeaseID;
        return CreatedAtAction(nameof(GetLease), new { id = lease.LeaseID }, dto);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLease(int id, LeaseDto dto)
    {
        var lease = await _context.Leases.FindAsync(id);
        if (lease == null) return NotFound();

        lease.PropertyID = dto.Property.PropertyID;
        lease.TenantID = dto.Tenant.TenantID;
        lease.AgentID = dto.Property.AgentID;
        lease.MonthlyRent = dto.MonthlyRent;
        lease.StartDate = dto.StartDate;
        lease.EndDate = dto.EndDate;
        lease.Status = dto.Status;

        _context.Entry(lease).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Lease/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLease(int id)
    {
        var lease = await _context.Leases.FindAsync(id);
        if (lease == null) return NotFound();

        _context.Leases.Remove(lease);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
