using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebApi.Data;
using RealEstateWebApi.Models;
using RealEstateWebApi.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class LandlordController : ControllerBase
{
    private readonly RealEstateWebApi.Data.RealEstateContext _context;

    public LandlordController(RealEstateWebApi.Data.RealEstateContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LandlordDto>>> GetLandlords()
    {
        var landlords = await _context.landlords
            .Include(l => l.User)
            .Include(l => l.Properties)
            .Select(l => new LandlordDto
            {
                LandlordID = l.LandlordID,
                Name = l.Name,
                Phone = l.Phone,
                Address = l.Address,
                Username = l.User.Username,
                Email = l.User.Email,
                Properties = l.Properties.Select(p => new PropertyDto
                {
                    PropertyID = p.PropertyID,
                    PropertyName = p.PropertyName,
                    Address = p.Address,
                    Type = p.Type,
                    MonthlyRent = p.MonthlyRent,
                    Status = p.Status,
                    Description = p.Description,
                    AgentID = p.AgentID,
                    LandlordID = p.LandlordID
                }).ToList()
            }).ToListAsync();

        return Ok(landlords);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LandlordDto>> GetLandlord(int id)
    {
        var landlord = await _context.landlords
            .Include(l => l.User)
            .Include(l => l.Properties)
            .Where(l => l.LandlordID == id)
            .Select(l => new LandlordDto
            {
                LandlordID = l.LandlordID,
                Name = l.Name,
                Phone = l.Phone,
                Address = l.Address,
                Username = l.User.Username,
                Email = l.User.Email,
                Properties = l.Properties.Select(p => new PropertyDto
                {
                    PropertyID = p.PropertyID,
                    PropertyName = p.PropertyName,
                    Address = p.Address,
                    Type = p.Type,
                    MonthlyRent = p.MonthlyRent,
                    Status = p.Status,
                    Description = p.Description,
                    AgentID = p.AgentID,
                    LandlordID = p.LandlordID
                }).ToList()
            }).FirstOrDefaultAsync();

        if (landlord == null) return NotFound();
        return Ok(landlord);
    }

    [HttpPost]
    public async Task<ActionResult<LandlordDto>> CreateLandlord(Landlord dto)
    {
        _context.landlords.Add(dto);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLandlord), new { id = dto.LandlordID }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLandlord(int id, Landlord dto)
    {
        if (id != dto.LandlordID) return BadRequest();
        _context.Entry(dto).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLandlord(int id)
    {
        var landlord = await _context.landlords.FindAsync(id);
        if (landlord == null) return NotFound();
        _context.landlords.Remove(landlord);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
