using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebApi.Data;
using RealEstateWebApi.Models;
using RealEstateWebApi.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceRequestController : ControllerBase
{
    private readonly RealEstateWebApi.Data.RealEstateContext _context;

    public MaintenanceRequestController(RealEstateWebApi.Data.RealEstateContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaintenanceRequestsDto>>> GetRequests()
    {
        var requests = await _context.MaintenanceRequests
            .Include(r => r.Property)
            .Include(r => r.Tenant)
            .Select(r => new MaintenanceRequestsDto
            {
                MaintenanceRequestID = r.MaintenanceRequestID,
                RequestDate = r.RequestDate,
                Status = r.Status,
                Description = r.Description,

                Property = new PropertyDto
                {
                    PropertyID = r.Property.PropertyID,
                    PropertyName = r.Property.PropertyName,
                    Address = r.Property.Address,
                    Type = r.Property.Type,
                    MonthlyRent = r.Property.MonthlyRent,
                    Status = r.Property.Status,
                    Description = r.Property.Description,
                    LandlordID = r.Property.LandlordID,
                    AgentID = r.Property.AgentID
                },

                Tenant = new TenantDto
                {
                    TenantID = r.Tenant.TenantID,
                    Name = r.Tenant.Name,
                    Phone = r.Tenant.Phone,
                    Address = r.Tenant.Address
                }
            })
            .ToListAsync();

        return Ok(requests);
    }

    // GET: api/MaintenanceRequest/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MaintenanceRequestsDto>> GetRequest(int id)
    {
        var request = await _context.MaintenanceRequests
            .Include(r => r.Property)
            .Include(r => r.Tenant)
            .Where(r => r.MaintenanceRequestID == id)
            .Select(r => new MaintenanceRequestsDto
            {
                MaintenanceRequestID = r.MaintenanceRequestID,
                RequestDate = r.RequestDate,
                Status = r.Status,
                Description = r.Description,

                Property = new PropertyDto
                {
                    PropertyID = r.Property.PropertyID,
                    PropertyName = r.Property.PropertyName,
                    Address = r.Property.Address,
                    Type = r.Property.Type,
                    MonthlyRent = r.Property.MonthlyRent,
                    Status = r.Property.Status,
                    Description = r.Property.Description,
                    LandlordID = r.Property.LandlordID,
                    AgentID = r.Property.AgentID
                },

                Tenant = new TenantDto
                {
                    TenantID = r.Tenant.TenantID,
                    Name = r.Tenant.Name,
                    Phone = r.Tenant.Phone,
                    Address = r.Tenant.Address
                }
            })
            .FirstOrDefaultAsync();

        if (request == null)
            return NotFound();

        return Ok(request);
    }

    [HttpPost]
    public async Task<ActionResult<MaintenanceRequestsDto>> CreateRequest(CreateMaintenanceRequestsDto dto)
    {
        var request = new MaintenanceRequests
        {
            PropertyID = dto.PropertyID,
            TenantID = dto.TenantID,
            Description = dto.Description,
            Status = "Pending",
            RequestDate = DateTime.UtcNow
        };

        _context.MaintenanceRequests.Add(request);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRequest), new { id = request.MaintenanceRequestID }, request);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var request = await _context.MaintenanceRequests.FindAsync(id);
        if (request == null)
            return NotFound();

        request.Status = status;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRequest(int id)
    {
        var request = await _context.MaintenanceRequests.FindAsync(id);
        if (request == null)
            return NotFound();

        _context.MaintenanceRequests.Remove(request);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
