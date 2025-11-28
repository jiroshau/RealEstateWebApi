using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebApi.Data;
using RealEstateWebApi.Models;
using RealEstateWebApi.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly RealEstateWebApi.Data.RealEstateContext _context;

    public PropertyController(RealEstateWebApi.Data.RealEstateContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetProperties()
    {
        var properties = await _context.properties
            .Include(p => p.Landlord)
            .Include(p => p.Agent)
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
                Landlord = p.Landlord == null ? null : new LandlordDto
                {
                    LandlordID = p.Landlord.LandlordID,
                    Name = p.Landlord.Name,
                    Phone = p.Landlord.Phone,
                    Address = p.Landlord.Address
                },
                Agent = p.Agent == null ? null : new AgentDto
                {
                    AgentID = p.Agent.AgentID,
                    Name = p.Agent.Name,
                    Phone = p.Agent.Phone
                }
            }).ToListAsync();

        return Ok(properties);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<PropertyDto>> GetProperty(int id)
    {
        var property = await _context.properties
            .Where(p => p.PropertyID == id)
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
                AgentID = p.AgentID
            })
            .FirstOrDefaultAsync();

        if (property == null) return NotFound();
        return Ok(property);
    }


    [HttpPost]
public async Task<ActionResult<PropertyDto>> CreateProperty(CreatePropertyDto dto)
{
    var property = new Property
    {
        PropertyName = dto.PropertyName,
        Address = dto.Address,
        Type = dto.Type,
        MonthlyRent = dto.MonthlyRent,
        Status = dto.Status,
        Description = dto.Description,
        LandlordID = dto.LandlordID,
        AgentID = dto.AgentID
    };

    _context.properties.Add(property);
    await _context.SaveChangesAsync();

    var response = new PropertyDto
    {
        PropertyID = property.PropertyID,
        PropertyName = property.PropertyName,
        Address = property.Address,
        Type = property.Type,
        MonthlyRent = property.MonthlyRent,
        Status = property.Status,
        Description = property.Description,
        LandlordID = property.LandlordID,
        AgentID = property.AgentID
    };

    return CreatedAtAction(nameof(GetProperty), new { id = property.PropertyID }, response);
}


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProperty(int id, CreatePropertyDto dto)
    {
        var property = await _context.properties.FindAsync(id);
        if (property == null) return NotFound();

        property.PropertyName = dto.PropertyName;
        property.Address = dto.Address;
        property.Type = dto.Type;
        property.MonthlyRent = dto.MonthlyRent;
        property.Status = dto.Status;
        property.Description = dto.Description;
        property.LandlordID = dto.LandlordID;
        property.AgentID = dto.AgentID;

        _context.Entry(property).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProperty(int id)
    {
        var property = await _context.properties.FindAsync(id);
        if (property == null) return NotFound();

        _context.properties.Remove(property);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
