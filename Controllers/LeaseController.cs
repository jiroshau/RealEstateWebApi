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
    public async Task<ActionResult<LeaseDto>> CreateLease(CreateLeaseDto dto)
    {
        var lease = new Lease
        {
            PropertyID = dto.PropertyID,
            TenantID = dto.TenantID,
            AgentID = dto.AgentID,
            MonthlyRent = dto.MonthlyRent,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status
        };

        _context.Leases.Add(lease);
        await _context.SaveChangesAsync();

        var leaseDto = new LeaseDto
        {
            LeaseID = lease.LeaseID,
            Property = await _context.properties
                .Include(p => p.Agent)
                .Where(p => p.PropertyID == lease.PropertyID)
                .Select(p => new PropertyDto
                {
                    PropertyID = p.PropertyID,
                    PropertyName = p.PropertyName,
                    Address = p.Address,
                    Type = p.Type,
                    MonthlyRent = p.MonthlyRent,
                    Status = p.Status,
                    Description = p.Description,
                    LandlordID = p.LandlordID,
                    AgentID = p.AgentID,
                    Agent = p.Agent != null ? new AgentDto
                    {
                        AgentID = p.Agent.AgentID,
                        Name = p.Agent.Name,
                        Phone = p.Agent.Phone,
                        Address = p.Agent.Address,
                        Username = p.Agent.User.Username
                    } : null
                }).FirstOrDefaultAsync(),
            Tenant = await _context.tenants
                .Where(t => t.TenantID == lease.TenantID)
                .Select(t => new TenantDto
                {
                    TenantID = t.TenantID,
                    Name = t.Name,
                    Phone = t.Phone,
                    Address = t.Address
                }).FirstOrDefaultAsync(),
            Agent = lease.AgentID != null ? await _context.agents
                .Include(a => a.User)
                .Where(a => a.AgentID == lease.AgentID)
                .Select(a => new AgentDto
                {
                    AgentID = a.AgentID,
                    Name = a.Name,
                    Phone = a.Phone,
                    Address = a.Address,
                    Username = a.User.Username
                }).FirstOrDefaultAsync() : null,
            MonthlyRent = lease.MonthlyRent,
            StartDate = lease.StartDate,
            EndDate = lease.EndDate,
            Status = lease.Status
        };

        return CreatedAtAction(nameof(GetLease), new { id = lease.LeaseID }, leaseDto);
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLease(int id, CreateLeaseDto dto)
    {
        var lease = await _context.Leases.FindAsync(id);
        if (lease == null) return NotFound();

        lease.PropertyID = dto.PropertyID;
        lease.TenantID = dto.TenantID;
        lease.AgentID = dto.AgentID;
        lease.MonthlyRent = dto.MonthlyRent;
        lease.StartDate = dto.StartDate;
        lease.EndDate = dto.EndDate;
        lease.Status = dto.Status;

        _context.Entry(lease).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

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
