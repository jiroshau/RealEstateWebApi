using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebApi.Data;
using RealEstateWebApi.Models;
using RealEstateWebApi.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AgentController : ControllerBase
{
    private readonly RealEstateWebApi.Data.RealEstateContext _context;

    public AgentController(RealEstateWebApi.Data.RealEstateContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<AgentDto>>> GetAgents()
    {
        var agents = await _context.agents
            .Include(a => a.Properties)
            .Include(a => a.Leases)
                .ThenInclude(l => l.Tenant)
            .Include(a => a.Leases)
                .ThenInclude(l => l.Property)
            .Select(a => new AgentDto
            {
                AgentID = a.AgentID,
                Name = a.Name,
                Phone = a.Phone,
                Address = a.Address,
                Username = a.User.Username,
                Properties = a.Properties.Select(p => new PropertyDto
                {
                    PropertyID = p.PropertyID,
                    PropertyName = p.PropertyName,
                    Address = p.Address,
                    Type = p.Type,
                    MonthlyRent = p.MonthlyRent,
                    Status = p.Status,
                    Description = p.Description,
                    LandlordID = p.LandlordID,
                    AgentID = p.AgentID
                }).ToList(),
                Leases = a.Leases.Select(l => new LeaseDto
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
                        AgentID = l.Property.AgentID
                    }
                }).ToList()
            }).ToListAsync();

        return Ok(agents);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<AgentDto>> GetAgent(int id)
    {
        var agent = await _context.agents
            .Include(a => a.Properties)
            .Include(a => a.Leases)
                .ThenInclude(l => l.Tenant)
            .Include(a => a.Leases)
                .ThenInclude(l => l.Property)
            .Where(a => a.AgentID == id)
            .Select(a => new AgentDto
            {
                AgentID = a.AgentID,
                Name = a.Name,
                Phone = a.Phone,
                Address = a.Address,
                Username = a.User.Username,
                Properties = a.Properties.Select(p => new PropertyDto
                {
                    PropertyID = p.PropertyID,
                    PropertyName = p.PropertyName,
                    Address = p.Address,
                    Type = p.Type,
                    MonthlyRent = p.MonthlyRent,
                    Status = p.Status,
                    Description = p.Description,
                    LandlordID = p.LandlordID,
                    AgentID = p.AgentID
                }).ToList(),
                Leases = a.Leases.Select(l => new LeaseDto
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
                        AgentID = l.Property.AgentID
                    }
                }).ToList()
            }).FirstOrDefaultAsync();

        if (agent == null) return NotFound();
        return Ok(agent);
    }


    [HttpPost]
    public async Task<ActionResult<AgentDto>> CreateAgent(CreateAgentDto dto)
    {
        var agent = new Agent
        {
            UserID = dto.UserID,
            Name = dto.Name,
            Phone = dto.Phone,
            Address = dto.Address
        };

        _context.agents.Add(agent);
        await _context.SaveChangesAsync();

        var agentDto = new AgentDto
        {
            AgentID = agent.AgentID,
            Name = agent.Name,
            Phone = agent.Phone,
            Address = agent.Address,
            Properties = new List<PropertyDto>(),
            Leases = new List<LeaseDto>()
        };

        return CreatedAtAction(nameof(GetAgent), new { id = agent.AgentID }, agentDto);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAgent(int id, CreateAgentDto dto)
    {
        var agent = await _context.agents.FindAsync(id);
        if (agent == null) return NotFound();

        agent.Name = dto.Name;
        agent.Phone = dto.Phone;
        agent.Address = dto.Address;

        _context.Entry(agent).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAgent(int id)
    {
        var agent = await _context.agents.FindAsync(id);
        if (agent == null) return NotFound();

        _context.agents.Remove(agent);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
